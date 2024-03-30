using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IDataService _dataService;

        public AuthController(IDataService dataService)
        {
            _dataService = dataService;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequest request)
        {
            var username = request.Username;
            var password = request.Password;

            bool isAuthenticated = _dataService.ValidateUser(username, password);

            if (isAuthenticated)
            {
                return Ok("Autentificare reușită!");
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
