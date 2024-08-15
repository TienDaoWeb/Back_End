using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TienDaoAPI.Data;
using TienDaoAPI.DTOs;
using TienDaoAPI.Extensions;
using TienDaoAPI.Helpers;
using TienDaoAPI.Models;
using TienDaoAPI.Services.IServices;

namespace TienDaoAPI.Services
{
    public class ChapterService : IChapterService
    {
        private readonly TienDaoDbContext _dbContext;
        private readonly IBookService _bookService;
        private readonly IMapper _mapper;
        private readonly EncryptionProvider _encryptionProvider;
        public ChapterService(TienDaoDbContext dbContext, IMapper mapper, EncryptionProvider encryptionProvider, IBookService bookService)
        {
            _mapper = mapper;
            _encryptionProvider = encryptionProvider;
            _bookService = bookService;
            _dbContext = dbContext;
        }

        public async Task<bool> CreateChapterAsync(CreateChapterDTO dto)
        {
            try
            {
                var book = await _bookService.GetBookByIdAsync(dto.BookId);
                ++book!.LastestIndex;
                if (dto.Index == 0 || dto.Index > book!.LastestIndex)
                {
                    dto.Index = book!.LastestIndex;
                }
                var chaptersToShift = await GetChaptersByBookIdAsync(dto.BookId);
                foreach (var existingChapter in chaptersToShift.Where(c => c!.Index >= dto.Index))
                {
                    existingChapter!.Index++;
                }

                var chapter = _mapper.Map<Chapter>(dto);
                chapter.WordCount = chapter.Content!.CountWords();
                chapter.Content = _encryptionProvider.Encrypt(chapter.Content!);

                _dbContext.Chapters.Add(chapter);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<bool> DeleteChapterAsync(Chapter chapter)
        {
            try
            {
                chapter.DeletedAt = DateTime.UtcNow;
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<IEnumerable<Chapter?>> GetChaptersByBookIdAsync(int bookId)
        {
            return await _dbContext.Chapters.Where(c => c.BookId == bookId && c.DeletedAt == null).OrderBy(c => c.Index).ToListAsync();
        }

        public async Task<Chapter?> GetChapterByIdAsync(int id)
        {
            return await _dbContext.Chapters.FirstOrDefaultAsync(c => c.Id == id && c.DeletedAt == null);
        }

        public async Task<bool> UpdateChapterAsync(Chapter chapter, UpdateChapterDTO dto)
        {
            try
            {
                var book = await _bookService.GetBookByIdAsync(chapter.BookId);
                if (dto.Index <= 0)
                {
                    return false;
                }
                if (dto.Index > book!.LastestIndex)
                {
                    dto.Index = book!.LastestIndex;
                }
                var chaptersToShift = await GetChaptersByBookIdAsync(chapter.BookId);
                if (chapter.Index > dto.Index)
                {
                    foreach (var existingChapter in chaptersToShift.Where(c => c!.Index >= dto.Index && c!.Index < chapter.Index))
                    {
                        existingChapter!.Index++;
                    }
                }
                else if (chapter.Index < dto.Index)
                {
                    foreach (var existingChapter in chaptersToShift.Where(c => c!.Index <= dto.Index && c!.Index > chapter.Index))
                    {
                        existingChapter!.Index--;
                    }
                }
                _mapper.Map(dto, chapter);
                chapter.WordCount = chapter.Content!.CountWords();
                chapter.Content = _encryptionProvider.Encrypt(chapter.Content!);
                chapter.UpdatedAt = DateTime.UtcNow;

                _dbContext.Chapters.Update(chapter);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public bool Modifiable(Chapter chapter, UserDTO user)
        {
            return chapter.OwnerId == user.Id;
        }
    }
}
