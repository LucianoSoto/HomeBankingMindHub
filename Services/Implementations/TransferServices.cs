using Clase_1.DTOS;
using Clase_1.Models;
using Clase_1.Repositories;
using HomeBankingMindHub.Models;
using System.Security.Claims;

namespace Clase_1.Services.Implementations
{
    public class TransferServices : ITransferServices
    {
        public IAccountRepository _accountRepository { get; set; }
        public IClientRepository _clientRepository { get; set; }
        public ITransactionRepository _transactionRepository { get; set; }

        public IClientServices _clientService { get; set; }

        public TransferServices(IAccountRepository accountRepository, IClientRepository clientRepository, ITransactionRepository transactionRepository, IClientServices clientService)
        {
            _accountRepository = accountRepository;
            _clientRepository = clientRepository;
            _transactionRepository = transactionRepository;
            _clientService = clientService;
        }

        public IEnumerable<TransactionDTO> GetAllTransactions()
        {
            var transaction = _transactionRepository.GetAllTransactions();
            return transaction.Select(transaction => new TransactionDTO(transaction)).ToList();
        }

        public TransactionDTO GetTransactionById(long id)
        {
            var transaction = _transactionRepository.GetTransactionById(id);
            if (transaction == null)
            {
                throw new Exception("Not Found!");
            }
            return new TransactionDTO(transaction);
        }

        public void CheckIfEmpty(TransferDTO transfer)
        {
            if (transfer.Amount == 0 || string.IsNullOrEmpty(transfer.Description) || string.IsNullOrEmpty(transfer.ToAccountNumber) || string.IsNullOrEmpty(transfer.FromAccountNumber))
            {
                throw new Exception("Por favor rellenar todos los campos");
            }
        }

        public void CheckIfEqual(TransferDTO transfer)
        {
            if (string.Equals(transfer.FromAccountNumber, transfer.ToAccountNumber))
            {
                throw new Exception("No podes transferirte a vos mismo"); //(boludo)
            }
        }

        public void CheckIfExists(TransferDTO transfer)
        {
            if (_accountRepository.GetAccountByNumber(transfer.FromAccountNumber) == null)
            {
                throw new Exception("La cuenta de origen no existe");
            }
            if (_accountRepository.GetAccountByNumber(transfer.ToAccountNumber) == null)
            {
                throw new Exception("La cuenta de destino no existe");
            }
        }

        public AccountDTO CheckClientAccount(ClaimsPrincipal User, TransferDTO transferDTO)
        {
            var currentUserAccounts = _clientService.GetCurrentAccounts(User);
            AccountDTO clientAccount = new AccountDTO(_accountRepository.GetAccountByNumber(transferDTO.FromAccountNumber));
            var flag = 0;

            if (currentUserAccounts == null)
            {
                throw new Exception("El cliente no tiene cuentas");
            }

            foreach (var account in currentUserAccounts)
            {
                if (string.Equals(account.Number.ToUpper(), transferDTO.FromAccountNumber.ToUpper()))
                {
                    flag = 1;
                    clientAccount = account;
                }
            }

            if (flag == 0)
            {
                throw new Exception("La cuenta de origen no pertenece a este cliente");
            }

            return clientAccount;
        }

        public void CheckBalance(ClaimsPrincipal User, TransferDTO transfer)
        {
            var accountBalance = CheckClientAccount(User, transfer).Balance;
            if (transfer.Amount > accountBalance)
            {
                throw new Exception("La cuenta seleccionada no tiene el balance suficiente para realizar la transacción");
            }
        }

        public void CreateTransaction(ClaimsPrincipal User, TransferDTO transfer)
        {
            try
            {
                CheckIfEmpty(transfer);
                CheckIfExists(transfer);
                CheckClientAccount(User, transfer);
                CheckIfEqual(transfer);
                CheckBalance(User, transfer);


                long toId = _accountRepository.GetAccountByNumber(transfer.ToAccountNumber).Id;
                long FromId = _accountRepository.GetAccountByNumber(transfer.FromAccountNumber).Id;


                Transaction fromTransaction = new Transaction
                {
                    Type = TransactionType.DEBIT,
                    Amount = transfer.Amount,
                    Description = transfer.Description,
                    Date = DateTime.Now,
                    AccountId = FromId,
                };

                Transaction toTransaction = new Transaction
                {
                    Type = TransactionType.CREDIT,
                    Amount = transfer.Amount,
                    Description = transfer.Description,
                    Date = DateTime.Now,
                    AccountId = toId,
                };

                var fromAccountNewBalance = _accountRepository.GetAccountByNumber(transfer.FromAccountNumber).Balance - transfer.Amount;
                var toAccountNewBalance = _accountRepository.GetAccountByNumber(transfer.ToAccountNumber).Balance + transfer.Amount;

                Account fromAccount = _accountRepository.GetAccountByNumber(transfer.FromAccountNumber);
                fromAccount.Balance = fromAccountNewBalance;
                Account toAccount = _accountRepository.GetAccountByNumber(transfer.ToAccountNumber);
                toAccount.Balance = toAccountNewBalance;

                _transactionRepository.Save(toTransaction);
                _transactionRepository.Save(fromTransaction);

                _accountRepository.Save(fromAccount);
                _accountRepository.Save(toAccount);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
