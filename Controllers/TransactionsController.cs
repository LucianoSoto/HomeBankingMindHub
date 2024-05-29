using Clase_1.DTOS;
using Clase_1.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Clase_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionsController(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        [HttpGet]

        public IActionResult GetAllTransactions()
        {
            try
            {
                var transaction = _transactionRepository.GetAllTransactions();
                var transactionDTO = transaction.Select(transaction => new TransactionDTO(transaction)).ToList();
                return Ok(transactionDTO);
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
                var transaction = _transactionRepository.GetTransactionById(id);
                if (transaction == null)
                {
                    return NotFound();
                }
                var transactionDTO = new TransactionDTO(transaction);
                return Ok(transaction);
            }

            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
    }
}
