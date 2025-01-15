using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using PruebaTecnica_API.Models;
using PruebaTecnica_API.Models.Auth;
using System.Security.Cryptography;
using System.Text;

namespace PruebaTecnica_API.Services
{
    public class UserService
    {
        private readonly MigrationDbContext _context;

        public UserService(MigrationDbContext context)
        {
            _context = context;
        }

        // Método para verificar si el nombre de usuario ya existe
        public async Task<bool> UserExistsAsync(string username)
        {
            return await _context.Usuarios.AnyAsync(u => u.Username == username);
        }

        // Método para registrar un nuevo usuario
        public async Task<Usuario> RegisterAsync(string username, string password, string role)
        {
            // Cifrar la contraseña antes de almacenarla
            var hashedPassword = HashPassword(password);

            var usuario = new Usuario
            {
                Username = username,
                Password = hashedPassword,
                Role = role
            };

            // Guardar el nuevo usuario en la base de datos
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return usuario;
        }

        // Método para cifrar la contraseña utilizando PBKDF2
        private string HashPassword(string password)
        {
            // Generar un salt (valor aleatorio)
            byte[] salt = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }

            // Aplicar el algoritmo de Hash PBKDF2
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return $"{Convert.ToBase64String(salt)}:{hashed}";
        }

        // Método para verificar la contraseña
        public bool VerifyPassword(string enteredPassword, string storedPassword)
        {
            var parts = storedPassword.Split(':');
            var salt = Convert.FromBase64String(parts[0]);
            var storedHash = parts[1];

            var hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: enteredPassword,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return storedHash == hash;
        }
    }
}
