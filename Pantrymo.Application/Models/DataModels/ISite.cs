#nullable disable
using Pantrymo.Domain.Models;

namespace Pantrymo.Application.Models
{
    public interface ISite : IWithName, IWithLastModifiedDate, IWithId
    {
        public string Url { get; set; }
        public bool EnableScraping { get; set; }
    }
}
