using AutoMapper;
using TienDaoAPI.DTOs;
using TienDaoAPI.Extensions;
using TienDaoAPI.Models;

namespace TienDaoAPI.Config
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<CreateGenreDTO, Genre>();

            CreateMap<RegisterDTO, User>().ForMember(d => d.UserName, s => s.MapFrom(x => x.Email));
            CreateMap<User, UserBaseDTO>();
            CreateMap<User, UserDTO>().IncludeBase<User, UserBaseDTO>();
            CreateMap<UpdateProfileDTO, User>();

            CreateMap<CreateBookDTO, Book>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
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
                .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status.GetBookStatusName()))
                .ForMember(dest => dest.WordCount, opt => opt.MapFrom(src => src.Chapters.Sum(c => c.WordCount)))
                .ForMember(dest => dest.ViewCount, opt => opt.MapFrom(src => src.Chapters.Sum(c => c.ViewCount)))
                .ReverseMap();

            CreateMap<AuthorDTO, Author>().ReverseMap();

            CreateMap<CreateChapterDTO, Chapter>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.PublishedAt, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
                .ForMember(dest => dest.ViewCount, opt => opt.Ignore())
                .ForMember(dest => dest.WordCount, opt => opt.Ignore());

            CreateMap<Chapter, ChapterInfoDTO>();
            CreateMap<Chapter, ChapterDetailDTO>().IncludeBase<Chapter, ChapterInfoDTO>();
        }
    }
}
