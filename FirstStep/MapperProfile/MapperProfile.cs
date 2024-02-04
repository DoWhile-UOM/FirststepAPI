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
            CreateMap<AddSeekerDto, Seeker>();
            CreateMap<CompanyDto, Company>();
        }
    }
}
