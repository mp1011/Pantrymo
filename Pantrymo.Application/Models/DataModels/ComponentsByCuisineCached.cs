using System;
using System.Collections.Generic;

namespace Pantrymo.Application.Models
{
    public partial class ComponentsByCuisineCached
    {
        public int Id { get; set; }
        public int ComponentId { get; set; }
        public string Cuisine { get; set; }
        public int NumRecipes { get; set; }
        public int TotalRecipes { get; set; }
        public int Frequency { get; set; }
        public byte FrequencyRank { get; set; }
    }
}
