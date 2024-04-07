using Microsoft.AspNetCore.Mvc;
using WebApplication1.Persistence;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PricesController : ControllerBase
    {
        private readonly IPriceService _priceService;

        public PricesController(IPriceService priceService)
        {
            _priceService = priceService;
        }

        [HttpPost]
        public IActionResult AddPrice(PriceRequest request)
        {
            try
            {
                int newPriceId = _priceService.AddPrice(request.LinkId, request.Price, "RON");
                return Ok(newPriceId);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "A apărut o eroare în timpul adăugării prețului: " + ex.Message);
            }
        }


       
        [HttpGet("{linkId}")]
        public IActionResult GetPrices(int linkId)
        {
            try
            {
                var prices = _priceService.GetPrices(linkId);
                if (prices.Count > 0)
                    return Ok(prices);
                else
                    return NotFound("Nu s-au găsit prețuri pentru link-ul specificat.");
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                return StatusCode(500, "A apărut o eroare în timpul obținerii prețurilor: " + ex.Message);
            }
        }

    }


    public class PriceRequest
    {
        public int LinkId { get; set; }
        public decimal Price { get; set; }
    }
}
