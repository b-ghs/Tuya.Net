using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using Tuya.Net.Data.Settings;

[assembly: InternalsVisibleTo("Tuya.Net.Tests")]
namespace Tuya.Net.Security
{
    /// <summary>
    /// Encryption utils class.
    /// </summary>
    public static class EncryptionUtils
    {
        /// <summary>
        /// Decrypt AES-128-CBC encrypted data without IV
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="passwordTicket">Password ticket</param>
        /// 
        public static string DecryptAES128(string encryptionKey, string data)
        {
            
            var key = Encoding.UTF8.GetBytes(encryptionKey);
            
            var iv = new byte[16];
            var encrypted = ConvertHexStringToByteArray(data);
            var aes = Aes.Create();
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.PKCS7;
            aes.KeySize = 128;
            aes.Key = key;
            aes.IV = iv;
            var decryptor = aes.CreateDecryptor();
            var decrypted = decryptor.TransformFinalBlock(encrypted, 0, encrypted.Length);
            return Encoding.UTF8.GetString(decrypted);
        }

        public static string EncryptAES128(string encryptionKey, string data)
        {
            var key = Encoding.UTF8.GetBytes(encryptionKey);
            var iv = new byte[16];
            var dataWA = Encoding.UTF8.GetBytes(data);
            var aes = Aes.Create();
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.PKCS7;
            aes.KeySize = 128;
            aes.Key = key;
            aes.IV = iv;
            var encryptor = aes.CreateEncryptor();
            var encrypted = encryptor.TransformFinalBlock(dataWA, 0, dataWA.Length);
            var encryptedHex = BitConverter.ToString(encrypted).Replace("-", "");
            return encryptedHex;
        }   

        private static byte[] ConvertHexStringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                .ToArray();
        }

        /// <summary>
        /// Calculate the signature of the Tuya API request.
        /// </summary>
        /// <param name="credentials">Tuya credentials.</param>
        /// <param name="method">The http method used.</param>
        /// <param name="relativeUrl">The relative url (endpoint).</param>
        /// <param name="payload">The request payload.</param>
        /// <param name="timestamp">The timestamp of the request.</param>
        /// <param name="accessToken">The access token if present.</param>
        /// <param name="nonce">The nonce.</param>
        /// <returns>A string containing the request signature.</returns>
        internal static string CalculateSignature(ITuyaCredentials credentials, HttpMethod method, string relativeUrl, string payload, string timestamp, string accessToken = "", string nonce = "")
        {
            var stringToSign = string.Join("\n", new List<string>() { method.ToString(), payload.ToSha256(), string.Empty, relativeUrl });

            return EncryptHmac(
                $"{credentials.ClientId}{accessToken}{timestamp}{nonce}{stringToSign}",
                credentials.ClientSecret);
        }

        /// <summary>
        /// Extension method to hash a string into a SHA-256 representation.
        /// </summary>
        /// <param name="rawString">The raw string.</param>
        /// <returns>A SHA-256 representation of the given string.</returns>
        internal static string ToSha256(this string rawString)
        {
            using var sha256Hash = SHA256.Create();
            var bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawString));
            return ConvertByteArrayToString(bytes);
        }

        /// <summary>
        /// Encrypt an HMAC string.
        /// </summary>
        /// <param name="message">Message to encryption.</param>
        /// <param name="secret">Secret string used for encryption.</param>
        /// <returns>The encrypted HMAC string.</returns>
        private static string EncryptHmac(string message, string secret)
        {
            var encoding = new UTF8Encoding();
            var keyByte = encoding.GetBytes(secret);
            var messageBytes = encoding.GetBytes(message);

            using var hmacSha256 = new HMACSHA256(keyByte);
            var hashMessage = hmacSha256.ComputeHash(messageBytes);
            return ConvertByteArrayToString(hashMessage).ToUpper();
        }

        /// <summary>
        /// Helper method to convert a byte array to a string.
        /// </summary>
        /// <param name="bytes">Byte array.</param>
        /// <returns>Resulting string.</returns>
        private static string ConvertByteArrayToString(byte[] bytes)
        {
            var builder = new StringBuilder();

            foreach (var b in bytes)
            {
                builder.Append(b.ToString("x2"));
            }

            return builder.ToString();
        }
    }
}
