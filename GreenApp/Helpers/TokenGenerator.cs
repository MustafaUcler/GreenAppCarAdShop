using System.Security.Cryptography;
using System.Text;

namespace GreenApp.Helpers
{
    public class TokenGenerator
    {
        public static string GenerateToken(int user, string role)
        {
            DateTime currentTime = DateTime.Now;

            DateTime expirationTime = currentTime.AddHours(1); 

            //Build token, expire date * userId * userRole
            string token = expirationTime.ToString() + "*" + user + "*" + role;

            string encodedToken = EncodeToken(token);

            return encodedToken;
        }
        public static string EncodeToken(string plainText)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            string base64String = Convert.ToBase64String(plainTextBytes);

            return base64String;
        }
    }
}

