using MediatR;
using Pantrymo.Domain.Extensions;
using Pantrymo.Domain.Models;
using Pantrymo.Domain.Services.Sync;

namespace Pantrymo.Domain.Services
{
    public interface IDataSyncService
    {
        SyncTypeStatus[] GetCurrentSyncStatus();
        void BackgroundSync();
        Task<bool> ImmediateSync();       
    }

    public class EmptyDataSyncService : IDataSyncService
    {
        public void BackgroundSync()
        {
        }

        public SyncTypeStatus[] GetCurrentSyncStatus()
        {
            return new SyncTypeStatus[] { };
        }

        public Task<bool> ImmediateSync()
        {
            return Task.FromResult(true);
        }
    }

    public abstract class BaseDataSyncService : IDataSyncService
    {

        private readonly TimeSpan _localDataExpiration = TimeSpan.FromMinutes(15);
        private readonly DataTypeSync[] _dataTypeSyncs;

        protected readonly IExceptionHandler _exceptionHandler;
        protected readonly IDataAccess _dataAccess;
        protected readonly IBaseDataContext _dataContext;
        protected readonly IMediator _mediator;

        protected BaseDataSyncService(IExceptionHandler exceptionHandler, IDataAccess dataAccess, IBaseDataContext dataContext, IMediator mediator) 
        {
            _dataContext = dataContext;
            _mediator = mediator;
            _exceptionHandler = exceptionHandler;
            _dataAccess = dataAccess;
            _dataTypeSyncs = SetupDataSync();
        }

        public void BackgroundSync()
        {
            Task.Run(() => StartBackgroundSync());
        }

        public SyncTypeStatus[] GetCurrentSyncStatus()
        {
            return _dataTypeSyncs
                .Select(s => new SyncTypeStatus(s.ModelType, s.SyncStatus))
                .ToArray();
        }

        public async Task<bool> ImmediateSync()
        {
            var synced = await TrySync().HandleError(_exceptionHandler);
            return synced;
        }

        protected abstract DataTypeSync[] SetupDataSync();

        private async Task StartBackgroundSync()
        {
            while(true)
            {
                await TrySync().DebugLogError();
                await Task.Delay(TimeSpan.FromSeconds(30));
            }
        }

        private async Task<bool> TrySync()
        {
            bool anyFailed = false;
            bool anySucceeded = false;

            foreach(var sync in _dataTypeSyncs)
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
