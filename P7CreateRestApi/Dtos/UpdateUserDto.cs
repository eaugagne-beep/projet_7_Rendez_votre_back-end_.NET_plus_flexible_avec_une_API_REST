using System.ComponentModel.DataAnnotations;

namespace Dot.Net.WebApi.Dtos
{
    public class UpdateUserDto
    {
        [StringLength(128)]
        public string? FullName { get; set; }

        [StringLength(64)]
        public string? Role { get; set; }
    }
}
