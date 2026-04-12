using System.Security.Cryptography;
using System.Text;
namespace Tamkeen.Infrastructure.Setting
{
    public static class ConfirmationCodeHasher
    {
        public static string Hash(string code)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(code);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        public static bool Verify(string code, string hashedCode)
        {
            var hashed = Hash(code);
            return hashed == hashedCode;
        }
    }
}
