using GreenApp.Helpers;
using GreenApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace GreenApp.Controllers
{
    [ApiController]

    public class PurchaseController:ControllerBase
    {
        private readonly GreenAppContext database;

        public PurchaseController(GreenAppContext database)
        {
            this.database = database;
        }

        [HttpGet]
        [Route("/Purchase")]
        [TokenValidation]
        public IActionResult PurchaseListing(int carId)
        {

            if (!HttpContext.Items.TryGetValue("UserId", out var userIdObj) || !(userIdObj is int userId))
            {
                return Conflict("Invalid token");
            }

            var listing = database.Listings.FirstOrDefault(i => i.ListingId == carId);

            //Listing no longer active / bought
            if (listing != null)
            {
                listing.Active = 0;

                PurchaseHistory purchaseHistory = new PurchaseHistory
                {
                    UserId = userId,
                    ListingId = carId
                };

                database.PurchaseHistories.Add(purchaseHistory);

                database.SaveChanges();

                return Ok("Purchase complete");
            }

            return Conflict("Purchase error");
        }
        
        
        [HttpGet]
        [Route("/History")]
        [TokenValidation]
        public IActionResult PurchaseHistory()
        {
            if (!HttpContext.Items.TryGetValue("UserId", out var userIdObj) || !(userIdObj is int userId))
            {
                return Conflict("Invalid token");
            }
            //Purchase history using userId
            var history = database.PurchaseHistories.Where(u => u.UserId == userId)
                .Select(l => l.ListingId).ToList();

            //Listings objects using history
            List<Listing> listings = database.Listings
                .Where(i => history.Contains(i.ListingId)).ToList();

            return Ok(listings);
        } 
    }
}
