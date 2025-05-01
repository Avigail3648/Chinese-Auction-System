using ChineseAuctionApi.Models;

namespace ChineseAuctionApi.Services.Lotteries
{
    public interface ILotteryService
    {
        public CardWhitDetails? randomWinPerGift(int giftID);
    }
}
