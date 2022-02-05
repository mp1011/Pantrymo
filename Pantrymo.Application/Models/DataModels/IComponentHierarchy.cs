using Pantrymo.Domain.Models;

namespace Pantrymo.Application.Models
{
    public interface IComponentHierarchy : IWithId, IWithLastModifiedDate
    {
        public int ComponentId { get; set; }
    }
}
