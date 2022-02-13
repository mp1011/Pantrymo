using MediatR;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using NUnit.Framework;
using Pantrymo.Application.Models;
using Pantrymo.Application.Models.AppModels;
using Pantrymo.Application.Services;
using Pantrymo.ClientInfrastructure;
using Pantrymo.ClientInfrastructure.Services;
using Pantrymo.Domain.Models;
using Pantrymo.Domain.Services;
using Pantrymo.ServerInfrastructure;
using Pantrymo.ServerInfrastructure.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Pantrymo.Tests
{
    internal class MockHelper
    {
        private readonly TestEnvironment _testEnvironment;
        
        public MockHelper(TestEnvironment testEnvironment)
        {
            _testEnvironment = testEnvironment;
            if(_testEnvironment == TestEnvironment.Sqlite)
                CopyLocalDBIfNeeded();
        }

        public List<ISite> MockRemoteSites { get; } = new List<ISite>();


        private void CopyLocalDBIfNeeded()
        {
            var projectFolder = GetProjectFolder();
            var testDataFile = new FileInfo($@"{projectFolder.FullName}\TestData\PantrymoDB.db");
            var originalTestDataFile = new FileInfo($@"{projectFolder.FullName}\TestData\PantrymoDB_Original.db");

            if(!testDataFile.Exists
                || testDataFile.LastWriteTimeUtc > originalTestDataFile.LastWriteTimeUtc)
            {
                testDataFile = originalTestDataFile.CopyTo(testDataFile.FullName, overwrite: true);
                testDataFile.LastWriteTimeUtc = originalTestDataFile.LastWriteTimeUtc;
            }
        }

        private DirectoryInfo GetProjectFolder()
        {
            var projectFolder = new DirectoryInfo(TestContext.CurrentContext.TestDirectory);
            while (projectFolder.Name != "Pantrymo.Tests")
                projectFolder = projectFolder.Parent;
            return projectFolder;
        }

        public IngredientSuggestionService CreateIngredientSuggestionService()
        {
            return new IngredientSuggestionService(CreateCategoryService());
        }

        public CategoryService CreateCategoryService()
        {
            return new CategoryService(CreateDataContext(), CreateFullHierarchyLoader(), CreateCacheService(), CreateMediator());
        }

        public IDataContext CreateDataContext()
        {
            if (_testEnvironment == TestEnvironment.Sqlite)
                return CreateSQLiteContext();
            else
                return new SqlServerDbContext(CreateSettingsService(), CreateObjectMapper(), new DbContextOptions<SqlServerDbContext>());
        }

        private SqliteDbContext _sqliteContext;
        public SqliteDbContext CreateSQLiteContext()
        {
            return _sqliteContext ?? (_sqliteContext = new SqliteDbContext(CreateSettingsService(),CreateObjectMapper(), new DbContextOptions<SqliteDbContext>()));
        }

        public IObjectMapper CreateObjectMapper()
        {
            return new ReflectionObjectMapper(CreateExceptionHandler());
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
            var projectFolder = GetProjectFolder();
           
            var mock = Substitute.For<ISettingsService>();

            if(_testEnvironment == TestEnvironment.Sqlite)
                mock.ConnectionString.Returns(@$"Data Source={projectFolder.FullName}\TestData\PantrymoDB.db");
            else
                mock.ConnectionString.Returns(@$"Server=localhost;Database=PantryMoDB;Trusted_Connection=True;");

            mock.LocalDataFolder.Returns(@$"{projectFolder.FullName}\TestData");
            mock.GetCacheDuration<Category>().ReturnsForAnyArgs(callInfo=> TimeSpan.Zero);

            return mock;
        }

        public ICacheService CreateCacheService()
        {
            var mock = Substitute.For<ICacheService>();
            return mock;
        }

        public IRecipeSearchService CreateRecipeSearchService()
        {
            return new LocalRecipeSearchService(
                CreateComponentSearchService(),
                CreateCuisineSearchService(),
                CreateRecipeSearchProvider(),
                CreateDataContext());
        }

        public ISearchService<IComponent> CreateComponentSearchService()
        {
            return new BasicComponentSearchService(CreateDataContext());
        }

        public ISearchService<ICuisine> CreateCuisineSearchService()
        {
            return new BasicCuisineSearchService(CreateDataContext());
        }

        public IRecipeSearchProvider CreateRecipeSearchProvider()
        {
            if (_testEnvironment == TestEnvironment.Sqlite)
                return new InMemoryRecipeSearchProvider(CreateDataContext(), CreateCategoryService());
            else
                return new DbRecipeSearchProvider(CreateDataContext() as SqlServerDbContext);
        }

        public IDataSyncService CreateDataSyncService()
        {
            return new PantrymoDataSyncService(CreateMediator(), CreateMockRemoteAccess(), CreateDataContext(), CreateExceptionHandler());
        }

        public IMediator CreateMediator()
        {
            return Substitute.For<IMediator>();
        }

        public IExceptionHandler CreateExceptionHandler() => new FailTestExceptionHandler();
        
        public IDataAccess CreateMockRemoteAccess()
        {
            var mock = Substitute.For<IDataAccess>();

            mock.GetRecordsByDate<IAlternateComponentName>(Arg.Any<DateTime>()).Returns(Task.FromResult(Result.Success(new IAlternateComponentName[] { })));
            mock.GetRecordsByDate<IComponent>(Arg.Any<DateTime>()).Returns(Task.FromResult(Result.Success(new IComponent[] { })));
            mock.GetRecordsByDate<IAuthor>(Arg.Any<DateTime>()).Returns(Task.FromResult(Result.Success(new IAuthor[] { })));
            mock.GetRecordsByDate<IComponentNegativeRelation>(Arg.Any<DateTime>()).Returns(Task.FromResult(Result.Success(new IComponentNegativeRelation[] { })));
            mock.GetRecordsByDate<ICuisine>(Arg.Any<DateTime>()).Returns(Task.FromResult(Result.Success(new ICuisine[] { })));

            mock.GetChangedRecords<IRecipeDTO>(Arg.Any<RecordUpdateTimestamp[]>()).Returns(Task.FromResult(Result.Success(new IRecipeDTO[] { })));


            mock.GetRecordsByDate<ISite>(Arg.Any<DateTime>())
                .ReturnsForAnyArgs(c =>
                {
                    var date = c.Arg<DateTime>();
                    var result = MockRemoteSites
                        .Where(p => p.LastModified > date)
                        .ToArray();

                    return Task.FromResult(Result.Success(result));
                });

            return mock;
        }
    }
}
