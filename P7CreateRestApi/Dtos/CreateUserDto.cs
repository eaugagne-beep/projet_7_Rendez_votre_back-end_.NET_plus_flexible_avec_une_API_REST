using System.ComponentModel.DataAnnotations;

namespace Dot.Net.WebApi.Dtos
{
    public class CreateUserDto
    {
        [Required]
        [StringLength(64)]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [StringLength(128)]
        public string Password { get; set; } = string.Empty;

        [StringLength(128)]
        public string? FullName { get; set; }

        [Required]
        [StringLength(64)]
        public string Role { get; set; } = "User";
    }
}
