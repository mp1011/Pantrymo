using Pantrymo.Domain.Extensions;
using Pantrymo.Domain.Models;

namespace Pantrymo.Domain.Services
{
    public abstract class DataSyncService
    {
        private readonly TimeSpan _localDataExpiration = TimeSpan.FromMinutes(15);
        private readonly IExceptionHandler _exceptionHandler;

        public DataSyncService(IExceptionHandler exceptionHandler)
        {
            _exceptionHandler = exceptionHandler;
        }

        protected abstract class DataSync
        {
            public abstract Task<bool> TrySync();

            public DateTime LastSuccessfulSync { get; protected set; } = DateTime.MinValue;
        }

        protected class DataSync<T> : DataSync
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
                {
                    var success = await InsertRecords(newFromServer.Data)
                                        .CheckSuccess();

                    if (!success)
                        return false;

                }

                LastSuccessfulSync = DateTime.Now;
                return true;
            }
        }

        public void BackgroundSync()
        {
            var dataSync = SetupDataSync();
            Task.Run(() => StartBackgroundSync(dataSync));
        }

        public async Task<bool> ImmediateSync()
        {
            var dataSync = SetupDataSync();
            var synced = await TrySync(dataSync).HandleError(_exceptionHandler);
            return synced;
        }

        protected abstract DataSync[] SetupDataSync();

        private async Task StartBackgroundSync(DataSync[] dataSync)
        {
            while(true)
            {
                var synced = await TrySync(dataSync).DebugLogError();
                if (synced)
                    await Task.Delay(TimeSpan.FromMinutes(15)); 
                else
                    await Task.Delay(TimeSpan.FromSeconds(30));
            }
        }

        private async Task<bool> TrySync(DataSync[] dataSync)
        {
            bool anyFailed = false;
            bool anySucceeded = false;

            foreach(var sync in dataSync)
            {
                if(sync.LastSuccessfulSync.TimeSince() > _localDataExpiration)
                {
                    bool success = await sync.TrySync().HandleError(_exceptionHandler);
                    if (success)
                        anySucceeded = true;
                    if (!success)
                        anyFailed = true;
                }
            }

            if (anySucceeded)
            {
                var savedLocally = await CommitLocalChanges().CheckSuccess(_exceptionHandler);
                if (!savedLocally)
                    return false;
            }

            return !anyFailed;
        }

        protected abstract Task CommitLocalChanges();
    }
}
