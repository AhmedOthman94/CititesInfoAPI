using AutoMapper;
using CititesInfoAPI.Entities;
using CititesInfoAPI.Models;

namespace CititesInfoAPI.Profiles
{
	public class CityProfile : Profile
	{
		public CityProfile()
		{
			CreateMap<City, CityWithoutPointsOfInterestDto>();
			CreateMap<City, CityDto>();
		}
	}
}
