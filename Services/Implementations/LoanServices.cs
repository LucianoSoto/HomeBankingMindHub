using Clase_1.DTOS;
using Clase_1.Models;
using Clase_1.Repositories;
using System.Security.Claims;
using System.Security.Cryptography.Xml;
using static Azure.Core.HttpHeader;

namespace Clase_1.Services.Implementations
{
    public class LoanServices
    {
        private readonly IClientRepository _clientRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IClientLoanRepository _clientLoanRepository;
        private readonly ILoanRepository _loanRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ClientServices _clientService;

        public LoanServices(IClientRepository clientRepository, IAccountRepository accountRepository, IClientLoanRepository clientLoanRepository, ILoanRepository loanRepository, ITransactionRepository transactionRepository, ClientServices clientService)
        {
            _clientRepository = clientRepository;
            _accountRepository = accountRepository;
            _clientLoanRepository = clientLoanRepository;
            _loanRepository = loanRepository;
            _transactionRepository = transactionRepository;
            _clientService = clientService;
        }

        public IEnumerable<ViewLoanDTO> GetAllLoans()
        {
            var loans = _loanRepository.GetAllLoans();
            return loans.Select(loans => new ViewLoanDTO(loans)).ToList();
        }

        public void VerifyData(LoanApplicationDTO loanApplicationDTO)
        {
            if (loanApplicationDTO.Amount == 0 || loanApplicationDTO.Payments == null)
            {
                throw new Exception("Por favor, inserte valores validos");
            }
        }

        public void VerifyIfLoanExists(LoanApplicationDTO loanApplicationDTO, ClaimsPrincipal User)
        {
            if (_loanRepository.GetLoanById(loanApplicationDTO.LoanId) == null)
            {
                throw new Exception("El prestamo solicitado no existe");
            }
        }

        public void CheckMaxAmount(LoanApplicationDTO loanApplicationDTO)
        {
            var loan = _loanRepository.GetLoanById(loanApplicationDTO.LoanId);

            if (loanApplicationDTO.Amount > loan.MaxAmount)
            {
                throw new Exception("No puedes solicitar un prestamo que exceda el limite permitido");
            }
        }

        public void CheckPayments(LoanApplicationDTO loanApplicationDTO)
        {
            var loan = _loanRepository.GetLoanById(loanApplicationDTO.LoanId);
            List<string> payments = loan.Payments.Split(new char[] { ',' }).ToList();
            int flag = 0;

            foreach (var item in payments)
            {
                if(loanApplicationDTO.Payments == item)
                {
                    flag = 1;
                }
            }

            if (flag == 0)
            {
                throw new Exception("El prestamo elegido no permite esa cantidad de cuotas");
            }
        }

        public Account CheckAccount(LoanApplicationDTO loanApplicationDTO)
        {
            var account = _accountRepository.GetAccountByNumber(loanApplicationDTO.ToAccountNumber);

            if (account == null)
            {
                throw new Exception("La cuenta Destino no existe");
            }
            else
            {
                return account;
            }
        }

        public void CheckUserAccount(LoanApplicationDTO loanApplicationDTO, ClaimsPrincipal User)
        {
            var client = _clientService.GetCurrentAccounts(User);
            var flag = 0;

            foreach (var account in client)
            {
                if(account.Number == loanApplicationDTO.ToAccountNumber)
                {
                    flag = 1;
                }
            }

            if (flag == 0)
            {
                throw new Exception("La cuenta seleccionada no pertenece al cliente autenticado");
            }
        }

        public void CreateLoan(LoanApplicationDTO loanApplicationDTO, ClaimsPrincipal User)
        {
            VerifyData(loanApplicationDTO);
            VerifyIfLoanExists(loanApplicationDTO, User);
            CheckMaxAmount(loanApplicationDTO);
            CheckPayments(loanApplicationDTO);
            CheckUserAccount(loanApplicationDTO, User);

            var account = CheckAccount(loanApplicationDTO);
            long clientId = _clientService.GetCurrent(User).Id;
            string description = "El prestamo " + _loanRepository.GetLoanById(loanApplicationDTO.LoanId).Name +" ha sido autorizado";
            double interest = loanApplicationDTO.Amount * 0.2;
            double newAmount = loanApplicationDTO.Amount + interest;
            var newBalance = account.Balance + loanApplicationDTO.Amount;

            ClientLoan loan = new ClientLoan()
            {
                LoanId = loanApplicationDTO.LoanId,
                ClientId = clientId,
                Amount = newAmount,
                Payments = loanApplicationDTO.Payments
            };

            _clientLoanRepository.Save(loan);

            Transaction transaction = new Transaction()
            {
                Type = TransactionType.CREDIT,
                Amount = loanApplicationDTO.Amount,
                Description = description,
                Date = DateTime.Now,
                AccountId = account.Id,
            };

            _transactionRepository.Save(transaction);

            account.Balance = newBalance;
            _accountRepository.Save(account);
        }
    }
}

/*
Debe recibir un objeto de solicitud de crédito con los datos del préstamo
>>Verificar que los datos sean correctos, es decir no estén vacíos, que el monto no sea 0 o que las cuotas no sean 0.
>>Verificar que el préstamo exista
>>Verificar que el monto solicitado no exceda el monto máximo del préstamo
>>Verifica que la cantidad de cuotas se encuentre entre las disponibles del préstamo
>>Verificar que la cuenta de destino exista
>>Verificar que la cuenta de destino pertenezca al cliente autenticado
>>Se debe crear una solicitud de préstamo con el monto solicitado sumando el 20% del mismo
>>Se debe crear una transacción “CREDIT” asociada a la cuenta de destino (el monto debe quedar positivo) con la descripción concatenando el nombre del préstamo y la frase “loan approved”
>>Se debe actualizar la cuenta de destino sumando el monto solicitado.
*/