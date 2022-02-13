namespace Pantrymo.Application.Services
{
    public class IngredientSuggestionService
    {
        private readonly CategoryService _categoryTreeBuilder;

        public IngredientSuggestionService(CategoryService categoryTreeBuilder)
        {
            _categoryTreeBuilder = categoryTreeBuilder;
        }

        public async Task<string[]> SuggestIngredients(string query)
        {
            if (query == null || query.Length < 3)
                return new string[] { };

            var masterCategory = await _categoryTreeBuilder.GetOrBuildCategoryTree();


            var matchedCategories = masterCategory
                .SubCategories
                .Where(c => !c.Value.IsNonComponent
                               && !c.Value.IsMasterCategory
                               && !c.Key.Contains("(")
                               && c.Key.Contains(query.ToLower()))
               .GroupBy(c => c.Value.Name)
               .Select(grp =>
               {
                   if (grp.Count() == 1)
                       return grp.First().Key;
                   else
                   {
                        //special case for flour, standardize if I think of any other examples
                        if (grp.Key == "all-purpose flour")
                            return "flour";

                       return grp.Select(g => g.Key)
                                   .First();
                   }
               })
               .Distinct()
               .ToArray();

            var matches = matchedCategories
                .OrderBy(r => r.StartsWith(query.ToLower()) ? 0 : 1)
                .ThenBy(r => r.Length)
                .ToArray();

            return matches;

        }
    }
}
