using AutoMapper;
using TasksAPI.DTOs;
using TasksAPI.Models;

namespace TasksAPI.AutoMapperProfiles;

public class SuggestedLabelProfile : Profile
{
    public SuggestedLabelProfile()
    {
    CreateMap<SuggestedLabel, SuggestedLabelDto>().ReverseMap();
        
    }
}
