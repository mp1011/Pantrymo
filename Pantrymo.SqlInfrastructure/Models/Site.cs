#nullable disable
using Pantrymo.Domain.Models;

namespace Pantrymo.Application.Models
{
    public partial class Site : ISite
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public bool EnableScraping { get; set; }
        public DateTime LastModified { get; set; }
    }
}
