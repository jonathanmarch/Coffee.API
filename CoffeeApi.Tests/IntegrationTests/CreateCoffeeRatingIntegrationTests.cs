
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Coffee.API.Models;
using CoffeeApi.Tests.Models;
using Newtonsoft.Json;
using Xunit;

namespace CoffeeApi.Tests.IntegrationTests
{
    public class CreateCoffeeRatingIntegrationTests : IClassFixture<WebApplicationWithDatabaseFixture>
    {
        private readonly WebApplicationWithDatabaseFixture fixture;

        public CreateCoffeeRatingIntegrationTests(WebApplicationWithDatabaseFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public async Task CreateCoffeeRatingIntegrationTests_Post_Coffee_Rating_Returns_Correct_Data()
        {
            // Arrange
            var client = this.fixture.factory.CreateClient();

            // Act
            var coffeeItem = new CoffeeRatingCreate()
            {
                CoffeeId = 2,
                Comment = "Amazing coffee, 5 stars.",
                Rating = 5
            };

            var payload = JsonConvert.SerializeObject(coffeeItem);
            var postContent = new StringContent(payload, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/api/rating", postContent);

            var json = await response.Content.ReadAsStringAsync();

            var content = JsonConvert.DeserializeObject<CreatedRead>(json);

            // Assert
            response.EnsureSuccessStatusCode();

            var nextAvailableIdInDatabase = 3;

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Equal(nextAvailableIdInDatabase, content.Id);
        }

        [Fact]
        public async Task CreateCoffeeRatingIntegrationTests_Post_Coffee_Rating_Returns_Correct_If_Coffee_Entity_Not_Found()
        {
            // Arrange
            var client = this.fixture.factory.CreateClient();

            // Act
            var coffeeItem = new CoffeeRatingCreate()
            {
                CoffeeId = 99,
                Comment = "Amazing coffee, 5 stars.",
                Rating = 5
            };

            var payload = JsonConvert.SerializeObject(coffeeItem);
            var postContent = new StringContent(payload, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/api/rating", postContent);

            var json = await response.Content.ReadAsStringAsync();

            var content = JsonConvert.DeserializeObject<CreatedRead>(json);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task CreateCoffeeRatingIntegrationTests_Post_Coffee_Rating_Returns_Expected_All_Validation_Errors()
        {
            // Arrange
            var client = this.fixture.factory.CreateClient();

            // Act
            var coffeeItem = new CoffeeRatingCreate()
            {
                CoffeeId = 0,
                Comment = "",
                Rating = 10
            };

            var payload = JsonConvert.SerializeObject(coffeeItem);
            var postContent = new StringContent(payload, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/api/rating", postContent);

            var json = await response.Content.ReadAsStringAsync();

            var content = JsonConvert.DeserializeObject<ValidationErrorsDto>(json);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            Assert.NotNull(content);
            Assert.Equal(4, content.ValidationErrors.Count());
        }


        [Fact]
        public async Task CreateCoffeeRatingIntegrationTests_Post_Coffee_Rating_Returns_Expected_Coffee_Id_Validation_Errors()
        {
            // Arrange
            var client = this.fixture.factory.CreateClient();

            // Act
            var coffeeItem = new CoffeeRatingCreate()
            {
                Comment = "Valid comment",
                Rating = 5
            };

            var payload = JsonConvert.SerializeObject(coffeeItem);
            var postContent = new StringContent(payload, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/api/rating", postContent);

            var json = await response.Content.ReadAsStringAsync();

            var content = JsonConvert.DeserializeObject<ValidationErrorsDto>(json);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            Assert.NotNull(content);
            Assert.Equal(1, content.ValidationErrors.Count());

            var firstValidationError = content.ValidationErrors.ElementAt(0);

            Assert.Equal("CoffeeId", firstValidationError.Field);
            Assert.Equal("Coffee Id is required.", firstValidationError.Message);
        }

        [Fact]
        public async Task CreateCoffeeRatingIntegrationTests_Post_Coffee_Rating_Returns_Expected_Comment_Validation_Errors()
        {
            // Arrange
            var client = this.fixture.factory.CreateClient();

            // Act
            var coffeeItem = new CoffeeRatingCreate()
            {
                CoffeeId = 2,
                Comment = "",
                Rating = 5
            };

            var payload = JsonConvert.SerializeObject(coffeeItem);
            var postContent = new StringContent(payload, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/api/rating", postContent);

            var json = await response.Content.ReadAsStringAsync();

            var content = JsonConvert.DeserializeObject<ValidationErrorsDto>(json);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            Assert.NotNull(content);
            Assert.Equal(2, content.ValidationErrors.Count());

            var firstValidationError = content.ValidationErrors.ElementAt(0);
            var secondValidationError = content.ValidationErrors.ElementAt(1);

            Assert.Equal("Comment", firstValidationError.Field);
            Assert.Equal("Comment is required.", firstValidationError.Message);
            Assert.Equal("Comment", secondValidationError.Field);
            Assert.Equal("Comment should be between 5 and 150 characters.", secondValidationError.Message);
        }

        [Fact]
        public async Task CreateCoffeeRatingIntegrationTests_Post_Coffee_Rating_Returns_Expected_Rating_Validation_Errors()
        {
            // Arrange
            var client = this.fixture.factory.CreateClient();

            // Act
            var coffeeItem = new CoffeeRatingCreate()
            {
                CoffeeId = 2,
                Comment = "Valid comment",
                Rating = 999
            };

            var payload = JsonConvert.SerializeObject(coffeeItem);
            var postContent = new StringContent(payload, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/api/rating", postContent);

            var json = await response.Content.ReadAsStringAsync();

            var content = JsonConvert.DeserializeObject<ValidationErrorsDto>(json);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            Assert.NotNull(content);
            Assert.Equal(1, content.ValidationErrors.Count());

            var firstValidationError = content.ValidationErrors.ElementAt(0);

            Assert.Equal("Rating", firstValidationError.Field);
            Assert.Equal("Rating must be between 1 and 5.", firstValidationError.Message);
        }
    }
}
