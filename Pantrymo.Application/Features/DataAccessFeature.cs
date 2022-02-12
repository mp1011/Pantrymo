using Pantrymo.Application.Models;
using Pantrymo.Domain.Services;
using static Pantrymo.Domain.Features.DataAccessFeature;

namespace Pantrymo.Application.Features
{
    public class PantrymoDataAccessFeature
    {
        public class GetSitesByDateHandler : QueryByDateHandler<ISite> { public GetSitesByDateHandler(IDataContext dataContext) : base(dataContext) { }  }
        public class GetAuthorsByDateHandler : QueryByDateHandler<IAuthor> { public GetAuthorsByDateHandler(IDataContext dataContext) : base(dataContext) { } }
        public class GetComponentsByDateHandler : QueryByDateHandler<IComponent> { public GetComponentsByDateHandler(IDataContext dataContext) : base(dataContext) { } }
        public class GetAlternateComponentNamesByDateHandler : QueryByDateHandler<IAlternateComponentName> { public GetAlternateComponentNamesByDateHandler(IDataContext dataContext) : base(dataContext) { } }
        public class GetComponentNegativeRelationByDateHandler : QueryByDateHandler<IComponentNegativeRelation> { public GetComponentNegativeRelationByDateHandler(IDataContext dataContext) : base(dataContext) { } }
        public class GetCuisineByDateHandler : QueryByDateHandler<ICuisine> { public GetCuisineByDateHandler(IDataContext dataContext) : base(dataContext) { } }
        public class GetChangedRecipesHandler : QueryChangedRecordsHandler<IRecipe> { public GetChangedRecipesHandler(IDataAccess dataAccess) : base(dataAccess) { } }
    }
}
