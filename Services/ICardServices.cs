using Clase_1.DTOS;
using System.Security.Claims;

namespace Clase_1.Services
{
    public interface ICardServices
    {
        public IEnumerable<CardDTO> GetAllCards();
        public CardDTO GetCardById(long id);
        public void CheckCards(NewCardDTO newCardDTO, long id);
        public string CreateCardNumber();
        public void CreateCard(ClaimsPrincipal User, NewCardDTO newCardDTO);
    }
}
