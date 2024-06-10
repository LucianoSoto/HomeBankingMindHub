using Clase_1.DTOS;
using Clase_1.Models;
using HomeBankingMindHub.Models;

namespace Clase_1.Repositories.Implementations
{
    public class ClientLoanRepository : RepositoryBase<ClientLoan>, IClientLoanRepository
    {
        public ClientLoanRepository(HomeBankingContext repositoryContext) : base(repositoryContext)
        {
        }

        public void Save(ClientLoan loan)
        {
            Create(loan);
            SaveChanges();
        }
    }
}
