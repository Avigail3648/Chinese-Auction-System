using ChineseAuctionApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace ChineseAuctionApi.Repositories.Lotteries
{
    public interface ILotteryRepository
    {
        public CardWhitDetails? randomWinPerGift(int giftID);
    }
}
