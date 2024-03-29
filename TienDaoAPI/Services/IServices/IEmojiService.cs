using TienDaoAPI.DTOs.Requests;
using TienDaoAPI.DTOs.Responses;
using TienDaoAPI.Models;

namespace TienDaoAPI.Services.IServices
{
    public interface IEmojiService 
    {
        public Task<Emoji> UserCreateSentimentChapter(EmojiPostRequestDTO emojiPost);
        public Task<CountSentimentClassifyChapterDTO> CountClassifySentinment(int chapterId);
        public Task DeleteEmoji(Emoji emoji);
        public Task DeleteAllEmojibyChapterId(int chapterId);
    }
}
