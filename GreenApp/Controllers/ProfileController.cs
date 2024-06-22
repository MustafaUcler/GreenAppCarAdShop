using GreenApp.Helpers;
using GreenApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace GreenApp.Controllers
{
    [ApiController]

    public class ProfileController : ControllerBase
    {
        private readonly GreenAppContext database;

        public ProfileController(GreenAppContext database)
        {
            this.database = database;
        }
        //Remember to add custom attribute to use token here also
        [Route("/Profile/{username}")]
        [HttpGet]
        public IActionResult UserProfile(string username)
        {
            var user = database.Users.FirstOrDefault(u => u.Username == username);

            if (user != null) // User info/data (Name and Email)
            {
                var userProfileData = new
                {
                    Name = user.Username,
                    Email = user.EmailAddress
                };

                return Ok(userProfileData);
            }
            else //User not found
            {
                return NotFound();
            }
        }

        [Route("/Delete")]
        [HttpPost]
        [TokenValidation]
        public IActionResult DeleteListing(int listingId)
        {
            if (!HttpContext.Items.TryGetValue("UserId", out var userIdObj) || !(userIdObj is int userId))
            {
                return Conflict("Invalid token");
            }

            var listingToDelete = database.Listings.Where(u => u.UserId == userId)
                .FirstOrDefault(i => i.ListingId == listingId);

            if (listingToDelete != null)
            {
                database.Listings.Remove(listingToDelete);

                database.SaveChanges();

                return Ok();
            }
            return Conflict();
        }
    }
}

