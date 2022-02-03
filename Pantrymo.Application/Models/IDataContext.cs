namespace Pantrymo.Application.Models
{
    public interface IDataContext
    {
        IQueryable<Site> Sites { get; }

        IQueryable<Component> Components { get; }
        IQueryable<Cuisine> Cuisines { get; }
        IQueryable<AlternateComponentName> AlternateComponentNames { get; }
        IQueryable<Recipe> RecipesWithIngredients { get; }

        Task InsertAsync(Site[] records);
        Task InsertAsync(Component[] records);
        Task InsertAsync(AlternateComponentName[] records);

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
