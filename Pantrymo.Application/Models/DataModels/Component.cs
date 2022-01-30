using System;
using System.Collections.Generic;

namespace Pantrymo.Application.Models
{
    public partial class Component
    {
        public Component()
        {
            AlternateComponentNames = new HashSet<AlternateComponentName>();
            ComponentHierarchies = new HashSet<ComponentHierarchy>();
            ComponentNegativeRelationComponents = new HashSet<ComponentNegativeRelation>();
            ComponentNegativeRelationNegativeComponents = new HashSet<ComponentNegativeRelation>();
            RecipeIngredients = new HashSet<RecipeIngredient>();
        }

        public string Name { get; set; }
        public int Id { get; set; }
        public bool NonComponent { get; set; }
        public bool Assumed { get; set; }
        public bool MasterCategory { get; set; }
        public bool SubCategory { get; set; }

        public virtual ComponentDiet ComponentDiet { get; set; }
        public virtual ICollection<AlternateComponentName> AlternateComponentNames { get; set; }
        public virtual ICollection<ComponentHierarchy> ComponentHierarchies { get; set; }
        public virtual ICollection<ComponentNegativeRelation> ComponentNegativeRelationComponents { get; set; }
        public virtual ICollection<ComponentNegativeRelation> ComponentNegativeRelationNegativeComponents { get; set; }
        public virtual ICollection<RecipeIngredient> RecipeIngredients { get; set; }
    }
}
