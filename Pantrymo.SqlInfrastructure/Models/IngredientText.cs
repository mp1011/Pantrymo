﻿#nullable disable
using Pantrymo.Application.Models;

namespace Pantrymo.SqlInfrastructure.Models
{
    public partial class IngredientText : IIngredientTextDetail
    {
        public IngredientText()
        {
            RecipeIngredients = new HashSet<RecipeIngredient>();
        }

        public int Id { get; set; }
        public int RecipeId { get; set; }
        public string Text { get; set; }

        public DateTime LastModified { get; set; }

        public virtual Recipe Recipe { get; set; }
        public virtual ICollection<RecipeIngredient> RecipeIngredients { get; set; }

        IEnumerable<IRecipeIngredientDetail> IIngredientTextDetail.RecipeIngredients => RecipeIngredients.Cast<IRecipeIngredientDetail>();

        IRecipeDetail IIngredientTextDetail.Recipe => Recipe;
    }
}
