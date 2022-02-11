namespace Pantrymo.Application.Models.AppModels
{
    public record RecipeSearchArgs(string[] Ingredients, string[] Cuisines, int From, int To);

}
