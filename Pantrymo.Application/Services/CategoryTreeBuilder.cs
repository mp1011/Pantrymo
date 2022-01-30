using Pantrymo.Application.Extensions;
using Pantrymo.Application.Models;
using Pantrymo.Application.Models.AppModels;

namespace Pantrymo.Application.Services
{

    public class CategoryTreeBuilder
    {
        private readonly IDataContext _dbContext;
        private readonly IFullHierarchyLoader _fullHierarchyLoader;

        public CategoryTreeBuilder(IDataContext dbContext, IFullHierarchyLoader fullHierarchyLoader)
        {
            _dbContext = dbContext;
            _fullHierarchyLoader = fullHierarchyLoader;
        }

        public async Task<Category> BuildCategoryTree()
        {
            var hierarchy = await _fullHierarchyLoader.GetFullHierarchy();
           
            if (hierarchy.Any(p => p.Level7 != null))
                throw new Exception("Additional level needed");

            var components = _dbContext.Components.ToArray();

            var master = new Category();

            FillHierarchy(1, components.ToDictionary(), hierarchy, master, master);

            foreach (var cat in master.SubCategories)
                cat.Value.InheritNegativeCategories();

            return master;
        }

        private void FillHierarchy(int level, Dictionary<string, Component> components, IEnumerable<FullHierarchy> hierarchies,
           Category node, Category master)
        {
            foreach (var group in hierarchies.GroupBy(p => p.GetLevel(level)).Where(g => g.Key != null))
            {
                var component = components[group.Key];
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
