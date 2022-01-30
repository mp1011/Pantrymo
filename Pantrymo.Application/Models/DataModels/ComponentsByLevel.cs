using System;
using System.Collections.Generic;

namespace Pantrymo.Application.Models
{
    public partial class ComponentsByLevel
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public short? MaxLevel { get; set; }
    }
}
