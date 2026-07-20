using AutoMapper;
using CititesInfoAPI.Entities;
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

		[HttpPost]
		public async Task<ActionResult<PointOfInterestDto>> CreatePointOfInterest(int cityId,
					[FromBody] PointOfInterestForCreationDto pointOfInterest)
		{

			if (!await cityInfoRepository.CityExistsAsync(cityId))
			{
				return NotFound();
			}

			var finalPointOfInterest = mapper.Map<PointOfInterest>(pointOfInterest);
			await cityInfoRepository.AddPointOfInterestForCityAsyc(cityId,
					finalPointOfInterest);
			await cityInfoRepository.SaveChangesAsync();
			var createdPointOfInterestToReturn =
				mapper.Map<PointOfInterestDto>(finalPointOfInterest);

			return CreatedAtRoute("GetPointOfInterest",
					new 
					{
						cityId,
						pointOfInterestId = createdPointOfInterestToReturn.Id
					},
					createdPointOfInterestToReturn);
		}

		[HttpDelete("{pointOfInterestId}")]
		public async Task<ActionResult> Delete(int cityId, int pointOfInterestId)
		{
			if (!await cityInfoRepository.CityExistsAsync(cityId))
			{
				return NotFound();
			}

			var pointOfInterestEntity = await cityInfoRepository
					.GetPointOfInterestForCityAsync(cityId, pointOfInterestId);
			if (pointOfInterestEntity is null)
			{
				return NotFound();
			}

			cityInfoRepository.DeletePointOfInterest(pointOfInterestEntity);
			await cityInfoRepository.SaveChangesAsync();
			mailServices.Send("Point of interest deleted",
				$"Point of interest {pointOfInterestEntity.Name} with id {pointOfInterestEntity.Id} was deleted");

			return NoContent();
		}

		[HttpPut("{pointOfInterestId}")]
		public async Task<ActionResult<PointOfInterestDto>> UpdatePointOfInterest(int cityId,
				int pointOfInterestId,
				pointOfInterestForUpdateDto pointOfInterest)
		{
			if (!await cityInfoRepository.CityExistsAsync(cityId))
			{
				return NotFound();
			}

			var pointOfInterestEntity = await cityInfoRepository
					.GetPointOfInterestForCityAsync(cityId, pointOfInterestId);
			if (pointOfInterestEntity is null)
			{
				return NotFound();
			}

			mapper.Map(pointOfInterest, pointOfInterestEntity);
			await cityInfoRepository.SaveChangesAsync();

			return NoContent();
		}

		[HttpPatch("{pointOfInterestId}")]
		public async Task<ActionResult<PointOfInterestDto>> PartiallyUpdatePointOfInterest(int cityId,
				int pointOfInterestId,
				JsonPatchDocument<pointOfInterestForUpdateDto> patchDocument)
		{
			if (!await cityInfoRepository.CityExistsAsync(cityId))
			{
				return NotFound();
			}

			var pointOfInterestEntity = await cityInfoRepository
					.GetPointOfInterestForCityAsync(cityId, pointOfInterestId);
			if (pointOfInterestEntity is null)
			{
				return NotFound();
			}

			var pointOfInterestToPatch = mapper
					.Map<pointOfInterestForUpdateDto>(pointOfInterestEntity);

			patchDocument.ApplyTo(pointOfInterestToPatch, jsonPatchError =>
			{
				var key = jsonPatchError.AffectedObject.GetType().Name;
				ModelState.AddModelError(key, jsonPatchError.ErrorMessage);
			});

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			if (!TryValidateModel(pointOfInterestToPatch))
			{
				return BadRequest(ModelState);
			}

			mapper.Map(pointOfInterestToPatch, pointOfInterestEntity);
			await cityInfoRepository.SaveChangesAsync();

			return NoContent();
		}

		[HttpPut("bulk")]
		public async Task<ActionResult> BulkUpdatePointsOfInterestDescription(int cityId,
				PointsOfInterestBulkUpdateDto pointsOfInterestBulkUpdateDto)
		{
			if (!await cityInfoRepository.CityExistsAsync(cityId))
			{
				return NotFound();
			}

			var affectedRows = await cityInfoRepository
					.UpdatePointsOfInterestDescriptionForCityAsync(cityId,
						pointsOfInterestBulkUpdateDto.NewDescription);
			return Ok(new {UpdatedCount = affectedRows});
		}
	}
}
