using System.ComponentModel.DataAnnotations;

namespace Dot.Net.WebApi.Dtos
{
    public class UpdateTradeDto
    {
        [Required]
        [StringLength(128)]
        public string Account { get; set; } = string.Empty;

        [StringLength(128)]
        public string? AccountType { get; set; }

        [Range(0, double.MaxValue)]
        public double? BuyQuantity { get; set; }

        [Range(0, double.MaxValue)]
        public double? SellQuantity { get; set; }

        [Range(0, double.MaxValue)]
        public double? BuyPrice { get; set; }

        [Range(0, double.MaxValue)]
        public double? SellPrice { get; set; }

        public DateTime? TradeDate { get; set; }

        [StringLength(128)]
        public string? TradeSecurity { get; set; }

        [StringLength(64)]
        public string? TradeStatus { get; set; }

        [StringLength(128)]
        public string? Trader { get; set; }

        [StringLength(128)]
        public string? Benchmark { get; set; }

        [StringLength(128)]
        public string? Book { get; set; }

        [StringLength(128)]
        public string? DealName { get; set; }

        [StringLength(128)]
        public string? DealType { get; set; }

        [StringLength(128)]
        public string? SourceListId { get; set; }

        [StringLength(32)]
        public string? Side { get; set; }
    }
}
