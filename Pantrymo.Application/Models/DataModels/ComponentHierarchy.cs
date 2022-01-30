using System;
using System.Collections.Generic;

namespace Pantrymo.Application.Models
{
    public partial class ComponentHierarchy
    {
        public int ComponentId { get; set; }
        public int Id { get; set; }

        public virtual Component Component { get; set; }
    }
}
