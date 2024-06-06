using HomeBankingMindHub.Models;
using Microsoft.EntityFrameworkCore;

namespace Clase_1.Repositories.Implementations
{
    public class ClientRepository : RepositoryBase<Client>, IClientRepository
    {
        public ClientRepository(HomeBankingContext repositoryContext) : base(repositoryContext)
        {
        }

        public Client GetClientById(long id)
        {
            return FindByCondition(client => client.Id == id)
                .Include(client => client.Accounts)
                .Include(client => client.Loans)
                    .ThenInclude(cl => cl.Loan)
                .Include(client => client.Cards)
                .FirstOrDefault();
        }

        public IEnumerable<Client> GetAllClients()
        {
            return FindAll()
                .Include(client => client.Accounts)
                .Include(client => client.Loans)
                    .ThenInclude(cl => cl.Loan)
                .Include(client => client.Cards)
                .ToList();
        }

        public void Save(Client client)
        {
            Create(client);
            SaveChanges();
        }

        public Client GetClientByEmail(string email)
        {
            return FindByCondition(client => client.Email.ToUpper() == email.ToUpper())
                    .Include(client => client.Accounts)
                    .Include(client => client.Loans)
                        .ThenInclude(cl => cl.Loan)
                    .Include(client => client.Cards)
                    .FirstOrDefault();
        }

    }
}

