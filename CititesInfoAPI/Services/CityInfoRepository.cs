using CititesInfoAPI.DbContexts;
using CititesInfoAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace CititesInfoAPI.Services
{
	public class CityInfoRepository(CityInfoContext context) : ICityInfoRepository
	{
		public async Task<IEnumerable<City>> GetCitiesAsync()
		{
			return await context.Cities.OrderBy(c => c.Name).ToListAsync();
		}

		public async Task<IEnumerable<City>> GetCitiesReadOnlyAsync()
		{
			return await context.Cities.AsNoTracking()
					.OrderBy(c => c.Name).ToListAsync();
		}

		public async Task<City?> GetCityAsync(int cityId, 
				bool includePointsOfInterest)
		{
			if (includePointsOfInterest)
			{
				return await context.Cities.Include(c => c.PointsOfInterest)
						.Where(c => c.Id == cityId).FirstOrDefaultAsync();
			}

			return await context.Cities
					.Where(c => c.Id == cityId).FirstOrDefaultAsync();
		}

		public async Task<bool> CityExistsAsync(int cityId)
		{
			return await context.Cities.AnyAsync(c => c.Id == cityId);
		}

		public async Task<PointOfInterest?> GetPointOfInterestForCityAsync(int cityId,
				int pointOfInterestId)
		{
			return await context.PointsOfInterest
					.Where(p => p.CityId == cityId && p.Id == pointOfInterestId)
					.FirstOrDefaultAsync();
		}

		public async Task<IEnumerable<PointOfInterest>> GetPointsOfInetestForCityAsync(int cityId)
		{
			return await context.PointsOfInterest
					.Where(p => p.CityId == cityId).ToListAsync();
		}
	}
}
