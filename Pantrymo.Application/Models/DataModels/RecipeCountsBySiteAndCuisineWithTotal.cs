using System;
using System.Collections.Generic;

namespace Pantrymo.Application.Models
{
    public partial class RecipeCountsBySiteAndCuisineWithTotal
    {
        public string Site { get; set; }
        public int? TotalRecipes { get; set; }
        public string Cuisine { get; set; }
        public int? CuisineRecipes { get; set; }
    }
}
