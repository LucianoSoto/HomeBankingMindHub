using Clase_1.Models;
using HomeBankingMindHub.Models;

namespace Clase_1.Repositories.Implementations
{
    public class TransactionRepository : RepositoryBase<Transaction>, ITransactionRepository
    {
        public TransactionRepository(HomeBankingContext repositoryContext) : base(repositoryContext)
        {
        }

        public IEnumerable<Transaction> GetAllTransactions()
        {
            return FindAll();
        }

        public Transaction GetTransactionById(long id)
        {
            return FindByCondition(transaction => transaction.Id == id)
                .FirstOrDefault();
        }

        public void Save(Transaction transaction)
        {
            Create(transaction);
            SaveChanges();
        }
    }
}
