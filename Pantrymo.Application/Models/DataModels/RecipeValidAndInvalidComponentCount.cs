using System;
using System.Collections.Generic;

namespace Pantrymo.Application.Models
{
    public partial class RecipeValidAndInvalidComponentCount
    {
        public int Recipeid { get; set; }
        public string Recipe { get; set; }
        public int? Components { get; set; }
        public int? UnknownComponents { get; set; }
    }
}
