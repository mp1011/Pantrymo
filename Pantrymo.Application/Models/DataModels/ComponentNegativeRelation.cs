using System;
using System.Collections.Generic;

namespace Pantrymo.Application.Models
{
    public partial class ComponentNegativeRelation
    {
        public int Id { get; set; }
        public int ComponentId { get; set; }
        public int NegativeComponentId { get; set; }

        public virtual Component Component { get; set; }
        public virtual Component NegativeComponent { get; set; }
    }
}
