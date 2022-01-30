using System;
using System.Collections.Generic;

namespace Pantrymo.Application.Models
{
    public partial class RecipeCountsByCuisine
    {
        public string Cuisine { get; set; }
        public int? NumRecipes { get; set; }
    }
}
