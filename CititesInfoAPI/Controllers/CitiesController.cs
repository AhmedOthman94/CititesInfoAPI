using CititesInfoAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CititesInfoAPI.Controllers
{
	[ApiController]
	[Route("api/cities")]
	public class CitiesController(CitiesDataStore citiesDataStore)
			: ControllerBase
	{
		[HttpGet()]
		public ActionResult<IEnumerable<CityDto>> GetCities()
		{
			return Ok(citiesDataStore.Cities);
		}

		[HttpGet("{id}")]
		public ActionResult<CityDto> GetCity(int id)
		{
			var city = citiesDataStore.Cities
							.FirstOrDefault(x => x.Id == id);

			if (city is null)
			{
				return NotFound();
			}

			return Ok(city);
		}
	}
}
