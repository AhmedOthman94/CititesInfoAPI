using CititesInfoAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CititesInfoAPI.Controllers
{
	[Route("api/cities/{cityId}/pointsofinterest")]
	[ApiController]
	public class PointsOfInterestController : ControllerBase
	{
		[HttpGet()]
		public ActionResult<IEnumerable<PointOfInterestDto>> GetPointsOfInterest(int cityId)
		{
			var city = CitiesDataStore.Current.Cities
						.FirstOrDefault(x => x.Id == cityId);
			if (city is null) 
			{
				return NotFound();
			}
			return Ok(city.PointsOfInterest);
		}

		[HttpGet("{pointOfInterestId}")]
		public ActionResult<PointOfInterestDto> GetPointOfInterest(int cityId,
					int pointOfInterestId)
		{
			var city = CitiesDataStore.Current.Cities
							.FirstOrDefault(x => x.Id == cityId);
			if (city is null)
			{
				return NotFound();
			}

			var pointsOfInterest = city.PointsOfInterest.FirstOrDefault(p => p.Id == pointOfInterestId);
			if (pointsOfInterest is null)
			{
				return NotFound();
			}

			return Ok(pointsOfInterest);
		}
	}
}
