using System;
using System.Collections.Generic;

namespace Pantrymo.Application.Models
{
    public partial class RecipeIngredientCount
    {
        public string Site { get; set; }
        public string Recipe { get; set; }
        public int? IngredientLines { get; set; }
    }
}
