using System;
using System.Collections.Generic;

namespace Pantrymo.Application.Models
{
    public partial class ComponentsByCuisineWithTotal
    {
        public int Id { get; set; }
        public string Component { get; set; }
        public string Cuisine { get; set; }
        public long? NumRecipes { get; set; }
        public int? TotalRecipes { get; set; }
        public long? Frequency { get; set; }
        public int FrequencyRank { get; set; }
    }
}
