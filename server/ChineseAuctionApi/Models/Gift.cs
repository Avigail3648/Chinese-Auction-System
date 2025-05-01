using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChineseAuctionApi.Models
{
    public class Gift
    {
        [Key,Required]
        public int GiftId { get; set; }

        [Required, MaxLength(20), MinLength(2)]
        public string Name { get; set; }

        [MaxLength(250)]
        public string? Details { get; set; }

        [Required]
        public int DonorId { get; set; }
       
        public int? MonetaryValue { get; set; }
        [Required, Range(10,50)]
        public int CardPrice { get; set; }
        [Required, MaxLength(300)]
        public string Image { get; set; }
        [Required]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category? FK_Gift_Category { get; set; }

        [ForeignKey("DonorId")]
        public virtual Donor? FK_Gift_Donor { get; set; }


    }
}
