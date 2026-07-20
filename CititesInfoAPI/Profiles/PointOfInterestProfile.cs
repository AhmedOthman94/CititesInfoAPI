using AutoMapper;
using CititesInfoAPI.Entities;
using CititesInfoAPI.Models;

namespace CititesInfoAPI.Profiles
{
	public class PointOfInterestProfile : Profile
	{	
		public PointOfInterestProfile() 
		{
			CreateMap<PointOfInterest, PointOfInterestDto>();
			CreateMap<PointOfInterestForCreationDto, PointOfInterest>();
			CreateMap<pointOfInterestForUpdateDto, PointOfInterest>();
			CreateMap<PointOfInterest, pointOfInterestForUpdateDto>();
		}
	}
}
