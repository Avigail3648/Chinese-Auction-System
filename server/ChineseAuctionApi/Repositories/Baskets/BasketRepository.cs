using ChineseAuctionApi.Models;
using ChineseAuctionApi.Repositories.Gifts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ChineseAuctionApi.Repositories.Baskets
{
    public class BasketRepository : IBasketRepository
    {

        private readonly DBContext _DbContext;
        private readonly ILogger<BasketRepository> _logger;

        private readonly IGiftRepository _giftRepository;
        public BasketRepository(DBContext DbContext, IGiftRepository giftRepository, ILogger<BasketRepository> logger)
        {
            _DbContext = DbContext;
            _giftRepository = giftRepository;
            _logger = logger;
        }

        public List<BasketRespons>? getBasketByUser(string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                var mail = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

                if (mail == null)
                {
                    throw new UnauthorizedAccessException("לא מורשה");
                }

                var user = _DbContext.Users.FirstOrDefault(u => u.Mail == mail);

                if (user == null)
                {
                    throw new KeyNotFoundException("משתמש לא רשום");
                }
                var gifts = _giftRepository.getAllWhitNames();
                var baskets = _DbContext.Baskets.Where(b => b.UserId == user.UserId).ToList();
                var basketRespons = baskets.Join(gifts, b => b.GiftId, g => g.GiftId, (b, g) =>
                new BasketRespons
                {
                    BasketId = b.BasketId,
                    Gift = g,
                    UserId = b.UserId
                }).ToList();

                if (basketRespons == null)
                    throw new KeyNotFoundException("אין סל " + user.UserId + " למשתמש");
                return basketRespons;
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError("", ex);
                throw ex;
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError("", ex);
                throw ex;
            }
            catch (SecurityTokenMalformedException ex)
            {
                _logger.LogError("", ex);
                throw new SecurityTokenMalformedException("  טוקן לא תקין");
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed Get Basket By User ", e);
                return null;
            }
        }

        public Basket? addBasket(int giftID, string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                var mail = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

                if (mail == null)
                {
                    throw new UnauthorizedAccessException("לא מורשה");
                }

                var user = _DbContext.Users.FirstOrDefault(u => u.Mail == mail);

                if (user == null)
                {
                    throw new KeyNotFoundException(" משתמש לא רשום ");
                }

                var gift = _DbContext.Gifts.FirstOrDefault(g => g.GiftId == giftID);
              
                if (gift == null)
                {
                    throw new KeyNotFoundException("המתנה לא נמצאה");
                }
                Basket basket = new Basket
                {
                    UserId = user.UserId,
                    GiftId = gift.GiftId,
                };

                _DbContext.Baskets.Add(basket);
                _DbContext.SaveChanges();

                return basket;
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError("", ex);
                throw ex;
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError("", ex);
                throw ex;
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed Add Basket For Gift {giftID}", e);
                return null;
            }
        }

        public Basket? deleteBasket(int basketID)
        {
            try
            {
                Basket? b = _DbContext.Baskets.FirstOrDefault(b => b.BasketId == basketID);
                if (b != null)
                {
                    _DbContext.Baskets.Remove(b);
                    _DbContext.SaveChanges();
                }
                return b;
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed Delete Basket By Id {basketID}", e);
                return null;
            }
        }
    }
}



