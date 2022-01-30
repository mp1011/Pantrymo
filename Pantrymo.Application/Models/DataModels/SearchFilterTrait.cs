using System;
using System.Collections.Generic;

namespace Pantrymo.Application.Models
{
    public partial class SearchFilterTrait
    {
        public int SearchFilterId { get; set; }
        public int TraitValueId { get; set; }

        public virtual SearchFilter SearchFilter { get; set; }
        public virtual TraitValue TraitValue { get; set; }
    }
}
