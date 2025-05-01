using ChineseAuctionApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace ChineseAuctionApi.Repositories.Gifts
{
    public interface IGiftRepository
    {
        public List<Gift>? getAll();
        public Gift? getGiftById(int id);
        public List<GiftWhitNames>? addGift(Gift gift);
        public List<GiftWhitNames>? deleteGift(Gift gift);
        public List<GiftWhitNames>? updateGift(Gift gift);
        public List<string>? getCategoriesNames();
        public int getCategoryIdByName(string category);
        public int getDonorIdByName(string donorName);
        public List<GiftWhitNames>? getAllWhitNames();
        public GiftWhitNames? getGiftWhitNameById(int id);
        public List<GiftWhitNames>? getGiftsByDonorID(int donorId);
    }
}
