using Pantrymo.Domain.Models;

namespace Pantrymo.Application.Models
{
    public interface IDataContext : IBaseDataContext
    {
        IQueryable<ISite> Sites { get; }
        IQueryable<IComponent> Components { get; }
        IQueryable<IComponentDetail> ComponentsDetail { get; }
        IQueryable<ICuisine> Cuisines { get; }
        IQueryable<IAlternateComponentName> AlternateComponentNames { get; }
        IQueryable<IAuthor> Authors { get; }
        IQueryable<IComponentNegativeRelation> ComponentNegativeRelations { get; }
        IQueryable<IRecipe> Recipes { get; }
        IQueryable<IRecipeDetail> RecipesDetail { get; }
        IQueryable<IRecipeDTO> RecipesDTO { get; }
        IQueryable<IIngredientText> IngredientTexts { get; }
        IQueryable<IIngredientTextDetail> IngredientTextDetail { get; }
        IQueryable<IRecipeIngredient> RecipeIngredients { get; }
        IQueryable<IRecipeIngredientDetail> RecipeIngredientsDetail { get; }

        Task Save(params ISite[] records);
        Task Save(params IComponent[] records);
        Task Save(params IAlternateComponentName[] records);
        Task Save(params IAuthor[] records);
        Task Save(params ICuisine[] records);
        Task Save(params IComponentNegativeRelation[] records);
        Task Save(params IRecipe[] records);
        Task Save(params IRecipeDTO[] records);
        Task Save(params IRecipeIngredient[] records);
        Task Save(params IIngredientText[] records);
    }
}
