using Pantrymo.Application.Extensions;
using Pantrymo.Application.Models;

namespace Pantrymo.Application.Services
{
    public interface IDataAccess
    {
        Task<Result<Site[]>> GetSites(DateTime from);
        Task<Result<Component[]>> GetComponents(DateTime from);
        Task<Result<AlternateComponentName[]>> GetAlternateComponentName(DateTime from);
    }

    public class LocalDataAccess : IDataAccess
    {
        private readonly IDataContext _dataContext;

        public LocalDataAccess(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<Result<Site[]>> GetSites(DateTime from) 
            => await _dataContext.Sites.GetByDateAsync(from);
                    
        public async Task<Result<Component[]>> GetComponents(DateTime from) 
            => await _dataContext.Components.GetByDateAsync(from);

        public async Task<Result<AlternateComponentName[]>> GetAlternateComponentName(DateTime from) 
            => await _dataContext.AlternateComponentNames.GetByDateAsync(from);
    }

    public class RemoteDataAccess : IDataAccess
    {
        private async Task<Result<T[]>> GetRecords<T>(DateTime from)
        {
            using var webClient = new HttpClient();
            return await webClient
                    .GetJsonArrayAsync<T>($"https://localhost:7188/api/{typeof(T).Name}/getByDate/{from.ToUrlDateString()}");
        }

        public async Task<Result<Site[]>> GetSites(DateTime from) => await GetRecords<Site>(from);

        public async Task<Result<Component[]>> GetComponents(DateTime from) => await GetRecords<Component>(from);

        public async Task<Result<AlternateComponentName[]>> GetAlternateComponentName(DateTime from) => await GetRecords<AlternateComponentName>(from);
       
    }

    public class RemoteDataAccessWithLocalFallback : IDataAccess
    {
        private readonly LocalDataAccess _fallbackAPI;
        private readonly RemoteDataAccess _webAPI;
        private readonly NetworkCheckService _networkCheckService;

        public RemoteDataAccessWithLocalFallback(LocalDataAccess fallbackAPI, RemoteDataAccess webAPI, NetworkCheckService networkCheckService)
        {
            _fallbackAPI = fallbackAPI;
            _webAPI = webAPI;
            _networkCheckService = networkCheckService;
        }

        private async Task<Result<T[]>> GetData<T>(Task<Result<T[]>> remoteTask, Task<Result<T[]>> fallbackTask)
        {
            bool hasNetwork = _networkCheckService.HasInternet();
            if (!hasNetwork)
                return await fallbackTask;

            var result = await remoteTask
                             .DefaultIfFaulted();

            if (result == null)
                result = await fallbackTask;

            return result;
        }

        public async Task<Result<Site[]>> GetSites(DateTime from) 
            => await GetData(_webAPI.GetSites(from), _fallbackAPI.GetSites(from));

        public async Task<Result<Component[]>> GetComponents(DateTime from) 
            => await GetData(_webAPI.GetComponents(from), _fallbackAPI.GetComponents(from));

        public async Task<Result<AlternateComponentName[]>> GetAlternateComponentName(DateTime from) 
            => await GetData(_webAPI.GetAlternateComponentName(from), _fallbackAPI.GetAlternateComponentName(from));

    }
}
