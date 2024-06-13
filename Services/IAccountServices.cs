using Clase_1.DTOS;
using System.Security.Claims;

namespace Clase_1.Services
{
    public interface IAccountServices
    {
        public IEnumerable<AccountDTO> GetAllAccounts();
        public AccountDTO GetAccountById(long id);
        public void CheckAccounts(ClaimsPrincipal User);
        public string GenerateAccountNumber();
        public void CreateAccount(ClaimsPrincipal User);
    }
}
