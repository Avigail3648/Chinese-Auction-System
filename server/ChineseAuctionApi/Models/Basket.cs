using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ChineseAuctionApi.Models
{
    public class Basket
    {
        
        [Key, Required]
        public int BasketId { get; set; }
        public int? GiftId { get; set; }
        public int? UserId { get; set; }

        [ForeignKey("GiftId")]
        public virtual Gift? FK_Basket_Gift { get; set; }

        [ForeignKey("UserId")]
        public virtual User? FK_Basket_User { get; set; }
    }
}
