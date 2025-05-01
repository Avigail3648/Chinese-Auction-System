using ChineseAuctionApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace ChineseAuctionApi.Services.Gifts
{
    public interface IGiftService
    {
        public List<Gift>? getAll();
        public Gift? getGiftById(int id);
        public List<GiftWhitNames>? addGift(GiftWhitNames gift);
        public List<GiftWhitNames>? deleteGift(int id);
        public List<GiftWhitNames>? updateGift(GiftWhitNames gift);
        public List<string>? getCategoriesNames();
        public List<GiftWhitNames>? getAllWhitNames();
        public List<GiftWhitNames>? getGiftsByDonorID(int donorId);
    }
}
