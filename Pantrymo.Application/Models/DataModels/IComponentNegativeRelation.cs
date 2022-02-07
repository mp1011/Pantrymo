#nullable disable
using Pantrymo.Domain.Models;

namespace Pantrymo.Application.Models
{
    public interface IComponentNegativeRelation: IWithId, IWithLastModifiedDate
    {
        int ComponentId { get; set; }
        int NegativeComponentId { get; set; }
    }


    public interface IComponentNegativeRelationDetail : IComponentNegativeRelation
    {
        IComponentDetail Component { get; }
        IComponentDetail NegativeComponent { get; }
    }
}
