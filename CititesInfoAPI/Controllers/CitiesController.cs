using AutoMapper;
using CititesInfoAPI.Entities;
using CititesInfoAPI.Models;
using CititesInfoAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CititesInfoAPI.Controllers
{
	[ApiController]
	[Route("api/cities")]
	public class CitiesController(ICityInfoRepository cityInfoRepository,
			IMapper mapper)
			: ControllerBase
	{
		[HttpGet()]
		public async Task<ActionResult<IEnumerable<CityWithoutPointsOfInterestDto>>> GetCities(
				[FromQuery]string? name,
				[FromQuery]string? searchQuery,
				CancellationToken cancellationToken = default)
		{
			var cityEntities = await cityInfoRepository.GetCitiesReadOnlyAsync(name, 
					searchQuery,
					cancellationToken);

			return Ok(mapper.Map<IEnumerable<CityWithoutPointsOfInterestDto>>(cityEntities));
		}


		[HttpGet("{id}")]
		public async Task<IActionResult> GetCity(int id,
					bool includePointsOfInterest = false)
		{
			var city = await cityInfoRepository.GetCityAsync(id,
						includePointsOfInterest);

			if (city is null)
			{
				return NotFound();
			}

			if (includePointsOfInterest) 
			{
				return Ok(mapper.Map<CityDto>(city));
			}

			return Ok(mapper.Map<CityWithoutPointsOfInterestDto>(city));
		}
	}
}
