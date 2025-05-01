using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace ChineseAuctionApi.Models
{
    public class User
    {
        [Key, Required]
        public int UserId { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(10)]
        public string? Phone { get; set; }

        [MaxLength(100)]
        public string? Address { get; set; }

        [Required, MaxLength(200), EmailAddress]
        public string Mail { get; set; }
        [Required]
        public string Password { get; set; }
        [Required, JsonIgnore]
        public string Role { get; set; } = "User";
    }
}
