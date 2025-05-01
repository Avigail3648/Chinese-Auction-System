using ChineseAuctionApi.Models;
using ChineseAuctionApi.Repositories.Lotteries;

namespace ChineseAuctionApi.Services.Lotteries
{
    public class LotteryService : ILotteryService
    {
        private readonly ILotteryRepository _lotteryRepository;

        public LotteryService(ILotteryRepository lotteryRepository)
        {
            _lotteryRepository = lotteryRepository;
        }

        public CardWhitDetails? randomWinPerGift(int giftID)
        {
            try
            {
                return _lotteryRepository.randomWinPerGift(giftID);
            }
            catch (BadHttpRequestException ex)
            {
                throw ex;
            }
        }
    }
}
