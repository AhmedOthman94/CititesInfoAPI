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

		[HttpGet("{pointOfInterestId}", Name = "GetPointOfInterest")]
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

		[HttpPost]
		public ActionResult<PointOfInterestDto> CreatePointOfInterest(int cityId, 
					[FromBody] PointOfInterestForCreationDto pointOfInterest)
		{

			var city = CitiesDataStore.Current.Cities
						.FirstOrDefault(c => c.Id == cityId);
			if (city is null)
			{
				return NotFound();
			}

			var maxPointOfInterest = CitiesDataStore.Current.Cities
					.SelectMany(c => c.PointsOfInterest).Max(p => p.Id);
			var finalPointOfInterest = new PointOfInterestDto
			{ 
				Id = ++maxPointOfInterest,
				Name = pointOfInterest.Name,
				Description = pointOfInterest.Description
			};
			city.PointsOfInterest.Add(finalPointOfInterest);

			return CreatedAtRoute("GetPointOfInterest",
					new 
					{
						cityId,
						pointOfInterestId = finalPointOfInterest.Id
					},
					finalPointOfInterest);
		}

		[HttpDelete("{pointOfInterestId}")]
		public ActionResult Delete(int cityId, int pointOfInterestId)
		{
			var city = CitiesDataStore.Current.Cities
						.FirstOrDefault(c => c.Id == cityId);
			if (city is null)
			{
				return NotFound();	
			}

			var pointOfInterest = city.PointsOfInterest
						.FirstOrDefault(p => p.Id == pointOfInterestId);
			if (pointOfInterest is null)
			{
				return NotFound();
			}

			city.PointsOfInterest.Remove(pointOfInterest);

			return NoContent();
		}
	}
}
