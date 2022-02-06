﻿using Pantrymo.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pantrymo.Tests
{
    internal class TestDataCreator
    {
        public static IEnumerable<ISite> CreateTestSites(int count)
        {
            foreach (var num in Enumerable.Range(0, count))
            {
                yield return new Site
                {
                    Name = $"Site {num}",
                    Url = "TEST",
                    LastModified = DateTime.Now.AddDays(-num)
                };
            }
        }
    }
}