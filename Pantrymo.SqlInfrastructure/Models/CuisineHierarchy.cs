using Pantrymo.Application.Models;

namespace Pantrymo.SqlInfrastructure.Models
{
    public partial class CuisineHierarchy : ICuisineHierarchy
    {
        public int Id { get; set; }
        public int CuisineId { get; set; }
    }
}
