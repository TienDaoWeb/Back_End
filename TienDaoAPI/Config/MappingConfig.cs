using AutoMapper;
using TienDaoAPI.DTOs;
using TienDaoAPI.DTOs.Responses;
using TienDaoAPI.Models;

namespace TienDaoAPI.Config
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<CreateGenreDTO, Genre>().ReverseMap();
            CreateMap<GenreResponse, Genre>().ReverseMap();
            CreateMap<RegisterDTO, User>().ForMember(d => d.UserName, s => s.MapFrom(x => x.Email));
            CreateMap<UserDTO, User>().ReverseMap();
            CreateMap<UpdateProfileDTO, User>().ReverseMap();
            CreateMap<Book, BookResponse>()
                .ForMember(d => d.NumberOfChapters, s => s.MapFrom(x => x.Chapters.Count))
                .ForPath(d => d.GenreResponse, s => s.MapFrom(x => x.Genre))
                .ReverseMap();
        }
    }
}
