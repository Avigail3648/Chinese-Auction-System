using ChineseAuctionApi.Models;
using ChineseAuctionApi.Repositories.Baskets;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChineseAuctionApi.Repositories.Donors
{
    public class DonorRepository : IDonorRepository
    {
        private readonly DBContext _DbContext;
        private readonly ILogger<BasketRepository> _logger;

        public DonorRepository(DBContext DbContext, ILogger<BasketRepository> logger)
        {
            _logger = logger;
            _DbContext = DbContext;
        }

        public List<Donor>? getAll()
        {
            try
            {
                return _DbContext.Donors.Include(u => u.DonorGifts).ToList();
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed Get All Donors", e);
                return null;
            }
        }


        public Donor? getDonorById(int id)
        {
            try
            {
                return _DbContext.Donors.FirstOrDefault(donor => donor.DonorId == id);
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed Get Donor ById {id}", e);
                return null;
            }
        }

        public List<Donor>? addDonor(Donor donor)
        {
            try
            {
                donor.DonorId = 0;
                _DbContext.Donors.Add(donor);
                _DbContext.SaveChanges();
                return getAll();
            }
            catch (Exception e)
            {
                _logger.LogError("Failed Add New Donor", e);
                return null;
            }
        }

        public List<Donor>? updateGift(Donor donor)
        {
            try
            {
                var existingDonor = _DbContext.Donors.SingleOrDefault(d => d.DonorId == donor.DonorId);
                if (existingDonor == null)
                    throw new KeyNotFoundException("לא נמצא " + donor.DonorId + "התורם"); 
                existingDonor.Name = donor.Name;
                existingDonor.Phone = donor.Phone;
                existingDonor.Address = donor.Address;
                existingDonor.Mail = donor.Mail;
                _DbContext.SaveChanges();
                return getAll(); 
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError("", ex);
                throw ex;
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed Update Donor", e);
                return null;
            }
        }

        public List<Donor>? deleteDonor(Donor donor)
        {
            try
            {
                var cards = _DbContext.Cards.ToList();
                foreach (var d in donor.DonorGifts)
                {
                    var exist = cards.FirstOrDefault(c => c.GiftId == d.GiftId);
                    if (exist != null)
                        throw new BadHttpRequestException("נרכשו כרטיסים עבור מתנה של התורם לא ניתן למחוק");
                }
                _DbContext.Donors.Remove(donor);
                _DbContext.SaveChanges();
                return getAll();
            }
            catch (BadHttpRequestException ex)
            {
                throw ex;
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed Delete Donor", e);
                return null;
            }
        }
    }
}
