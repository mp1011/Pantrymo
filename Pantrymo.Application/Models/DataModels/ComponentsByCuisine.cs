using System;
using System.Collections.Generic;

namespace Pantrymo.Application.Models
{
    public partial class ComponentsByCuisine
    {
        public int Id { get; set; }
        public string Component { get; set; }
        public string Cuisine { get; set; }
        public long? NumRecipes { get; set; }
    }
}
