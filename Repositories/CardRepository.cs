using Clase_1.Models;
using HomeBankingMindHub.Models;

namespace Clase_1.Repositories
{
    public class CardRepository : RepositoryBase<Card>, ICardRepository
    {
        public CardRepository(HomeBankingContext repositoryContext) : base(repositoryContext)
        {
        }

        public IEnumerable<Card> GetAllCards()
        {
            return FindAll();
        }

        public Card GetCardById(long id)
        {
            return FindByCondition(card => card.Id == id)
                .FirstOrDefault();
        }
        public Card GetCardByNumber(string Number)
        {
            return FindByCondition(card => card.Number == Number)
                .FirstOrDefault();
        }

        public IEnumerable<Card> GetCardsByClient(long clientId)
        {
            return FindByCondition(card => card.ClientId == clientId)
                .ToList();
        }
        
        public IEnumerable<Card> GetDebitCards(long clientId)
        {
            var cards = GetCardsByClient(clientId);
            var debitCards = new List<Card>();

            foreach (var item in cards)
            {
                if(item.Type == "DEBIT")
                {
                    debitCards.Add(item);
                }
            }

            return debitCards;
        }

        public IEnumerable<Card> GetCreditCards(long clientId)
        {
            var cards = GetCardsByClient(clientId);
            var creditCards = new List<Card>();

            foreach (var item in cards)
            {
                if (item.Type == "CREDIT")
                {
                    creditCards.Add(item);
                }
            }

            return creditCards;
        }

        public void Save(Card card)
        {
            Create(card);
            SaveChanges();
        }
    }
}
