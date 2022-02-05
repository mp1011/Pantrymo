﻿using Pantrymo.Domain.Models;

namespace Pantrymo.Application.Models
{
    public partial class Cuisine : ICuisine
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Generic { get; set; }
        public bool Esque { get; set; }
        public DateTime LastModified { get; set; }
    }
}
