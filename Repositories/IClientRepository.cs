using HomeBankingMindHub.Models;

namespace Clase_1.Repositories
{
    public interface IClientRepository
    {
        IEnumerable<Client> GetAllClients();

        void Save (Client client);

        Client GetClientById(long id);
    }
}
