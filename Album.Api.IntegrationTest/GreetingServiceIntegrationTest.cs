using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using System.Text.Json;
using System;


namespace Album.Api.Tests
{
    public class GreetingServiceIntegrationTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public GreetingServiceIntegrationTest(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetGreeting_WithName_ReturnsHelloName()
        {
            // Arrange
            var client = _factory.CreateClient();
            var name = "John";
            var expectedGreeting = $"Hello, {name}";

            // Act
            var response = await client.GetAsync($"/api/hello?name={name}");
            var content = await response.Content.ReadAsStringAsync();
            var greeting = JsonSerializer.Deserialize<HelloController.HelloResponse>(content)?.Result;

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(expectedGreeting, greeting);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task GetGreeting_WithNullOrWhitespace_ReturnsHelloWorld(string name)
        {
            // Arrange
            var client = _factory.CreateClient();
            var expectedGreeting = "Hello, World";

            // Act
            var response = await client.GetAsync($"/api/hello?name={name}");
            var content = await response.Content.ReadAsStringAsync();
            var greeting = JsonSerializer.Deserialize<HelloController.HelloResponse>(content)?.Result;

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(expectedGreeting, greeting);
        }
    }
}
