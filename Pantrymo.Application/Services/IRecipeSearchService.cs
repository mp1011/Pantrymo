using Pantrymo.Application.Models;
using Pantrymo.Application.Models.AppModels;

namespace Pantrymo.Application.Services
{
    public record RecipeSearchArgs(string[] Ingredients, string[] Cuisines, int From, int To);

    public class RecipeSearchService 
    {
        private readonly ISearchService<IComponent> _componentSearchService;
        private readonly ISearchService<ICuisine> _cuisineSearchService;
        private readonly IRecipeSearchProvider _recipeSearchProvider;
        private readonly IDataContext _dataContext;

        public RecipeSearchService(ISearchService<IComponent> componentSearchService, ISearchService<ICuisine> cuisineSearchService, IRecipeSearchProvider recipeSearchProvider, IDataContext dataContext)
        {
            _componentSearchService = componentSearchService;
            _cuisineSearchService = cuisineSearchService;
            _recipeSearchProvider = recipeSearchProvider;
            _dataContext = dataContext;
        }

        public async Task<IRecipe[]> Search(RecipeSearchArgs args)
        {
            var components = await _componentSearchService.Search(args.Ingredients);
            var cuisines = await _cuisineSearchService.Search(args.Cuisines);

            var results = await _recipeSearchProvider.Search(components, cuisines,args);

            return results
                .Select(GetRecipe)
                .Where(p=>p!=null)
                .ToArray();
        }

        private IRecipe? GetRecipe(RecipeSearchResult result) 
            => _dataContext.Recipes.FirstOrDefault(p => p.Id == result.RecipeId);
    }
}
