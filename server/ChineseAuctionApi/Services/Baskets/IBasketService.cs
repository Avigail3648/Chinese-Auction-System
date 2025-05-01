using ChineseAuctionApi.Models;

namespace ChineseAuctionApi.Services.aa
{
    public interface IBasketService
    {
        public List<BasketRespons>? getBasketByUser();
        public Basket? addBasket(int giftID);
        public Basket? deleteBasket(int basketID);
    }
}
