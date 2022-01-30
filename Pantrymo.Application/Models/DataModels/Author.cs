using System;
using System.Collections.Generic;

namespace Pantrymo.Application.Models
{
    public partial class Author
    {
        public Author()
        {
            Recipes = new HashSet<Recipe>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Recipe> Recipes { get; set; }
    }
}
