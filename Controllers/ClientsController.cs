using Clase_1.DTOS;
using Clase_1.Repositories;
using HomeBankingMindHub.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HomeBankingMindHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly IClientRepository _clientRepository;

        //ctor es un atajo para crear controladores
        public ClientsController(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        /*Configura el controlador para que pueda hacer una peticion GET cuando se acceda a la ruta(api/client en este caso)

        [HttpGet]

        public IActionResult Get()
        {
            return Ok("Hello world!");
        }*/

        [HttpGet]

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

        public IActionResult GetCurrent()
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

                var clientDTO = new ClientDTO(client);

                return Ok(clientDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]

        public IActionResult Post([FromBody] Client client)
        {
            try
            {
                if (String.IsNullOrEmpty(client.Email) ||
                   String.IsNullOrEmpty(client.Password) ||
                   String.IsNullOrEmpty(client.FirstName) ||
                   String.IsNullOrEmpty(client.LastName))
                {
                    return StatusCode(403, "Oops! Datos Invalidos!");
                }

                Client user = _clientRepository.GetClientByEmail(client.Email);

                if (user != null)
                {
                    return StatusCode(403, "El Email ya esta en uso!");
                }

                Client newClient = new Client
                {
                    Email = client.Email,
                    Password = client.Password,
                    FirstName = client.FirstName,
                    LastName = client.LastName,
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

