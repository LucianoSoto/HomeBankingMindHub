using Clase_1.DTOS;
using Clase_1.Models;
using Clase_1.Repositories;
using HomeBankingMindHub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Security.Principal;

namespace HomeBankingMindHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly IClientRepository _clientRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly ICardRepository _cardRepository;

        //ctor es un atajo para crear controladores
        public ClientsController(IClientRepository clientRepository, IAccountRepository accountRepository, ICardRepository cardRepository)
        {
            _clientRepository = clientRepository;
            _accountRepository = accountRepository;
            _cardRepository = cardRepository;
        }

        /*Configura el controlador para que pueda hacer una peticion GET cuando se acceda a la ruta(api/client en este caso)

        [HttpGet]

        public IActionResult Get()
        {
            return Ok("Hello world!");
        }*/

        [HttpGet]
        [Authorize(Policy = "AdminOnly")]

        public IActionResult GetAllClients()
        {
            try
            {
                var clients = _clientRepository.GetAllClients();
                var clientsDTO = clients.Select(clients => new ClientDTO(clients)).ToList();
                return Ok(clientsDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]

        public IActionResult GetClientById(long id)
        {
            try
            {
                var client = _clientRepository.GetClientById(id);
                if (client == null)
                {
                    return NotFound();
                }
                var clientDTO = new ClientDTO(client);
                return Ok(clientDTO);
            }

            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
        
        [HttpGet("current")]
        [Authorize(Policy = "ClientOnly")]

        public IActionResult GetCurrent()
        {
            //Pasar a Service
            try
            {
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;

                if (email == string.Empty)
                {
                    return Forbid();
                }

                Client client = _clientRepository.GetClientByEmail(email);

                if (client == null)
                {
                    return Forbid();
                }

                var clientDTO = new ClientDTO(client);

                return Ok(clientDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        
        [HttpPost("current/accounts")]
        [Authorize(Policy = "ClientOnly")]

        public IActionResult CreateAccount()
        {
            try
            {
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;

                if (email == string.Empty)
                {
                    return Forbid();
                }

                Client client = _clientRepository.GetClientByEmail(email);

                if (client == null)
                {
                    return Forbid();
                }

                var userAccounts = _accountRepository.GetAccountsByClient(client.Id);

                if (userAccounts.Count() >= 3)
                {
                    return StatusCode(403, "No pueden haber mas de 3 cuentas por cliente");
                }

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

                Account newAccount = new Account
                {
                    Number = codeAccount,
                    CreationDate = DateTime.Now,
                    Balance = 0,
                    ClientId = client.Id,
                };

                _accountRepository.Save(newAccount);
                return Ok();
            }
            catch(Exception ex) 
            {
                return StatusCode(500, ex.Message);
            }

            
        }
        
        [HttpPost("current/cards")]
        [Authorize(Policy = "ClientOnly")]

        public IActionResult CreateCard ([FromBody]NewCardDTO newCardDTO)
        {
            try
            {
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;

                if (email == string.Empty)
                {
                    return Forbid();
                }

                Client client = _clientRepository.GetClientByEmail(email);

                if (client == null)
                {
                    return Forbid();
                }

                var userCards = _cardRepository.GetCardsByClient(client.Id);

                if(newCardDTO.Type == "DEBIT")
                {
                    var debitCards = _cardRepository.GetDebitCards(client.Id);
                    if (debitCards.Count() >= 3)
                    {
                        return StatusCode(403, "No pueden haber mas de 3 cartas por tipo");
                    }

                    foreach (var card in debitCards)
                    {
                        if(card.Color == newCardDTO.Color)
                        {
                            return StatusCode(403, "No pueden haber mas de 1 carta por color y tipo");
                        }
                    }

                }else if(newCardDTO.Type == "CREDIT")
                {
                    var creditCards = _cardRepository.GetCreditCards(client.Id);
                    if (creditCards.Count() >= 3)
                    {
                        return StatusCode(403, "No pueden haber mas de 3 cartas por tipo");
                    }
                    foreach (var card in creditCards)
                    {
                        if (card.Color == newCardDTO.Color)
                        {
                            return StatusCode(403, "No pueden haber mas de 1 carta por color y tipo");
                        }
                    }
                }

                string cardCode = "";
                long cardNumber = 0;
                int flag = 0;

                do
                {
                    for(int i = 0; i < 4; i++)
                    {
                        cardNumber = RandomNumberGenerator.GetInt32(0, 9999);
                        cardCode += (cardNumber).ToString() +"-";
                    }
                    Card card = _cardRepository.GetCardByNumber(cardCode);
                    if (card == null)
                    {
                        flag = 1;
                    }
                } while (flag == 0);

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
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        

        [HttpPost]

        public IActionResult Post([FromBody] Client clientUserDTO)
        {
            try
            {
                if (String.IsNullOrEmpty(clientUserDTO.Email) ||
                   String.IsNullOrEmpty(clientUserDTO.Password) ||
                   String.IsNullOrEmpty(clientUserDTO.FirstName) ||
                   String.IsNullOrEmpty(clientUserDTO.LastName))
                {
                    return StatusCode(403, "Oops! Datos Invalidos!");
                }

                Client user = _clientRepository.GetClientByEmail(clientUserDTO.Email);

                if (user != null)
                {
                    return StatusCode(403, "El Email ya esta en uso!");
                }

                Client newClient = new Client
                {
                    Email = clientUserDTO.Email,
                    Password = clientUserDTO.Password,
                    FirstName = clientUserDTO.FirstName,
                    LastName = clientUserDTO.LastName,
                };

                _clientRepository.Save(newClient);
                return Created("", newClient);
            }
            catch(Exception ex)
            {  
                return StatusCode(500, ex.Message);}
            }
        }
        
    }

