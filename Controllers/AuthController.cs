using Clase_1.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Numerics;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using HomeBankingMindHub.Models;
using Clase_1.DTOS;

namespace Clase_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IClientRepository _clientRepository;

        public AuthController(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }
        
        [HttpPost("login")]

        public async Task<IActionResult> Login([FromBody] ClientUserDTO client)
        {
            try
            {
                Client user = _clientRepository.GetClientByEmail(client.Email);
                 
                if (user == null || !String.Equals(user.Password, client.Password))
                {
                    return Unauthorized();
                }

                var claims = new List<Claim>
                {
                    new Claim("Client", user.Email)
                };
                if (client.Email.Equals("lusoto@gmail.com"))
                {
                    claims.Add(new Claim("Admin", "true"));
           
                }
                
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("logout")]

        public async Task<IActionResult> Logout()
        {
            try
            {
                await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
