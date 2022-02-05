#nullable disable
using Pantrymo.Domain.Models;

namespace Pantrymo.Application.Models
{

    public partial class RecipeIngredient : IRecipeIngredientDetail
    {
        public int Id { get; set; }
        public int TextId { get; set; }
        public int ComponentId { get; set; }
        public bool MultipleChoice { get; set; }

        public virtual Component Component { get; set; }
        public virtual IngredientText Text { get; set; }

        IComponentDetail IRecipeIngredientDetail.Component => Component;

        IIngredientTextDetail IRecipeIngredientDetail.Text => Text;
    }
}
