using AutoMapper;
using FirstStep.Models;
using FirstStep.Models.DTOs;

namespace FirstStep.MapperProfile
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<ProfessionKeywordDto, ProfessionKeyword>();

            CreateMap<AddAdvertisementDto, Advertisement>();
            CreateMap<UpdateAdvertisementDto, Advertisement>();
            CreateMap<Advertisement, AdvertisementDto>();
            CreateMap<Advertisement, AdvertisementShortDto>();
            CreateMap<Advertisement, JobOfferDto>();
            
            CreateMap<Company, CompanyProfileDto>();
            
            CreateMap<AddSeekerDto, Seeker>();
            CreateMap<AddCompanyDto, Company>();
            CreateMap<AddEmployeeDto, HRManager>();
            CreateMap<AddEmployeeDto, HRAssistant>();
        }
    }
}
