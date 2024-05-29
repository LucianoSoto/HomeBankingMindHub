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

        public ClientDTO(Client client)
        {
            Id = client.Id;
            FirstName = client.FirstName;
            LastName = client.LastName;
            Email = client.Email;
            Accounts = client.Accounts.Select(a => new AccountClientDTO(a)).ToList();
        }
    }
}
