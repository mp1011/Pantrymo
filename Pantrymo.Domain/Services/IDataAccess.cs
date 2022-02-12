using Pantrymo.Domain.Extensions;
using Pantrymo.Domain.Models;

namespace Pantrymo.Domain.Services
{
    //reconsider this class, maybe its only to retrieve data for a sync
    public interface IDataAccess
    {
        Task<Result<T[]>> GetRecordsByDate<T>(DateTime from) where T:IWithLastModifiedDate;

        Task<Result<T[]>> GetChangedRecords<T>(RecordUpdateTimestamp[] localTimestamps) where T: IWithLastModifiedDate,IWithId;
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

        public Task<Result<T[]>> GetChangedRecords<T>(RecordUpdateTimestamp[] queryTimestamps)
            where T: IWithLastModifiedDate,IWithId
        {
            var ids = queryTimestamps
                .Select(x => x.Id)
                .ToArray();

            var idToDate = queryTimestamps.ToDictionary(k => k.Id, v => v.LastModified);

            var localRecords = _dataContext
                .GetQuery<T>()
                .Where(p => ids.Contains(p.Id))
                .ToArray();

            var result = localRecords
                .Where(p => p.LastModified > idToDate[p.Id])
                .ToArray();

            return Task.FromResult(new Result<T[]>(result));
        }
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
            var modelName = typeof(T).GetModelName();
            return await _httpService.GetJsonArrayAsync<T>($"{_settingsService.Host}/api/{modelName}/GetByDate/{from.ToUrlDateString()}");
        }

        public async Task<Result<T[]>> GetChangedRecords<T>(RecordUpdateTimestamp[] localTimestamps) where T : IWithLastModifiedDate, IWithId
        {
            var modelName = typeof(T).GetModelName();
            return await _httpService.GetJsonArrayAsync<T>($"{_settingsService.Host}/api/{modelName}/GetUpdatedRecords", localTimestamps);
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

        public async Task<Result<T[]>> GetChangedRecords<T>(RecordUpdateTimestamp[] localTimestamps) where T : IWithLastModifiedDate, IWithId
            => await _webAPI.GetChangedRecords<T>(localTimestamps);
    }
}
