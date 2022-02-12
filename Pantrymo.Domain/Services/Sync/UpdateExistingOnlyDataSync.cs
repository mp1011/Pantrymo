using Pantrymo.Domain.Extensions;
using Pantrymo.Domain.Models;

namespace Pantrymo.Domain.Services.Sync
{
    public class UpdateExistingOnlyDataSync<T> : DataTypeSync<T>
         where T : class, IWithId, IWithLastModifiedDate
    {
        private readonly IDataAccess _dataAccess;

        public UpdateExistingOnlyDataSync(IExceptionHandler exceptionHandler, IBaseDataContext dataContext, IDataAccess dataAccess) 
            : base(exceptionHandler, dataContext)
        {
            _dataAccess = dataAccess;
        }

        protected override async Task<Result<T[]>> GetUpdatedRecordsFromServer()
        {
            var localLastModified = _dataContext
                .GetQuery<T>()
                .GetRecordUpdateTimestamps()
                .ToArray();

            var serverUpdated = await _dataAccess.GetChangedRecords<T>(localLastModified);
            return serverUpdated;
        }
    }
}
