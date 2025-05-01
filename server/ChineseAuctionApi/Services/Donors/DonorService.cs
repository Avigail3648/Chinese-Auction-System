using ChineseAuctionApi.Models;
using ChineseAuctionApi.Repositories.Donors;
using Microsoft.AspNetCore.Mvc;

namespace ChineseAuctionApi.Services.Donors
{
    public class DonorService : IDonorService
    {

        private readonly IDonorRepository _donorRepository;

        public DonorService(IDonorRepository donorRepository)
        {
            _donorRepository = donorRepository;
        }

        public List<Donor>? getAll()
        {
            return _donorRepository.getAll();
        }

        public Donor? getDonorById(int id)
        {
            var existDonor = _donorRepository.getAll().FirstOrDefault(g => g.DonorId == id);
            if (existDonor == null)
                throw new KeyNotFoundException("donor id " + id + " is not exist");
            return _donorRepository.getDonorById(id);
        }

        public List<Donor>? addDonor(Donor donor)
        {
            return _donorRepository.addDonor(donor);
        }

        public List<Donor>? updateGift(Donor donor)
        {
            try
            {
                return _donorRepository.updateGift(donor);
            }
            catch (KeyNotFoundException e)
            {
                throw e;
            }
        }

        public List<Donor>? deleteDonor(int id)
        {
            try
            {
                var existDonor = _donorRepository.getAll().FirstOrDefault(d => d.DonorId == id);
                if (existDonor == null)
                    throw new KeyNotFoundException("donor id " + id + " is not exist");
                return _donorRepository.deleteDonor(existDonor);
            }
            catch (BadHttpRequestException ex)
            {
                throw ex;
            }
        }
    }
}
