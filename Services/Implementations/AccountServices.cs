using Clase_1.DTOS;
using Clase_1.Models;
using Clase_1.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Clase_1.Services.Implementations
{
    public class AccountServices : IAccountServices
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IClientServices _clientServices;
        public AccountServices(IAccountRepository accountRepository, IClientServices clientServices)
        {
            _accountRepository = accountRepository;
            _clientServices = clientServices;
        }

        public IEnumerable<AccountDTO> GetAllAccounts()
        {
            var accounts = _accountRepository.GetAllAccounts();
            return accounts.Select(accounts => new AccountDTO(accounts)).ToList();
        }

        public AccountDTO GetAccountById(long id)
        {
            var account = _accountRepository.GetAccountById(id);
            if (account == null)
            {
                throw new Exception("Not Found!");
            }
            return new AccountDTO(account);
        }

        public void CheckAccounts(ClaimsPrincipal User)
        {
            var client = _clientServices.GetCurrent(User);
            var userAccounts = _accountRepository.GetAccountsByClient(client.Id);

            if (userAccounts.Count() >= 3)
            {
                throw new Exception("No pueden haber mas de 3 cuentas por cliente");
            }
        }

        public string GenerateAccountNumber()
        {
            string codeAccount = "";
            int flag = 0;

            do
            {
                codeAccount = "VIN" + RandomNumberGenerator.GetInt32(0, 99999999);
                Account account = _accountRepository.GetAccountByNumber(codeAccount);
                if (account == null)
                {
                    flag = 1;
                }
            } while (flag == 0);

            return codeAccount;
        }

        public void CreateAccount(ClaimsPrincipal User)
        {
            CheckAccounts(User);
            var client = _clientServices.GetCurrent(User);
            string codeAccount = GenerateAccountNumber();


            Account newAccount = new Account
            {
                Number = codeAccount,
                CreationDate = DateTime.Now,
                Balance = 0,
                ClientId = client.Id,
            };

            _accountRepository.Save(newAccount);
        }


    }
}
