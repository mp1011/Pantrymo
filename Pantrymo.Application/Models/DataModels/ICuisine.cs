using Pantrymo.Domain.Models;

namespace Pantrymo.Application.Models
{
    public interface ICuisine : IWithName, IWithLastModifiedDate, IWithId
    {
        bool Generic { get; set; }
        bool Esque { get; set; }
    }
}
