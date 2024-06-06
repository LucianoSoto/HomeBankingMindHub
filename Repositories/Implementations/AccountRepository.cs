using Clase_1.Models;
using HomeBankingMindHub.Models;
using Microsoft.EntityFrameworkCore;

namespace Clase_1.Repositories.Implementations
{
    public class AccountRepository : RepositoryBase<Account>, IAccountRepository
    {

        public AccountRepository(HomeBankingContext repositoryContext) : base(repositoryContext)
        {
        }

        public IEnumerable<Account> GetAllAccounts()
        {
            return FindAll()
                .Include(account => account.Transactions)
                .ToList();
        }

        public Account GetAccountById(long id)
        {
            return FindByCondition(account => account.Id == id)
                .Include(account => account.Transactions)
                .FirstOrDefault();
        }

        public Account GetAccountByNumber(string Number)
        {
            return FindByCondition(account => account.Number == Number)
                .Include(account => account.Transactions)
                .FirstOrDefault();
        }

        public void Save(Account account)
        {
            Create(account);
            SaveChanges();
        }

        public IEnumerable<Account> GetAccountsByClient(long clientId)
        {
            return FindByCondition(account => account.ClientId == clientId)
                .Include(account => account.Transactions)
                .ToList();
        }


    }
}
