using Pantrymo.Application.Extensions;
using Pantrymo.Application.Models;

namespace Pantrymo.Application.Services
{
    public interface IFullHierarchyLoader
    {
        Task<FullHierarchy[]> GetFullHierarchy();
    }

    public class RemoteFullHierarchyLoader : IFullHierarchyLoader
    {
        private readonly ILocalStorage _localStorage;

        public RemoteFullHierarchyLoader(ILocalStorage localStorage)
        {
            _localStorage = localStorage;
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
            var response = await webClient
                    .GetJsonArrayAsync<FullHierarchy>($"https://localhost:7188/api/Component/FullHierarchy");

            if (response.Success)
                return response.Data;
            else
                return new FullHierarchy[] { };
        }
    }
}
