using Pantrymo.Domain.Extensions;
using Pantrymo.Domain.Models;

namespace Pantrymo.Domain.Services.Sync
{
    /// <summary>
    /// Syncs all records from the server who's LastModifiedDate is greater than the latest date on the client
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DateBasedDataSync<T> : DataTypeSync<T>
       where T : IWithLastModifiedDate
    {
        private Func<DateTime, Task<Result<T[]>>> _getNewFromServer;

        public DateBasedDataSync(IExceptionHandler exceptionHandler, IQueryable<T> localQuery, Func<DateTime, Task<Result<T[]>>> getNewFromServer, Func<T[], Task> insertRecords)
            : base(exceptionHandler, localQuery, insertRecords)
        {
            _getNewFromServer = getNewFromServer;
        }

        protected override async Task<Result<T[]>> GetUpdatedRecordsFromServer()
        {
            var localLastModified = LocalQuery.GetLatestModifiedDate();
            return await _getNewFromServer(localLastModified);
        }
    }
}
