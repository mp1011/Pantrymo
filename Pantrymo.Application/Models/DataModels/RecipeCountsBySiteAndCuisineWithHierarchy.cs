using System;
using System.Collections.Generic;

namespace Pantrymo.Application.Models
{
    public partial class RecipeCountsBySiteAndCuisineWithHierarchy
    {
        public string Site { get; set; }
        public int? TotalRecipes { get; set; }
        public int? CuisineRecipes { get; set; }
        public string Cuisine { get; set; }
        public string Cuisine2 { get; set; }
        public string Cuisine3 { get; set; }
        public string Cuisine4 { get; set; }
        public string Cuisine5 { get; set; }
    }
}
