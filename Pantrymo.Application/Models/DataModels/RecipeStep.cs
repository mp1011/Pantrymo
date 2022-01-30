using System;
using System.Collections.Generic;

namespace Pantrymo.Application.Models
{
    public partial class RecipeStep
    {
        public int Id { get; set; }
        public int RecipeId { get; set; }
        public string Text { get; set; }

        public virtual Recipe Recipe { get; set; }
    }
}
