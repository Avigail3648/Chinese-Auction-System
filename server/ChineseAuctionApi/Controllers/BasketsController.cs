using ChineseAuctionApi.Models;
using ChineseAuctionApi.Services.aa;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ChineseAuctionApi.Controllers
{
    [Authorize(Roles = "User")]
    [Route("api/[controller]")]
    [ApiController]
    public class BasketsController : ControllerBase
    {
        private readonly IBasketService _BasketService;
        public BasketsController(IBasketService basketService)
        {
            _BasketService = basketService;
        }
        [HttpGet]
        public IActionResult getBasketByUser()
        {
            try
            {
                var basket = _BasketService.getBasketByUser();
                if (basket == null)
                    return BadRequest("failed");
                return Ok(basket);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.GetType() + ": " + ex.Message);
            }
            catch (SecurityTokenMalformedException ex)
            {
                return BadRequest(ex.GetType() + ": " + ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return BadRequest(ex.GetType() + ": " + ex.Message);
            }
        }

        [HttpPost("{giftID}")]
        public IActionResult addBasket(int giftID)
        {
            try
            {
                var basketAfterAdd = _BasketService.addBasket(giftID);
                if (basketAfterAdd == null)
                    return BadRequest("failed");
                return Ok(basketAfterAdd);
            }
            catch (UnauthorizedAccessException ex)
            {
                return BadRequest(ex.GetType() + ": " + ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.GetType() + ": " + ex.Message);
            }
        }

        [HttpDelete("{basketID}")]
        public IActionResult deleteBasket(int basketID)
        {
            var basketAfterDelete = _BasketService.deleteBasket(basketID);
            if (basketAfterDelete == null)
                return BadRequest("failed");
            return Ok(basketAfterDelete);
        }
    }
}
