using System;
using System.Collections.Generic;

namespace Pantrymo.Application.Models
{
    public partial class UnknownTextCleaned
    {
        public int IngredientTextId { get; set; }
        public string IngredientText { get; set; }
        public string CleanedText { get; set; }
    }
}
