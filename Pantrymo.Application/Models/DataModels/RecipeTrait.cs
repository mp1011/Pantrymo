using System;
using System.Collections.Generic;

namespace Pantrymo.Application.Models
{
    public partial class RecipeTrait
    {
        public int Id { get; set; }
        public int TraitValueId { get; set; }
        public int RecipeId { get; set; }

        public virtual Recipe Recipe { get; set; }
        public virtual TraitValue TraitValue { get; set; }
    }
}
