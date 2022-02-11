#nullable disable
using Pantrymo.Domain.Models;

namespace Pantrymo.Application.Models
{
    public interface IRecipeIngredient : IWithId, IWithLastModifiedDate
    {
        int ComponentId { get; set; }
        bool MultipleChoice { get; set; }
        int TextId { get; set; }
    }

    public interface IRecipeIngredientDetail : IRecipeIngredient
    {
        IComponentDetail Component { get; }
        IIngredientTextDetail Text { get; }
    }
}
