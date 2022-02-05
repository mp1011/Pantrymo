using NUnit.Framework;

namespace Pantrymo.Tests
{

    [TestFixture(TestEnvironment.Sqlite)]
    [TestFixture(TestEnvironment.SqlServer)]
    abstract class MultiEnvironmentTest
    {
        private readonly TestEnvironment _testEnvironment;

        protected MockHelper MockHelper { get; }

        public MultiEnvironmentTest(TestEnvironment testEnvironment)
        {
            _testEnvironment = testEnvironment;
            MockHelper = new MockHelper(_testEnvironment);
        }
    }
}
