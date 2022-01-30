using System;
using System.Collections.Generic;

namespace Pantrymo.Application.Models
{
    public partial class UnknownTextWord
    {
        public int IngredientTextId { get; set; }
        public string Word { get; set; }
        public int KnownComponent { get; set; }
        public int KnownNonComponent { get; set; }
    }
}
