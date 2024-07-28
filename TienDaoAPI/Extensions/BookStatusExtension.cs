using TienDaoAPI.Enums;

namespace TienDaoAPI.Extensions
{
    public static class BookStatusExtension
    {
        public static string GetStatusName(this BookStatusEnum status)
        {
            return status switch
            {
                BookStatusEnum.Completed => "Hoàn thành",
                BookStatusEnum.Ongoing => "Đang ra",
                _ => "Tạm dừng",
            };
        }
    }
}
