using MediatR;
using Pantrymo.Application.Models;
using Pantrymo.Domain.Extensions;
using Pantrymo.Domain.Models;

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

        public class GetSitesByDateHandler : Handler<ISite>
        {
            public GetSitesByDateHandler(IDataContext dataContext) : base(dataContext) { }

            protected override async Task<Result<ISite[]>> GetByDate(IDataContext dataContext, DateTime dateFrom)
            {
                return await dataContext.Sites.GetByDateAsync(dateFrom);
            }
        }

        public class GetComponentsByDateHandler : Handler<IComponent>
        {
            public GetComponentsByDateHandler(IDataContext dataContext) : base(dataContext) { }

            protected override async Task<Result<IComponent[]>> GetByDate(IDataContext dataContext, DateTime dateFrom)
            {
                return await dataContext.Components.GetByDateAsync(dateFrom);
            }
        }

        public class GetAlternateComponentNamesByDateHandler : Handler<IAlternateComponentName>
        {
            public GetAlternateComponentNamesByDateHandler(IDataContext dataContext) : base(dataContext) { }

            protected override async Task<Result<IAlternateComponentName[]>> GetByDate(IDataContext dataContext, DateTime dateFrom)
            {
                return await dataContext.AlternateComponentNames.GetByDateAsync(dateFrom);
            }
        }
    }
}
