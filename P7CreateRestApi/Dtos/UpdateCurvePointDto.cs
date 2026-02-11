using System.ComponentModel.DataAnnotations;

namespace Dot.Net.WebApi.Dtos
{
    public class UpdateCurvePointDto
    {
        [Required]
        public byte? CurveId { get; set; }

        public DateTime? AsOfDate { get; set; }
        public double? Term { get; set; }
        public double? CurvePointValue { get; set; }
    }
}
