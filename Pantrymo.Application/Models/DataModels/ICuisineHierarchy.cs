using Pantrymo.Domain.Models;

namespace Pantrymo.Application.Models
{
    public interface ICuisineHierarchy : IWithId 
    {
        public int CuisineId { get; set; }
    }
}
