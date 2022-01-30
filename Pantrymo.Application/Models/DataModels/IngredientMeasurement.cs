using System;
using System.Collections.Generic;

namespace Pantrymo.Application.Models
{
    public partial class IngredientMeasurement
    {
        public string Recipe { get; set; }
        public string Text { get; set; }
        public decimal? Measurement { get; set; }
        public string Type { get; set; }
    }
}
