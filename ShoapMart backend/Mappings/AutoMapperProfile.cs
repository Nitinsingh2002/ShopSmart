using AutoMapper;
using ShoapMart.Api.DTOs;
using ShopMart.Api.Entities;

namespace ShoapMart.Api.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<RegisterRequestDto, ApplicationUser>().ReverseMap();
            
        }
    }
}