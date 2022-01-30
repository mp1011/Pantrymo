using System;
using System.Collections.Generic;

namespace Pantrymo.Application.Models
{
    public partial class Trait
    {
        public Trait()
        {
            TraitValues = new HashSet<TraitValue>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<TraitValue> TraitValues { get; set; }
    }
}
