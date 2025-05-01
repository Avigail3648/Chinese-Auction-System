using ChineseAuctionApi.Models;
using ChineseAuctionApi.Services.Lotteries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChineseAuctionApi.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class LotteryController : ControllerBase
    {
        private readonly ILotteryService _lotteryService;

        public LotteryController(ILotteryService lotteryService)
        {
            _lotteryService = lotteryService;
        }
        
        [HttpGet("{giftID}")]
        public IActionResult randomWinPerGift(int giftID)
        {
            try
            {
                CardWhitDetails? winnerCard = _lotteryService.randomWinPerGift(giftID);
                if (winnerCard == null)
                    return BadRequest("failed");
                return Ok(winnerCard);
            }
            catch(BadHttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
