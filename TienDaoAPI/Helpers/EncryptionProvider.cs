using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;
using TienDaoAPI.Utils;

namespace TienDaoAPI.Helpers
{
    public class EncryptionProvider
    {
        private readonly EncryptionSettings _encryptionSettings;

        public EncryptionProvider(IOptions<EncryptionSettings> encryptionSettings)
        {
            _encryptionSettings = encryptionSettings.Value;
        }

        public string Encrypt(string plainText)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(_encryptionSettings.Key);
                aesAlg.IV = Encoding.UTF8.GetBytes(_encryptionSettings.IV);

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(plainText);
                    }

                    byte[] encrypted = msEncrypt.ToArray();
                    return Convert.ToBase64String(encrypted);
                }
            }
        }
    }
}
