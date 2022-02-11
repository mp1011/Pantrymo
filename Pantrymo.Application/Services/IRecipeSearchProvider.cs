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

        public InMemoryRecipeSearchProvider(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public Task<RecipeSearchResult[]> Search(IComponent[] components, ICuisine[] cuisines, RecipeSearchArgs args)
        {
            return Task.FromResult(new RecipeSearchResult[] { });
        }
    }
}
