using GreenApp.Models;
using GreenApp.Helpers;
using Microsoft.AspNetCore.Mvc;
using GreenApp.Models.DTOs;


namespace GreenApp.Controllers
{
    [ApiController]

    public class LoginController : ControllerBase
    {
        private readonly GreenAppContext database;

        public LoginController(GreenAppContext database)
        {
            this.database = database;
        }

        [Route("/Register")]
        [HttpPost]
        public IActionResult Register(VueUserReg account)
        {
            // Check if the username already exists
            if (database.Users.Any(a => a.Username == account.Username))
            {
                return Conflict("Username");
            }
            //Check if email is already in use
            if (database.Users.Any(e => e.EmailAddress == account.Email))
            {
                return Conflict("Email");
            }

            //Validation - Validation - Validation - Validation 
            if (account.Password.Length < 8)
            {
                return Conflict("ShortPassword");
            }

            string hashedPassword = BCrypt.Net.BCrypt.EnhancedHashPassword(account.Password, 13);

            User newUser = new User
            {
                Username = account.Username,
                PasswordHash = hashedPassword,
                EmailAddress = account.Email,
                Role = "User"
            };

            database.Users.Add(newUser);
            database.SaveChanges();

            return Ok("Account created successfully.");
        }

        [Route("/Login")]
        [HttpPost]
        public IActionResult Login(VueUserLogin account)
        {
            var user = database.Users.FirstOrDefault(u => u.Username == account.Username);

            if (user != null)
            {
                if (BCrypt.Net.BCrypt.EnhancedVerify(account.Password, user.PasswordHash))
                {

                    string token = TokenGenerator.GenerateToken(user.UserId, user.Role);

                    Response.Headers.Append("Authorization", "Bearer " + token);

                    return Ok(new { Token = token, Username = user.Username});
                }
                else //Wrong password
                {
                    return Conflict("UsernameOrPassword");
                }
            }
            else //User not found (username invalid)
            {
                return Conflict("UsernameOrPassword");
            }



            
        }
    }
}
