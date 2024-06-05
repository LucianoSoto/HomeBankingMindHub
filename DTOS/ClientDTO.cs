using Clase_1.Models;
using HomeBankingMindHub.Models;
using System.Text.Json.Serialization;

namespace Clase_1.DTOS
{
    public class ClientDTO
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public ICollection<AccountClientDTO> Accounts { get; set; }
        public ICollection<ClientLoanDTO> Loans { get; set; }
        public ICollection<CardDTO> Cards { get; set; }

        public ClientDTO(Client client)
        {
            Id = client.Id;
            FirstName = client.FirstName;
            LastName = client.LastName;
            Email = client.Email;
            Accounts = client.Accounts.Select(account => new AccountClientDTO(account)).ToList();
            Loans = client.Loans.Select(loan => new ClientLoanDTO(loan)).ToList();
            Cards = client.Cards.Select(card => new CardDTO(card)).ToList();
        }
        public ClientDTO(ClientUserDTO client)
        {
            Email = client.Email;
        }
    }
}
