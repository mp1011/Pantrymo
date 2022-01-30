using MediatR;
using Pantrymo.Application.Extensions;
using Pantrymo.Application.Models;
using Pantrymo.Application.Queries;

namespace Pantrymo.Application.Handlers
{
    public abstract class GetByDateHandler<T> : IRequestHandler<GetByDateQuery<T>, T[]>
        where T : IWithLastModifiedDate
    {
        private readonly IDataContext _dataContext;

        public GetByDateHandler(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<T[]> Handle(GetByDateQuery<T> request, CancellationToken cancellationToken)
        {
            var result = await GetByDate(_dataContext, request.DateFrom);
            if (result.Success)
                return result.Data;
            else
                return new T[] { };
        }
       
        protected abstract Task<Result<T[]>> GetByDate(IDataContext dataContext, DateTime dateFrom);
    }

    public class GetSitesByDateHandler : GetByDateHandler<Site>
    {
        public GetSitesByDateHandler(IDataContext dataContext) : base(dataContext) { }

        protected override async Task<Result<Site[]>> GetByDate(IDataContext dataContext, DateTime dateFrom)
        {
            return await dataContext.Sites.GetByDateAsync(dateFrom);
        }
    }

    public class GetComponentsByDateHandler : GetByDateHandler<Component>
    {
        public GetComponentsByDateHandler(IDataContext dataContext) : base(dataContext) { }

        protected override async Task<Result<Component[]>> GetByDate(IDataContext dataContext, DateTime dateFrom)
        {
            return await dataContext.Components.GetByDateAsync(dateFrom);
        }
    }

    public class GetAlternateComponentNamesByDateHandler : GetByDateHandler<AlternateComponentName>
    {
        public GetAlternateComponentNamesByDateHandler(IDataContext dataContext) : base(dataContext) { }

        protected override async Task<Result<AlternateComponentName[]>> GetByDate(IDataContext dataContext, DateTime dateFrom)
        {
            return await dataContext.AlternateComponentNames.GetByDateAsync(dateFrom);
        }
    }
}
