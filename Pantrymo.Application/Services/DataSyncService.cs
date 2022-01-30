using Pantrymo.Application.Extensions;
using Pantrymo.Application.Models;

namespace Pantrymo.Application.Services
{
    public class DataSyncService
    {
        private readonly TimeSpan _localDataExpiration = TimeSpan.FromMinutes(15);

        private readonly RemoteDataAccess _dataAccess;
        private readonly IDataContext _dataContext;
        private readonly DataSync[] _dataSync;

        private abstract class DataSync
        {
            public abstract Task<bool> TrySync();

            public DateTime LastSuccessfulSync { get; protected set; } = DateTime.MinValue;
        }

        private class DataSync<T> : DataSync
            where T:IWithLastModifiedDate
        {
            public IQueryable<T> LocalQuery { get; }
            public Func<DateTime, Task<Result<T[]>>> GetNewFromServer;
            public Func<T[], Task> InsertRecords;

            public DataSync(IQueryable<T> localQuery, Func<DateTime, Task<Result<T[]>>> getNewFromServer, Func<T[], Task> insertRecords)
            {
                LocalQuery = localQuery;
                GetNewFromServer = getNewFromServer;
                InsertRecords = insertRecords;
            }

            public override async Task<bool> TrySync()
            {
                var localLastModified = LocalQuery.GetLatestModifiedDate();
                var newFromServer = await GetNewFromServer(localLastModified);
                if (newFromServer.Failure)
                    return false;

                if (newFromServer.Data.Any())
                    await InsertRecords(newFromServer.Data);

                LastSuccessfulSync = DateTime.Now;
                return true;
            }
        }

        public DataSyncService(RemoteDataAccess dataAccess, IDataContext dataContext)
        {
            _dataAccess = dataAccess;
            _dataContext = dataContext;
            _dataSync = new DataSync[]
            {
                new DataSync<Site>(_dataContext.Sites, dateFrom => _dataAccess.GetSites(dateFrom), records => _dataContext.InsertAsync(records)),
                new DataSync<Component>(_dataContext.Components, dateFrom => _dataAccess.GetComponents(dateFrom), records => _dataContext.InsertAsync(records)),
                new DataSync<AlternateComponentName>(_dataContext.AlternateComponentNames, dateFrom => _dataAccess.GetAlternateComponentName(dateFrom), records => _dataContext.InsertAsync(records)),
            };           
        }

        public void BackgroundSync()
        {
            Task.Run(() => StartBackgroundSync());
        }

        private async Task StartBackgroundSync()
        {
            while(true)
            {
                var synced = await TrySync().DebugLogError();
                if (synced)
                    await Task.Delay(TimeSpan.FromMinutes(15)); 
                else
                    await Task.Delay(TimeSpan.FromSeconds(30));
            }
        }

        private async Task<bool> TrySync()
        {
            bool anyFailed = false;
            bool anySucceeded = false;

            foreach(var sync in _dataSync)
            {
                if(sync.LastSuccessfulSync.TimeSince() > _localDataExpiration)
                {
                    bool success = await sync.TrySync().DebugLogError();
                    if (success)
                        anySucceeded = true;
                    if (!success)
                        anyFailed = true;
                }
            }

            if (anySucceeded)
                await _dataContext.SaveChangesAsync();

            return !anyFailed;
        }
    }
}
