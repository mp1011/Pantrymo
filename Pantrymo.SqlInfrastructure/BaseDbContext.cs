#nullable disable
using Microsoft.EntityFrameworkCore;
using Pantrymo.Application.Models;
using Pantrymo.Domain.Extensions;
using Pantrymo.Domain.Models;
using Pantrymo.Domain.Services;
using System.Reflection;

namespace Pantrymo.SqlInfrastructure
{
    public abstract class BaseDbContext : DbContext, IDataContext
    {
        protected readonly ISettingsService _settingsService;
        private readonly IObjectMapper _objectMapper;
        private readonly Dictionary<Type, MethodInfo> _queryProperties = new Dictionary<Type, MethodInfo>();
        private readonly Dictionary<Type, MethodInfo> _saveMethods = new Dictionary<Type, MethodInfo>();

        protected abstract bool AllowIdentityInsert { get; }

        public BaseDbContext(ISettingsService settingsService, IObjectMapper objectMapper, DbContextOptions options)
          : base(options)
        {
            _settingsService = settingsService;
            _objectMapper = objectMapper;
            FillTypedProperties();
        }

        public string GetQueryString<T>(IQueryable<T> query) => query.ToQueryString();


        private void FillTypedProperties()
        {
            foreach(var method in GetType().GetInterfaceMap(typeof(IDataContext)).TargetMethods)
            {
                if ((method.ReturnType.IsGenericType && method.ReturnType.GetGenericTypeDefinition() == typeof(IQueryable<>)))
                {
                    var genericParam = method.ReturnType.GetGenericArguments()[0];
                    _queryProperties.Add(genericParam, method);
                }

                if(method.Name == nameof(Save))
                {
                    var parameters = method.GetParameters();
                    if(parameters.Length == 1 && parameters[0].ParameterType.IsArray)
                        _saveMethods.Add(parameters[0].ParameterType.GetElementType(), method);
                }
            }
        }

        #region IQueryables

        public IQueryable<T> GetQuery<T>()
        {
            var queryProperty = _queryProperties.GetValueOrDefault(typeof(T));
            if (queryProperty == null)
                throw new NotSupportedException($"No Query for {typeof(T).Name} is defined on this data context");

            return (IQueryable<T>)queryProperty.Invoke(this, new object[] { });
        }

        IQueryable<IAuthor> IDataContext.Authors => Authors;
        IQueryable<IComponentNegativeRelation> IDataContext.ComponentNegativeRelations => ComponentNegativeRelations;
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

        public async Task Save<T>(params T[] records) where T: IWithId, IWithLastModifiedDate
        {
            var saveMethod = _saveMethods.GetValueOrDefault(typeof(T));
            if (saveMethod == null)
                throw new NotSupportedException($"There is no save method defined for {typeof(T).Name}[]");

            var result = saveMethod.Invoke(this, new object[] { records }) as Task;
            if (result != null)
                await result;
        }

        public async Task Save(params ISite[] records) => await Save(records, Sites);
        public async Task Save(params IComponent[] records) => await Save(records, Components);
        public async Task Save(params IAlternateComponentName[] records) => await Save(records, AlternateComponentNames);
        public async Task Save(params IAuthor[] records) => await Save(records, Authors);
        public async Task Save(params ICuisine[] records) => await Save(records, Cuisines);
        public async Task Save(params IComponentNegativeRelation[] records) => await Save(records, ComponentNegativeRelations);

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

            var serverRecordsToUpdate = dataSet
                .Where(p=> idsToUpdate.Contains(p.Id))
                .ToArray();

            if(AllowIdentityInsert)
            {
                var idsUpdated = serverRecordsToUpdate.Select(x => x.Id).ToArray();
                await dataSet.AddRangeAsync(oldRecords.Where(p=> !idsUpdated.Contains(p.Id)));
            }

            foreach(var serverRecord in serverRecordsToUpdate)
            {
                var changedRecord = oldRecords.First(x => x.Id == serverRecord.Id);
                _objectMapper.CopyAllProperties(changedRecord, serverRecord);
                Entry(serverRecord).State = EntityState.Modified;
            }
        }


        #endregion

    }
}