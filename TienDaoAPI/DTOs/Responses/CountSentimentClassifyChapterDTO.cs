using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace TienDaoAPI.DTOs.Responses
{
    public class CountSentimentClassifyChapterDTO
    {
        public int Fun { get; set; } = 0;
        public int Angry { get; set; } = 0;
        public int Attack { get; set; } = 0;
        public int Heart { get; set; } = 0;
        public int Like { get; set; } = 0;
        public int Sad { get; set; } = 0;
    }
}
