using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoffeeApi.Tests.Models;
using Newtonsoft.Json;
using Xunit;

namespace CoffeeApi.Tests.IntegrationTests
{
    public class GetCoffeeRatingIntegrationTests : IClassFixture<WebApplicationWithDatabaseFixture>
    {
        private readonly WebApplicationWithDatabaseFixture fixture;

        public GetCoffeeRatingIntegrationTests(WebApplicationWithDatabaseFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public async Task GetCoffeeRatingIntegrationTests_Get_Coffee_Rating_Returns_Correct_Data()
        {
            // Arrange
            var client = this.fixture.factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/rating");

            var json = await response.Content.ReadAsStringAsync();

            var content = JsonConvert.DeserializeObject<List<CoffeeRatingRead>>(json);

            var expectedObject1 = new CoffeeRatingRead()
            {
                Id = 1,
                CoffeeId = 1,
                Rating = 5,
                Comment = "Great coffee"
            };

            var expectedObject2 = new CoffeeRatingRead()
            {
                Id = 2,
                CoffeeId = 1,
                Rating = 5,
                Comment = "My favorite"
            };

            // Assert
            response.EnsureSuccessStatusCode();

            var expectedObjectsReturned = 2;

            Assert.NotNull(content);
            Assert.NotEmpty(content);
            Assert.Equal(content.Count, expectedObjectsReturned);

            AssertCoffeeRatingEqual(content.ElementAt(0), expectedObject1);
            AssertCoffeeRatingEqual(content.ElementAt(1), expectedObject2);
        }

        private void AssertCoffeeRatingEqual(CoffeeRatingRead original, CoffeeRatingRead compareTo)
        {
            Assert.Equal(compareTo.Id, original.Id);
            Assert.Equal(compareTo.CoffeeId, original.CoffeeId);
            Assert.Equal(compareTo.Comment, original.Comment);
            Assert.Equal(compareTo.Rating, original.Rating);
        }
    }
}