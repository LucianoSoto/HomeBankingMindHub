using Clase_1.DTOS;
using HomeBankingMindHub.Models;
using System.Security.Claims;

namespace Clase_1.Services
{
    public interface IClientServices
    {
        IEnumerable<ClientDTO> GetAllClients();
        ClientDTO GetClientById(long id);
        ClientDTO GetCurrent(ClaimsPrincipal User);
        void CreateAccount(ClaimsPrincipal User);
        void CheckCards(NewCardDTO newCardDTO, long id);
        void CreateCard(ClaimsPrincipal User, NewCardDTO newCardDTO);
        IEnumerable<AccountDTO> GetCurrentAccounts(ClaimsPrincipal User);
        IEnumerable<CardDTO> GetCurrentCards(ClaimsPrincipal User);
        void CreateClient(ClaimsPrincipal User, Client client);

    }
}
