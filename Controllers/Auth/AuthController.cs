using Microsoft.AspNetCore.Mvc;
using PruebaTecnica_API.Services;

namespace PruebaTecnica_API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel login)
        {
            // Simulación de autenticación de usuario
            var username = login.Username; // Por ejemplo, puede ser validado contra una base de datos
            var role = "Admin"; // Simulación de rol (debería provenir de la base de datos)

            var token = _authService.GenerateJwtToken(username, role);

            return Ok(new { Token = token });
        }
    }

    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
