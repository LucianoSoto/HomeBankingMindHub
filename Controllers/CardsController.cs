using Clase_1.DTOS;
using Clase_1.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Clase_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardsController : ControllerBase
    {
        private readonly ICardRepository _cardRepository;

        public CardsController(ICardRepository cardRepository)
        {
            _cardRepository = cardRepository;
        }

        [HttpGet]

        public IActionResult GetAllCards()
        {
            try
            {
                var cards = _cardRepository.GetAllCards();
                var cardsDTO = cards.Select(cards => new CardDTO(cards)).ToList();
                return Ok(cards);
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
                var card = _cardRepository.GetCardById(id);
                if (card == null)
                {
                    return NotFound();
                }
                var cardDTO = new CardDTO(card);
                return Ok(cardDTO);
            }

            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
    }
}
