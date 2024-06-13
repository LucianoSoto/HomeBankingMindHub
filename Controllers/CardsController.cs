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
    public class CardsController : ControllerBase
    {
        private readonly ICardRepository _cardRepository;
        private readonly ICardServices _cardServices;

        public CardsController(ICardRepository cardRepository, ICardServices cardServices)
        {
            _cardRepository = cardRepository;
            _cardServices = cardServices;
        }

        [HttpGet]

        public IActionResult GetAllCards()
        {
            try
            {
                return Ok(_cardServices.GetAllCards());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]

        public IActionResult GetCardById(long id)
        {
            try
            {
                return Ok(_cardServices.GetCardById(id));
            }

            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
    }
}
