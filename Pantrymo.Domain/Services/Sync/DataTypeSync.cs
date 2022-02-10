using Pantrymo.Domain.Extensions;
using Pantrymo.Domain.Models;

namespace Pantrymo.Domain.Services.Sync
{
    /// <summary>
    /// Specifies logic for syncing a particular type of data between client and server
    /// </summary>
    public abstract class DataTypeSync
    {
        public abstract Task<bool> TrySync();

        public abstract Type ModelType { get; }

        public SyncStatus SyncStatus { get; protected set; } = new SyncStatus(DateTime.MinValue, DateTime.MinValue, null, 0);
        public DateTime LastSuccessfulSync => SyncStatus.LastSuccessfulSync;
    }

    /// <summary>
    /// Specifies logic for syncing a particular type of data between client and server
    /// </summary>
    public abstract class DataTypeSync<T> : DataTypeSync
        where T:class, IWithId, IWithLastModifiedDate
    {
        protected readonly IExceptionHandler _exceptionHandler;
        protected readonly IBaseDataContext _dataContext;
        
        public override Type ModelType => typeof(T);

        public DataTypeSync(IExceptionHandler exceptionHandler, IBaseDataContext dataContext)
        {
            _exceptionHandler = exceptionHandler;
            _dataContext = dataContext;
        }

        protected abstract Task<Result<T[]>> GetUpdatedRecordsFromServer();

        public override async Task<bool> TrySync()
        {
            var updatedFromServer = await GetUpdatedRecordsFromServer();
            if (updatedFromServer.Failure)
            {
                SyncStatus = SyncStatus.UpdateFailure(updatedFromServer.Error);
                return false;
            }


            if (updatedFromServer.Data != null && updatedFromServer.Data.Any())
            {
                var insertResult = await _dataContext.Save<T>(updatedFromServer.Data)
                                        .AsResult()
                                        .HandleError(_exceptionHandler);

                if (insertResult.Failure)
                {
                    SyncStatus = SyncStatus.UpdateFailure(insertResult.Error);
                    return false;
                }

            }

            SyncStatus = SyncStatus.UpdateSuccess(updatedFromServer.Data.Length);
            return true;
        }
    }
}
