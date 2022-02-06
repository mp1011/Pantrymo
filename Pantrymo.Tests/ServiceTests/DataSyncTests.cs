using FluentAssertions;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Pantrymo.Tests.ServiceTests
{

    [TestFixture(TestEnvironment.Sqlite)]
    internal class DataSyncTests : EnvironmentTest
    {
        public DataSyncTests(TestEnvironment testEnvironment) : base(testEnvironment)
        {
        }

        [TestCase]
        public async Task CanSyncRecordsIntoEmptyDatabase()
        {
            var ctx = MockHelper.CreateSQLiteContext();
            ctx.Sites.RemoveRange(ctx.Sites);
            ctx.SaveChanges();

            ctx.Sites.Count().Should().Be(0);

            var syncService = MockHelper.CreateDataSyncService();
            MockHelper.MockRemoteSites.AddRange(TestDataCreator.CreateTestSites(10));
            var result = await syncService.ImmediateSync();
            result.Should().BeTrue();
            ctx.Sites.Count().Should().BeGreaterThan(0);
        }

        [TestCase]
        public async Task CanUpdateExistingRecords()
        {
            var ctx = MockHelper.CreateSQLiteContext();

            MockHelper.MockRemoteSites.AddRange(ctx.Sites.Take(2));

            var changeString = $"CHANGE {Guid.NewGuid()}";

            foreach(var site in MockHelper.MockRemoteSites)
            {
                site.LastModified = DateTime.Now;
                site.Url = changeString;
            }

            ctx.Sites.FirstOrDefault(p=>p.Url == changeString).Should().BeNull();
            var syncService = MockHelper.CreateDataSyncService();
            var result = await syncService.ImmediateSync();
            result.Should().BeTrue();

            ctx.Sites.Count(p => p.Url == changeString).Should().Be(2);
        }

    }
}
