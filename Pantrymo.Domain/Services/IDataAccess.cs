using Pantrymo.Domain.Extensions;
using Pantrymo.Domain.Models;

namespace Pantrymo.Domain.Services
{
    public interface IDataAccess
    {
        Task<Result<T[]>> GetRecordsByDate<T>(DateTime from) where T:IWithLastModifiedDate;
    }

    public class LocalDataAccess : IDataAccess
    {
        private readonly IBaseDataContext _dataContext;

        public LocalDataAccess(IBaseDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<Result<T[]>> GetRecordsByDate<T>(DateTime from) where T : IWithLastModifiedDate
            => await _dataContext.GetQuery<T>().GetByDateAsync(from);
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

        public async Task<Result<T[]>> GetRecordsByDate<T>(DateTime from) where T : IWithLastModifiedDate
        {
            var modelName = typeof(T).Name;
            if (modelName.StartsWith("I"))
                modelName = modelName.Substring(1);

            return await _httpService.GetJsonArrayAsync<T>($"{_settingsService.Host}/api/{modelName}/GetByDate/{from.ToUrlDateString()}");
        }
    }

    public class RemoteDataAccessWithLocalFallback : IDataAccess
    {
        private readonly LocalDataAccess _fallbackAPI;
        private readonly RemoteDataAccess _webAPI;
        private readonly HttpService _httpService;

        public RemoteDataAccessWithLocalFallback(LocalDataAccess fallbackAPI, RemoteDataAccess webAPI, HttpService httpService)
        {
            _fallbackAPI = fallbackAPI;
            _webAPI = webAPI;
            _httpService = httpService;
        }

        private async Task<Result<T[]>> GetData<T>(Task<Result<T[]>> remoteTask, Task<Result<T[]>> fallbackTask)
        {
            bool hasNetwork = _httpService.HasInternet();
            if (!hasNetwork)
                return await fallbackTask;

            var result = await remoteTask
                             .DefaultIfFaulted();

            if (result == null)
                result = await fallbackTask;

            return result;
        }

        public async Task<Result<T[]>> GetRecordsByDate<T>(DateTime from) where T : IWithLastModifiedDate
            => await GetData(_webAPI.GetRecordsByDate<T>(from), _fallbackAPI.GetRecordsByDate<T>(from));
    }
}
