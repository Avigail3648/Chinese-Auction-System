using System.ComponentModel.DataAnnotations;

namespace ChineseAuctionApi.Models
{
    public class BasketRespons
    {
        public int BasketId { get; set; }
        public GiftWhitNames Gift { get; set; }
        public int? UserId { get; set; }

    }
}
