#nullable disable
using Pantrymo.Domain.Models;

namespace Pantrymo.Application.Models
{
    public partial class ComponentNegativeRelation : IComponentNegativeRelationDetail
    {
        public int Id { get; set; }
        public int ComponentId { get; set; }
        public int NegativeComponentId { get; set; }

        public virtual Component Component { get; set; }
        public virtual Component NegativeComponent { get; set; }

        IComponentDetail IComponentNegativeRelationDetail.Component => Component;

        IComponentDetail IComponentNegativeRelationDetail.NegativeComponent => NegativeComponent;
    }
}
