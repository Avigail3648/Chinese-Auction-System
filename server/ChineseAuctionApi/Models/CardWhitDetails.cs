using System.ComponentModel.DataAnnotations;

namespace ChineseAuctionApi.Models
{
    public class CardWhitDetails
    {
        [Required]
        public int CardId { get; set; }

        [Required]
        public GiftWhitNames gift { get; set; }

        [Required]
        public User user { get; set; }
       
        public bool IsWin { get; set; } = false;
    }
}
