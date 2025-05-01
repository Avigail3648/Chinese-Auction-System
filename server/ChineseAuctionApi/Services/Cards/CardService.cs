using ChineseAuctionApi.Models;
using ChineseAuctionApi.Repositories.Cards;

namespace ChineseAuctionApi.Services.Cards
{
    public class CardService : ICardService
    {
        private readonly ICardRepository _cardRepository;

        public CardService(ICardRepository cardRepository)
        {
            _cardRepository = cardRepository;
        }

        public List<CardWhitDetails>? getAllCardsByGiftID(int giftID)
        {
            return _cardRepository.getAllCardsByGiftID(giftID);
        }

        public List<Card>? addCards(List<Card> cards)
        {
            try
            {
                return _cardRepository.addCards(cards);
            }
            catch (KeyNotFoundException ex)
            {
                throw ex;
            }
        }
        
        public Card? updateCardToWin(int cardID)
        {
            return _cardRepository.updateCardToWin(cardID);
        }

        public List<CardWhitDetails>? getAllWinCard()
        {
            return _cardRepository.getAllWinCard();
        }

        public int? getAllRevenue()
        {
            return _cardRepository.getAllRevenue();
        }
    }
}
