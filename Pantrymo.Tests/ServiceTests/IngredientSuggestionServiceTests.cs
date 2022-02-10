using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Pantrymo.Tests.ServiceTests
{
    [TestFixture(TestEnvironment.Sqlite)]
    [TestFixture(TestEnvironment.SqlServer)]
    class IngredientSuggestionServiceTests : EnvironmentTest
    {
        public IngredientSuggestionServiceTests(TestEnvironment testEnvironment) : base(testEnvironment)
        {
        }

        [TestCase("chicken", "chicken broth", null)]
        [TestCase("rice", "rice", null)]
        [TestCase("pasta", "pasta", null)]
        [TestCase("chile pepper", "chile pepper", null)]
        [TestCase("mayonnais", "mayonnaise", null)]
        [TestCase("arugula", "arugula", null)]
        [TestCase("rocket", "rocket", null)]
        [TestCase("arrowroot star", "arrowroot starch", "arrowroot starch flour")]
        [TestCase("cele", "celery", "celery ribs")]
        [TestCase("chee", "cheese", null)]
        [TestCase("appl", "apples", "apple")]
        [TestCase("flou", "flour", "flours (general)")]
        public async Task CanSuggestIngredients(string query, string shouldContain, string shouldNotContain)
        {
            var service = MockHelper.CreateIngredientSuggestionService();
            var results = await service.SuggestIngredients(query);

            results.Should().Contain(shouldContain);

            if (shouldNotContain != null)
                results.Should().NotContain(shouldNotContain);
        }
    }
}
