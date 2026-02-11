using System.ComponentModel.DataAnnotations;

namespace Dot.Net.WebApi.Domain
{
    public class Rating
    {
        public int Id { get; set; }

        [Required]
        [StringLength(125, MinimumLength = 1)]
        public string MoodysRating { get; set; } = string.Empty;

        [Required]
        [StringLength(125, MinimumLength = 1)]
        public string SandPRating { get; set; } = string.Empty;

        [Required]
        [StringLength(125, MinimumLength = 1)]
        public string FitchRating { get; set; } = string.Empty;

        [Range(1, 255, ErrorMessage = "OrderNumber must be between 1 and 255")]
        public byte? OrderNumber { get; set; }
    }
}
