using System;
using System.Collections.Generic;

namespace Pantrymo.Application.Models
{
    public partial class UnknownClassificationsBySite
    {
        public string Site { get; set; }
        public int? Unknown { get; set; }
        public int? Total { get; set; }
        public double? Pct { get; set; }
    }
}
