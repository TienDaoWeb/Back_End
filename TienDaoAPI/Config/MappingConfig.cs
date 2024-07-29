using AutoMapper;
using TienDaoAPI.DTOs;
using TienDaoAPI.DTOs.Responses;
using TienDaoAPI.Extensions;
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
            CreateMap<User, UserBaseDTO>().ReverseMap();
            CreateMap<User, UserDTO>().IncludeBase<User, UserBaseDTO>(); ;
            CreateMap<UpdateProfileDTO, User>().ReverseMap();
            CreateMap<CreateBookDTO, Book>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
                .ForMember(dest => dest.ReviewCount, opt => opt.Ignore())
                .ForMember(dest => dest.BookmarkCount, opt => opt.Ignore())
                .ForMember(dest => dest.ViewCount, opt => opt.Ignore())
                .ForMember(dest => dest.VoteCount, opt => opt.Ignore())
                .ForMember(dest => dest.WordCount, opt => opt.Ignore())
                .ForMember(dest => dest.LastestIndex, opt => opt.Ignore())
                .ForMember(dest => dest.Chapters, opt => opt.Ignore())
                .ForMember(dest => dest.Comments, opt => opt.Ignore())
                .ForMember(dest => dest.Reviews, opt => opt.Ignore())
                .ForMember(dest => dest.PosterUrl, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.Author, opt => opt.Ignore())
                .ForMember(dest => dest.AuthorId, opt => opt.Ignore())
                .ForMember(dest => dest.PublishedAt, opt => opt.Ignore());

            CreateMap<Book, BookDTO>()
                .ForMember(d => d.Owner, s => s.MapFrom(x => x.User))
                .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status.GetStatusName()))
                .ReverseMap();
            CreateMap<AuthorDTO, Author>().ReverseMap();
        }
    }
}
