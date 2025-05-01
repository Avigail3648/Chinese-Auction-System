using ChineseAuctionApi.Models;
using ChineseAuctionApi.Repositories.Gifts;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ChineseAuctionApi.Services.Gifts
{
    public class GiftService : IGiftService
    {
        private readonly IGiftRepository _giftRepository;

        public GiftService(IGiftRepository giftRepositor)
        {
            _giftRepository = giftRepositor;
        }

        public List<Gift>? getAll()
        {
            return _giftRepository.getAll();
        }

        public List<GiftWhitNames>? getAllWhitNames()
        {
            return _giftRepository.getAllWhitNames();
        }

        public Gift? getGiftById(int id)
        {
            var existgift = _giftRepository.getAll().FirstOrDefault(g => g.GiftId == id);
            if (existgift == null)
                throw new KeyNotFoundException("id " + id + " is not exist");
            return _giftRepository.getGiftById(id);
        }

        public List<GiftWhitNames>? addGift(GiftWhitNames gift)
        {
            Gift? thisgift = _giftRepository.getGiftById(gift.GiftId);
            Gift? nameExist = null;
            nameExist = _giftRepository.getAll().ToList().FirstOrDefault(g => g.Name == gift.Name);
            if (nameExist != null && gift.Name != thisgift.Name)
                throw new InvalidDataException("this gift already exist, enter other name");
            Gift g = new Gift();
            g.GiftId = gift.GiftId;
            g.Name = gift.Name;
            g.Details = gift.Details;
            g.DonorId = _giftRepository.getDonorIdByName(gift.DonorName);
            g.MonetaryValue = gift.MonetaryValue;
            g.CardPrice = gift.CardPrice;
            g.CategoryId = _giftRepository.getCategoryIdByName(gift.categoryName);
            g.Image = gift.Image;
            if (g.DonorId == -1)
                throw new KeyNotFoundException("this " + gift.DonorName + " is not exist");
            if (g.CategoryId == -1)
                throw new KeyNotFoundException("this " + gift.categoryName + "category is not exist");
            return _giftRepository.addGift(g);
        }

        public List<GiftWhitNames>? deleteGift(int id)
        {
            try
            {
                var gift = _giftRepository.getAll().FirstOrDefault(g => g.GiftId == id);
                if (gift == null)
                    throw new KeyNotFoundException("id " + id + " is not exist");
                return _giftRepository.deleteGift(gift);
            }
            catch (BadHttpRequestException ex)
            {
                throw ex;
            }
        }

        public List<GiftWhitNames>? updateGift(GiftWhitNames gift)
        {
            try
            {
                Gift? thisgift = _giftRepository.getGiftById(gift.GiftId);
                Gift? nameExist = null;
                nameExist = _giftRepository.getAll().ToList().FirstOrDefault(g => g.Name == gift.Name);
                if (nameExist != null && gift.Name != thisgift.Name)
                    throw new InvalidDataException("this gift already exist, enter other name");
                Gift g = new Gift();
                g.GiftId = gift.GiftId;
                g.Name = gift.Name;
                g.Details = gift.Details;
                g.DonorId = _giftRepository.getDonorIdByName(gift.DonorName);
                g.MonetaryValue = gift.MonetaryValue;
                g.CardPrice = gift.CardPrice;
                g.CategoryId = _giftRepository.getCategoryIdByName(gift.categoryName);
                g.Image = gift.Image;
                if (g.DonorId == -1)
                    throw new KeyNotFoundException("this " + gift.DonorName + " is not exist");
                if (g.CategoryId == -1)
                    throw new KeyNotFoundException("this " + gift.categoryName + "category is not exist");

                return _giftRepository.updateGift(g);
            }
            catch (KeyNotFoundException e)
            {
                throw e;
            }
            catch (BadHttpRequestException ex)
            {
                throw ex;
            }
        }

        public List<string>? getCategoriesNames()
        {

            return _giftRepository.getCategoriesNames();

        }

        public List<GiftWhitNames>? getGiftsByDonorID(int donorId)
        {
            return _giftRepository.getGiftsByDonorID(donorId);
        }
    }
}


