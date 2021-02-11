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
    public class CreateCoffeeIntegrationTests : IClassFixture<WebApplicationWithDatabaseFixture>
    {
        private readonly WebApplicationWithDatabaseFixture fixture;
        
        public CreateCoffeeIntegrationTests(WebApplicationWithDatabaseFixture fixture)
        {
            this.fixture = fixture;
        }
        
        [Fact]
        public async Task CreateCoffeeIntegrationTests_Post_Coffee_Returns_Correct_Data()
        {
            // Arrange
            var client = this.fixture.factory.CreateClient();

            // Act
            var coffeeItem = new CoffeeItemCreate()
            {
                Name = "New Coffee",
                Description = "New coffee made by me",
                CaffeineContent = 150
            };

            var payload = JsonConvert.SerializeObject(coffeeItem);
            var postContent = new StringContent(payload, Encoding.UTF8, "application/json");
            
            var response = await client.PostAsync("/api/coffee", postContent);

            var json = await response.Content.ReadAsStringAsync();

            var content = JsonConvert.DeserializeObject<CreatedRead>(json);
            
            // Assert
            response.EnsureSuccessStatusCode();

            var nextAvailableIdInDatabase = 4;
            
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Equal(nextAvailableIdInDatabase, content.Id);
        }

        [Fact]
        public async Task CreateCoffeeIntegrationTests_Post_Coffee_Returns_Expected_All_Validation_Errors()
        {
            // Arrange
            var client = this.fixture.factory.CreateClient();

            // Act
            var coffeeItem = new CoffeeItemCreate()
            {
                Name = "",
                Description = "",
                CaffeineContent = 9999
            };

            var payload = JsonConvert.SerializeObject(coffeeItem);
            var postContent = new StringContent(payload, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/api/coffee", postContent);

            var json = await response.Content.ReadAsStringAsync();
            var content = JsonConvert.DeserializeObject<ValidationErrorsDto>(json);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            Assert.NotNull(content);
            Assert.Equal(5, content.ValidationErrors.Count());
        }
        
        [Fact]
        public async Task CreateCoffeeIntegrationTests_Post_Coffee_Returns_Expected_Name_Validation_Errors()
        {
            // Arrange
            var client = this.fixture.factory.CreateClient();

            // Act
            var coffeeItem = new CoffeeItemCreate()
            {
                Name = "",
                Description = "Valid description",
                CaffeineContent = 50
            };

            var payload = JsonConvert.SerializeObject(coffeeItem);
            var postContent = new StringContent(payload, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/api/coffee", postContent);

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
        public async Task CreateCoffeeIntegrationTests_Post_Coffee_Returns_Expected_Description_Validation_Errors()
        {
            // Arrange
            var client = this.fixture.factory.CreateClient();

            // Act
            var coffeeItem = new CoffeeItemCreate()
            {
                Name = "Valid name",
                Description = "",
                CaffeineContent = 50
            };

            var payload = JsonConvert.SerializeObject(coffeeItem);
            var postContent = new StringContent(payload, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/api/coffee", postContent);

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
        public async Task CreateCoffeeIntegrationTests_Post_Coffee_Returns_Expected_Caffeine_Content_Validation_Errors()
        {
            // Arrange
            var client = this.fixture.factory.CreateClient();

            // Act
            var coffeeItem = new CoffeeItemCreate()
            {
                Name = "Valid name",
                Description = "Valid Description",
                CaffeineContent = 9999
            };

            var payload = JsonConvert.SerializeObject(coffeeItem);
            var postContent = new StringContent(payload, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/api/coffee", postContent);

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
