#nullable disable
using Pantrymo.Application.Models;
using Pantrymo.Domain.Models;

namespace Pantrymo.SqlInfrastructure.Models
{
    public partial class ComponentNegativeRelation : IComponentNegativeRelationDetail, IWithLastModifiedDate
    {
        public int Id { get; set; }
        public int ComponentId { get; set; }
        public int NegativeComponentId { get; set; }
        public DateTime LastModified { get; set; }

        public virtual Component Component { get; set; }
        public virtual Component NegativeComponent { get; set; }

        IComponentDetail IComponentNegativeRelationDetail.Component => Component;

        IComponentDetail IComponentNegativeRelationDetail.NegativeComponent => NegativeComponent;
    }
}
