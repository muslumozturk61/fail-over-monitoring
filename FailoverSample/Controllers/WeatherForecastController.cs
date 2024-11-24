using Microsoft.AspNetCore.Mvc;

namespace FailoverSample.Controllers
{
    [ApiController]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IConfiguration _configuration;

        public WeatherForecastController(ILogger<WeatherForecastController> logger,IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpGet( "weather/{city}")]
        public async Task<IActionResult> Get([FromRoute] string city, [FromQuery] int days = 5)
        {
            if (Random.Shared.Next(1, 1000) == 61)
            {
                throw new Exception("Hatasız kul olmaz, hatamla sev beni :)");
            }

            var forecasts = Enumerable.Range(1, days).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();

            await Task.Delay(Random.Shared.Next(5, 100));

            return Ok(new WeatherForecastResponse
            {
                City = city,
                Forecasts = forecasts,
                ServiceName = _configuration["ServiceName"]!
            });
        }
    }

    public class WeatherForecastResponse
    {
        public string City { get; set; }
        public WeatherForecast[] Forecasts { get; set; }
        public string ServiceName { get; set; }
    }
}
