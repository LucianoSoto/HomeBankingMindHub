using Clase_1.Models;

namespace Clase_1.Repositories
{
    public interface ITransactionRepository
    {
        IEnumerable<Transaction> GetAllTransactions();

        void Save(Transaction transaction);

        Transaction GetTransactionById(long id);
    }
}
