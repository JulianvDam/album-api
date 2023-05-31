using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using  Microsoft.AspNetCore.Mvc.Testing;
namespace Album.Api.IntegrationTests
{
    public class GreetingServiceIntegrationTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly WebApplicationFactory<Program> _factory;

        public GreetingServiceIntegrationTest(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
            _factory = factory;

        }

        [Fact]
        public async Task GetHello_WithValidName_ReturnsHelloName()
        {
            // Arrange
            var expectedResponse = "{\"result\":\"Hello, John\"}";

            // Act
            var response = await _client.GetAsync("/api/hello?name=John");
            var responseContent = await response.Content.ReadAsStringAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(expectedResponse, responseContent);
        }
    }
}