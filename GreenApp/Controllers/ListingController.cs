using GreenApp.Helpers;
using GreenApp.Migrations;
using GreenApp.Models;
using Microsoft.AspNetCore.Mvc;
using GreenApp.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace GreenApp.Controllers
{
    [ApiController]

    public class ListingsController : ControllerBase
    {
        private readonly GreenAppContext database;
        private readonly IWebHostEnvironment env;

        public ListingsController(GreenAppContext database, IWebHostEnvironment env)
        {
            this.database = database;
            this.env = env;
        }

        [HttpGet]
        [Route("/GetListings")]
        public IActionResult GetListings()
        {
            //Temporary --- --- --- --- Temporary
            foreach (var listing in database.Listings)
            {
                if (listing.Active == null)
                {
                    listing.Active = 1;
                }
            }
            database.SaveChanges();
            //Temporary --- --- --- --- Temporary

            List<Listing> listings = new List<Listing>();

            var sortedListings = database.Listings.OrderBy(d => d.PublishDate)
                .Where(a => a.Active == 1).ToList();

            foreach (var sortedListing in sortedListings)
            {
                listings.Add(sortedListing);
            }

            return Ok(listings);
        }

        [HttpPost]
        [Route("/Listing")]
        [TokenValidation]
        public async Task<IActionResult> CreateListing(VueListing listing)
        {
            //Fetch userId and validate returned by custom Attribute
            if (!HttpContext.Items.TryGetValue("UserId", out var userIdObj) || !(userIdObj is int userId))
            {
                return Conflict("Invalid token");
            }

            //image
            if (listing.File == null || listing.File.Length == 0)
            {
                return Conflict("image");
            }

            var uploadFolder = Path.Combine(env.ContentRootPath, "images");

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(listing.File.FileName);

            var filePath = Path.Combine(uploadFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await listing.File.CopyToAsync(stream);
            }

            var fileUrl = $"{Request.Scheme}://{Request.Host}/images/{uniqueFileName}";

            //image

            Listing newListing = new Listing
            {
                UserId = userId,
                Color = listing.Color,
                Brand = listing.Brand,
                ModelName = listing.ModelName,
                Milage = listing.Milage,
                FuelType = listing.FuelType,
                Gearbox = listing.Gearbox,
                Price = listing.Price,
                Horsepower = listing.Horsepower,
                ProductionYear = listing.ProductionYear,
                Description = listing.Description,
                PublishDate = DateTime.Now,
                Active = 1,
                GUID = uniqueFileName
                ///images/filename + file extension is now saved in database = image
            };

            database.Listings.Add(newListing);

            database.SaveChanges();

            return Ok("Listing created successfully");

        }
        //Endpoint for getting user specific listings
        [HttpGet]
        [Route("/ProfileListings")]
        [TokenValidation]
        public IActionResult GetUserListing()
        {
            //Fetch and validate userId returned by custom Attribute
            if (!HttpContext.Items.TryGetValue("UserId", out var userIdObj) || !(userIdObj is int userId))
            {
                return Conflict("Invalid token");
            }

            if (userId != -1) //Success 
            {
                List<Listing> listingsResult = new List<Listing>();

                var tempListings = database.Listings.Where(u => u.UserId == userId);

                foreach (var listing in tempListings)
                {
                    listingsResult.Add(listing);
                }

                return Ok(listingsResult);
            }
            else
            {
                return Conflict("Token invalid");
            }
        }


        // GetCarDetails endpoint
        [HttpGet]
        [Route("GetCarDetails/{carId}")]
        [TokenValidation]
        public ActionResult<Listing> GetCarDetails(int carId)
        {
            var listing = database.Listings.FirstOrDefault(l => l.ListingId == carId);

            if (listing == null)
            {
                return NotFound();
            }

            return Ok(listing);
        }

        [HttpPost]
        [Route("/DeleteListing")]
        [TokenValidation]
        public IActionResult DeleteSingleListing(int listingId)
        {
            //Fetch and validate userId returned by custom Attribute
            if (!HttpContext.Items.TryGetValue("UserId", out var userIdObj) || !(userIdObj is int userId))
            {
                return Conflict("Invalid token");
            }
            //Fetch userRole and validate from attribute
            if (!HttpContext.Items.TryGetValue("UserRole", out var userRoleObj) || !(userRoleObj is string userRole))
            {
                return Conflict("Error");
            }


            //Delete your own listing
            var ownerDelete = database.Listings.Where(u => u.UserId == userId)
                .FirstOrDefault(l => l.ListingId == listingId);

            if (ownerDelete != null)
            {
                database.Listings.Remove(ownerDelete);
                database.SaveChanges();

                return Ok();
            }

            //Admin delete
            if (userRole == "Admin")
            {
                var adminDelete = database.Listings.FirstOrDefault(l => l.ListingId == listingId);

                if (adminDelete != null)
                {
                    database.Listings.Remove(adminDelete);
                    database.SaveChanges();

                    return Ok();
                }
            }
            return Conflict(); 
        }
    }
}
