using Pantrymo.Domain.Extensions;
using Pantrymo.Domain.Services.Sync;

namespace Pantrymo.Domain.Services
{
    public abstract class DataSyncService
    {

        private readonly TimeSpan _localDataExpiration = TimeSpan.FromMinutes(15);
        protected readonly IExceptionHandler _exceptionHandler;

        protected DataSyncService(IExceptionHandler exceptionHandler)
        {
            _exceptionHandler = exceptionHandler;
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

        protected abstract DataTypeSync[] SetupDataSync();

        private async Task StartBackgroundSync(DataTypeSync[] dataSync)
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

        private async Task<bool> TrySync(DataTypeSync[] dataSync)
        {
            bool anyFailed = false;
            bool anySucceeded = false;

            foreach(var sync in dataSync)
            {
                if(sync.LastSuccessfulSync.TimeSince() > _localDataExpiration)
                {
                    bool success = await sync.TrySync().HandleError(_exceptionHandler);
                    OnSyncStatusChanged(sync);

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

        protected abstract void OnSyncStatusChanged(DataTypeSync dataTypeSync);

        protected abstract Task CommitLocalChanges();
    }
}
