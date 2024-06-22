using GreenApp.Models;
using GreenApp.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace GreenApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly GreenAppContext _context;

        public SearchController(GreenAppContext context)
        {
            _context = context;
        }

        [HttpGet("SearchListings")]
        public IActionResult SearchListings([FromQuery] SearchCriteria criteria)
        {
            if (string.IsNullOrEmpty(criteria.SearchPhrase) && string.IsNullOrEmpty(criteria.Color))
            {
                return BadRequest("Search criteria cannot be empty");
            }

            var query = _context.Listings.AsQueryable();

            if (!string.IsNullOrEmpty(criteria.SearchPhrase))
            {
                string searchPhrase = criteria.SearchPhrase.ToLower();
                query = query.Where(l => l.Brand.ToLower().Contains(searchPhrase) || l.ModelName.ToLower().Contains(searchPhrase));
            }

            if (!string.IsNullOrEmpty(criteria.Color) && criteria.Color.ToLower() != "all")
            {
                query = query.Where(l => l.Color.ToLower() == criteria.Color.ToLower());
            }

            var listings = query.OrderBy(d => d.PublishDate).ToList();

            return Ok(listings);
        }
    }
}
