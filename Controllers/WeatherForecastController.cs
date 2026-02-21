using Microsoft.AspNetCore.Mvc;

namespace WebApi2026.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries =
        [
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        ];

        private static WeatherForecast[] ListWeatherForecast;

        //Constructor to initialize the ListWeatherForecast with random data
        public WeatherForecastController()
        {    
            ListWeatherForecast = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return ListWeatherForecast;
        }
        // Get a specific WeatherForecast by id (index in the array)
        [HttpGet]
        [Route("{id}")]
        public ActionResult<WeatherForecast> GetById(int id)
        {
            if (id>5 || id<0)
            {
                return BadRequest();
            }
            return Ok(ListWeatherForecast[id]);
        }
    }
}
