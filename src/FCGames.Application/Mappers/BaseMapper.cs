using AutoMapper;
using FCGames.Domain.Entities;
using DTO = FCGames.Application.Dto;

namespace FCGames.Application.Mappers;

public class BaseMapper : Profile
{
    public BaseMapper()
    {
        CreateMap<DTO.BaseModel, BaseEntity>()
            .ForMember(dest => dest.Id, opt =>
            {
                opt.Condition(src => src.Id.HasValue == false || src.Id.Value == default);
                opt.UseDestinationValue();
            })
            .ForMember(dest => dest.Id, opt =>
            {
                opt.Condition(src => src.Id.HasValue == true && src.Id.Value != default);
                opt.MapFrom(src => src.Id);
            });
    }
}