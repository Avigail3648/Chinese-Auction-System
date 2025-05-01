using ChineseAuctionApi.Models;
using ChineseAuctionApi.Repositories;
using ChineseAuctionApi.Services.Donors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChineseAuctionApi.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class DonorsController : ControllerBase
    {
        private readonly IDonorService _donorService;

        public DonorsController(IDonorService donorService)
        {
            _donorService = donorService;
        }

        [HttpGet]
        public ActionResult<List<Donor>?> getAll()
        {
            var donors = _donorService.getAll();
            if (donors != null)
                return Ok(donors);
            return BadRequest("failed");
        }

        [HttpGet("{id}")]
        public IActionResult getDonorById(int id)
        {
            try
            {
                Donor? donor = _donorService.getDonorById(id);
                if (donor == null)
                    return BadRequest("failed");
                return Ok(donor);
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(e.GetType() + ": " + e.Message);
            }
        }

        [HttpPost]
        public IActionResult addDonor(Donor donor)
        {
            var donorsAfterAdd = _donorService.addDonor(donor);
            if (donorsAfterAdd == null)
                return BadRequest("failed");
            return Ok(donorsAfterAdd);

        }

        [HttpPut]
        public IActionResult updateDonor(Donor donor)
        {
            try
            {
                var donorsAfterUpdate = _donorService.updateGift(donor);
                if (donorsAfterUpdate == null)
                    return BadRequest("failedddd update");

                return Ok(donorsAfterUpdate);
            }
            catch(KeyNotFoundException e)
            {
                return NotFound(e.GetType() + ": " + e.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult deleteDonor(int id)
        {
            try
            {
                List<Donor>? donorAfterDelete = _donorService.deleteDonor(id);
                if (donorAfterDelete == null)
                    return BadRequest("failed");
                return Ok(donorAfterDelete);
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(e.GetType() + ": " + e.Message);
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
