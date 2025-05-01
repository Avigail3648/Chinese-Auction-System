using ChineseAuctionApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace ChineseAuctionApi.Repositories.Baskets
{
    public interface IBasketRepository
    {
        public List<BasketRespons>? getBasketByUser(string token);
        public Basket? addBasket(int giftID, string token);
        public Basket? deleteBasket(int basketID);
    }
}
