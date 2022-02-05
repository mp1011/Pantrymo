namespace Pantrymo.Application.Models
{
    public interface IDataContext
    {
        IQueryable<ISite> Sites { get; }
        IQueryable<IComponent> Components { get; }
        IQueryable<IComponentDetail> ComponentsDetail { get; }
        IQueryable<ICuisine> Cuisines { get; }
        IQueryable<IAlternateComponentName> AlternateComponentNames { get; }
        IQueryable<IRecipe> Recipes { get; }
        IQueryable<IRecipeDetail> RecipesDetail { get; }

        Task InsertAsync(ISite[] records);
        Task InsertAsync(IComponent[] records);
        Task InsertAsync(IAlternateComponentName[] records);

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
