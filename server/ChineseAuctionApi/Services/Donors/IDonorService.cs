using ChineseAuctionApi.Models;

namespace ChineseAuctionApi.Services.Donors
{
    public interface IDonorService
    {
        public List<Donor>? getAll();
        public Donor? getDonorById(int id);
        public List<Donor>? addDonor(Donor donor);
        public List<Donor>? updateGift(Donor donor);
        public List<Donor>? deleteDonor(int id);
    }
}
