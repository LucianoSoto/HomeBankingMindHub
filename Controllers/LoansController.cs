using Clase_1.DTOS;
using Clase_1.Services;
using Clase_1.Services.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Clase_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoansController : ControllerBase
    {
        private readonly ILoanServices _loanService;

        public LoansController(ILoanServices loanService)
        {
            _loanService = loanService;
        }

        [HttpGet]

        public IActionResult GetAll()
        {
            try
            {
                return Ok(_loanService.GetAllLoans());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Policy = "ClientOnly")]

        public IActionResult CreateLoan([FromBody] LoanApplicationDTO loanApplicationDTO)
        {
            try
            {
                _loanService.CreateLoan(loanApplicationDTO, User);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
