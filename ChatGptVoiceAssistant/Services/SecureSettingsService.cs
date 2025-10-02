using System;
using System.Security.Cryptography;
using System.Text;

namespace HeyGPT.Services
{
    public class SecureSettingsService
    {
        public string EncryptApiKey(string apiKey)
        {
            if (string.IsNullOrEmpty(apiKey))
            {
                return string.Empty;
            }

            try
            {
                byte[] plainBytes = Encoding.UTF8.GetBytes(apiKey);
                byte[] encryptedBytes = ProtectedData.Protect(
                    plainBytes,
                    null,
                    DataProtectionScope.CurrentUser
                );
                return Convert.ToBase64String(encryptedBytes);
            }
            catch (CryptographicException ex)
            {
                System.Diagnostics.Debug.WriteLine($"Encryption error: {ex.Message}");
                return apiKey;
            }
        }

        public string DecryptApiKey(string encryptedKey)
        {
            if (string.IsNullOrEmpty(encryptedKey))
            {
                return string.Empty;
            }

            try
            {
                byte[] encryptedBytes = Convert.FromBase64String(encryptedKey);
                byte[] plainBytes = ProtectedData.Unprotect(
                    encryptedBytes,
                    null,
                    DataProtectionScope.CurrentUser
                );
                return Encoding.UTF8.GetString(plainBytes);
            }
            catch (CryptographicException ex)
            {
                System.Diagnostics.Debug.WriteLine($"Decryption error (key may be from different user): {ex.Message}");
                return string.Empty;
            }
            catch (FormatException)
            {
                return encryptedKey;
            }
        }

        public bool IsEncrypted(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }

            try
            {
                Convert.FromBase64String(value);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
