using AutoMapper;
using TasksAPI.DTOs;
using TasksAPI.Models;

namespace TasksAPI.AutoMapperProfiles;

public class PinnedTaskProfile : Profile
{
    public PinnedTaskProfile()
    {
        CreateMap<PinnedTask, PinnedTaskDto>().ReverseMap();

    }
}
