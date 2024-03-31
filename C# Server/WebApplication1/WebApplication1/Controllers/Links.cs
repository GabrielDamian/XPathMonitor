using Microsoft.AspNetCore.Mvc;
using WebApplication1.Persistence;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LinksController : ControllerBase
    {
        private readonly ILinkService _linkService;
        private readonly IConfiguration _configuration;

        public LinksController(ILinkService linkService, IConfiguration configuration)
        {
            _linkService = linkService;
            _configuration = configuration;
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetAllLinks()
        {
            try
            {
                var userId = Helpers.GetUserIdFromToken(HttpContext, _configuration);
                var links = _linkService.GetLinks(userId);
                return Ok(links);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "A apărut o eroare în timpul obținerii legăturilor: " + ex.Message);
            }
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddLink(LinkRequest request)
        {
            try
            {
                var userId = Helpers.GetUserIdFromToken(HttpContext, _configuration);
                var linkId = _linkService.AddLink(userId, request.url, request.description);
                return Ok(new { linkId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "A apărut o eroare în timpul adăugării legăturii: " + ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public IActionResult UpdateLink(int id, LinkRequest request)
        {
            try
            {
                var userId = Helpers.GetUserIdFromToken(HttpContext, _configuration);
                bool success = _linkService.UpdateLink(id, userId, request.url, request.description);
                if (success)
                {
                    return Ok("Legătura a fost actualizată cu succes.");
                }
                else
                {
                    return NotFound("Legătura nu a fost găsită sau nu aparține utilizatorului autentificat.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "A apărut o eroare în timpul actualizării legăturii: " + ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult DeleteLink(int id)
        {
            try
            {
                var userId = Helpers.GetUserIdFromToken(HttpContext, _configuration);
                bool success = _linkService.DeleteLink(id, userId);
                if (success)
                {
                    return Ok("Legătura a fost ștearsă cu succes.");
                }
                else
                {
                    return NotFound("Legătura nu a fost găsită sau nu aparține utilizatorului autentificat.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "A apărut o eroare în timpul ștergerii legăturii: " + ex.Message);
            }
        }
    }

    public class LinkRequest
    {
        public string url { get; set; }
        public string description { get; set; }
    }
}
