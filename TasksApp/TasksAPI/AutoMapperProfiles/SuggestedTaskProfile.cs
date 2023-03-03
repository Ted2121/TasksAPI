using AutoMapper;
using TasksAPI.DTOs;
using TasksAPI.Models;

namespace TasksAPI.AutoMapperProfiles;

public class SuggestedTaskProfile : Profile
{
    public SuggestedTaskProfile()
    {
    CreateMap<SuggestedTask, SuggestedTaskDto>().ReverseMap();

    }
}
