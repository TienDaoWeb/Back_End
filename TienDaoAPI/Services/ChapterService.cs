using AutoMapper;
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

        public async Task<bool> DeleteChapterAsync(Chapter chapter)
        {
            try
            {
                chapter.DeletedAt = DateTime.UtcNow;
                await _chapterRepository.SaveAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<bool> DeleteAllChapterAsync(IEnumerable<Chapter> chapters)
        {
            await _chapterRepository.RemoveRangeAsync(chapters);
            return true;
        }

        public async Task<IEnumerable<Chapter?>> GetAllChaptersByBookIdAsync(int bookId)
        {
            var chapters = await _chapterRepository.FilterAsync(c => c.BookId == bookId && c.DeletedAt == null,
                                                                null,
                                                                q => q.OrderBy(c => c.Index));
            return chapters;
        }

        public async Task<Chapter?> GetChapterByIdAsync(int id)
        {
            return await _chapterRepository.GetAsync(c => c.Id == id && c.DeletedAt == null);
        }

        public async Task<Chapter?> UpdateChapterAsync(Chapter chapter, UpdateChapterDTO dto)
        {
            try
            {
                _mapper.Map(dto, chapter);
                chapter.UpdatedAt = DateTime.UtcNow;
                return await _chapterRepository.UpdateAsync(chapter);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public bool Modifiable(Chapter chapter, UserDTO user)
        {
            return chapter.OwnerId == user.Id;
        }
    }
}
