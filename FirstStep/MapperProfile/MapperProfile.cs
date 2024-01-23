using AutoMapper;
using FirstStep.Models;
using FirstStep.Models.DTOs;

namespace FirstStep.MapperProfile
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<AddAdvertisementDto, Advertisement>();
        }
    }
}
