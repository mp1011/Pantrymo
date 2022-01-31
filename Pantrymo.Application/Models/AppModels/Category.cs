using Pantrymo.Application.Extensions;

namespace Pantrymo.Application.Models.AppModels
{
    public class Category
    {
        public bool IsValid => Name != null && !IsNonComponent;

        public bool IsNonComponent { get; set; }

        public bool IsAssumed { get; set; }

        public bool IsMasterCategory { get; set; }

        public bool IsSubCategory { get; set; }

        public string Name { get; set; }

        public bool NeverMatch { get; set; }

        public List<string> AlternateNames { get; } = new List<string>();

        public Dictionary<string, Category> SubCategories { get; } = new Dictionary<string, Category>();

        public Dictionary<string, Category> ParentCategories { get; } = new Dictionary<string, Category>();

        public Dictionary<string, Category> NegativeCategories { get; } = new Dictionary<string, Category>();

        private Category(string name)
        {
            Name = name;
        }

        public Category()
        {
            Name = "master";
            IsNonComponent = true;
        }

        public Category GetOrAddSubcategory(string name)
        {
            var subcategory = SubCategories.TryGet(name);

            if (subcategory == null)
            {
                subcategory = new Category(name);
                SubCategories[name] = subcategory;
                subcategory.AddParent(this);
            }

            return subcategory;
        }

        public void AddNegativeSubcategory(Category negativeCategory)
        {
            NegativeCategories[negativeCategory.Name] = negativeCategory;
        }

        public void SetSubcategory(string name, Category category)
        {
            var existingSubcategory = SubCategories.TryGet(name);
            if (existingSubcategory != null && existingSubcategory != category)
                throw new Exception("Conflict detected");

            if (existingSubcategory == null)
            {
                SubCategories[name] = category;
                category.AddParent(this);
            }
        }

        public Category GetCategoryOrDefault(string name)
        {
            if (name.Equals(this.Name, StringComparison.OrdinalIgnoreCase))
                return this;

            return SubCategories.TryGet(name) ?? new Category(name);
        }

        public Category GetCategoryOrNull(string name)
        {
            if (name.Equals(this.Name, StringComparison.OrdinalIgnoreCase))
                return this;

            return SubCategories.TryGet(name);
        }

        public Category AddParent(Category parent)
        {
            if (parent != null && parent.Name != null)
            {
                if (ParentCategories.TryGet(parent.Name) == null)
                    ParentCategories[parent.Name] = parent;
            }
            return this;
        }

        public bool IsAncestorOfAny(Category[] categories)
        {
            return categories.Any(c => IsAncestorOf(c));
        }

        public bool IsAncestorOfOrEqualsAny(Category[] categories)
        {
            return categories.Contains(this) || IsAncestorOfAny(categories);
        }

        public bool IsAncestorOf(Category other)
        {
            return IsAncestorOf(other, 0);
        }

        private bool IsAncestorOf(Category other, int steps)
        {
            if (steps > 10)
                return false;

            if (other == this)
                return false;

            if (other.ParentCategories.Values.Contains(this))
                return true;

            return other.ParentCategories.Values.Any(p => IsAncestorOf(p, steps + 1));
        }

        public IEnumerable<Category[]> GetAllHierarchies()
        {
            if (ParentCategories == null || !ParentCategories.Any())
            {
                yield return new Category[] { this };
            }
            else
            {
                foreach (var parent in ParentCategories)
                {
                    foreach (var parentHierarchies in parent.Value.GetAllHierarchies())
                    {
                        var list = new List<Category>();
                        list.Add(this);
                        list.AddRange(parentHierarchies);
                        yield return list.ToArray();
                    }
                }
            }
        }

        public Category[] GetThisAndAllAncestors()
        {
            List<Category> ret = new List<Category>();
            GetThisAndAllAncestors(ret);
            return ret.ToArray();
        }

        private void GetThisAndAllAncestors(List<Category> ret)
        {
            if (ret.Contains(this))
                return;

            ret.Add(this);

            foreach (var parent in ParentCategories.Values)
                parent.GetThisAndAllAncestors(ret);
        }

        public bool IsIncompatibleWithAny(IEnumerable<Category> categories)
        {
            foreach (var c in categories)
            {
                if (c != this && IsIncompatibleWith(c))
                    return true;
            }

            return false;
        }

        public void InheritNegativeCategories()
        {
            var allNegativeCategories = GetThisAndAllAncestors()
                .SelectMany(c => c.NegativeCategories.Values)
                .ToArray();

            foreach (var nc in allNegativeCategories)
            {
                NegativeCategories[nc.Name] = nc;
            }
        }

        private bool IsIncompatibleWith(Category other)
        {
            foreach (var negative in NegativeCategories.Values)
            {
                if (negative == other || negative.IsAncestorOf(other))
                    return true;
            }

            return false;
        }

        public override string ToString()
        {
            return Name;
        }
    }

}
