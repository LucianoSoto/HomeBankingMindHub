using Clase_1.DTOS;
using Clase_1.Models;
using Clase_1.Repositories;
using Clase_1.Services;
using HomeBankingMindHub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;

namespace HomeBankingMindHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly ClientServices _clientService;
        //ctor es un atajo para crear constructores
        public ClientsController(ClientServices clientService)
        {
            _clientService = clientService;
        }

        [HttpGet]
        [Authorize(Policy = "AdminOnly")]

        public IActionResult GetAllClients()
        {
            try
            {
                return Ok(_clientService.GetAllClients());
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
               return Ok(_clientService.GetClientById(id));
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
            try
            {
                return Ok(_clientService.GetCurrent(User));
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
                _clientService.CreateAccount(User);
                return Ok(); ;
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
                _clientService.CreateCard(User, newCardDTO);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("current/accounts")]

        public IActionResult GetCurrentAccounts()
        {
            return Ok(_clientService.GetCurrentAccounts(User));
        }

        [HttpGet("current/cards")]

        public IActionResult GetCurrentCards()
        {

            return Ok(_clientService.GetCurrentCards(User));
        }


        [HttpPost]

        public IActionResult Post([FromBody] Client clientUserDTO)
        {
            try
            {
                _clientService.CreateClient(User, clientUserDTO);
                return Ok();
            }
            catch(Exception ex)
            {  
                return StatusCode(500, ex.Message);}
            }
        }
        
    }

