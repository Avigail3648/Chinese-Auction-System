using ChineseAuctionApi.Models;
using ChineseAuctionApi.Repositories.Baskets;
using Microsoft.IdentityModel.Tokens;

namespace ChineseAuctionApi.Services.aa
{
    public class BasketService : IBasketService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public BasketService(IBasketRepository basketRepository, IHttpContextAccessor httpContextAccessor)
        {
            _basketRepository = basketRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public List<BasketRespons>? getBasketByUser()
        {
            var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                throw new UnauthorizedAccessException("User token is missing");
            }
            try
            {
                return _basketRepository.getBasketByUser(token);
            }
            catch (SecurityTokenMalformedException ex)
            {
                throw ex;
            }
            catch (UnauthorizedAccessException ex)
            {
                throw ex;
            }
            catch (KeyNotFoundException ex)
            {
                throw ex;
            }
        }

        public Basket? addBasket(int giftID)
        {
            var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            if (string.IsNullOrEmpty(token))
            {
                throw new UnauthorizedAccessException("User token is missing");
            }
            try
            {
                return _basketRepository.addBasket(giftID, token);
            }
            catch (UnauthorizedAccessException ex)
            {
                throw ex;
            }
            catch (KeyNotFoundException ex)
            {
                throw ex;
            }
        }

        public Basket? deleteBasket(int basketID)
        {
            return _basketRepository.deleteBasket(basketID);
        }
    }
}
