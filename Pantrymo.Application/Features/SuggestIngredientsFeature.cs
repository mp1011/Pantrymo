using MediatR;
using Pantrymo.Application.Services;

namespace Pantrymo.Application.Features
{
    public class SuggestIngredientsFeature
    {
        public record Query(string Text, int Limit) : IRequest<string[]>
        {
        }

        public class Handler : IRequestHandler<Query, string[]>
        {
            private readonly IngredientSuggestionService _ingredientSuggestionService;

            public Handler(IngredientSuggestionService ingredientSuggestionService)
            {
                _ingredientSuggestionService = ingredientSuggestionService;
            }

            public async Task<string[]> Handle(Query request, CancellationToken cancellationToken)
            {
                var ingredients = await _ingredientSuggestionService.SuggestIngredients(request.Text);

                return ingredients.Take(request.Limit).ToArray();
            }
        }
    }
}
