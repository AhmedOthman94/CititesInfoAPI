using CititesInfoAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CititesInfoAPI.Services
{
	public interface IPointOfInterestService
	{
		Task<PointOfInterestCreationResult> CreatePointOfInterestAsync(int cityId,
					PointOfInterestForCreationDto pointOfInterest,
					CancellationToken cancellationToken);
	}
}
