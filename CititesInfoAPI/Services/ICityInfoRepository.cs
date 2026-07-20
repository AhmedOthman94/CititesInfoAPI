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
		Task AddPointOfInterestForCityAsyc(int cityId,
				PointOfInterest pointOfInterest);
		void DeletePointOfInterest(PointOfInterest pointOfInterest);
		Task<int> UpdatePointsOfInterestDescriptionForCityAsync(int cityId,
					string newDescription);
		Task<int> DeleteAllPointsOfInterestForCityAsync(int cityId);
		Task<bool> SaveChangesAsync();
	}
}
