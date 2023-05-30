using Microsoft.AspNetCore.Mvc;

namespace Album.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HelloController : ControllerBase
{
    private readonly GreetingService _greetingService;
    private readonly ILogger<HelloController> _logger;

    public HelloController(GreetingService greetingService, ILogger<HelloController> logger)
    {
        _greetingService = greetingService;
        _logger = logger;
    }

    public class HelloResponse {
        public string Result { get; set; }
    }

    [HttpGet]
    public ActionResult<HelloResponse> GetHello([FromQuery] string? name)
    {
        //logs that the method has been called
        _logger.LogInformation($"HelloController: GetHello method called with name: {name} at {DateTime.UtcNow}");

        //use greeting service to get the response (also timed)
        var startTime = DateTime.UtcNow;

        string greeting = _greetingService.GetGreeting(name);
        HelloResponse response = new HelloResponse();
        response.Result = greeting;

        var endTime = DateTime.UtcNow;

        //logs that the greeting is returned
        _logger.LogInformation($"HelloController: Greeting returned from GreetingService: {greeting} at {DateTime.UtcNow}. Duration: {endTime-startTime}");

        return response;
    }
}
