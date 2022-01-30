using System;
using System.Collections.Generic;

namespace Pantrymo.Application.Models
{
    public partial class ComponentDiet
    {
        public int Id { get; set; }
        public int ComponentId { get; set; }
        public bool? HasGluten { get; set; }
        public bool? IsVegan { get; set; }
        public bool? IsVegetarian { get; set; }
        public bool? HasSoy { get; set; }
        public bool? HasNuts { get; set; }
        public bool? HasPeanuts { get; set; }
        public bool? IsDairy { get; set; }

        public virtual Component Component { get; set; }
    }
}
