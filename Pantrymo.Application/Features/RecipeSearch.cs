using MediatR;
using Pantrymo.Application.Models.AppModels;
using Pantrymo.Application.Services;

namespace Pantrymo.Application.Features
{
    public class RecipeSearchFeature
    {
        public record Query(string[] Ingredients, string[] Cuisines, int From, int To) : IRequest<RecipeDetail[]> { }

        public class Handler : IRequestHandler<Query, RecipeDetail[]>
        {
            private readonly RecipeSearchService _recipeSearchService;

            public Handler(RecipeSearchService recipeSearchService)
            {
                _recipeSearchService = recipeSearchService;
            }

            public async Task<RecipeDetail[]> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _recipeSearchService.Search(new RecipeSearchArgs(request.Ingredients, request.Cuisines, request.From, request.To));
            }
        }
    }
}
