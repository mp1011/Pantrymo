using System;
using System.Collections.Generic;

namespace Pantrymo.Application.Models
{
    public partial class Recipe
    {
        //public Recipe()
        //{
        //    IngredientTexts = new HashSet<IngredientText>();
        //    RecipeAudits = new HashSet<RecipeAudit>();
        //    RecipeSteps = new HashSet<RecipeStep>();
        //    RecipeTraits = new HashSet<RecipeTrait>();
        //}

        public int Id { get; set; }
        public string Title { get; set; }
        public int? AuthorId { get; set; }
        public int SiteId { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
        public bool IncludeInSearches { get; set; }

        //public virtual Author Author { get; set; }
        //public virtual Site Site { get; set; }
        //public virtual ICollection<IngredientText> IngredientTexts { get; set; }
        //public virtual ICollection<RecipeAudit> RecipeAudits { get; set; }
        //public virtual ICollection<RecipeStep> RecipeSteps { get; set; }
        //public virtual ICollection<RecipeTrait> RecipeTraits { get; set; }
    }
}
