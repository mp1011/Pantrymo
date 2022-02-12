using Pantrymo.Application.Models;
using Pantrymo.Domain.Extensions;
using Pantrymo.SqlInfrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pantrymo.Tests
{
    internal class TestDataCreator
    {
        public static IEnumerable<ISite> CreateTestSites(int count, bool assignIds)
        {
            foreach (var num in Enumerable.Range(0, count))
            {
                yield return new Site
                {
                    Id = assignIds ? num + 1 : 0,
                    Name = $"Site {num}",
                    Url = "TEST",
                    LastModified = DateTime.Now.AddDays(-num)
                };
            }
        }

        public static Recipe CreateTestRecipe(IDataContext dataContext, string title)
        {
            int recipeId = dataContext.Recipes.NextId();
            int siteId = dataContext.Sites.Select(p => p.Id).First();

            var recipe = new Recipe
            {
                Id = recipeId,
                Title = title,
                ImageUrl = "TEST",
                Url = "TEST",
                SiteId = siteId,
                IncludeInSearches = false
            };

            foreach (var text in CreateTestIngredientTextDetail(dataContext, recipe, 5))
                recipe.IngredientTexts.Add(text);

            return recipe;
        }

        public static IEnumerable<IngredientText> CreateTestIngredientTextDetail(IDataContext dataContext, IRecipeDetail recipe, int count)
        {
            int lastId = dataContext.IngredientTexts.NextId();

            int recipeIngredientId = dataContext.RecipeIngredients.NextId();

            foreach (int index in Enumerable.Range(0, count))
            {
                var text = new IngredientText
                {
                    Id = lastId + index + 1,
                    Text = "TEST" + index,
                    RecipeId = recipe.Id
                };

                text.RecipeIngredients.Add(CreateTestRecipeIngredient(dataContext, text, recipeIngredientId));
                recipeIngredientId++;

                yield return text;
            }
        }

        public static RecipeIngredient CreateTestRecipeIngredient(IDataContext dataContext, IIngredientTextDetail text, int id)
        {
            return new RecipeIngredient
            {
                Id = id,
                TextId = text.Id,
                ComponentId = dataContext.Components.First(p => !p.NonComponent).Id
            };
        }
    }
}
