using Microsoft.AspNetCore.Mvc;
using PruebaTecnica_API.Models.Auth;
using PruebaTecnica_API.Services;

namespace PruebaTecnica_API.Controllers
{
    [Route("api/usuarios")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly UserService _usuarioService;

        public UsuarioController(UserService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Usuario model)
        {
            // Validar los datos de entrada
            if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password) || string.IsNullOrEmpty(model.Role))
            {
                return BadRequest("Faltan parámetros.");
            }

            // Verificar si el nombre de usuario ya está registrado
            var userExists = await _usuarioService.UserExistsAsync(model.Username);
            if (userExists)
            {
                return Conflict("El nombre de usuario ya está en uso.");
            }

            // Registrar el nuevo usuario
            var user = await _usuarioService.RegisterAsync(model.Username, model.Password, model.Role);
            return Ok(new { Id = user.Id, Username = user.Username, Role = user.Role });
        }
    }

}
