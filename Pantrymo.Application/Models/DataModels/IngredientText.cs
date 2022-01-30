using System;
using System.Collections.Generic;

namespace Pantrymo.Application.Models
{
    public partial class IngredientText
    {
        public IngredientText()
        {
            RecipeIngredientMeasurements = new HashSet<RecipeIngredientMeasurement>();
            RecipeIngredients = new HashSet<RecipeIngredient>();
        }

        public int Id { get; set; }
        public int RecipeId { get; set; }
        public string Text { get; set; }

        public virtual Recipe Recipe { get; set; }
        public virtual ICollection<RecipeIngredientMeasurement> RecipeIngredientMeasurements { get; set; }
        public virtual ICollection<RecipeIngredient> RecipeIngredients { get; set; }
    }
}
