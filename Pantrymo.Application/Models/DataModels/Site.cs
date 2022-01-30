using System;
using System.Collections.Generic;

namespace Pantrymo.Application.Models
{
    public partial class Site
    {
        public Site()
        {
            NavigationHints = new HashSet<NavigationHint>();
            Recipes = new HashSet<Recipe>();
            ScraperHints = new HashSet<ScraperHint>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public bool EnableScraping { get; set; }
        public DateTime LastModified { get; set; }

        public virtual ICollection<NavigationHint> NavigationHints { get; set; }
        public virtual ICollection<Recipe> Recipes { get; set; }
        public virtual ICollection<ScraperHint> ScraperHints { get; set; }
    }
}
