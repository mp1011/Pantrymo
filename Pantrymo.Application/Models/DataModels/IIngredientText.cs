#nullable disable
using Pantrymo.Domain.Models;

namespace Pantrymo.Application.Models
{
    public interface IIngredientText : IWithId
    {
        int RecipeId { get; set; }
        string Text { get; set; }
    }

    public interface IIngredientTextDetail : IIngredientText
    {
        IRecipeDetail Recipe { get; }
        IEnumerable<IRecipeIngredientDetail> RecipeIngredients { get;  }
    }
}
