using Pantrymo.Application.Models;
using Pantrymo.Domain.Extensions;
using Pantrymo.Domain.Services;

namespace Pantrymo.Application.Services
{
    public record RecipeSearchArgs(string[] Ingredients, string[] Cuisines, int From, int To);

    public interface IRecipeSearchService
    {
        Task<IRecipe[]> Search(RecipeSearchArgs args);
    }

    public class LocalRecipeSearchService : IRecipeSearchService
    {
        private readonly ISearchService<IComponent> _componentSearchService;
        private readonly ISearchService<ICuisine> _cuisineSearchService;
        private readonly IRecipeSearchProvider _recipeSearchProvider;
        private readonly IDataContext _dataContext;

        public LocalRecipeSearchService(ISearchService<IComponent> componentSearchService, ISearchService<ICuisine> cuisineSearchService, IRecipeSearchProvider recipeSearchProvider, IDataContext dataContext)
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


    public class RemoteRecipeSearchService : IRecipeSearchService
    {
        private readonly ISettingsService _settingsService;
        private readonly HttpService _httpService;
        private readonly IRecipeSearchService _fallbackService;

        public RemoteRecipeSearchService(ISettingsService settingsService, HttpService httpService,
            LocalRecipeSearchService fallbackService)
        {
            _settingsService = settingsService;
            _httpService = httpService;
            _fallbackService = fallbackService;
        }

        public async Task<IRecipe[]> Search(RecipeSearchArgs args)
        {
            if (!_httpService.HasInternet())
                return await _fallbackService.Search(args);

            var result = await _httpService.GetJsonArrayAsync<IRecipe>(
                $@"{_settingsService.Host}/api/Recipe/Find?ingredients={args.Ingredients.ToCSV()}&traits=none&cuisines={args.Cuisines.ToCSV()}&from={args.From}&to={args.To}");

            if (result.Success)
                return result.Data;
            else
                return await _fallbackService.Search(args);
        }


    }
}
