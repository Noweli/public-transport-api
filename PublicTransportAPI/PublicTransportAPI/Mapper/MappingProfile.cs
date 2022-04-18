using AutoMapper;
using PublicTransportAPI.Data.DTOs;
using PublicTransportAPI.Data.Models;

namespace PublicTransportAPI.Mapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<StopPointLineEvent, StopPointLineEventDTO>().ReverseMap();
    }
}