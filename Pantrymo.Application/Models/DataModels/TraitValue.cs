using System;
using System.Collections.Generic;

namespace Pantrymo.Application.Models
{
    public partial class TraitValue
    {
        public TraitValue()
        {
            RecipeTraits = new HashSet<RecipeTrait>();
        }

        public int Id { get; set; }
        public int TraitId { get; set; }
        public string Name { get; set; }

        public virtual Trait Trait { get; set; }
        public virtual ICollection<RecipeTrait> RecipeTraits { get; set; }
    }
}
