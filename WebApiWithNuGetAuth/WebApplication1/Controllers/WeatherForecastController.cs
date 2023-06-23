using System.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace WebApplication1.Controllers;
[Authorize]
[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] _summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    public void Post()
    {
        SharedStuff.DatabaseStuff("Data Source=localhost;Database=StuffDB;Integrated Security=sspi;", "Data Source=SomeOtherServer.somewhere.test;Database=OtherStuffDB;Integrated Security=sspi;");
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        var user = HttpContext.User.Identity?.Name;
        if (SharedStuff.FileStuff().Any())
        {
            user += " - FILE";
        }

        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = _summaries[Random.Shared.Next(_summaries.Length)],
            User = user
        })
        .ToArray();
    }
}
