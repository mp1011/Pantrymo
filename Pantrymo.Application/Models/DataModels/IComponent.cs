#nullable disable
using Pantrymo.Domain.Models;

namespace Pantrymo.Application.Models
{
    public interface IComponent : IWithName, IWithLastModifiedDate, IWithId
    {
        bool Assumed { get; set; }
        bool MasterCategory { get; set; }
        bool NonComponent { get; set; }
        bool SubCategory { get; set; }
    }

    public interface IComponentDetail : IComponent
    {        
        IEnumerable<IAlternateComponentName> AlternateComponentNames { get; }
        IEnumerable<IComponentHierarchy> ComponentHierarchies { get; }
        IEnumerable<IComponentNegativeRelationDetail> ComponentNegativeRelationComponents { get; }
        IEnumerable<IComponentNegativeRelationDetail> ComponentNegativeRelationNegativeComponents { get; }
        IEnumerable<IRecipeIngredientDetail> RecipeIngredients { get; }
    }

}
