using Clase_1.Models;

namespace Clase_1.Repositories
{
    public interface ILoanRepository
    {
        IEnumerable<Loan> GetAllLoans();
        Loan GetLoanById(long id);
        Loan GetLoanByName(string name);
    }
}
