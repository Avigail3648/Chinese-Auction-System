using System.ComponentModel.DataAnnotations;

namespace ChineseAuctionApi.Models
{
    public class GiftWhitNames
    {
        [Key, Required]
        public int GiftId { get; set; }

        [Required, MaxLength(20), MinLength(2)]

        public string Name { get; set; }
        [MaxLength(250)]
        public string? Details { get; set; }
        [Required]
        public string DonorName { get; set; }
        public int NumberWinners { get; set; }
        public int? MonetaryValue { get; set; }
        [Required, Range(10, 50)]
        public int CardPrice { get; set; }
        [Required, MaxLength(300)]
        public string Image { get; set; }
        [Required]
        public string categoryName { get; set; }
    }
}
