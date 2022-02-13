using Pantrymo.Application.Models;
using Pantrymo.Application.Models.AppModels;

namespace Pantrymo.Application.Services
{
    public interface IRecipeSearchProvider
    {
        Task<RecipeSearchResult[]> Search(IComponent[] components, ICuisine[] cuisines, RecipeSearchArgs args);
    }

    public class InMemoryRecipeSearchProvider : IRecipeSearchProvider
    {
        private readonly IDataContext _dataContext;
        private readonly CategoryService _categoryService;

        public InMemoryRecipeSearchProvider(IDataContext dataContext, CategoryService categoryTreeBuilder)
        {
            _dataContext = dataContext;
            _categoryService = categoryTreeBuilder;
        }

        public async Task<RecipeSearchResult[]> Search(IComponent[] components, ICuisine[] cuisines, RecipeSearchArgs args)
        {
            var categories = await _categoryService.GetCategoriesWithDescendants(components);
           
            int[] componentIds = _categoryService
                .GetComponents(categories)
                .Select(x => x.Id)
                .ToArray();

            var recipes = _dataContext.RecipesDetail
                .Where(r => r.IngredientTexts
                    .SelectMany(it => it.RecipeIngredients)
                    .Any(ri => componentIds.Contains(ri.ComponentId)))
                .ToArray();
            
            //todo, ranking and such
            var rankedRecipes = recipes 
                .Select(p => new RecipeSearchResult { RecipeId = p.Id, RecipeScore = 1.0M })
                .ToArray();


            return rankedRecipes.OrderByDescending(r => r.RecipeScore)
                .Skip(args.From)
                .Take(args.To)
                .ToArray();
        }
    }
}
