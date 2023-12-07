using System.Security.Cryptography;
using System.Text;

namespace Utilities
{
    public class HashString
    {
        public static string GenerateHashString(string text, string salt)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            using (var sha = new HMACSHA256())
            {
                byte[] textbytes = Encoding.UTF8.GetBytes(text + salt);
                byte[] hashbytes = sha.ComputeHash(textbytes);

                string hash = BitConverter.ToString(hashbytes).Replace("-", string.Empty);

                return hash;
            }
        }

        public static string GenerateHashString(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            using (var sha = new SHA256Managed())
            {
                byte[] textbytes = Encoding.UTF8.GetBytes(text);
                byte[] hashbytes = sha.ComputeHash(textbytes);

                string hash = BitConverter.ToString(hashbytes).Replace("-", string.Empty);

                return hash;
            }
        }
    }
}
