using System;
using System.Collections.Generic;

namespace Pantrymo.Application.Models
{
    public partial class Cuisine
    {
        public Cuisine()
        {
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool Generic { get; set; }
        public bool Esque { get; set; }

    }
}
