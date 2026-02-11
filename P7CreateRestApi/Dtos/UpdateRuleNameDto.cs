using System.ComponentModel.DataAnnotations;

namespace Dot.Net.WebApi.Dtos
{
    public class UpdateRuleNameDto
    {
        [Required]
        [StringLength(128)]
        public string Name { get; set; } = string.Empty;

        [StringLength(512)]
        public string? Description { get; set; }

        public string? Json { get; set; }
        public string? Template { get; set; }

        [StringLength(512)]
        public string? SqlStr { get; set; }

        [StringLength(512)]
        public string? SqlPart { get; set; }
    }
}
