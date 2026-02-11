
using System.ComponentModel.DataAnnotations;

namespace Dot.Net.WebApi.Domain
{
    public class BidList
    {
        [Key]
        public int BidListId { get; set; }

       
        [Required]
        [StringLength(128)]
        public string Account { get; set; } = string.Empty;

        [Required]
        [StringLength(128)]
        public string BidType { get; set; } = string.Empty;

        
        [Range(0, double.MaxValue)]
        public double? BidQuantity { get; set; }

        [Range(0, double.MaxValue)]
        public double? AskQuantity { get; set; }

        [Range(0, double.MaxValue)]
        public double? Bid { get; set; }

        [Range(0, double.MaxValue)]
        public double? Ask { get; set; }

        
        [StringLength(128)]
        public string? Benchmark { get; set; }

        public DateTime? BidListDate { get; set; }

        [StringLength(512)]
        public string? Commentary { get; set; }

        [StringLength(128)]
        public string? BidSecurity { get; set; }

        [StringLength(64)]
        public string? BidStatus { get; set; }

        [StringLength(128)]
        public string? Trader { get; set; }

        [StringLength(128)]
        public string? Book { get; set; }

        [StringLength(128)]
        public string? CreationName { get; set; }

        public DateTime? CreationDate { get; set; }

        [StringLength(128)]
        public string? RevisionName { get; set; }

        public DateTime? RevisionDate { get; set; }

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
