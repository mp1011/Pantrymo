using MediatR;
using Pantrymo.Application.Models;
using Pantrymo.Application.Services;

namespace Pantrymo.Application.Features
{
    public class RecipeSearchFeature
    {
        public record Query(string[] Ingredients, string[] Cuisines, int From, int To) : IRequest<IRecipe[]> { }

        public class Handler : IRequestHandler<Query, IRecipe[]>
        {
            private readonly RecipeSearchService _recipeSearchService;

            public Handler(RecipeSearchService recipeSearchService)
            {
                _recipeSearchService = recipeSearchService;
            }

            public async Task<IRecipe[]> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _recipeSearchService.Search(new RecipeSearchArgs(request.Ingredients, request.Cuisines, request.From, request.To));
            }
        }
    }
}
