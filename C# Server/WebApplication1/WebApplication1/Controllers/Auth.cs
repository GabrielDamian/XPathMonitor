using System.Text;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IDataService _dataService;
        private readonly IConfiguration _configuration;

        public AuthController(IDataService dataService, IConfiguration configuration)
        {
            _dataService = dataService;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequest request)
        {
            var username = request.username;
            var password = request.password;

            (bool isAuthenticated, int userId) = _dataService.ValidateUser(username, password);

            if (isAuthenticated)
            {
                var token = GenerateJwtToken(username, userId);
                return Ok(new { Token = token });
            }
            else
            {
                return Unauthorized("Nume de utilizator sau parolă incorectă!");
            }
        }

        [HttpPost("signup")]
        public IActionResult SignUp(SignUpRequest request)
        {
            var username = request.username;
            var password = request.password;

            if (_dataService.UserExists(username))
            {
                return Conflict("Numele de utilizator este deja înregistrat!");
            }

            bool userCreated = _dataService.CreateUser(username, password);

            if (userCreated)
            {
                return Ok("Înregistrare reușită!");
            }
            else
            {
                return StatusCode(500, "A apărut o eroare în timpul înregistrării!");
            }
        }

        [HttpGet("verify-token")]
        [Authorize]
        public IActionResult VerifyToken()
        {
            try
            {
                var userId = Helpers.GetUserIdFromToken(HttpContext, _configuration);
                return Ok("Tokenul și utilizatorul sunt valide.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest("Tokenul nu este valid: " + ex.Message);
            }
        }

        private string GenerateJwtToken(string username, int userId)
        {
            var jwtToken = _configuration["JWT_TOKEN"];

            var key = Encoding.ASCII.GetBytes(jwtToken);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
            new Claim("username", username),
            new Claim("userId", userId.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }

    public class LoginRequest
    {
        public string username { get; set; }
        public string password { get; set; }
    }

    public class SignUpRequest
    {
        public string username { get; set; }
        public string password { get; set; }
    }
}
