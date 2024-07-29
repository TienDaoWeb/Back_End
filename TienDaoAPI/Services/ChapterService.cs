using System.Linq.Expressions;
using TienDaoAPI.DTOs;
using TienDaoAPI.Models;
using TienDaoAPI.Repositories.IRepositories;
using TienDaoAPI.Services.IServices;

namespace TienDaoAPI.Services
{
    public class ChapterService : IChapterService
    {
        private readonly IChapterRepository _chapterRepository;
        private readonly IBookRepository _bookRepository;
        public ChapterService(IChapterRepository chapterRepository, IBookRepository bookRepository)
        {
            _chapterRepository = chapterRepository;
            _bookRepository = bookRepository;
        }

        public async Task<Chapter?> CreateChapterAsync(CreateChapterDTO chapterRequestDTO)
        {
            // Set order new Chapter
            var finalChapter = await GetFinalChapterByIdBookAsync(chapterRequestDTO.BookId);
            var orderNewChapter = (finalChapter?.Index ?? 0) + 1;
            //Create the new Chapter
            Chapter newChapter = new Chapter
            {
                Name = chapterRequestDTO.Name,
                Content = chapterRequestDTO.Content,
                Index = orderNewChapter,
                BookId = chapterRequestDTO.BookId,
                PublishedAt = DateTime.Now,
            };

            return await _chapterRepository.CreateAsync(newChapter);
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
