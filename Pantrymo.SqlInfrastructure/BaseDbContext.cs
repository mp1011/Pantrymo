#nullable disable
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pantrymo.Application.Models;
using Pantrymo.Domain.Extensions;
using Pantrymo.Domain.Models;
using Pantrymo.Domain.Services;

namespace Pantrymo.SqlInfrastructure
{
    public abstract class BaseDbContext : DbContext, IDataContext
    {
        protected readonly ISettingsService _settingsService;
        private readonly IObjectMapper _objectMapper;

        public BaseDbContext(ISettingsService settingsService, IObjectMapper objectMapper, DbContextOptions options)
          : base(options)
        {
            _settingsService = settingsService;
            _objectMapper = objectMapper;
        }

        public string GetQueryString<T>(IQueryable<T> query) => query.ToQueryString();

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
        public virtual DbSet<FullHierarchy> FullHierarchy { get; set; }
        public virtual DbSet<RecipeSearchResult> RecipeSearchResults { get; set; }
        #endregion

        #region Saving

        public async Task Save(params ISite[] records) => await Save(records, Sites);
        public async Task Save(params IComponent[] records) => await Save(records, Components);
        public async Task Save(params IAlternateComponentName[] records) => await Save(records, AlternateComponentNames);

        private async Task Save<TInterface,TModel>(TInterface[] records, DbSet<TModel> dataSet)
            where TInterface : IWithId, IWithLastModifiedDate
            where TModel : class, IWithId
        {
            if (!records.Any())
                return;

            foreach(var record in records)
                record.LastModified = DateTime.UtcNow;
                   
            ICollection<TModel> newRecords, oldRecords;
            var recordsAsModels = records.Cast<TModel>().ToArray();

            recordsAsModels.SplitUp(p => p.Id <= 0, out newRecords, out oldRecords);
            await dataSet.AddRangeAsync(newRecords);

            var idsToUpdate = oldRecords.Select(x => x.Id).ToArray();

            var serverRecorsdToUpdate = dataSet
                .Where(p=> idsToUpdate.Contains(p.Id))
                .ToArray();

            foreach(var serverRecord in serverRecorsdToUpdate)
            {
                var changedRecord = oldRecords.First(x => x.Id == serverRecord.Id);
                _objectMapper.CopyAllProperties(changedRecord, serverRecord);
                Entry(serverRecord).State = EntityState.Modified;
            }
        }


        #endregion

    }
}