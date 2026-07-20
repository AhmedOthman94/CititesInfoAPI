using CititesInfoAPI.Entities;

namespace CititesInfoAPI.Services
{
	public interface ICityInfoRepository
	{
		Task<IEnumerable<City>> GetCitiesAsync();
		Task<IEnumerable<City>> GetCitiesReadOnlyAsync();
		Task<City?> GetCityAsync(int cityId, bool includePointsOfInterest);
		Task<bool> CityExistsAsync(int cityId);
		Task<IEnumerable<PointOfInterest>> GetPointsOfInetestForCityAsync(int cityId);
		Task<PointOfInterest?> GetPointOfInterestForCityAsync(int cityId,
				int pointOfInterestId);
	}
}
