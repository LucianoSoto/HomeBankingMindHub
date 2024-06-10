using Clase_1.Models;
using HomeBankingMindHub.Models;

namespace Clase_1.Repositories.Implementations
{
    public class LoanRepository : RepositoryBase<Loan>, ILoanRepository
    {

        public LoanRepository(HomeBankingContext repositoryContext) : base(repositoryContext)
        {
        }

        public IEnumerable<Loan> GetAllLoans()
        {
            return FindAll();
        }

        public Loan GetLoanById(long id)
        {
            return FindByCondition(loan => loan.Id == id)
                .FirstOrDefault();
        }

        public Loan GetLoanByName(string name)
        {
            return FindByCondition(loan => loan.Name == name)
                .FirstOrDefault();
        }
    }
}
