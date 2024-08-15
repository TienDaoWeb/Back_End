using AutoMapper;
using TienDaoAPI.DTOs.Authors;
using TienDaoAPI.DTOs.Books;
using TienDaoAPI.DTOs.Chapters;
using TienDaoAPI.DTOs.Comments;
using TienDaoAPI.DTOs.Genres;
using TienDaoAPI.DTOs.Readings;
using TienDaoAPI.DTOs.Reviews;
using TienDaoAPI.DTOs.Tags;
using TienDaoAPI.DTOs.TagTypes;
using TienDaoAPI.DTOs.Users;
using TienDaoAPI.Extensions;
using TienDaoAPI.Models;

namespace TienDaoAPI.Config
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<CreateGenreDTO, Genre>();

            CreateMap<CreateTagDTO, Tag>();

            CreateMap<Tag, TagDTO>();

            CreateMap<CreateTagTypeDTO, TagType>();

            CreateMap<RegisterDTO, User>().ForMember(d => d.UserName, s => s.MapFrom(x => x.Email));

            CreateMap<User, UserBaseDTO>();

            CreateMap<User, UserDTO>().IncludeBase<User, UserBaseDTO>();

            CreateMap<UpdateProfileDTO, User>().ReverseMap();

            CreateMap<CreateReviewDTO, Review>().ReverseMap();

            CreateMap<UpdateReviewDTO, Review>().ReverseMap();

            CreateMap<Review, ReviewDTO>().ReverseMap();

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

            CreateMap<UpdateBookDTO, Book>();

            CreateMap<Book, BookDTO>()
                .ForMember(d => d.Owner, s => s.MapFrom(x => x.User))
                .ForMember(d => d.Genre, s => s.MapFrom(x => x.Genre))
                .ForMember(d => d.Tags, s => s.MapFrom(x => x.BookTags.Select(bt => bt.Tag)))
                .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status.GetBookStatusName()))
                .ForMember(dest => dest.WordCount, opt => opt.MapFrom(src => src.Chapters.Sum(c => c.WordCount)))
                .ForMember(dest => dest.ViewCount, opt => opt.MapFrom(src => src.Chapters.Sum(c => c.ViewCount)))
                .ReverseMap();

            CreateMap<Book, BookReadingDTO>();
            CreateMap<Comment, CreateCommentDTO>().ReverseMap();

            CreateMap<Comment, CreateReplyCommentDTO>().ReverseMap();

            CreateMap<AuthorDTO, Author>().ReverseMap();

            CreateMap<CreateChapterDTO, Chapter>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.PublishedAt, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
                .ForMember(dest => dest.ViewCount, opt => opt.Ignore())
                .ForMember(dest => dest.WordCount, opt => opt.Ignore());

            CreateMap<UpdateChapterDTO, Chapter>();

            CreateMap<Chapter, ChapterShortDTO>();

            CreateMap<Chapter, ChapterInfoDTO>();

            CreateMap<Chapter, ChapterDetailDTO>().IncludeBase<Chapter, ChapterInfoDTO>();

            CreateMap<CreateReadingDTO, Reading>();

            CreateMap<Reading, ReadingDTO>()
                .ForMember(dest => dest.ChapterIndex, opt => opt.MapFrom(src => src.Chapter.Index))
                .ForMember(dest => dest.BookId, opt => opt.MapFrom(src => src.Chapter.BookId))
                .ForMember(dest => dest.Book, opt => opt.MapFrom(src => src.Chapter.Book));

            CreateMap<Comment, CommentDTO>()
                .ForMember(d => d.Owner, s => s.MapFrom(x => x.User))
                .ForMember(dest => dest.LikeCount, opt => opt.MapFrom(src => src.CommentLikes.Count));
            CreateMap<Comment, CommentReplyDTO>()
                .ForMember(d => d.Owner, s => s.MapFrom(x => x.User))
                .ForMember(dest => dest.LikeCount, opt => opt.MapFrom(src => src.CommentLikes.Count));
        }
    }
}
