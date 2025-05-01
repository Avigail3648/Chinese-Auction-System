using ChineseAuctionApi.Models;

namespace ChineseAuctionApi.Repositories.Donors
{
    public interface IDonorRepository
    {
        public List<Donor>? getAll();
        public Donor? getDonorById(int id);
        public List<Donor>? addDonor(Donor donor);
        public List<Donor>? updateGift(Donor donor);
        public List<Donor>? deleteDonor(Donor donor);
    }
}
