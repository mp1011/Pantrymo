using FluentAssertions;
using NUnit.Framework;
using Pantrymo.Domain.Models;
using Pantrymo.ServerInfrastructure;
using Pantrymo.SqlInfrastructure.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Pantrymo.Tests.ServiceTests
{
    [TestFixture(TestEnvironment.Sqlite)]
    [TestFixture(TestEnvironment.SqlServer)]
    class DataContextTests : EnvironmentTest
    {
        public DataContextTests(TestEnvironment testEnvironment) : base(testEnvironment)
        {
        }

        [Test]
        public void TestQueryComponents()
        {
            var dataContext = MockHelper.CreateDataContext();

            var componentsDetailQuery = dataContext.ComponentsDetail
                .Where(p => p.AlternateComponentNames.Any())
                .Take(5);

            var queryString = dataContext.GetQueryString(componentsDetailQuery);
            Debug.WriteLine(queryString);

            var result = componentsDetailQuery.ToArray();
            Assert.AreEqual(5, result.Length);

            foreach(var component in result)
            {
                Assert.That(component.AlternateComponentNames.Count() > 0);
            }
        }

        [Test]
        public void TestSaveRecipeDetail()
        {
            var dataContext = MockHelper.CreateDataContext();

            //would like to test this on sql server but not until we have a good way to remove test records
            if(dataContext is SqlServerDbContext)
            {
                Assert.Inconclusive();
                return;
            }

           
            var recipe = TestDataCreator.CreateTestRecipe(dataContext, nameof(TestSaveRecipeDetail));
            dataContext.Save(recipe);

            var loaded = dataContext.RecipesDetail.FirstOrDefault(p=>p.Id == recipe.Id);
            loaded.Should().NotBeNull();

            loaded.IngredientTexts.Count().Should().Be(5);
            foreach (var ingredient in loaded.IngredientTexts)
                ingredient.RecipeIngredients.Count().Should().Be(1);
        }
    }
}
