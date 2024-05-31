using Clase_1.Models;


namespace Clase_1.Repositories
{
    public interface IClientLoanRepository
    {
        IEnumerable<ClientLoan> GetAllLoans();

        void Save(ClientLoan loan);

        ClientLoan GetLoanById(long id);
    }
}
