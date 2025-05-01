using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ChineseAuctionApi.Models
{
    public class Donor
    {
        [Key, Required]
        public int DonorId { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(10)]
        public string? Phone { get; set; }

        [MaxLength(100)]
        public string? Address { get; set; }

        [Required, MaxLength(200), EmailAddress]
        public string Mail { get; set; }

        [JsonIgnore]
        public virtual ICollection<Gift>? DonorGifts { get; set; } = new List<Gift>();
    }
}
