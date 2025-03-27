using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebApplication1.Models;
using Newtonsoft.Json;
using WebApplication1;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication1.Controllers
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

        private readonly AppSettings _appSettings;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IOptions<AppSettings> settings)
        {
            _logger = logger;
            _appSettings = settings.Value;
        }

        [Authorize]
        [HttpGet(Name = "GetWeatherForecast")]
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

        [HttpPost(Name = "CreateFile")]
        public async Task<IActionResult> Post([FromBody] ReqData reqData)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!Directory.Exists(_appSettings.DirPath))
            {
                Directory.CreateDirectory(_appSettings.DirPath);
            }

            await System.IO.File.WriteAllTextAsync(Path.Combine(_appSettings.DirPath, reqData.FileName), JsonConvert.SerializeObject(reqData));

            return Ok();
        }
    }
}

