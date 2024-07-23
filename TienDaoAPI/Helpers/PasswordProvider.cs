using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace TienDaoAPI.Helpers
{
    public class PasswordProvider
    {
        private const string PasswordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&_-])[A-Za-z\d@$!%_*?&-]{8,30}$";
        private const int ResetPasswordLength = 20;
        public static bool CheckPasswordStrength(string password)
        {
            return Regex.IsMatch(password, PasswordPattern);
        }

        public static string GenerateStrongPassword()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()_+";
            StringBuilder result = new StringBuilder(ResetPasswordLength);
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                byte[] buffer = new byte[sizeof(uint)];
                while (result.Length < ResetPasswordLength)
                {
                    rng.GetBytes(buffer);
                    uint num = BitConverter.ToUInt32(buffer, 0);
                    result.Append(chars[(int)(num % (uint)chars.Length)]);
                }
            }
            return result.ToString();
        }
    }
}
