using System.Text;

namespace GreenApp.Helpers
{
    public class TokenReader
    {
        public static (int userId, string userRole) ReadToken(string token)
        {
            string decodedToken = DecodeToken(token);

            if (decodedToken.StartsWith("Bearer "))
            {
                decodedToken = decodedToken.Substring("Bearer ".Length);
            }

            string[] parts = decodedToken.Split('*');

            string firstDateTimeString = parts[0];
            string userIdString = parts[1];
            //Might use this later on
            string userRole = parts[2];

            DateTime firstDateTime = DateTime.Parse(firstDateTimeString);

            int userId = int.Parse(userIdString);

            if (firstDateTime > DateTime.Now)
            {
                //Valid
                return (userId, userRole);
            }
            else
            {
                //Expired
                return (-1, "null");
            }
        }

        public static string DecodeToken(string cipherText)
        {
            if (cipherText.StartsWith("Bearer "))
            {
                cipherText = cipherText.Substring("Bearer ".Length);
            }

            try
            {
                byte[] base64Bytes = Convert.FromBase64String(cipherText);
                string plainText = Encoding.UTF8.GetString(base64Bytes);
                return plainText;
            }
            catch (FormatException ex)
            {
                Console.WriteLine("Error decoding Base64 string: " + ex.Message);
                return null;
            }
        }
    }
}
