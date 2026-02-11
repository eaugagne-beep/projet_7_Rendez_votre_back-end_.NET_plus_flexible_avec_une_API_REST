using System.ComponentModel.DataAnnotations;

namespace Dot.Net.WebApi.Domain
{
    public class RuleName
    {
        public int Id { get; set; }

        [Required]
        [StringLength(125, MinimumLength = 1)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(1024)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string Json { get; set; } = string.Empty;

        [Required]
        public string Template { get; set; } = string.Empty;

        [Required]
        public string SqlStr { get; set; } = string.Empty;

        [Required]
        public string SqlPart { get; set; } = string.Empty;
    }
}
