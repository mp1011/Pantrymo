using NUnit.Framework;
using Pantrymo.Domain.Models;
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
    }
}
