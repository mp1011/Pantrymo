namespace Pantrymo.Application.Models.AppModels
{
    public class RecipeDetail
    {
        public int Id { get; }
        public string Title { get; }
        public string Url { get; }
        public string ImageUrl { get;}

        public RecipeDetail(int id, string title, string url, string imageUrl)
        {
            Id = id;
            Title = title;
            Url = url;
            ImageUrl = imageUrl;
        }
    }
}
