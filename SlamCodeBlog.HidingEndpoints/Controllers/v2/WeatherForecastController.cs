using Microsoft.AspNetCore.Mvc;
using SlamCodeBlog.HidingEndpoints.Extensions;
using SlamCodeBlog.HidingEndpoints.Filter;

namespace SlamCodeBlog.HidingEndpoints.Controllers.v2
{
    [ApiController]
    [Route("api/v{version:apiVersion}")]
    [ApiVersion("2.0")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet("weather-forecast", Name = "GetWeatherForecastV2")]
        [ExcludeOnEnvironments("Development")]
        //[ApiExplorerSettings(IgnoreApi = true)]
        //[DevelopmentOnly]
        //[HideEndpoint]
        public IEnumerable<WeatherForecastV2> GetV2()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecastV2
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("weather-forecast-other", Name = "GetWeatherForecastOtherV2")]
        [ExcludeOnEnvironments("Production")]
        public IEnumerable<WeatherForecastV2> GetOtherV2()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecastV2
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}