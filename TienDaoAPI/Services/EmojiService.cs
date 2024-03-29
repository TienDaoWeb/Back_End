using TienDaoAPI.DTOs.Requests;
using TienDaoAPI.DTOs.Responses;
using TienDaoAPI.Models;
using TienDaoAPI.Repositories.IRepositories;
using TienDaoAPI.Services.IServices;

namespace TienDaoAPI.Services
{
    public class EmojiService : IEmojiService
    {
        private readonly IEmojiRepository _emojiRepository;
        private readonly IChapterRepository _chapterRepository;
        public EmojiService(IEmojiRepository emojiRepository, IChapterRepository chapterRepository)
        {
            _emojiRepository = emojiRepository;
            _chapterRepository = chapterRepository;
        }

        public async Task<Emoji?> UserCreateSentimentChapter(EmojiPostRequestDTO emojiPost)
        {
            var user = await _emojiRepository.GetAsync(emoji => emoji.UserId == emojiPost.UserId);
            if(user != null)
            { 
                var oldEmoji = await _emojiRepository.GetAsync(emoji => emoji.UserId == user.UserId);
                if(user.TypeEmoji == emojiPost.TypeEmoji)
                {
                    await _emojiRepository.DeleteAsync(oldEmoji);
                    return null;
                }
                else
                {
                    oldEmoji.TypeEmoji = emojiPost.TypeEmoji;
                    return await _emojiRepository.UpdateAsync(oldEmoji);
                }
            }
            
            Emoji emoji =  new Emoji
            {
                UserId = emojiPost.UserId,
                ChapterId = emojiPost.ChapterId,
                TypeEmoji = emojiPost.TypeEmoji

            };
            return await _emojiRepository.CreateAsync(emoji);
        }

        public async Task<CountSentimentClassifyChapterDTO> CountClassifySentinment(int chapterId)
        {
            var listSentimentbyChapterId = await _emojiRepository.GetAllbyQueryrAsync(emoji => emoji.ChapterId == chapterId);
            CountSentimentClassifyChapterDTO Sentiment = new CountSentimentClassifyChapterDTO();

            foreach(var item in listSentimentbyChapterId)
            {
                switch (item.TypeEmoji)
                {
                    case 1:
                        Sentiment.Heart += 1;
                        break;
                    case 2:
                        Sentiment.Like += 1;
                        break;
                    case 3:
                        Sentiment.Fun += 1;
                        break;
                    case 4:
                        Sentiment.Sad += 1;
                        break;
                    case 5:
                        Sentiment.Angry += 1;
                        break;
                    case 6:
                        Sentiment.Attack += 1;
                        break;
                }
            }
            return Sentiment;
        }

        public async Task DeleteEmoji(Emoji emoji)
        {
            await _emojiRepository.DeleteAsync(emoji);
        }

        public async Task DeleteAllEmojibyChapterId(int chapterId)
        {
            var listSentiment = await _emojiRepository.GetAllbyQueryrAsync(emoji => emoji.ChapterId == chapterId);
            if(listSentiment != null)
            {
                foreach(var sentiment in listSentiment)
                {
                    await _emojiRepository.DeleteAsync(sentiment);
                }
            }
        }
    }
}
