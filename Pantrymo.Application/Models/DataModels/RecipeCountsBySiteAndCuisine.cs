using System;
using System.Collections.Generic;

namespace Pantrymo.Application.Models
{
    public partial class RecipeCountsBySiteAndCuisine
    {
        public string Site { get; set; }
        public string Cuisine { get; set; }
        public int? NumRecipes { get; set; }
    }
}
