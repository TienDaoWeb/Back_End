using System.Globalization;

namespace TienDaoAPI.Extensions
{
    public static class StringExtension
    {
        public static int CountWords(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return 0;
            }

            // Tách văn bản thành các từ dựa trên dấu cách và các ký tự xuống dòng
            char[] delimiters = new char[] { ' ', '\n', '\r' };
            string[] words = text.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

            // Đếm số từ
            return words.Length;
        }

        public static string ToPascalCase(this string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(text.ToLower());
            return text.Replace(" ", string.Empty);
        }
    }
}
