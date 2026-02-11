using System.ComponentModel.DataAnnotations;

namespace Dot.Net.WebApi.Dtos
{
    public class CreateRatingDto
    {
        [Required]
        [StringLength(20)]
        public string? MoodysRating { get; set; }

        [StringLength(20)]
        public string? SandPRating { get; set; }

        [StringLength(20)]
        public string? FitchRating { get; set; }

        [Range(0, 255)]
        public byte? OrderNumber { get; set; }
    }
}
