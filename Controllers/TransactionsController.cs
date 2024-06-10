using Clase_1.DTOS;
using Clase_1.Repositories;
using Clase_1.Services.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Clase_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly TransferServices _transferService;

        public TransactionsController(TransferServices transferService)
        {
            _transferService = transferService;
        }

        [HttpGet]

        public IActionResult GetAllTransactions()
        {
            try
            {
                return Ok(_transferService.GetAllTransactions());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]

        public IActionResult GetTransactionById(long id)
        {
            try
            {
                return Ok(_transferService.GetTransactionById(id));
            }

            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpPost]
        [Authorize(Policy = "ClientOnly")]

        public IActionResult createTransaction([FromBody]TransferDTO transferDTO)
        {
            try
            {
                _transferService.CreateTransaction(User, transferDTO);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
    }
}
