using MediatR;
using Pantrymo.Application.Models;
using Pantrymo.Application.Models.AppModels;
using Pantrymo.Application.Services;

namespace Pantrymo.Application.Features
{
    public class RecipeSearchFeature
    {
        public record Query(string[] Ingredients, string[] Cuisines, int From, int To) : IRequest<IRecipeDTO[]> { }

        public class Handler : IRequestHandler<Query, IRecipeDTO[]>
        {
            private readonly IRecipeSearchService _recipeSearchService;

            public Handler(IRecipeSearchService recipeSearchService)
            {
                _recipeSearchService = recipeSearchService;
            }

            public async Task<IRecipeDTO[]> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _recipeSearchService.Search(new RecipeSearchArgs(request.Ingredients, request.Cuisines, request.From, request.To));
            }
        }
    }
}
