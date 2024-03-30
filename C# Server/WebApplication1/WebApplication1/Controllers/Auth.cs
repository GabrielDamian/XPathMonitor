using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using System;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

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
            var username = request.Username;
            var password = request.Password;

            bool isAuthenticated = _dataService.ValidateUser(username, password);

            if (isAuthenticated)
            {
                var token = GenerateJwtToken(username);
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
            var username = request.Username;
            var password = request.Password;

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
            // Obține tokenul JWT din antetul cererii
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token == null)
            {
                return Unauthorized("Tokenul lipsește.");
            }

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"]);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                }, out var validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var username = jwtToken.Claims.First(x => x.Type == ClaimTypes.Name).Value;

                var userExists = _dataService.UserExists(username);
                if (!userExists)
                {
                    return NotFound("Utilizatorul nu există.");
                }

                return Ok("Tokenul și utilizatorul sunt valide.");
            }
            catch (Exception ex)
            {
                return BadRequest("Tokenul nu este valid: " + ex.Message);
            }
        }

        private string GenerateJwtToken(string username)
        {
            var jwtToken = _configuration["Jwt:Secret"];

            var key = Encoding.ASCII.GetBytes(jwtToken);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, username)
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
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class SignUpRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
