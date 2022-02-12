using Pantrymo.Application.Models;
using Pantrymo.SqlInfrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pantrymo.Tests
{
    internal class TestDataCreator
    {
        public static IEnumerable<ISite> CreateTestSites(int count, bool assignIds)
        {
            foreach (var num in Enumerable.Range(0, count))
            {
                yield return new Site
                {
                    Id = assignIds ? num+1 : 0,
                    Name = $"Site {num}",
                    Url = "TEST",
                    LastModified = DateTime.Now.AddDays(-num)
                };
            }
        }
    }
}
