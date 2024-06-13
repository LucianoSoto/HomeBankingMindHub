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
        IEnumerable<AccountDTO> GetCurrentAccounts(ClaimsPrincipal User);
        IEnumerable<CardDTO> GetCurrentCards(ClaimsPrincipal User);
        void CreateClient(ClaimsPrincipal User, Client client);

    }
}
