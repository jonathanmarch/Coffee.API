using System;
using System.Collections.Generic;
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
    public class UpdateCoffeeRatingIntegrationTests : IClassFixture<WebApplicationWithDatabaseFixture>
    {
        private readonly WebApplicationWithDatabaseFixture fixture;

        public UpdateCoffeeRatingIntegrationTests(WebApplicationWithDatabaseFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public async Task UpdateCoffeeIntegrationTests_Put_Coffee_Rating_Returns_Correct_Data()
        {
            // Arrange
            var client = this.fixture.factory.CreateClient();

            // Act
            var coffeeRatingUpdate = new CoffeeRatingUpdate()
            {
                Id = 2,
                Comment = "Bad coffee, changed my mind.",
                Rating = 1
            };

            var payload = JsonConvert.SerializeObject(coffeeRatingUpdate);
            var putContent = new StringContent(payload, Encoding.UTF8, "application/json");

            var response = await client.PutAsync("/api/rating/2", putContent);

            // Assert
            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task UpdateCoffeeIntegrationTests_Put_Coffee_Rating_Returns_Not_Found_If_Entity_Does_Not_Exist()
        {
            // Arrange
            var client = this.fixture.factory.CreateClient();

            // Act
            var coffeeRatingUpdate = new CoffeeRatingUpdate()
            {
                Id = 55,
                Comment = "Bad coffee, changed my mind.",
                Rating = 1
            };

            var payload = JsonConvert.SerializeObject(coffeeRatingUpdate);
            var putContent = new StringContent(payload, Encoding.UTF8, "application/json");

            var response = await client.PutAsync("/api/rating/55", putContent);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task UpdateCoffeeIntegrationTests_Put_Coffee_Rating_Returns_Expected_All_Validation_Errors()
        {
            // Arrange
            var client = this.fixture.factory.CreateClient();

            // Act
            var coffeeRatingUpdate = new CoffeeRatingUpdate()
            {
                Id = 2,
                Comment = "",
                Rating = 99
            };

            var payload = JsonConvert.SerializeObject(coffeeRatingUpdate);
            var putContent = new StringContent(payload, Encoding.UTF8, "application/json");

            var response = await client.PutAsync("/api/rating/55", putContent);

            var json = await response.Content.ReadAsStringAsync();
            var content = JsonConvert.DeserializeObject<ValidationErrorsDto>(json);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotNull(content);
            Assert.Equal(4, content.ValidationErrors.Count());
        }

        [Fact]
        public async Task UpdateCoffeeIntegrationTests_Put_Coffee_Rating_Returns_Id_Validation_Errors()
        {
            // Arrange
            var client = this.fixture.factory.CreateClient();

            // Act
            var coffeeRatingUpdate = new CoffeeRatingUpdate()
            {
                Comment = "Valid comment",
                Rating = 5
            };

            var payload = JsonConvert.SerializeObject(coffeeRatingUpdate);
            var putContent = new StringContent(payload, Encoding.UTF8, "application/json");

            var response = await client.PutAsync("/api/rating/55", putContent);

            var json = await response.Content.ReadAsStringAsync();
            var content = JsonConvert.DeserializeObject<ValidationErrorsDto>(json);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            Assert.NotNull(content);
            Assert.Equal(2, content.ValidationErrors.Count());

            var firstValidationError = content.ValidationErrors.ElementAt(0);
            var secondValidationError = content.ValidationErrors.ElementAt(1);

            Assert.Equal("Id", firstValidationError.Field);
            Assert.Equal("Id is required.", firstValidationError.Message);
            Assert.Equal("Id", secondValidationError.Field);
            Assert.Equal("Id must match route parameter.", secondValidationError.Message);
        }

        [Fact]
        public async Task UpdateCoffeeIntegrationTests_Put_Coffee_Rating_Returns_Comment_Validation_Errors()
        {
            // Arrange
            var client = this.fixture.factory.CreateClient();

            // Act
            var coffeeRatingUpdate = new CoffeeRatingUpdate()
            {
                Id = 2,
                Comment = "",
                Rating = 1
            };

            var payload = JsonConvert.SerializeObject(coffeeRatingUpdate);
            var putContent = new StringContent(payload, Encoding.UTF8, "application/json");

            var response = await client.PutAsync("/api/rating/2", putContent);

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
        public async Task UpdateCoffeeIntegrationTests_Put_Coffee_Rating_Returns_Rating_Validation_Errors()
        {
            // Arrange
            var client = this.fixture.factory.CreateClient();

            // Act
            var coffeeRatingUpdate = new CoffeeRatingUpdate()
            {
                Id = 2,
                Comment = "Valid comment",
                Rating = 9999
            };

            var payload = JsonConvert.SerializeObject(coffeeRatingUpdate);
            var putContent = new StringContent(payload, Encoding.UTF8, "application/json");

            var response = await client.PutAsync("/api/rating/2", putContent);

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
