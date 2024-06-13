using Clase_1.DTOS;
using Clase_1.Models;
using Clase_1.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Clase_1.Services.Implementations
{
    public class CardServices : ICardServices
    {
        private readonly ICardRepository _cardRepository;
        private readonly IClientServices _clientServices;
        public CardServices(ICardRepository cardRepository, IClientServices clientServices)
        {
            _cardRepository = cardRepository;
            _clientServices = clientServices;
        }

        public IEnumerable<CardDTO> GetAllCards()
        {
            var cards = _cardRepository.GetAllCards();
            return cards.Select(cards => new CardDTO(cards)).ToList();
        }
        
        public CardDTO GetCardById(long id)
        {
            var card = _cardRepository.GetCardById(id);
            if (card == null)
            {
                throw new Exception("Not found!");
            }
            return new CardDTO(card);
        }

        public void CheckCards(NewCardDTO newCardDTO, long id)
        {
            if (newCardDTO.Type == null || newCardDTO.Color == null)
            {
                throw new Exception("Por favor rellenar todos los datos");
            }
            if (newCardDTO.Type != "DEBIT" || newCardDTO.Type != "CREDIT")
            {
                throw new Exception("Por favor, inserte un tipo valido");
            }
            if (newCardDTO.Color != "GOLD" || newCardDTO.Color != "SILVER" || newCardDTO.Color != "TITANIUM")
            {
                throw new Exception("Por favor, inserte un color valido");
            }

            var cardsByType = newCardDTO.Type == "DEBIT" ? _cardRepository.GetDebitCards(id) : _cardRepository.GetCreditCards(id);

            if (cardsByType.Count() >= 3)
            {
                throw new Exception("No pueden haber mas de 3 cartas por tipo");
            }

            foreach (var card in cardsByType)
            {
                if (card.Color == newCardDTO.Color)
                {
                    throw new Exception("No pueden haber mas de 1 carta por color y tipo");
                }
            }
        }

        public string CreateCardNumber()
        {
            string cardCode = "";
            long cardNumber = 0;
            int flag = 0;

            do
            {
                for (int i = 0; i < 3; i++)
                {
                    cardNumber = RandomNumberGenerator.GetInt32(0000, 9999);
                    cardCode += cardNumber.ToString("D4") + "-";
                }

                cardNumber = RandomNumberGenerator.GetInt32(0000, 9999);
                cardCode += cardNumber.ToString("D4");

                Card card = _cardRepository.GetCardByNumber(cardCode);
                if (card == null)
                {
                    flag = 1;
                }
            } while (flag == 0);

            return cardCode;
        }

        public void CreateCard(ClaimsPrincipal User, NewCardDTO newCardDTO)
        {

            var client = _clientServices.GetCurrent(User);
            CheckCards(newCardDTO, client.Id);

            var cardCode = CreateCardNumber();
            int cvv = RandomNumberGenerator.GetInt32(0, 999);

            Card newCard = new Card()
            {
                CardHolder = client.FirstName + " " + client.LastName,
                Type = newCardDTO.Type,
                Color = newCardDTO.Color,
                Number = cardCode,
                Cvv = cvv,
                FromDate = DateTime.Now,
                ThruDate = DateTime.Now.AddYears(5),
                ClientId = client.Id
            };

            _cardRepository.Save(newCard);
        }

    }
}
