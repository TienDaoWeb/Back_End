using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Org.BouncyCastle.Utilities.Collections;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using TienDaoAPI.DTOs.Requests;
using TienDaoAPI.Models;
using TienDaoAPI.Repositories;
using TienDaoAPI.Repositories.IRepositories;
using TienDaoAPI.Services.IServices;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TienDaoAPI.Services
{
    public class ChapterService : IChapterService
    {
        private readonly IChapterRepository _chapterRepository;
        private readonly IStoryRepository _storyRepository;
        private readonly IEmojiRepository _emojiRepository;
        public ChapterService(IEmojiRepository emojiRepository ,IChapterRepository chapterRepository ,IStoryRepository storyRepository)
        {
            _chapterRepository = chapterRepository;
            _storyRepository = storyRepository;
            _emojiRepository = emojiRepository;
        }

        public async Task<Chapter?> CreateChapterAsync(ChapterPostRequestDTO chapterRequestDTO)
        {
            // Set order new Chapter
            var finalChapter = await GetFinalChapterByIdStoryAsync(chapterRequestDTO.StoryId);
            var orderNewChapter = 0;
            if (finalChapter == null)
            {
                orderNewChapter = 1;
            }
            else
            {
                orderNewChapter = (int)(finalChapter.Order + 1);
            }

            //Create the new Chapter
            Chapter newChapter = new Chapter
            {
                Name = chapterRequestDTO.Name,
                Content = chapterRequestDTO.Content,
                Order = orderNewChapter,
                StoryId = chapterRequestDTO.StoryId,
                PublishedDate = DateTime.Now,
            };

            return await _chapterRepository.CreateAsync(newChapter);
        }
        public async Task<Chapter?> GetFinalChapterByIdStoryAsync(int storyId)
        {
            var allChapterbyStoryId = await _chapterRepository.GetAllbyQueryrAsync(chapter => chapter.StoryId == storyId);
            var finnalChapter = allChapterbyStoryId.Max(chapter => chapter.Order);
            return await _chapterRepository.GetAsync(chapter=>chapter.Order == finnalChapter);
        }
        public async Task DeleteChapterAsync(Chapter chapter)
        {
       
           await _chapterRepository.DeleteAsync(chapter);
            
        }

        public async Task<IEnumerable<Chapter?>> GetAllChapteofStoryrAsync(Expression<Func<Chapter, bool>> filter)
        {
            var chapters = _chapterRepository.GetAllbyQueryrAsync(filter);
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
