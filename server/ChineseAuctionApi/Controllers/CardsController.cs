using ChineseAuctionApi.Models;
using ChineseAuctionApi.Services.Cards;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChineseAuctionApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardsController : ControllerBase
    {
        private readonly ICardService _cardService;

        public CardsController(ICardService cardService)
        {
            _cardService = cardService;
        }

        [Authorize(Roles = "Admin,User")]
        [HttpGet("{giftID}")]
        public IActionResult getAllCardsByGiftID(int giftID)
        {
            var cards = _cardService.getAllCardsByGiftID(giftID);
            if (cards == null)
                return BadRequest("failed");
            return Ok(cards);
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        public IActionResult addCards(List<Card> cards)
        {
            try
            {
                var cardsList = _cardService.addCards(cards);
                if (cardsList == null)
                    return BadRequest("failed");
                return Ok(cardsList);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.GetType() + ": " + ex.Message);
            }
        }

        [Authorize(Roles = "User,Admin")]
        [HttpPut]
        public IActionResult updateCardToWin(int cardID)
        {
            var cardAfterUpdate = _cardService.updateCardToWin(cardID);
            if (cardAfterUpdate == null)
                return BadRequest("failed");
            return Ok(cardAfterUpdate);
        }

        [Authorize(Roles = "User,Admin")]
        [Route("getAllWinCard")]
        [HttpGet]
        public IActionResult getAllWinCard()
        {
            var cardsWin = _cardService.getAllWinCard();
            if (cardsWin == null)
                return BadRequest("failed");
            return Ok(cardsWin);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("getAllRevenue")]
        public IActionResult getAllRevenue()
        {
            var revenue = _cardService.getAllRevenue();
            if (revenue == null)
                return BadRequest("failed");
            return Ok(revenue);
        }
    }
}
