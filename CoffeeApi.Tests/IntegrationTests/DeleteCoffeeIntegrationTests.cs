using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CoffeeApi.Tests.Models;
using Newtonsoft.Json;
using Xunit;

namespace CoffeeApi.Tests.IntegrationTests
{
    public class DeleteCoffeeIntegrationTests : IClassFixture<WebApplicationWithDatabaseFixture>
    {
        private readonly WebApplicationWithDatabaseFixture fixture;

        public DeleteCoffeeIntegrationTests(WebApplicationWithDatabaseFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public async Task DeleteCoffeeIntegrationTests_Delete_Coffee_Returns_Correct_Data()
        {
            // Arrange
            var client = this.fixture.factory.CreateClient();

            // Act
            var response = await client.DeleteAsync("/api/coffee/3");

            var json = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }


        [Fact]
        public async Task DeleteCoffeeIntegrationTests_Delete_Coffee_Returns_Correct_Data_If_Entity_Does_Not_Exist()
        {
            // Arrange
            var client = this.fixture.factory.CreateClient();

            // Act
            var response = await client.DeleteAsync("/api/coffee/99999");

            var json = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
