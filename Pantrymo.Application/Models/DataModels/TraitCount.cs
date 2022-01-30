using System;
using System.Collections.Generic;

namespace Pantrymo.Application.Models
{
    public partial class TraitCount
    {
        public string Trait { get; set; }
        public string Value { get; set; }
        public int? Count { get; set; }
    }
}
