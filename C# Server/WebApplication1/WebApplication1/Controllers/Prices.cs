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
                int newPriceId = _priceService.AddPrice(request.LinkId, request.Price, request.Currency);
                return Ok(newPriceId);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "A apărut o eroare în timpul adăugării prețului: " + ex.Message);
            }
        }

        [HttpPut("{priceId}")]
        public IActionResult UpdatePrice(int priceId, PriceRequest request)
        {
            try
            {
                bool updated = _priceService.UpdatePrice(priceId, request.LinkId, request.Price, request.Currency);
                if (updated)
                    return Ok("Prețul a fost actualizat cu succes.");
                else
                    return NotFound("Prețul nu a fost găsit.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "A apărut o eroare în timpul actualizării prețului: " + ex.Message);
            }
        }

        [HttpDelete("{priceId}")]
        public IActionResult DeletePrice(int priceId, int linkId)
        {
            try
            {
                bool deleted = _priceService.DeletePrice(priceId, linkId);
                if (deleted)
                    return Ok("Prețul a fost șters cu succes.");
                else
                    return NotFound("Prețul nu a fost găsit.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "A apărut o eroare în timpul ștergerii prețului: " + ex.Message);
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
                return StatusCode(500, "A apărut o eroare în timpul obținerii prețurilor: " + ex.Message);
            }
        }

    }


    public class PriceRequest
    {
        public int LinkId { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; }
    }
}
