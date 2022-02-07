#nullable disable
using Pantrymo.Domain.Models;

namespace Pantrymo.Application.Models
{
    public interface IAuthor : IWithName, IWithId, IWithLastModifiedDate
    {
    }
}
