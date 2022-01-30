using System;
using System.Collections.Generic;

namespace Pantrymo.Application.Models
{
    public partial class RecipeIngredient
    {
        public int Id { get; set; }
        public int TextId { get; set; }
        public int ComponentId { get; set; }
        public bool MultipleChoice { get; set; }

        public virtual Component Component { get; set; }
        public virtual IngredientText Text { get; set; }
    }
}
