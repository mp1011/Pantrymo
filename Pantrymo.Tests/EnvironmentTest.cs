using NUnit.Framework;

namespace Pantrymo.Tests
{


    abstract class EnvironmentTest 
    {
        private readonly TestEnvironment _testEnvironment;

        protected MockHelper MockHelper { get; private set; }

        public EnvironmentTest(TestEnvironment testEnvironment)
        {
            _testEnvironment = testEnvironment;
            MockHelper = new MockHelper(_testEnvironment);
        }

        [SetUp]
        public void TestSetup()
        {
            MockHelper = new MockHelper(_testEnvironment);
        }
    }
}
