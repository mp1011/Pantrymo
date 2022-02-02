using Pantrymo.Domain.Extensions;
using Pantrymo.Domain.Models;

namespace Pantrymo.Domain.Services
{
    public abstract class DataSyncService
    {
        private readonly TimeSpan _localDataExpiration = TimeSpan.FromMinutes(15);

        private DataSync[] _dataSync;

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
                    await InsertRecords(newFromServer.Data);

                LastSuccessfulSync = DateTime.Now;
                return true;
            }
        }

        public void BackgroundSync()
        {
            _dataSync = SetupDataSync();
            Task.Run(() => StartBackgroundSync());
        }

        protected abstract DataSync[] SetupDataSync();

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
                await CommitLocalChanges();

            return !anyFailed;
        }

        protected abstract Task CommitLocalChanges();
    }
}
