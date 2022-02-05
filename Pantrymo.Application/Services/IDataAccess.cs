using Pantrymo.Application.Models;
using Pantrymo.Domain.Extensions;
using Pantrymo.Domain.Models;
using Pantrymo.Domain.Services;

namespace Pantrymo.Application.Services
{
    public interface IDataAccess
    {
        Task<Result<ISite[]>> GetSites(DateTime from);
        Task<Result<IComponent[]>> GetComponents(DateTime from);
        Task<Result<IAlternateComponentName[]>> GetAlternateComponentName(DateTime from);
    }

    public class LocalDataAccess : IDataAccess
    {
        private readonly IDataContext _dataContext;

        public LocalDataAccess(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<Result<ISite[]>> GetSites(DateTime from) 
            => await _dataContext.Sites.GetByDateAsync(from);
                    
        public async Task<Result<IComponent[]>> GetComponents(DateTime from) 
            => await _dataContext.Components.GetByDateAsync(from);

        public async Task<Result<IAlternateComponentName[]>> GetAlternateComponentName(DateTime from) 
            => await _dataContext.AlternateComponentNames.GetByDateAsync(from);
    }

    public class RemoteDataAccess : IDataAccess
    {
        private readonly ISettingsService _settingsService;
        private readonly HttpService _httpService;

        public RemoteDataAccess(ISettingsService settingsService, HttpService httpService)
        {
            _settingsService = settingsService;
            _httpService = httpService;
        }

        private async Task<Result<T[]>> GetRecords<T>(DateTime from)
        {
            var modelName = typeof(T).Name;
            if (modelName.StartsWith("I"))
                modelName = modelName.Substring(1);

            return await _httpService.GetJsonArrayAsync<T>($"{_settingsService.Host}/api/{modelName}/GetByDate/{from.ToUrlDateString()}");
        }

        public async Task<Result<ISite[]>> GetSites(DateTime from) => await GetRecords<ISite>(from);

        public async Task<Result<IComponent[]>> GetComponents(DateTime from) => await GetRecords<IComponent>(from);

        public async Task<Result<IAlternateComponentName[]>> GetAlternateComponentName(DateTime from) => await GetRecords<IAlternateComponentName>(from);
       
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

        public async Task<Result<ISite[]>> GetSites(DateTime from) 
            => await GetData(_webAPI.GetSites(from), _fallbackAPI.GetSites(from));

        public async Task<Result<IComponent[]>> GetComponents(DateTime from) 
            => await GetData(_webAPI.GetComponents(from), _fallbackAPI.GetComponents(from));

        public async Task<Result<IAlternateComponentName[]>> GetAlternateComponentName(DateTime from) 
            => await GetData(_webAPI.GetAlternateComponentName(from), _fallbackAPI.GetAlternateComponentName(from));

    }
}
