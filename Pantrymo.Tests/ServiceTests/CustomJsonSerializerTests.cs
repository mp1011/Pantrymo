#nullable disable

using NUnit.Framework;
using Pantrymo.Application.Models;
using Pantrymo.Domain.Services;
using Pantrymo.SqlInfrastructure.Models;
using System;
using System.Text.Json;

namespace Pantrymo.Tests.ServiceTests
{
    public class CustomJsonSerializerTests
    {
        private CustomJsonSerializer CreateSerializer()
        {
            return new CustomJsonSerializer(typeof(IRecipe).Assembly, typeof(Recipe).Assembly);
        }

        [Test]
        public void CanDeserializeJsonAsInterface()
        {
            var serializer = CreateSerializer();
            var recipe = new Recipe
            {
                Id = 1,
                Title = "Test"
            };

            var json = serializer.Serialize(recipe);
            var deserialized = serializer.Deserialize<IRecipe>(json);
            Assert.AreEqual(recipe.Title, deserialized.Title);
        }


        [Test]
        public void CanDeserializeJsonArrayAsInterface()
        {
            var serializer = CreateSerializer();
            var recipes = new Recipe[]
            {
                new Recipe
                {
                    Id = 1,
                    Title = "Test"
                },
                new Recipe
                {
                    Id = 2,
                    Title = "Test2"
                },
            };

            var json = serializer.Serialize(recipes);
            var deserialized = serializer.DeserializeArray<IRecipe>(json);
            Assert.AreEqual(recipes[0].Title, deserialized[0].Title);
            Assert.AreEqual(recipes[1].Title, deserialized[1].Title);
        }

        [Test]
        public void ProperExceptionIsThrownIfTypeHasNoImplementation()
        {
            var serializer = CreateSerializer();
            try
            {
                serializer.Deserialize<IDummyInterface>("");
                Assert.Fail();
            }
            catch(Exception e)
            {
                Assert.AreEqual(e.Message, "There is no implementation for type IDummyInterface");
            }
        }

        public interface IDummyInterface { }
    }
}
