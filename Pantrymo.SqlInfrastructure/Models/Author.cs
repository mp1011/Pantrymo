#nullable disable

using Pantrymo.Application.Models;

namespace Pantrymo.SqlInfrastructure.Models
{
    public partial class Author : IAuthor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime LastModified { get; set; }
    }
}
