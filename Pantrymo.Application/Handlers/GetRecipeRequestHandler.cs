using MediatR;
using Pantrymo.Application.Models;
using Pantrymo.Application.Queries;

namespace Pantrymo.Application.Handlers
{
    //public class GetRecipeRequestHandler : IRequestHandler<GetRecipeQuery, IRecipe>
    //{
    //    private readonly IDataContext _dataContext;

    //    public GetRecipeRequestHandler(IDataContext dataContext)
    //    {
    //        _dataContext = dataContext;
    //    }

    //    public Task<IRecipe> Handle(GetRecipeQuery request, CancellationToken cancellationToken)
    //    {
    //        throw new NotImplementedException();
    //        //var result = _dataContext.Recipes.FirstOrDefault(p => p.Id == request.Id);
    //        //var tk= Task.FromResult(result);
    //        //return tk;
    //    }
    //}
}
