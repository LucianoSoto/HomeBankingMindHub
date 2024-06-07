using Clase_1.Models;
using HomeBankingMindHub.Models;

namespace Clase_1.Repositories
{
    public interface IClientRepository
    {
        IEnumerable<Client> GetAllClients();

        void Save (Client client);

        Client GetClientById(long id);

        Client GetClientByEmail(string email);
        Client GetClientByAccount(Account account);
    }
}
