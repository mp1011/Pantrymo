#nullable disable
using Pantrymo.Domain.Models;

namespace Pantrymo.Application.Models
{
    public partial class AlternateComponentName : IAlternateComponentName
    {
        public int Id { get; set; }
        public string AlternateName { get; set; } = "";
        public int ComponentId { get; set; }
        public DateTime LastModified { get; set; }
    }
}
