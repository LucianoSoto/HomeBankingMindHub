using Clase_1.Models;
using HomeBankingMindHub.Models;

namespace Clase_1.Repositories.Implementations
{
    public class ClientLoanRepository : RepositoryBase<ClientLoan>, IClientLoanRepository
    {
        public ClientLoanRepository(HomeBankingContext repositoryContext) : base(repositoryContext)
        {
        }

        public IEnumerable<ClientLoan> GetAllLoans()
        {
            return FindAll();
        }

        public ClientLoan GetLoanById(long id)
        {
            return FindByCondition(loan => loan.Id == id)
                .FirstOrDefault();
        }

        public void Save(ClientLoan loan)
        {
            Create(loan);
            SaveChanges();
        }
    }
}
