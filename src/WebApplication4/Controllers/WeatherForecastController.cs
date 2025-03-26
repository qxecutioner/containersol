using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebApplication1.Models;
using WebApplication4.Models;
using WebApplication4.Services.Interfaces;

namespace WebApplication4.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        private readonly IApiServices<TransferData> _services;

        private readonly AppSettings _settings;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IApiServices<TransferData> apiServices, IOptions<AppSettings> settings)
        {
            _logger = logger;
            _services = apiServices;
            _settings = settings.Value;
        }

        [HttpGet("weather",Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("data",Name = "GetData")]
        public async Task<TransferData> GetData()
        {
            Console.WriteLine("inside data");

            var res = Environment.GetEnvironmentVariables();

            foreach (var key in res)
            {
                Console.WriteLine($"{key} - {res[key]}");
            }

            return await _services.GetTransferData("http://dev-app1-service:8080/WeatherForecast");
        }
        
    }
}
