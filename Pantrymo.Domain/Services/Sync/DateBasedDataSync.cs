using Pantrymo.Domain.Extensions;
using Pantrymo.Domain.Models;

namespace Pantrymo.Domain.Services.Sync
{
    /// <summary>
    /// Syncs all records from the server who's LastModifiedDate is greater than the latest date on the client
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DateBasedDataSync<T> : DataTypeSync<T>
       where T : class, IWithId, IWithLastModifiedDate
    {
        private readonly IDataAccess _dataAccess;

        public DateBasedDataSync(IExceptionHandler exceptionHandler, IBaseDataContext dataContext, IDataAccess dataAccess)
            : base(exceptionHandler, dataContext)
        {
            _dataAccess = dataAccess.CheckNotNull();
        }

        protected override async Task<Result<T[]>> GetUpdatedRecordsFromServer()
        {
            var localLastModified = _dataContext.GetQuery<T>().GetLatestModifiedDate();
            return await _dataAccess.GetRecordsByDate<T>(localLastModified);
        }
    }
}
