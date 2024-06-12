using Clase_1.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HomeBankingMindHub.Models;
using Clase_1.DTOS;
using Clase_1.Services.Implementations;
using System.Security.Claims;

namespace Clase_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IClientRepository _clientRepository;
        private ClientServices _clientService;
        private UserServices _userService;

        public AuthController(IClientRepository clientRepository, ClientServices clientService, UserServices userService)
        {
            _clientRepository = clientRepository;
            _clientService = clientService;
            _userService = userService;
        }

        [HttpPost("login")]

        public async Task<IActionResult> Login([FromBody] ClientUserDTO client)
        {
            try
            {
                var claimsIdentity = _userService.Login(client);

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
