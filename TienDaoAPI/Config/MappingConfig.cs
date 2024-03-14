using AutoMapper;
using TienDaoAPI.DTOs.Responses;
using TienDaoAPI.Models;

namespace TienDaoAPI.Config
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<GenreResponse, Genre>().ReverseMap();
            CreateMap<Story, StoryResponse>()
                .ForMember(d => d.NumberOfChapters, s => s.MapFrom(x => x.Chapters.Count))
                .ForPath(d => d.GenreResponse, s => s.MapFrom(x => x.Genre))
                .ReverseMap();
        }
    }
}
