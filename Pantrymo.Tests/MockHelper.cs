using Microsoft.EntityFrameworkCore;
using NSubstitute;
using NUnit.Framework;
using Pantrymo.Application.Models;
using Pantrymo.Application.Models.AppModels;
using Pantrymo.Application.Services;
using Pantrymo.ClientInfrastructure;
using Pantrymo.ClientInfrastructure.Services;
using Pantrymo.Domain.Services;
using System;
using System.IO;

namespace Pantrymo.Tests
{
    internal static class MockHelper
    {
        public static IngredientSuggestionService CreateIngredientSuggestionService()
        {
            return new IngredientSuggestionService(CreateCategoryTreeBuilder());
        }

        public static CategoryTreeBuilder CreateCategoryTreeBuilder()
        {
            return new CategoryTreeBuilder(CreateDataContext(), CreateFullHierarchyLoader(), CreateCacheService());
        }

        public static IDataContext CreateDataContext()
        {
            return new PantryMoDBContext(CreateSettingsService(), new DbContextOptions<PantryMoDBContext>());
        }

        public static IFullHierarchyLoader CreateFullHierarchyLoader()
        {
            var localStorage = CreateLocalStorage();

            var mock = Substitute.For<IFullHierarchyLoader>();
            mock.GetFullHierarchy().Returns(_ => localStorage.Get<FullHierarchy[]>());
            return mock;
        }

        public static ILocalStorage CreateLocalStorage()
        {
            return new LocalStorage(CreateSettingsService());
        }

        public static ISettingsService CreateSettingsService()
        {
            var projectFolder = new DirectoryInfo(TestContext.CurrentContext.TestDirectory);
            while (projectFolder.Name != "Pantrymo.Tests")
                projectFolder = projectFolder.Parent;
           
            var mock = Substitute.For<ISettingsService>();
            mock.ConnectionString.Returns(@$"Data Source={projectFolder.FullName}\TestData\PantrymoDB.db");
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
