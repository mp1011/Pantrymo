using Pantrymo.Application.Models;
using Pantrymo.Domain.Services;

namespace Pantrymo.Application.Services
{
    public interface IFullHierarchyLoader
    {
        Task<FullHierarchy[]> GetFullHierarchy();
    }

    public class RemoteFullHierarchyLoader : IFullHierarchyLoader
    {
        private readonly ILocalStorage _localStorage;
        private readonly ISettingsService _settingsService;
        private readonly HttpService _httpService;

        public RemoteFullHierarchyLoader(ILocalStorage localStorage, ISettingsService settingsService, HttpService httpService)
        {
            _localStorage = localStorage;
            _settingsService = settingsService;
            _httpService = httpService;
        }

        public async Task<FullHierarchy[]> GetFullHierarchy()
        {
            var result = await TryGetFullHierarchyRemote();

            if (result.Any())
                await _localStorage.Set(result);
            else
                result = await _localStorage.Get<FullHierarchy[]>() ?? new FullHierarchy[] { };

            return result;
        }

        private async Task<FullHierarchy[]> TryGetFullHierarchyRemote()
        {
            using var webClient = new HttpClient();
            var response = await _httpService.GetJsonArrayAsync<FullHierarchy>($"{_settingsService.Host}/api/Component/FullHierarchy");

            if (response.Success)
                return response.Data;
            else
                return new FullHierarchy[] { };
        }
    }
}
