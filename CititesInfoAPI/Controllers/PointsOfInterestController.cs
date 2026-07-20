using AutoMapper;
using CititesInfoAPI.Models;
using CititesInfoAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;
using Microsoft.AspNetCore.Mvc;

namespace CititesInfoAPI.Controllers
{
	[Route("api/cities/{cityId}/pointsofinterest")]
	[ApiController]
	public class PointsOfInterestController(ILogger<PointsOfInterestController> logger,
			IMailServices mailServices,
			ICityInfoRepository cityInfoRepository,
			IMapper mapper) 
			: ControllerBase
	{
		[HttpGet()]
		public async Task<ActionResult<IEnumerable<PointOfInterestDto>>> GetPointsOfInterest(int cityId)
		{
			if (!await cityInfoRepository.CityExistsAsync(cityId))
			{
				logger.LogInformation("City with ID: {cityId} is not exist", cityId);
				return NotFound();
			}

			var pointsOfInterestForCity = await cityInfoRepository
						.GetPointsOfInetestForCityAsync(cityId);

			return Ok(mapper.Map<IEnumerable<PointOfInterestDto>>(pointsOfInterestForCity));
		}

		[HttpGet("{pointOfInterestId}", Name = "GetPointOfInterest")]
		public async Task<ActionResult<PointOfInterestDto>> GetPointOfInterest(int cityId,
					int pointOfInterestId)
		{
			if (!await cityInfoRepository.CityExistsAsync(cityId))
			{
				return NotFound();
			}

			var pointOfInterest = await cityInfoRepository
						.GetPointOfInterestForCityAsync(cityId, pointOfInterestId);
			if (pointOfInterest is null)
			{
				return NotFound();
			}

			return Ok(mapper.Map<PointOfInterestDto>(pointOfInterest));
		}

		//[HttpPost]
		//public ActionResult<PointOfInterestDto> CreatePointOfInterest(int cityId, 
		//			[FromBody] PointOfInterestForCreationDto pointOfInterest)
		//{

		//	var city = citiesDataStore.Cities
		//				.FirstOrDefault(c => c.Id == cityId);
		//	if (city is null)
		//	{
		//		return NotFound();
		//	}

		//	var maxPointOfInterest = citiesDataStore.Cities
		//			.SelectMany(c => c.PointsOfInterest).Max(p => p.Id);
		//	var finalPointOfInterest = new PointOfInterestDto
		//	{ 
		//		Id = ++maxPointOfInterest,
		//		Name = pointOfInterest.Name,
		//		Description = pointOfInterest.Description
		//	};
		//	city.PointsOfInterest.Add(finalPointOfInterest);

		//	return CreatedAtRoute("GetPointOfInterest",
		//			new 
		//			{
		//				cityId,
		//				pointOfInterestId = finalPointOfInterest.Id
		//			},
		//			finalPointOfInterest);
		//}

		//[HttpDelete("{pointOfInterestId}")]
		//public ActionResult Delete(int cityId, int pointOfInterestId)
		//{
		//	var city = citiesDataStore.Cities
		//				.FirstOrDefault(c => c.Id == cityId);
		//	if (city is null)
		//	{
		//		return NotFound();	
		//	}

		//	var pointOfInterest = city.PointsOfInterest
		//				.FirstOrDefault(p => p.Id == pointOfInterestId);
		//	if (pointOfInterest is null)
		//	{
		//		return NotFound();
		//	}

		//	city.PointsOfInterest.Remove(pointOfInterest);
		//	mailServices.Send("Point of interest deleted",
		//		$"Point of interest {pointOfInterest.Name} with id {pointOfInterest.Id} was deleted");

		//	return NoContent();
		//}

		//[HttpPut("{pointOfInterestId}")]
		//public ActionResult<PointOfInterestDto> UpdatePointOfInterest(int cityId,
		//		int pointOfInterestId,
		//		pointOfInterestForUpdateDto pointOfInterest)
		//{
		//	var city = citiesDataStore.Cities
		//			.FirstOrDefault(c => c.Id == cityId);
		//	if ( city is null)
		//	{
		//		return NotFound();
		//	}

		//	var pointOfInterestFromStore = city.PointsOfInterest
		//			.FirstOrDefault(p => p.Id == pointOfInterestId);
		//	if (pointOfInterestFromStore is null)
		//	{
		//		return NotFound();
		//	}

		//	pointOfInterestFromStore.Name = pointOfInterest.Name;
		//	pointOfInterestFromStore.Description = pointOfInterest.Description;

		//	return NoContent();
		//}

		//[HttpPatch("{pointOfInterestId}")]
		//public ActionResult<PointOfInterestDto> PartiallyUpdatePointOfInterest(int cityId,
		//		int pointOfInterestId,
		//		JsonPatchDocument<pointOfInterestForUpdateDto> patchDocument)
		//{
		//	var city = citiesDataStore.Cities
		//			.FirstOrDefault (c => c.Id == cityId);
		//	if ( city is null)
		//	{
		//		return NotFound();
		//	}

		//	var pointOfInterestFromStore = city.PointsOfInterest
		//			.FirstOrDefault(p => p.Id == pointOfInterestId);
		//	if ( pointOfInterestFromStore is null)
		//	{
		//		 return NotFound();
		//	}

		//	var pointOfInterestToPatch = new pointOfInterestForUpdateDto
		//	{
		//		Name = pointOfInterestFromStore.Name,
		//		Description = pointOfInterestFromStore.Description
		//	};

		//	patchDocument.ApplyTo(pointOfInterestToPatch, jsonPatchError => 
		//	{
		//		var key = jsonPatchError.AffectedObject.GetType().Name;
		//		ModelState.AddModelError(key, jsonPatchError.ErrorMessage); 
		//	});

		//	if (!ModelState.IsValid)
		//	{
		//		return BadRequest(ModelState);
		//	}

		//	if (!TryValidateModel(pointOfInterestToPatch))
		//	{
		//		return BadRequest(ModelState);
		//	}

		//	pointOfInterestFromStore.Name = pointOfInterestToPatch.Name;
		//	pointOfInterestFromStore.Description = pointOfInterestToPatch.Description;

		//	return NoContent();
		//}
	}
}
