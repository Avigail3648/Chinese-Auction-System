using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

namespace ChineseAuctionApi.Models
{
    public class Card
    {
        [Key, Required]
        public int CardId { get; set; }

        public int GiftId { get; set; } 
        public int? UserId { get; set; }

        [Required, DefaultValue(false)]
        public bool IsWin { get; set; } = false;

        [ForeignKey("GiftId"),JsonIgnore]
        public virtual Gift? FK_Gift_Card { get; set; }

        [ForeignKey("UserId"), JsonIgnore]
        public virtual User? FK_User_Card { get; set; }       


    }
}
