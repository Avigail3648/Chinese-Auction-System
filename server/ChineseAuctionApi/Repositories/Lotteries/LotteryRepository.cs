using ChineseAuctionApi.Models;
using ChineseAuctionApi.Repositories.Cards;

namespace ChineseAuctionApi.Repositories.Lotteries
{
    public class LotteryRepository : ILotteryRepository
    {
        private readonly DBContext _DbContext;
        private readonly ICardRepository _cardRepository;
        private readonly ILogger<LotteryRepository> _logger;

        public LotteryRepository(DBContext DbContext, ICardRepository cardRepository, ILogger<LotteryRepository> logger)
        {
            _DbContext = DbContext;
            _cardRepository = cardRepository;
            _logger = logger;
        }
        public CardWhitDetails? randomWinPerGift(int giftID)
        {
            try
            {
                List<CardWhitDetails>? cards = _cardRepository.getAllCardsByGiftID(giftID);
                if (cards == null)
                    throw new BadHttpRequestException("לא נרכשו כרטיסים עבור מתנה זו");
                if (cards.Count() == 0)
                    throw new BadHttpRequestException("לא נרכשו כרטיסים עבור מתנה זו");
                Random random = new Random();
                int index = random.Next(0, cards.Count());
                CardWhitDetails winnerCard = cards[index];
                _cardRepository.updateCardToWin(winnerCard.CardId);
                winnerCard.IsWin = true;
                return winnerCard;
            }
            catch (BadHttpRequestException ex)
            {
                _logger.LogError("", ex);
                throw ex;
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed random Win Per Gift{giftID}", e);
                return null;
            }
        }
    }
}
