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
		IMailService mailService,
		ICityInfoRepository cityInfoRepository,
		IMapper mapper,
		IPointOfInterestService pointOfInterestService) : ControllerBase
	{
		[HttpGet]
		public async Task<ActionResult<IEnumerable<PointOfInterestDto>>> GetPointsOfInterest(int cityId,
			CancellationToken cancellationToken = default)
		{
			if (!await cityInfoRepository.CityExistsAsync(cityId, cancellationToken))
			{
				logger.LogInformation(
				  "City with id {CityId} wasn't found when accessing points of interest.", cityId);
				return NotFound();
			}

			var pointsOfInterestForCity = await cityInfoRepository
				.GetPointsOfInterestForCityAsync(cityId, cancellationToken);

			return Ok(mapper.Map<IEnumerable<PointOfInterestDto>>(pointsOfInterestForCity));
		}

		[HttpGet("{pointOfInterestId}", Name = "GetPointOfInterest")]
		public async Task<ActionResult<PointOfInterestDto>> GetPointOfInterest(int cityId,
			int pointOfInterestId,
			CancellationToken cancellationToken = default)
		{
			if (!await cityInfoRepository.CityExistsAsync(cityId, cancellationToken))
			{
				return NotFound();
			}

			var pointOfInterest = await cityInfoRepository
				.GetPointOfInterestForCityAsync(cityId,
					pointOfInterestId,
					cancellationToken);

			if (pointOfInterest == null)
			{
				return NotFound();
			}

			return Ok(mapper.Map<PointOfInterestDto>(pointOfInterest));
		}

		[HttpPost]
		public async Task<ActionResult<PointOfInterestDto>> CreatePointOfInterest(int cityId,
			PointOfInterestForCreationDto pointOfInterest,
			CancellationToken cancellationToken = default)
		{
			var result = await pointOfInterestService.CreatePointOfInterestAsync(cityId,
				pointOfInterest,
				cancellationToken);

			if (!result.Success)
			{
				return BadRequest(new { error = result.ErrorMessage });
			}

			return CreatedAtRoute("GetPointOfInterest",
				new { cityId, pointOfInterestId = result.PointOfInterest!.Id },
				result.PointOfInterest);

			//if (!await cityInfoRepository.CityExistsAsync(cityId, 
			//    cancellationToken))
			//{
			//    return NotFound();
			//}

			//var finalPointOfInterest = mapper.Map<Entities.PointOfInterest>(pointOfInterest);

			//await cityInfoRepository.AddPointOfInterestForCityAsync(cityId, 
			//    finalPointOfInterest, 
			//    cancellationToken);
			//await cityInfoRepository.SaveChangesAsync(cancellationToken);

			//var createdPointOfInterestToReturn = mapper.Map<PointOfInterestDto>(finalPointOfInterest);

			//return CreatedAtRoute("GetPointOfInterest",
			//    new
			//    {
			//        cityId,
			//        pointOfInterestId = createdPointOfInterestToReturn.Id
			//    },
			//    createdPointOfInterestToReturn);
		}

		[HttpDelete("{pointOfInterestId}")]
		public async Task<ActionResult> DeletePointOfInterest(int cityId,
			int pointOfInterestId,
			CancellationToken cancellationToken = default)
		{
			if (!await cityInfoRepository.CityExistsAsync(cityId,
				cancellationToken))
			{
				return NotFound();
			}

			var pointOfInterestEntity = await cityInfoRepository
				.GetPointOfInterestForCityAsync(cityId,
				pointOfInterestId,
				cancellationToken);

			if (pointOfInterestEntity == null)
			{
				return NotFound();
			}

			cityInfoRepository.DeletePointOfInterest(pointOfInterestEntity);
			await cityInfoRepository.SaveChangesAsync(cancellationToken);

			mailService.Send("Point of interest deleted.",
				$"Point of interest {pointOfInterestEntity.Name} with id {pointOfInterestEntity.Id} was deleted.");

			return NoContent();
		}

		[HttpPut("{pointOfInterestId}")]
		public async Task<ActionResult> UpdatePointOfInterest(int cityId,
			int pointOfInterestId,
			PointOfInterestForUpdateDto pointOfInterest,
			CancellationToken cancellationToken = default)
		{
			if (!await cityInfoRepository.CityExistsAsync(cityId,
				cancellationToken))
			{
				return NotFound();
			}

			var pointOfInterestEntity = await cityInfoRepository
				.GetPointOfInterestForCityAsync(cityId,
					pointOfInterestId,
					cancellationToken);

			if (pointOfInterestEntity == null)
			{
				return NotFound();
			}

			mapper.Map(pointOfInterest,
				pointOfInterestEntity);

			await cityInfoRepository.SaveChangesAsync(cancellationToken);

			return NoContent();
		}

		[HttpPatch("{pointOfInterestId}")]
		public async Task<ActionResult> PartiallyUpdatePointOfInterest(int cityId,
			int pointOfInterestId,
			JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument,
			CancellationToken cancellationToken = default)
		{
			if (!await cityInfoRepository.CityExistsAsync(cityId,
				cancellationToken))
			{
				return NotFound();
			}

			var pointOfInterestEntity = await cityInfoRepository
				.GetPointOfInterestForCityAsync(cityId,
					pointOfInterestId,
					cancellationToken);

			if (pointOfInterestEntity == null)
			{
				return NotFound();
			}

			var pointOfInterestToPatch = mapper
				.Map<PointOfInterestForUpdateDto>(pointOfInterestEntity);

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
			await cityInfoRepository.SaveChangesAsync(cancellationToken);
			return NoContent();
		}

		[HttpPut("bulk")]
		public async Task<ActionResult> BulkUpdatePointsOfInterestDescription(int cityId,
			PointsOfInterestBulkUpdateDto pointsOfInterestBulkUpdateDto,
			CancellationToken cancellationToken = default)
		{
			if (!await cityInfoRepository.CityExistsAsync(cityId,
				cancellationToken))
			{
				return NotFound();
			}
			var affectedRows = await cityInfoRepository
				.UpdatePointsOfInterestDescriptionForCityAsync(cityId,
					pointsOfInterestBulkUpdateDto.NewDescription,
					cancellationToken);

			return Ok(new { UpdatedCount = affectedRows });
		}
	}

}
