using Clase_1.Models;
using Clase_1.DTOS;
using HomeBankingMindHub.Models;

namespace Clase_1.DTOS
{
    public class AccountDTO
    {
        public long Id { get; set; }

        public string Number { get; set; }

        public DateTime CreationDate { get; set; }

        public double Balance { get; set; }

        public ICollection<TransactionDTO> Transactions { get; set; }

        public AccountDTO(Account account) 
        {
            Id = account.Id;
            Number = account.Number;
            CreationDate = account.CreationDate;
            Balance = account.Balance;
            Transactions = account.Transactions.Select(transaction => new TransactionDTO(transaction)).ToList();
        }
    }
}
