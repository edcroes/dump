using System.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        var connectionString1 = "Data Source=localhost;Database=StuffDB;Integrated Security=sspi;";
        var connectionString2 = "Data Source=SomeOtherServer.somewhere.test;Database=OtherStuffDB;Integrated Security=sspi;";

        using (var scope = new System.Transactions.TransactionScope())
        {
            using (var connection = new SqlConnection(connectionString1))
            {
                connection.Open();

                // Mutate table 1
                using var command1 = connection.CreateCommand();
                command1.CommandText = "UPDATE Table1 SET Column1 = 'New Value' WHERE Id = 1";
                command1.ExecuteNonQuery();
            }

            using (var connection = new SqlConnection(connectionString2))
            {
                connection.Open();

                // Mutate table 2
                using var command2 = connection.CreateCommand();
                command2.CommandText = "UPDATE Table2 SET Column1 = 'New Value' WHERE Id = 1";
                command2.ExecuteNonQuery();
            }

            // Complete the distributed transaction
            scope.Complete();
        }
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        var user = HttpContext.User.Identity?.Name;
        if (System.IO.File.Exists("C:\\temp\\file.txt"))
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
