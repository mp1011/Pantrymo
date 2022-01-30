using System;
using System.Collections.Generic;

namespace Pantrymo.Application.Models
{
    public partial class MeasurementType
    {
        public MeasurementType()
        {
            RecipeIngredientMeasurements = new HashSet<RecipeIngredientMeasurement>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }

        public virtual ICollection<RecipeIngredientMeasurement> RecipeIngredientMeasurements { get; set; }
    }
}
