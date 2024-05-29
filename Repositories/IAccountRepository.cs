using Clase_1.Models;
using HomeBankingMindHub.Models;

namespace Clase_1.Repositories
{
    public interface IAccountRepository
    {
        IEnumerable<Account> GetAllAccounts();

        void Save(Account account);

        Account GetAccountById(long id);
    }
}
