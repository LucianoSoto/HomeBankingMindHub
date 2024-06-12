using BCrypt.Net;
using Clase_1.DTOS;
using Clase_1.Models;
using Clase_1.Repositories;
using HomeBankingMindHub.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Clase_1.Services.Implementations
{
    public class ClientServices : ControllerBase, IClientServices
    {
        private readonly IClientRepository _clientRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly ICardRepository _cardRepository;

        public ClientServices(IClientRepository clientRepository, IAccountRepository accountRepository, ICardRepository cardRepository)
        {
            _clientRepository = clientRepository;
            _accountRepository = accountRepository;
            _cardRepository = cardRepository;
        }

        public IEnumerable<ClientDTO> GetAllClients()
        {
            var clients = _clientRepository.GetAllClients();
            return clients.Select(clients => new ClientDTO(clients)).ToList();
        }

        public ClientDTO GetClientById(long id)
        {
            var client = _clientRepository.GetClientById(id);
            if (client == null)
            {
                throw new Exception("Not Found!");
            }
            var clientDTO = new ClientDTO(client);
            return clientDTO;
        }

        public ClientDTO GetCurrent(ClaimsPrincipal User)
        {
            string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;

            if (email == string.Empty)
            {
                throw new Exception("Not Found!");
            }

            Client client = _clientRepository.GetClientByEmail(email);

            if (client == null)
            {
                throw new Exception("Not Found!");
            }

            ClientDTO clientDTO = new ClientDTO(client);

            return clientDTO;
        }

        public void CheckAccounts(ClaimsPrincipal User)
        {
            var client = GetCurrent(User);
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
            var client = GetCurrent(User);
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

        public void CheckCards(NewCardDTO newCardDTO, long id)
        {
            var cardsByType = newCardDTO.Type == "DEBIT" ? _cardRepository.GetDebitCards(id) : _cardRepository.GetCreditCards(id);
            if(newCardDTO.Type == null || newCardDTO.Color == null)
            {
                throw new Exception("Por favor rellenar todos los datos");
            }

            if (cardsByType.Count() >= 3)
            {
                throw new Exception("No pueden haber mas de 3 cartas por tipo");
            }

            foreach (var card in cardsByType)
            {
                if (card.Color == newCardDTO.Color)
                {
                    throw new Exception("No pueden haber mas de 1 carta por color y tipo");
                }
            }
        }

        public string CreateCardNumber()
        {
            string cardCode = "";
            long cardNumber = 0;
            int flag = 0;

            do
            {
                for (int i = 0; i < 3; i++)
                {
                    cardNumber = RandomNumberGenerator.GetInt32(0000, 9999);
                    cardCode += cardNumber.ToString("D4") + "-";
                }

                cardNumber = RandomNumberGenerator.GetInt32(0000, 9999);
                cardCode += cardNumber.ToString("D4");

                Card card = _cardRepository.GetCardByNumber(cardCode);
                if (card == null)
                {
                    flag = 1;
                }
            } while (flag == 0);

            return cardCode;
        }

        public void CreateCard(ClaimsPrincipal User, NewCardDTO newCardDTO)
        {

            var client = GetCurrent(User);
            CheckCards(newCardDTO, client.Id);

            var userCards = _cardRepository.GetCardsByClient(client.Id);
            var cardCode = CreateCardNumber();
            int cvv = RandomNumberGenerator.GetInt32(0, 999);

            Card newCard = new Card()
            {
                CardHolder = client.FirstName + "" + client.LastName,
                Type = newCardDTO.Type,
                Color = newCardDTO.Color,
                Number = cardCode,
                Cvv = cvv,
                FromDate = DateTime.Now,
                ThruDate = DateTime.Now.AddYears(5),
                ClientId = client.Id
            };

            _cardRepository.Save(newCard);
        }

        public IEnumerable<AccountDTO> GetCurrentAccounts(ClaimsPrincipal User)
        {
            var client = GetCurrent(User);
            var clientAccounts = _accountRepository.GetAccountsByClient(client.Id);
            var accountsDTO = clientAccounts.Select(accounts => new AccountDTO(accounts)).ToList();
            return accountsDTO;
        }

        public IEnumerable<CardDTO> GetCurrentCards(ClaimsPrincipal User)
        {
            var client = GetCurrent(User);
            var clientCards = _cardRepository.GetCardsByClient(client.Id);
            var cardsDTO = clientCards.Select(cards => new CardDTO(cards)).ToList();
            return cardsDTO;
        }

        public void CheckClientInfo(Client client)
        {
            if (string.IsNullOrEmpty(client.Email) ||
                   string.IsNullOrEmpty(client.Password) ||
                   string.IsNullOrEmpty(client.FirstName) ||
                   string.IsNullOrEmpty(client.LastName))
            {
                throw new Exception("Rellena todos los campos");
            }
        }

        public Client CheckEmail(Client client)
        {
            Client user = _clientRepository.GetClientByEmail(client.Email);

            if (user != null)
            {
                throw new Exception("Email ya en uso");
            }

            return user;
        }

        public void CreateClient(ClaimsPrincipal User, Client client)
        {
            CheckClientInfo(client);
            CheckEmail(client);
            string password = BCrypt.Net.BCrypt.EnhancedHashPassword(client.Password, 13);

            Client newClient = new Client
            {
                Email = client.Email,
                Password = password,
                FirstName = client.FirstName,
                LastName = client.LastName,
            };

            _clientRepository.Save(newClient);
        }
    }
}
