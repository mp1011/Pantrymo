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
                var result = await _dataContext.GetQuery<T>().GetByDateAsync(request.DateFrom);
                if (result.Success)
                    return result.Data;
                else
                    return new T[] { };
            }
        }

        public class GetSitesByDateHandler : Handler<ISite> { public GetSitesByDateHandler(IDataContext dataContext) : base(dataContext) { }  }
        public class GetAuthorsByDateHandler : Handler<IAuthor> { public GetAuthorsByDateHandler(IDataContext dataContext) : base(dataContext) { } }
        public class GetComponentsByDateHandler : Handler<IComponent> { public GetComponentsByDateHandler(IDataContext dataContext) : base(dataContext) { } }
        public class GetAlternateComponentNamesByDateHandler : Handler<IAlternateComponentName> { public GetAlternateComponentNamesByDateHandler(IDataContext dataContext) : base(dataContext) { } }
        public class GetComponentNegativeRelationByDateHandler : Handler<IComponentNegativeRelation> { public GetComponentNegativeRelationByDateHandler(IDataContext dataContext) : base(dataContext) { } }
        public class GetCuisineByDateHandler : Handler<ICuisine> { public GetCuisineByDateHandler(IDataContext dataContext) : base(dataContext) { } }

    }
}
