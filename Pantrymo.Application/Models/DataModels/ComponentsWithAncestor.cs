using System;
using System.Collections.Generic;

namespace Pantrymo.Application.Models
{
    public partial class ComponentsWithAncestor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AncestorId { get; set; }
        public string AncestorName { get; set; }
    }
}
