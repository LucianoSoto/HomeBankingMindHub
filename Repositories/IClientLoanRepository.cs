using Clase_1.DTOS;
using Clase_1.Models;


namespace Clase_1.Repositories
{
    public interface IClientLoanRepository
    { 
        void Save(ClientLoan loan);
    }
}
