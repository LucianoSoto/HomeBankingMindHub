using Clase_1.DTOS;
using Clase_1.Models;
using HomeBankingMindHub.Models;

namespace Clase_1.Models
{
    public class Account
    {
        public long Id { get; set; }

        public string Number { get; set; }

        public DateTime CreationDate { get; set; }

        public double Balance { get; set; }

        public Client Client { get; set; }

        public long ClientId { get; set; }

        public ICollection<Transaction> Transactions { get; set; }

    }
}
