#nullable disable

using Pantrymo.Domain.Models;

namespace Pantrymo.Application.Models
{
    public class Recipe : IRecipeDetail
    {
        public Recipe()
        {
            IngredientTexts = new HashSet<IngredientText>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public int? AuthorId { get; set; }
        public int SiteId { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
        public bool IncludeInSearches { get; set; }
        public DateTime LastModified { get; set; }
        public virtual Author Author { get; set; }
        public virtual Site Site { get; set; }
        public virtual ICollection<IngredientText> IngredientTexts { get; set; }

        IEnumerable<IIngredientTextDetail> IRecipeDetail.IngredientTexts => IngredientTexts.Cast<IIngredientTextDetail>();

        IAuthor IRecipeDetail.Author => Author;

        ISite IRecipeDetail.Site => Site;
    }
}
