using System;
using System.Collections.Generic;

namespace Pantrymo.Application.Models
{
    public partial class RecipesWithoutCuisine
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int? AuthorId { get; set; }
        public int SiteId { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
        public bool IncludeInSearches { get; set; }
        public string Site { get; set; }
    }
}
