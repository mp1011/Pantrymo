using Pantrymo.Application.Models;
using Pantrymo.Application.Models.AppModels;

namespace Pantrymo.Application.Services
{
    public record RecipeSearchArgs(string[] Ingredients, string[] Cuisines, int From, int To);

    public interface IRecipeSearchProvider
    {
        Task<RecipeSearchResult[]> Search(Component[] components, Cuisine[] cuisines, RecipeSearchArgs args);
    }

    public class EmptyRecipeSearchProvider : IRecipeSearchProvider
    {
        public Task<RecipeSearchResult[]> Search(Component[] components, Cuisine[] cuisines, RecipeSearchArgs args)
        {
            return Task.FromResult(new RecipeSearchResult[] { });
        }
    }

    public class RecipeSearchService 
    {
        private readonly ISearchService<Component> _componentSearchService;
        private readonly ISearchService<Cuisine> _cuisineSearchService;
        private readonly IRecipeSearchProvider _recipeSearchProvider;
        private readonly IDataContext _dataContext;

        public RecipeSearchService(ISearchService<Component> componentSearchService, ISearchService<Cuisine> cuisineSearchService, IRecipeSearchProvider recipeSearchProvider, IDataContext dataContext)
        {
            _componentSearchService = componentSearchService;
            _cuisineSearchService = cuisineSearchService;
            _recipeSearchProvider = recipeSearchProvider;
            _dataContext = dataContext;
        }

        public async Task<RecipeDetail[]> Search(RecipeSearchArgs args)
        {
            var components = await _componentSearchService.Search(args.Ingredients);
            var cuisines = await _cuisineSearchService.Search(args.Cuisines);

            var results = await _recipeSearchProvider.Search(components, cuisines,args);

            return results
                .Select(GetRecipeDetail)
                .Where(p=>p!=null)
                .ToArray();
        }

        private RecipeDetail? GetRecipeDetail(RecipeSearchResult result)
        {
            var recipe = _dataContext.RecipesWithIngredients.FirstOrDefault(p => p.Id == result.RecipeId);
            if(recipe == null)
                return null;

            return new RecipeDetail(recipe.Id, recipe.Title, recipe.Url, recipe.ImageUrl);
        }
    }
}
