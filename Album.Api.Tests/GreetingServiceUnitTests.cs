using Xunit;
using Microsoft.Extensions.Logging;
using System;

namespace Album.Api.Tests;
public class FakeLogger<T> : ILogger<T>
{
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        // Do nothing, as this is a fake logger
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return false; // Not enabled for logging
    }

    public IDisposable BeginScope<TState>(TState state)
    {
        return null; // No scope in this fake logger
    }
}

public class GreetingServiceUnitTests
{
    [Fact]
    public void GetGreeting_WithName_ReturnsHelloName()
    {
        // Arrange
        var greetingService = new GreetingService(new FakeLogger<GreetingService>());

        // Act
        var result = greetingService.GetGreeting("John");

        // Assert
        Assert.Equal("Hello, John", result);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void GetGreeting_WithNullOrWhitespace_ReturnsHelloWorld(string name)
    {
        // Arrange
        var greetingService = new GreetingService(new FakeLogger<GreetingService>());

        // Act
        var result = greetingService.GetGreeting(name);

        // Assert
        Assert.Equal("Hello, World", result);
    }
}
