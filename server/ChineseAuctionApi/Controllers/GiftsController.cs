using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ChineseAuctionApi.Models;
using ChineseAuctionApi.Repositories;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using ChineseAuctionApi.Services.Gifts;

namespace ChineseAuctionApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GiftsController : ControllerBase
    {
        private readonly IGiftService _GiftService;

        public GiftsController(IGiftService giftService)
        {
            _GiftService = giftService;
        }
        
        [HttpGet]
        public IActionResult getAllWhitNames()
        {
            var listGift = _GiftService.getAllWhitNames();
            if (listGift != null)
                return Ok(listGift);
            return BadRequest("failed");
        }

        [HttpGet("{id}")]
        public IActionResult getGiftById(int id)
        {
            try
            {
                Gift? gift = _GiftService.getGiftById(id);
                if (gift != null)
                    return Ok(gift);
                return BadRequest("failed");
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(e.GetType() + ": " + e.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult<string> addGift([FromBody] GiftWhitNames gift)
        {
            try
            {
                var giftsAfterCreat = _GiftService.addGift(gift);
                if (giftsAfterCreat == null)
                    return BadRequest("failed");
                return Ok(giftsAfterCreat);
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(e.GetType() + ": " + e.Message);
            }
            catch (InvalidDataException e)
            {
                return BadRequest(e.GetType() + ": " + e.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult deleteGift(int id)
        {
            try
            {
                List<GiftWhitNames>? giftAfterDelete = _GiftService.deleteGift(id);
                if (giftAfterDelete == null)
                    return BadRequest("failed");
                return Ok(giftAfterDelete);
            }
            catch(KeyNotFoundException e)
            {
                return NotFound(e.GetType() + ": " + e.Message);
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public IActionResult updateGift([FromBody] GiftWhitNames gift)
        {
            try
            {
                var giftsAfterUpdate = _GiftService.updateGift(gift);
                if (giftsAfterUpdate == null)
                    return BadRequest("failedddd");
                return Ok(giftsAfterUpdate);
            }
            catch (InvalidDataException e)
            {
                return BadRequest(e.GetType() + ": " + e.Message);
            }
            catch(KeyNotFoundException e)
            {
                return NotFound(e.GetType() + ": " + e.Message);
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("CategoriesNames")]
        [HttpGet]
        public IActionResult getCategoriesNames()
        {
            var categories = _GiftService.getCategoriesNames();
            if (categories != null)
                return Ok(categories);
            return BadRequest("failed");
        }

        [Route("getGiftsByDonorID/{donorid}")]
        [HttpGet]
        public IActionResult getGiftsByDonorID(int donorid)
        {
            var gifts = _GiftService.getGiftsByDonorID(donorid);
            if (gifts != null)
                return Ok(gifts);
            return BadRequest("failed");

        }
    }
}