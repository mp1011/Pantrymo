using Pantrymo.Application.Extensions;
using Pantrymo.Application.Models;
using System.Net;
using System.Net.NetworkInformation;

namespace Pantrymo.Application.Services
{
    public interface ISiteAPI
    {
        Task<Site[]?> GetSites(DateTime from);
    }

    public class LocalSiteAPI : ISiteAPI
    {
        private readonly IDataContext _dataContext;

        public LocalSiteAPI(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<Site[]> GetSites(DateTime from)
            => await _dataContext.Sites.GetByDateAsync(from);        
    }

    public class WebSiteAPI : ISiteAPI
    {
        public async Task<Site[]?> GetSites(DateTime from)
        {
            using var webClient = new HttpClient();
            return await webClient.GetJsonAsync<Site[]>($"https://localhost:7188/api/Sites/getByDate/{from.ToUrlDateString()}");
        }
    }

    public class WebSiteAPIWithFallback : ISiteAPI
    {
        private readonly LocalSiteAPI _fallbackAPI;
        private readonly WebSiteAPI _webAPI;
        private readonly NetworkCheckService _networkCheckService;

        public WebSiteAPIWithFallback(LocalSiteAPI fallbackAPI, WebSiteAPI webAPI, NetworkCheckService networkCheckService)
        {
            _fallbackAPI = fallbackAPI;
            _webAPI = webAPI;
            _networkCheckService = networkCheckService;
        }

        public async Task<Site[]> GetSites(DateTime from)
        {
            bool hasNetwork = NetworkInterface.GetIsNetworkAvailable();
            if(!hasNetwork)
                return await _fallbackAPI.GetSites(from);

            var result = await _webAPI.GetSites(from)
                             .DefaultIfFaulted();

            if(result == null)
                result = await _fallbackAPI.GetSites(from);

            return result;
        }
    }
}
