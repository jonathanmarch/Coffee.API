using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using CoffeeApi.Tests.Models;
using Newtonsoft.Json;
using Xunit;

namespace CoffeeApi.Tests.IntegrationTests
{
    public class GetCoffeeIntegrationTests : IClassFixture<WebApplicationWithDatabaseFixture>
    {
        private readonly WebApplicationWithDatabaseFixture fixture;
        
        public GetCoffeeIntegrationTests(WebApplicationWithDatabaseFixture fixture)
        {
            this.fixture = fixture;
        }
        
        [Fact]
        public async Task GetCoffeeIntegrationTests_Get_Coffee_Returns_Correct_Data()
        {
            // Arrange
            var client = this.fixture.factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/coffee");

            var json = await response.Content.ReadAsStringAsync();

            var content =  JsonConvert.DeserializeObject<List<CoffeeItemRead>>(json);

            // Assert
            response.EnsureSuccessStatusCode();

            var expectedObjectsReturned = 3;

            Assert.NotNull(content);
            Assert.NotEmpty(content);
            Assert.Equal(content.Count, expectedObjectsReturned);
        }

        [Fact]
        public async Task GetCoffeeIntegrationTests_Get_Single_Coffee_Returns_Correct_Data()
        {
            // Arrange
            var client = this.fixture.factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/coffee/2");

            var json = await response.Content.ReadAsStringAsync();

            var content = JsonConvert.DeserializeObject<CoffeeItemRead>(json);

            // Assert
            response.EnsureSuccessStatusCode();

            var expectedObject = new CoffeeItemRead()
            {
                Id = 2,
                Name = "Americano",
                Description = "About the Americano",
                CaffeineContent = 50,
                Comments = new List<string>(),
                AverageRating = null,
                TotalRatings = 0
            };
            
            Assert.NotNull(content);
            AssertCoffeeItemEqual(content, expectedObject);
        }

        [Fact]
        public async Task GetCoffeeIntegrationTests_Get_Single_Coffee_Returns_Not_Found_When_Id_Does_Not_Exist()
        {
            // Arrange
            var client = this.fixture.factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/coffee/999");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        private void AssertCoffeeItemEqual(CoffeeItemRead original, CoffeeItemRead compareTo)
        {
            Assert.Equal(compareTo.Id, original.Id);
            Assert.Equal(compareTo.Description, original.Description);
            Assert.Equal(compareTo.CaffeineContent, original.CaffeineContent);
            Assert.Equal(compareTo.Comments, original.Comments);
            Assert.Equal(compareTo.AverageRating, original.AverageRating);
            Assert.Equal(compareTo.TotalRatings, original.TotalRatings);
        }
    }
}
