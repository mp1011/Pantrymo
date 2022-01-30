using System;
using System.Collections.Generic;

namespace Pantrymo.Application.Models
{
    public partial class RecipeIngredientMeasurement
    {
        public int Id { get; set; }
        public decimal? MeasurementAmount { get; set; }
        public int MeasurementTypeId { get; set; }
        public int IngredientTextId { get; set; }

        public virtual IngredientText IngredientText { get; set; }
        public virtual MeasurementType MeasurementType { get; set; }
    }
}
