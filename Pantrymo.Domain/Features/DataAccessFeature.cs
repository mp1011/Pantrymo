#nullable disable
using MediatR;
using Pantrymo.Domain.Extensions;
using Pantrymo.Domain.Models;
using Pantrymo.Domain.Services;

namespace Pantrymo.Domain.Features
{
    public class DataAccessFeature
    {
        public record ByDateQuery<T>(DateTime DateFrom) : IRequest<T[]>
            where T : IWithLastModifiedDate
        { }

        public record ChangedRecordsQuery<T>(RecordUpdateTimestamp[] LocalTimestamps) : IRequest<T[]>
            where T : IWithLastModifiedDate, IWithId
        { }

        public abstract class QueryByDateHandler<T> : IRequestHandler<ByDateQuery<T>, T[]>
            where T : IWithLastModifiedDate
        {
            private readonly IBaseDataContext _dataContext;

            public QueryByDateHandler(IBaseDataContext dataContext)
            {
                _dataContext = dataContext;
            }

            public async Task<T[]> Handle(ByDateQuery<T> request, CancellationToken cancellationToken)
            {
                var result = await _dataContext.GetQuery<T>().GetByDateAsync(request.DateFrom);
                if (result.Success)
                    return result.Data;
                else
                    return new T[] { };
            }
        }


        public abstract class QueryChangedRecordsHandler<T> : IRequestHandler<ChangedRecordsQuery<T>, T[]>
            where T : IWithLastModifiedDate, IWithId
        {
            private readonly IDataAccess _dataAccess;

            public QueryChangedRecordsHandler(IDataAccess dataAccess)
            {
                _dataAccess = dataAccess;
            }

            public async Task<T[]> Handle(ChangedRecordsQuery<T> request, CancellationToken cancellationToken)
            {
                var result = await _dataAccess.GetChangedRecords<T>(request.LocalTimestamps);
                if (result.Success)
                    return result.Data;
                else
                    return new T[] { };
            }
        }
    }
}
