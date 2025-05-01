namespace ChineseAuctionApi.Repositories.Gifts
{
    using ChineseAuctionApi.Models;
    using ChineseAuctionApi.Repositories;
    using Microsoft.AspNetCore.Http.HttpResults;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Drawing;

    public class GiftRepository : IGiftRepository
    {
        private readonly DBContext _DbContext;
        private readonly ILogger<GiftRepository> _logger;

        public GiftRepository(DBContext DbContext, ILogger<GiftRepository> logger)
        {
            _DbContext = DbContext;
            _logger = logger;
        }

        public List<Gift>? getAll()
        {
            try
            {
                return _DbContext.Gifts.ToList();
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed Get All Gifts", e);
                return null;
            }
        }

        public List<GiftWhitNames>? getAllWhitNames()
        {
            try
            {
                var gifts = _DbContext.Gifts.ToList();
                var donors = _DbContext.Donors.ToList();
                var categories = _DbContext.Categories.ToList();

                var giftsWhitCategoyName = gifts.Join(categories, gift => gift.CategoryId, category => category.CategoryId, (gift, category) => new
                {
                    giftId = gift.GiftId,
                    name = gift.Name,
                    detailes = gift.Details,
                    image = gift.Image,
                    categoryName = category.Name,
                    monetaryValue = gift.MonetaryValue,
                    cardPrice = gift.CardPrice,
                    donorId = gift.DonorId

                }).ToList();

                var giftWhitDonorName = giftsWhitCategoyName.Join(donors, gift => gift.donorId, donor => donor.DonorId, (gift, donor) =>
                new GiftWhitNames
                {
                    GiftId = gift.giftId,
                    Name = gift.name,
                    Details = gift.detailes,
                    Image = gift.image,
                    categoryName = gift.categoryName,
                    MonetaryValue = gift.monetaryValue,
                    CardPrice = gift.cardPrice,
                    DonorName = donor.Name

                }).ToList();

                return giftWhitDonorName;
            }
            catch (Exception e)
            {
                _logger.LogError("Failed Get All Gifts", e);
                return null;
            }
        }

        public Gift? getGiftById(int id)
        {
            try
            {
                return _DbContext.Gifts.FirstOrDefault(gift => gift.GiftId == id);
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed get Gift By Id", e);
                return null;
            }
        }

        public GiftWhitNames? getGiftWhitNameById(int id)
        {
            try
            {
                var gift = getAllWhitNames().FirstOrDefault(g => g.GiftId == id);
                return gift;
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed get Gif Whit Name ById", e);
                return null;
            }
        }

        public List<GiftWhitNames>? addGift(Gift gift)
        {
            try
            {
                _DbContext.Gifts.Add(gift);
                _DbContext.SaveChanges();
                return getAllWhitNames();
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed Add New Gift {gift.GiftId} ", e);
                return null;
            }
        }

        public List<GiftWhitNames>? deleteGift(Gift gift)
        {
            try
            {
                var cards = _DbContext.Cards.ToList();
                foreach (var c in cards)
                {
                    if (c.GiftId == gift.GiftId)
                        throw new BadHttpRequestException("נרכשו כרטיסים עבור מתנה זו לא ניתן למחוק");
                }

                var baskets = _DbContext.Baskets.ToList();
                foreach (var b in baskets)
                {
                    if (b.GiftId == gift.GiftId)
                        _DbContext.Baskets.Remove(b);
                }
                _DbContext.Gifts.Remove(gift);
                _DbContext.SaveChanges();
                return getAllWhitNames();
            }
            catch (BadHttpRequestException ex)
            {
                throw ex;
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed Delete Gift {gift.GiftId} ", e);
                return null;
            }
        }

        public List<GiftWhitNames>? updateGift(Gift gift)
        {
            try
            {
                var cards = _DbContext.Cards.ToList();
                foreach (var c in cards)
                {
                    if (c.GiftId == gift.GiftId)
                        throw new BadHttpRequestException("נרכשו כרטיסים עבור מתנה זו לא ניתן לעדכן");
                }
                var existingGift = _DbContext.Gifts.SingleOrDefault(g => g.GiftId == gift.GiftId);
                if (existingGift == null)
                    throw new KeyNotFoundException(" לא נמצאה " + gift.GiftId + "מתנה ");
                existingGift.Name = gift.Name;
                existingGift.DonorId = gift.DonorId;
                existingGift.Image = gift.Image;
                existingGift.Details = gift.Details;
                existingGift.CardPrice = gift.CardPrice;
                existingGift.CategoryId = gift.CategoryId;
                existingGift.MonetaryValue = gift.MonetaryValue;
                _DbContext.SaveChanges();
                return getAllWhitNames();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError("", ex);
                throw ex;
            }
            catch (BadHttpRequestException ex)
            {
                throw ex;
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed Update Gift {gift.GiftId} ", e);
                return null;
            }
        }

        public List<string>? getCategoriesNames()
        {
            try
            {
                return _DbContext.Categories.Select(c => c.Name).ToList();
            }
            catch (Exception e)
            {
                _logger.LogError("get Categories Names", e);
                return null;
            }
        }

        public int getCategoryIdByName(string category)
        {
            try
            {
                return _DbContext.Categories.ToList().FirstOrDefault(c => c.Name == category).CategoryId;
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed Get Category Id By Name ", e);
                return -1;
            }
        }

        public int getDonorIdByName(string donorName)
        {
            try
            {
                return _DbContext.Donors.FirstOrDefault(d => d.Name == donorName).DonorId;
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed Get Donor Id By Name", e);
                return -1;
            }
        }

        public List<GiftWhitNames>? getGiftsByDonorID(int donorId)
        {
            try
            {
                var gifts = _DbContext.Gifts.Where(g => g.DonorId == donorId).ToList();
                List<GiftWhitNames> giftWhitNames = new List<GiftWhitNames>();
                foreach (var g in gifts)
                {
                    GiftWhitNames? gift = getGiftWhitNameById(g.GiftId);
                    if (gift != null)
                        giftWhitNames.Add(gift);
                }
                return giftWhitNames;
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed get Gifts By Donor ID{donorId}", e);
                return null;
            }
        }
    }
}
