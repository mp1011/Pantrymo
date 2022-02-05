#nullable disable
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pantrymo.Application.Models;
using Pantrymo.Domain.Models;
using Pantrymo.Domain.Services;

namespace Pantrymo.SqlInfrastructure
{
    public abstract class BaseDbContext : DbContext, IDataContext
    {
        protected readonly ISettingsService _settingsService;

        public BaseDbContext(ISettingsService settingsService, DbContextOptions options)
          : base(options)
        {
            _settingsService = settingsService;
        }

        #region IQueryables
        IQueryable<ISite> IDataContext.Sites => Sites;
        IQueryable<IComponent> IDataContext.Components => Components;
        IQueryable<IComponentDetail> IDataContext.ComponentsDetail => Components
                                                                        .Include(c => c.AlternateComponentNames)
                                                                        .Include(c => c.ComponentNegativeRelationComponents)
                                                                            .ThenInclude(c => c.NegativeComponent);

        IQueryable<IAlternateComponentName> IDataContext.AlternateComponentNames => AlternateComponentNames;

        IQueryable<IRecipe> IDataContext.Recipes => Recipes;

        IQueryable<IRecipeDetail> IDataContext.RecipesDetail => Recipes
                                                                .Include(r => r.Site)
                                                                .Include(r => r.IngredientTexts)
                                                                    .ThenInclude(t => t.RecipeIngredients)
                                                                    .ThenInclude(t => t.Component);
        IQueryable<ICuisine> IDataContext.Cuisines => Cuisines;
        #endregion

        public virtual DbSet<FullHierarchy> FullHierarchy { get; set; }
        public virtual DbSet<RecipeSearchResult> RecipeSearchResults { get; set; }

        public async Task InsertAsync(ISite[] records) => await Sites.AddRangeAsync(records.OfType<Site>());
        public async Task InsertAsync(IComponent[] records) => await Components.AddRangeAsync(records.OfType<Component>());
        public async Task InsertAsync(IAlternateComponentName[] records) => await AlternateComponentNames.AddRangeAsync(records.OfType<AlternateComponentName>());

        #region DB Sets
        public virtual DbSet<AlternateComponentName> AlternateComponentNames { get; set; }
        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<Component> Components { get; set; }
        public virtual DbSet<ComponentNegativeRelation> ComponentNegativeRelations { get; set; }
        public virtual DbSet<Cuisine> Cuisines { get; set; }
        public virtual DbSet<IngredientText> IngredientTexts { get; set; }
        public virtual DbSet<Recipe> Recipes { get; set; }
        public virtual DbSet<RecipeIngredient> RecipeIngredients { get; set; }
        public virtual DbSet<Site> Sites { get; set; }
        #endregion
    }
}