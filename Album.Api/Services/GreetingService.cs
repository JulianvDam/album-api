using System.Net;

public class GreetingService
{
    private readonly ILogger<GreetingService> _logger;

    public GreetingService(ILogger<GreetingService> logger)
    {
        _logger = logger;
    }

    public string GetGreeting(string name)
    {
        //logs that the method has been called
        _logger.LogInformation($"GreetingService: GetGreeting method called with name: {name} at {DateTime.UtcNow}");
        var startTime = DateTime.UtcNow;
        string hostname = Dns.GetHostName();

        if (string.IsNullOrWhiteSpace(name))
        {
            var greeting = $"Hello, World from {hostname}";
            var endTime = DateTime.UtcNow;
            _logger.LogInformation($"GreetingService: returning greeting: {greeting} at {DateTime.UtcNow}. Duration: {endTime-startTime}");

            return greeting;
        }
        else
        {
            var greeting = $"Hello, {name} from {hostname}";
            var endTime = DateTime.UtcNow;
            _logger.LogInformation($"GreetingService: returning greeting: {greeting} at {DateTime.UtcNow}. Duration: {endTime-startTime}");

            return greeting;
        }
    }
}