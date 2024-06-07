﻿using AutoMapper;
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

            CreateMap<Advertisement, UpdateAdvertisementDto>();
            CreateMap<UpdateAdvertisementDto, Advertisement>()
                .ForMember(ad => ad.advertisement_id, act => act.Ignore());

            CreateMap<Advertisement, AdvertisementDto>()
                .ForMember(
                    des => des.field_name,
                    opt => opt.MapFrom(src => src.job_Field!.field_name));

            CreateMap<Advertisement, AdvertisementShortDto>()
                .ForMember(
                    des => des.company_id,
                    opt => opt.MapFrom(src => src.hrManager!.company_id))
                .ForMember(
                    des => des.field_name,
                    opt => opt.MapFrom(src => src.job_Field!.field_name));

            CreateMap<Advertisement, AppliedAdvertisementShortDto>()
                .ForMember(
                    des => des.company_id,
                    opt => opt.MapFrom(src => src.hrManager!.company_id))
                .ForMember(
                    des => des.field_name,
                    opt => opt.MapFrom(src => src.job_Field!.field_name));

            CreateMap<Advertisement, AdvertisementTableRowDto>();
            CreateMap<Advertisement, AdvertisementHRATableRowDto>();
            
            CreateMap<Company, CompanyProfileDto>();
            CreateMap<Seeker, SeekerApplicationDto>();
            CreateMap<AddSeekerDto, Seeker>();
            CreateMap<AddCompanyDto, Company>();
            CreateMap<AddEmployeeDto, HRManager>();
            CreateMap<AddEmployeeDto, HRAssistant>();
            CreateMap<Company, CompanyProfileDetailsDto>();

            CreateMap<Application, ApplicationListDto>()
                .ForMember(
                    des => des.seekerName,
                    opt => opt.MapFrom(src => src.seeker!.first_name + " " + src.seeker!.last_name));

            CreateMap<AddApplicationDto, Application>();
            CreateMap<Seeker, ApplicationViewDto>();
            
            CreateMap<Company, ViewCompanyListDto>();
            CreateMap<Company, CompanyApplicationDto>();
            CreateMap<Company, CompanyApplicationDto>();

            CreateMap<Advertisement, ApplicationListingPageDto>()
                .ForMember(
                    des => des.field_name,
                    opt => opt.MapFrom(src => src.job_Field!.field_name));
        }
    }
}
