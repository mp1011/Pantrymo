using NUnit.Framework;
using Pantrymo.Application.Models.AppModels;
using Pantrymo.Domain.Extensions;
using System.Threading.Tasks;

namespace Pantrymo.Tests.ServiceTests
{
    [TestFixture(TestEnvironment.Sqlite)]
    [TestFixture(TestEnvironment.SqlServer)]
    class RecipeSearchServiceTests : EnvironmentTest
    {
        public RecipeSearchServiceTests(TestEnvironment testEnvironment) : base(testEnvironment)
        {
        }

        [TestCase("garlic","Italian",0,5)]
        public async Task CanSearchForRecipes(string ingredientsCsv, string cuisineCsv, int from, int to)
        {
            var service = MockHelper.CreateRecipeSearchService();
            var results = await service.Search(new RecipeSearchArgs(ingredientsCsv.FromCSV(), cuisineCsv.FromCSV(), from, to));
            Assert.AreEqual(to - from, results.Length);
        }
    }
}
