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
    public class UpdateCoffeeIntegrationTests : IClassFixture<WebApplicationWithDatabaseFixture>
    {
        private readonly WebApplicationWithDatabaseFixture fixture;

        public UpdateCoffeeIntegrationTests(WebApplicationWithDatabaseFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public async Task UpdateCoffeeIntegrationTests_Put_Coffee_Returns_Correct_Data()
        {
            // Arrange
            var client = this.fixture.factory.CreateClient();

            // Act
            var coffeeItem = new CoffeeItemUpdate()
            {
                Id = 2,
                Name = "Updated Name",
                Description = "I updated this description",
                CaffeineContent = 55
            };

            var payload = JsonConvert.SerializeObject(coffeeItem);
            var putContent = new StringContent(payload, Encoding.UTF8, "application/json");

            var response = await client.PutAsync("/api/coffee/2", putContent);

            // Assert
            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }


        [Fact]
        public async Task UpdateCoffeeIntegrationTests_Put_Coffee_Returns_Not_Found_If_Entity_Does_Not_Exist()
        {
            // Arrange
            var client = this.fixture.factory.CreateClient();

            // Act
            var coffeeItem = new CoffeeItemUpdate()
            {
                Id = 99,
                Name = "Updated Name",
                Description = "I updated this description",
                CaffeineContent = 55
            };

            var payload = JsonConvert.SerializeObject(coffeeItem);
            var putContent = new StringContent(payload, Encoding.UTF8, "application/json");

            var response = await client.PutAsync("/api/coffee/99", putContent);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
        
        [Fact]
        public async Task UpdateCoffeeIntegrationTests_Put_Coffee_Returns_Expected_All_Validation_Errors()
        {
            // Arrange
            var client = this.fixture.factory.CreateClient();

            // Act
            var coffeeItem = new CoffeeItemUpdate()
            {
                Id = 2,
                Name = "",
                Description = "",
                CaffeineContent = 9999
            };

            var payload = JsonConvert.SerializeObject(coffeeItem);
            var putContent = new StringContent(payload, Encoding.UTF8, "application/json");

            var response = await client.PutAsync("/api/coffee/3", putContent);

            var json = await response.Content.ReadAsStringAsync();
            var content = JsonConvert.DeserializeObject<ValidationErrorsDto>(json);
            
            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotNull(content);
            Assert.Equal(6, content.ValidationErrors.Count());
        }

        [Fact]
        public async Task UpdateCoffeeIntegrationTests_Put_Coffee_Returns_Expected_Id_Validation_Errors()
        {
            // Arrange
            var client = this.fixture.factory.CreateClient();

            // Act
            var coffeeItem = new CoffeeItemUpdate()
            {
                Name = "Valid name",
                Description = "Valid description",
                CaffeineContent = 55
            };

            var payload = JsonConvert.SerializeObject(coffeeItem);
            var putContent = new StringContent(payload, Encoding.UTF8, "application/json");

            var response = await client.PutAsync("/api/coffee/2", putContent);

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
        public async Task UpdateCoffeeIntegrationTests_Put_Coffee_Returns_Expected_Name_Validation_Errors()
        {
            // Arrange
            var client = this.fixture.factory.CreateClient();

            // Act
            var coffeeItem = new CoffeeItemUpdate()
            {
                Id = 2,
                Name = "",
                Description = "Valid description",
                CaffeineContent = 55
            };

            var payload = JsonConvert.SerializeObject(coffeeItem);
            var putContent = new StringContent(payload, Encoding.UTF8, "application/json");

            var response = await client.PutAsync("/api/coffee/2", putContent);

            var json = await response.Content.ReadAsStringAsync();
            var content = JsonConvert.DeserializeObject<ValidationErrorsDto>(json);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            Assert.NotNull(content);
            Assert.Equal(2, content.ValidationErrors.Count());

            var firstValidationError = content.ValidationErrors.ElementAt(0);
            var secondValidationError = content.ValidationErrors.ElementAt(1);

            Assert.Equal("Name", firstValidationError.Field);
            Assert.Equal("Name is required.", firstValidationError.Message);
            Assert.Equal("Name", secondValidationError.Field);
            Assert.Equal("Name should be between 3 and 50 characters.", secondValidationError.Message);
        }

        [Fact]
        public async Task UpdateCoffeeIntegrationTests_Put_Coffee_Returns_Expected_Description_Validation_Errors()
        {
            // Arrange
            var client = this.fixture.factory.CreateClient();

            // Act
            var coffeeItem = new CoffeeItemUpdate()
            {
                Id = 2,
                Name = "Valid name",
                Description = "",
                CaffeineContent = 55
            };

            var payload = JsonConvert.SerializeObject(coffeeItem);
            var putContent = new StringContent(payload, Encoding.UTF8, "application/json");

            var response = await client.PutAsync("/api/coffee/2", putContent);

            var json = await response.Content.ReadAsStringAsync();
            var content = JsonConvert.DeserializeObject<ValidationErrorsDto>(json);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            Assert.NotNull(content);
            Assert.Equal(2, content.ValidationErrors.Count());

            var firstValidationError = content.ValidationErrors.ElementAt(0);
            var secondValidationError = content.ValidationErrors.ElementAt(1);


            Assert.Equal("Description", firstValidationError.Field);
            Assert.Equal("Description is required.", firstValidationError.Message);
            Assert.Equal("Description", secondValidationError.Field);
            Assert.Equal("Description should be between 3 and 150 characters.", secondValidationError.Message);
        }

        [Fact]
        public async Task UpdateCoffeeIntegrationTests_Put_Coffee_Returns_Expected_Caffeine_Content_Validation_Errors()
        {
            // Arrange
            var client = this.fixture.factory.CreateClient();

            // Act
            var coffeeItem = new CoffeeItemUpdate()
            {
                Id = 2,
                Name = "Valid name",
                Description = "Valid description",
                CaffeineContent = 99999
            };

            var payload = JsonConvert.SerializeObject(coffeeItem);
            var putContent = new StringContent(payload, Encoding.UTF8, "application/json");

            var response = await client.PutAsync("/api/coffee/2", putContent);

            var json = await response.Content.ReadAsStringAsync();
            var content = JsonConvert.DeserializeObject<ValidationErrorsDto>(json);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            Assert.NotNull(content);
            Assert.Equal(1, content.ValidationErrors.Count());

            var firstValidationError = content.ValidationErrors.ElementAt(0);

            Assert.Equal("CaffeineContent", firstValidationError.Field);
            Assert.Equal("Caffeine Content should be between 5 and 400.", firstValidationError.Message);
        }
    }
}
