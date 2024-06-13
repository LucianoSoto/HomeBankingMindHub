using Clase_1.DTOS;
using Clase_1.Repositories;
using Clase_1.Services;
using Clase_1.Services.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Clase_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountServices _accountServices;

        public AccountsController(IAccountServices accountServices)
        {
            _accountServices = accountServices;
        }

        [HttpGet]

        public IActionResult GetAllAccounts()
        {
            try
            {
                return Ok(_accountServices.GetAllAccounts());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]

        public IActionResult GetAccountById(long id)
        {
            try
            {
                return Ok(_accountServices.GetAccountById(id));
            }

            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
    }
}
