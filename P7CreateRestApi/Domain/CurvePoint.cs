
using System.ComponentModel.DataAnnotations;

namespace Dot.Net.WebApi.Domain
{
    public class CurvePoint
    {
        public int Id { get; set; }

        [Required]
        [Range(0, byte.MaxValue, ErrorMessage = "CurveId must be positive")]
        public byte? CurveId { get; set; }

        public DateTime? AsOfDate { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Term must be >= 0")]
        public double? Term { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "CurvePointValue must be >= 0")]
        public double? CurvePointValue { get; set; }

        public DateTime? CreationDate { get; set; }
    }
}
