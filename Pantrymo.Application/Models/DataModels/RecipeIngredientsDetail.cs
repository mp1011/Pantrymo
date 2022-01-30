using System;
using System.Collections.Generic;

namespace Pantrymo.Application.Models
{
    public partial class RecipeIngredientsDetail
    {
        public string Site { get; set; }
        public int RecipeId { get; set; }
        public string Recipe { get; set; }
        public string IngredientText { get; set; }
        public string Component { get; set; }
        public int ComponentId { get; set; }
        public int RecipeIngredientId { get; set; }
        public int IngredientTextId { get; set; }
        public bool MultipleChoice { get; set; }
    }
}
