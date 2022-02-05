#nullable disable

using Pantrymo.Domain.Models;

namespace Pantrymo.Application.Models
{
    public interface IRecipe : IWithLastModifiedDate, IWithId
    {
        public string Title { get; set; }
        public int? AuthorId { get; set; }
        public int SiteId { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
        public bool IncludeInSearches { get; set; }
    }

    public interface IRecipeDetail : IRecipe
    {
        IAuthor Author { get; }
        ISite Site { get; }
        IEnumerable<IIngredientTextDetail> IngredientTexts { get; }
    }
}
