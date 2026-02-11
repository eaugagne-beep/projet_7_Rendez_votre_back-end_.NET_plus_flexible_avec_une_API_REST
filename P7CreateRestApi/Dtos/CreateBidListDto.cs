using System.ComponentModel.DataAnnotations;

namespace Dot.Net.WebApi.Dtos
{
    public class CreateBidListDto
    {
        [Required]
        public string Account { get; set; } = string.Empty;

        [Required]
        public string BidType { get; set; } = string.Empty;

        public double? BidQuantity { get; set; }
        public double? AskQuantity { get; set; }
        public double? Bid { get; set; }
        public double? Ask { get; set; }

        public string? Benchmark { get; set; }
        public DateTime? BidListDate { get; set; }
        public string? Commentary { get; set; }
        public string? BidSecurity { get; set; }
        public string? BidStatus { get; set; }
        public string? Trader { get; set; }
        public string? Book { get; set; }
        public string? DealName { get; set; }
        public string? DealType { get; set; }
        public string? SourceListId { get; set; }
        public string? Side { get; set; }
    }
}
