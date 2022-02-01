using MediatR;
using Pantrymo.Application.Extensions;
using Pantrymo.Application.Models;

namespace Pantrymo.Application.Features
{
    public class GetByDateFeature
    {
        public record Query<T>(DateTime DateFrom) : IRequest<T[]>
           where T : IWithLastModifiedDate { }

        public abstract class Handler<T> : IRequestHandler<Query<T>, T[]>
        where T : IWithLastModifiedDate
        {
            private readonly IDataContext _dataContext;

            public Handler(IDataContext dataContext)
            {
                _dataContext = dataContext;
            }

            public async Task<T[]> Handle(Query<T> request, CancellationToken cancellationToken)
            {
                var result = await GetByDate(_dataContext, request.DateFrom);
                if (result.Success)
                    return result.Data;
                else
                    return new T[] { };
            }

            protected abstract Task<Result<T[]>> GetByDate(IDataContext dataContext, DateTime dateFrom);
        }

        public class GetSitesByDateHandler : Handler<Site>
        {
            public GetSitesByDateHandler(IDataContext dataContext) : base(dataContext) { }

            protected override async Task<Result<Site[]>> GetByDate(IDataContext dataContext, DateTime dateFrom)
            {
                return await dataContext.Sites.GetByDateAsync(dateFrom);
            }
        }

        public class GetComponentsByDateHandler : Handler<Component>
        {
            public GetComponentsByDateHandler(IDataContext dataContext) : base(dataContext) { }

            protected override async Task<Result<Component[]>> GetByDate(IDataContext dataContext, DateTime dateFrom)
            {
                return await dataContext.Components.GetByDateAsync(dateFrom);
            }
        }

        public class GetAlternateComponentNamesByDateHandler : Handler<AlternateComponentName>
        {
            public GetAlternateComponentNamesByDateHandler(IDataContext dataContext) : base(dataContext) { }

            protected override async Task<Result<AlternateComponentName[]>> GetByDate(IDataContext dataContext, DateTime dateFrom)
            {
                return await dataContext.AlternateComponentNames.GetByDateAsync(dateFrom);
            }
        }
    }
}
