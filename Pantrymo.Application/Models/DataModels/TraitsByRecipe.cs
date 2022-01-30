using System;
using System.Collections.Generic;

namespace Pantrymo.Application.Models
{
    public partial class TraitsByRecipe
    {
        public int Id { get; set; }
        public string Recipe { get; set; }
        public string Site { get; set; }
        public string Trait { get; set; }
        public string Value { get; set; }
    }
}
