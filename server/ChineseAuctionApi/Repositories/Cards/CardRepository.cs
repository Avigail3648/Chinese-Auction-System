using ChineseAuctionApi.Models;
using ChineseAuctionApi.Repositories.Gifts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Text.Json.Serialization;

namespace ChineseAuctionApi.Repositories.Cards
{
    public class CardRepository : ICardRepository
    {
        private readonly DBContext _DbContext;
        private readonly IGiftRepository _giftRepository;
        private readonly ILogger<CardRepository> _logger;

        public CardRepository(DBContext DbContext, IGiftRepository giftRepository, ILogger<CardRepository> logger)
        {
            _DbContext = DbContext;
            _giftRepository = giftRepository;
            _logger = logger;
        }
        public List<CardWhitDetails>? getAllCardsByGiftID(int giftID)
        {
            try
            {
                var cards = _DbContext.Cards.Where(c => c.GiftId == giftID).ToList();
                var gift = _giftRepository.getGiftWhitNameById(giftID);
                var users = _DbContext.Users.ToList();

                List<CardWhitDetails> cardsWithUsers = new List<CardWhitDetails>();
                cardsWithUsers = cards.Join(users, c => c.UserId, u => u.UserId, (c, u) =>
                new CardWhitDetails
                {
                    CardId = c.CardId,
                    gift = gift,
                    user = new User
                    {
                        UserId = u.UserId,
                        Name = u.Name,
                        Phone = u.Phone,
                        Address = u.Address,
                        Mail = u.Mail,
                        Password = u.Password,
                        Role = u.Role
                    },
                    IsWin = c.IsWin
                }).ToList();
                return cardsWithUsers;
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed get All Cards By Gift ID{giftID}", e);
                return null;
            }

        }

        public List<Card>? addCards(List<Card> cards)
        {
            try
            {
                foreach (var c in cards)
                {
                    User? user = _DbContext.Users.FirstOrDefault(u => u.UserId == c.UserId);
                    Gift? gift = _DbContext.Gifts.FirstOrDefault(g => g.GiftId == c.GiftId);

                    if (user == null)
                    {
                        throw new KeyNotFoundException("  משתמש לא רשום");
                    }
                    if (gift == null)
                    {
                        throw new KeyNotFoundException("המתנה לא נמצאה");
                    }
                    _DbContext.Cards.Add(c);
                    _DbContext.SaveChanges();
                    Basket? b = _DbContext.Baskets.FirstOrDefault(b => b.GiftId == c.GiftId && b.UserId == c.UserId);
                    if (b != null)
                    {
                        _DbContext.Baskets.Remove(b);
                        _DbContext.SaveChanges();

                    }
                }
                return cards;
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError("", ex);
                throw ex;
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed Add Cards {cards}", e);
                return null;
            }
        }

        public Card? updateCardToWin(int cardID)
        {
            try
            {
                Card? c = _DbContext.Cards.FirstOrDefault(c => c.CardId == cardID);
                if (c != null)
                    c.IsWin = true;
                _DbContext.SaveChanges();
                Card? cardAfterUpdate = _DbContext.Cards.FirstOrDefault(c => c.CardId == cardID);
                return c;
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed Update To Win For Card {cardID} ", e);
                return null;
            }
        }



        public List<CardWhitDetails>? getAllWinCard()
        {
            try
            {
                var gifts = _DbContext.Gifts.ToList();

                List<CardWhitDetails> cardsWin = new List<CardWhitDetails>();

                foreach (var g in gifts)
                {
                    var c = getAllCardsByGiftID(g.GiftId);
                    CardWhitDetails? cardWin = null;
                    if (c != null)
                        cardWin = c.FirstOrDefault(c => c.IsWin == true);
                    if (cardWin != null)
                        cardsWin.Add(cardWin);
                }

                return cardsWin;
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed get All Win Card", e);
                return null;
            }
        }

        public int? getAllRevenue()
        {
            try
            {
                int revenue = 0;
                var cards = _DbContext.Cards.ToList();
                foreach (var c in cards)
                {
                    if (c.GiftId != null)
                    {
                        int id = c.GiftId;
                        Gift g = _giftRepository.getGiftById(id);
                        if (g != null)
                            revenue += g.CardPrice;
                    }
                }
                return revenue;
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed Get All Revenue", e);
                return null;
            }
        }
    }
}
