﻿using Clase_1.Models;

namespace Clase_1.Repositories
{
    public interface ICardRepository
    {
        IEnumerable<Card> GetAllCards();

        void Save(Card card);

        Card GetCardById(long id);
        Card GetCardByNumber(string id); 
        IEnumerable<Card> GetCardsByClient(long clientId);
        IEnumerable<Card> GetCreditCards(long clientId);
        IEnumerable<Card> GetDebitCards(long clientId);
    }
}
