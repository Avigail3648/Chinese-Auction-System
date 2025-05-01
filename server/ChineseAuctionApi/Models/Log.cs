using System.ComponentModel.DataAnnotations;

namespace ChineseAuctionApi.Models
{
    public class Log
    {
        [Key]
        public int MyProperty { get; set; }
        public DateTime Date { get; set; }
        public string Message { get; set; }
    }
}
