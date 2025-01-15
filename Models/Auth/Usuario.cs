using System.ComponentModel.DataAnnotations;

namespace PruebaTecnica_API.Models.Auth
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }  // La contraseña será cifrada

        [Required]
        [StringLength(50)]
        public string Role { get; set; }
    }
}
    