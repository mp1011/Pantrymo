#nullable disable
using Pantrymo.Domain.Models;

namespace Pantrymo.Application.Models
{
    public interface IIngredientText : IWithId, IWithLastModifiedDate
    {
        int RecipeId { get; set; }
        string Text { get; set; }
    }

    public interface IIngredientTextDTO :IIngredientText
    {
        IEnumerable<IRecipeIngredient> RecipeIngredients { get; }
    }

    public interface IIngredientTextDetail : IIngredientText
    {
        IRecipeDetail Recipe { get; }
        IEnumerable<IRecipeIngredientDetail> RecipeIngredients { get;  }
    }
}
