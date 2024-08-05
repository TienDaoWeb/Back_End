using AutoMapper;
using System.Linq.Expressions;
using TienDaoAPI.DTOs;
using TienDaoAPI.Extensions;
using TienDaoAPI.Helpers;
using TienDaoAPI.Models;
using TienDaoAPI.Repositories.IRepositories;
using TienDaoAPI.Services.IServices;

namespace TienDaoAPI.Services
{
    public class ChapterService : IChapterService
    {
        private readonly IChapterRepository _chapterRepository;
        private readonly IBookService _bookService;
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;
        private readonly EncryptionProvider _encryptionProvider;
        public ChapterService(IChapterRepository chapterRepository, IBookRepository bookRepository,
            IMapper mapper, EncryptionProvider encryptionProvider, IBookService bookService)
        {
            _chapterRepository = chapterRepository;
            _bookRepository = bookRepository;
            _mapper = mapper;
            _encryptionProvider = encryptionProvider;
            _bookService = bookService;
        }

        public async Task<Chapter?> CreateChapterAsync(CreateChapterDTO dto)
        {
            try
            {
                var chapter = _mapper.Map<Chapter>(dto);
                chapter.WordCount = chapter.Content!.CountWords();
                chapter.Content = _encryptionProvider.Encrypt(chapter.Content!);
                return await _chapterRepository.CreateAsync(chapter);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<Chapter?> GetFinalChapterByIdBookAsync(int bookId)
        {
            var chapters = await _chapterRepository.FilterAsync(chapter => chapter.BookId == bookId);
            var finalChapter = chapters.Max(chapter => chapter.Index);
            return await _chapterRepository.GetAsync(chapter => chapter.Index == finalChapter);
        }
        public async Task DeleteChapterAsync(Chapter chapter)
        {
            await _chapterRepository.DeleteAsync(chapter);
        }

        public async Task DeleteAllChapterAsync(IEnumerable<Chapter> chapters)
        {
            await _chapterRepository.RemoveRangeAsync(chapters);
        }

        public async Task<IEnumerable<Chapter?>> GetAllChapterOfBookAsync(Expression<Func<Chapter, bool>> filter)
        {
            var chapters = _chapterRepository.FilterAsync(filter);
            return await chapters;
        }

        public async Task<Chapter?> GetChapterByIdAsync(int chapterId)
        {
            return await _chapterRepository.GetByIdAsync(chapterId);
        }

        public async Task<Chapter?> UpdateChapterAsync(Chapter chapter)
        {
            return await _chapterRepository.UpdateAsync(chapter);
        }

        public async Task<IEnumerable<Chapter?>> GetAllChapterAsync(Expression<Func<Chapter, bool>> filter)
        {
            return await _chapterRepository.GetAllAsync();
        }
    }
}
