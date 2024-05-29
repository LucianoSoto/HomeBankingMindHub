using Clase_1.Models;
using HomeBankingMindHub.Models;
using Microsoft.EntityFrameworkCore;

namespace Clase_1.Repositories
{
    public class AccountRepository : RepositoryBase<Account>,IAccountRepository
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

        public void Save(Account account)
        {
            Create(account);
            SaveChanges();
        }
    }
}
