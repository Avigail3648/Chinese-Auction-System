using System.ComponentModel.DataAnnotations;

namespace ChineseAuctionApi.Models
{
    public class LoginRequestt
    {
        [Required, MaxLength(200), EmailAddress]
        public string Mail { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
