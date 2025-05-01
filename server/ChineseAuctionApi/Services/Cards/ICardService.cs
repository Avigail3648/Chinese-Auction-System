using ChineseAuctionApi.Models;

namespace ChineseAuctionApi.Services.Cards
{
    public interface ICardService
    {
        public List<CardWhitDetails>? getAllCardsByGiftID(int giftID);
        public List<Card>? addCards(List<Card> cards);
        public Card? updateCardToWin(int cardID);
        public List<CardWhitDetails>? getAllWinCard();
        public int? getAllRevenue();

    }
}
