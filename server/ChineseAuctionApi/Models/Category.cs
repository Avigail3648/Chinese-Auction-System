using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ChineseAuctionApi.Models
{
    public class Category
    {
        [Key,Required]
        public int CategoryId { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }
    }
}
