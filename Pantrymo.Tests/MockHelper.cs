using Microsoft.EntityFrameworkCore;
using NSubstitute;
using NUnit.Framework;
using Pantrymo.Application.Models;
using Pantrymo.Application.Models.AppModels;
using Pantrymo.Application.Services;
using Pantrymo.ClientInfrastructure;
using Pantrymo.ClientInfrastructure.Services;
using Pantrymo.Domain.Services;
using Pantrymo.ServerInfrastructure;
using System;
using System.IO;

namespace Pantrymo.Tests
{
    internal class MockHelper
    {
        private readonly TestEnvironment _testEnvironment;

        public MockHelper(TestEnvironment testEnvironment)
        {
            _testEnvironment = testEnvironment;
        }

        public IngredientSuggestionService CreateIngredientSuggestionService()
        {
            return new IngredientSuggestionService(CreateCategoryTreeBuilder());
        }

        public CategoryTreeBuilder CreateCategoryTreeBuilder()
        {
            return new CategoryTreeBuilder(CreateDataContext(), CreateFullHierarchyLoader(), CreateCacheService());
        }

        public IDataContext CreateDataContext()
        {
            if(_testEnvironment == TestEnvironment.Sqlite)
                return new SqliteDbContext(CreateSettingsService(), new DbContextOptions<SqliteDbContext>());
            else
                return new SqlServerDbContext(CreateSettingsService(), new DbContextOptions<SqlServerDbContext>());
        }

        public IFullHierarchyLoader CreateFullHierarchyLoader()
        {
            var localStorage = CreateLocalStorage();

            var mock = Substitute.For<IFullHierarchyLoader>();
            mock.GetFullHierarchy().Returns(_ => localStorage.Get<FullHierarchy[]>());
            return mock;
        }

        public ILocalStorage CreateLocalStorage()
        {
            return new LocalStorage(CreateSettingsService());
        }

        public ISettingsService CreateSettingsService()
        {
            var projectFolder = new DirectoryInfo(TestContext.CurrentContext.TestDirectory);
            while (projectFolder.Name != "Pantrymo.Tests")
                projectFolder = projectFolder.Parent;
           
            var mock = Substitute.For<ISettingsService>();

            if(_testEnvironment == TestEnvironment.Sqlite)
                mock.ConnectionString.Returns(@$"Data Source={projectFolder.FullName}\TestData\PantrymoDB.db");
            else
                mock.ConnectionString.Returns(@$"Server=localhost;Database=PantryMoDB;Trusted_Connection=True;");

            mock.LocalDataFolder.Returns(@$"{projectFolder.FullName}\TestData");
            mock.GetCacheDuration<Category>().ReturnsForAnyArgs(callInfo=> TimeSpan.Zero);

            return mock;
        }

        public static ICacheService CreateCacheService()
        {
            var mock = Substitute.For<ICacheService>();
            return mock;
        }
    }
}
