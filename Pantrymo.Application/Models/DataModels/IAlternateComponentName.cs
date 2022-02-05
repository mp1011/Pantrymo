#nullable disable
using Pantrymo.Domain.Models;

namespace Pantrymo.Application.Models
{
    public interface IAlternateComponentName : IWithLastModifiedDate, IWithId
    {
        string AlternateName { get; set; } 
        int ComponentId { get; set; }
    }
}
