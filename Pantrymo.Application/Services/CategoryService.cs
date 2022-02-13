#nullable disable
using Pantrymo.Domain.Extensions;
using Pantrymo.Application.Models;
using Pantrymo.Application.Models.AppModels;
using Pantrymo.Domain.Services;
using MediatR;
using Pantrymo.Domain.Features;

namespace Pantrymo.Application.Services
{

    public class CategoryService
    {
        private readonly IDataContext _dbContext;
        private readonly IFullHierarchyLoader _fullHierarchyLoader;
        private readonly ICacheService _cache;
        private readonly IMediator _mediator;

        public CategoryService(IDataContext dbContext, IFullHierarchyLoader fullHierarchyLoader, ICacheService cache, IMediator mediator)
        {
            _dbContext = dbContext;
            _fullHierarchyLoader = fullHierarchyLoader;
            _cache = cache;
            _mediator = mediator;
        }

        public async Task<Category[]> GetCategories(IComponent[] components)
        {
            var tree = await GetOrBuildCategoryTree();
            return components
                .Select(c => tree.SubCategories[c.Name])
                .ToArray();
        }

        public async Task<Category[]> GetCategoriesWithDescendants(IComponent[] components)
        {
            var categories = await GetCategories(components);

            return categories
                .SelectMany(c => c.GetThisAndAllDescendants())
                .ToArray();
        }

        public IComponent[] GetComponents(Category[] categories)
        {
            var names = categories
                .Select(c => c.Name)
                .ToArray();

            return _dbContext.Components
                .Where(c => names.Contains(c.Name))
                .ToArray();
        }

        public async Task<Category> GetOrBuildCategoryTree()
        {
            var categoryTree = await _cache.TryGet<Category>();
            if(categoryTree == null)
            {
                categoryTree = await BuildCategoryTree();
                if(categoryTree.SubCategories.Any())
                    await _cache.Add(categoryTree);
            }

            return categoryTree;
        }

        public async Task<Category> BuildCategoryTree()
        {
            await _mediator.Publish(new ShowProgressFeature.Notification($"Started building category tree"));

            var hierarchy = await _fullHierarchyLoader.GetFullHierarchy();
           
            if (hierarchy.Any(p => p.Level7 != null))
                throw new Exception("Additional level needed");

            var components = _dbContext.ComponentsDetail.ToArray();

            var master = new Category();

            FillHierarchy(1, components.ToDictionary(), hierarchy, master, master);

            foreach (var cat in master.SubCategories)
                cat.Value.InheritNegativeCategories();

            await _mediator.Publish(new ShowProgressFeature.Notification($"Finished building category tree"));

            return master;
        }

        private void FillHierarchy(int level, Dictionary<string, IComponentDetail> components, IEnumerable<FullHierarchy> hierarchies,
           Category node, Category master)
        {
            foreach (var group in hierarchies.GroupBy(p => p.GetLevel(level)).Where(g => g.Key != null))
            {
                var component = components.GetValueOrDefault(group.Key);
                if(component == null)
                    continue;

                var category = master.GetOrAddSubcategory(group.Key);
                if (!category.AlternateNames.Any())
                {
                    category.AlternateNames.AddRange(component.AlternateComponentNames.Select(a => a.AlternateName));
                }

                node.SetSubcategory(group.Key, category);

                category.IsAssumed = component.Assumed;
                category.IsNonComponent = component.NonComponent;
                category.IsSubCategory = component.SubCategory;

                foreach (var altName in component.AlternateComponentNames)
                {
                    master.SetSubcategory(altName.AlternateName, category);
                }

                if (!category.NegativeCategories.Keys.Any()
                    && component.ComponentNegativeRelationComponents != null
                    && component.ComponentNegativeRelationComponents.Any())
                {
                    foreach (var negativeComponent in component.ComponentNegativeRelationComponents)
                    {
                        var negativeCategory = master.GetOrAddSubcategory(negativeComponent.NegativeComponent.Name);
                        category.AddNegativeSubcategory(negativeCategory);
                    }
                }


                if (level <= 5)
                    FillHierarchy(level + 1, components, group, category, master);
            }
        }

    }
}
