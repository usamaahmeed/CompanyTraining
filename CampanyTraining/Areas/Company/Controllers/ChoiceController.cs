using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CompanyTraining.Areas.Company.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    [Authorize(Roles = "Company")]
    public class ChoiceController : ControllerBase
    {
        private readonly IChoiceRepository _choiceRepository;
        private readonly IQuestionRepository _questionRepository;

        public ChoiceController(IChoiceRepository choiceRepository, IQuestionRepository questionRepository)
        {
            this._choiceRepository = choiceRepository;
            this._questionRepository = questionRepository;
        }

        [HttpPost("Questions/{questionId}")]
        public async Task<IActionResult> AssignChoice([FromRoute] int questionId, [FromBody] AssignChoiceRequest request)
        {
            var company = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (company == null) return Unauthorized();

            var question = _questionRepository.GetOne(e => e.Id == questionId);
            if (question == null) return NotFound();

            if (request.Choices.Count() < 2)
                return BadRequest("Choices Must be greater than 2");

            if (!request.Choices.Any(e => e.IsCorrect))
                return BadRequest("Should have one correct answer");

            foreach (var choice in request.Choices)
            {
                var choiceMapped = choice.Adapt<Choice>();
                choiceMapped.QuestionId = questionId;
                await _choiceRepository.CreateAsync(choiceMapped);
            }
            return Ok();
        }


        [HttpGet("Questions/{questionId}")]
        public  IActionResult GetChoicesForQuestion([FromRoute] int questionId)
        {
            var company = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (company == null) return Unauthorized();

            var question = _questionRepository.GetOne(e => e.Id == questionId);
            if (question == null) return NotFound();

            var choices = _choiceRepository.Get(expression:e=>e.QuestionId==questionId);
            return Ok(choices.Adapt<IEnumerable<GetChoicesForQuestionResponse>>());
        }

        [HttpPut("Questions/{questionId}/Choices/{choiceId}")]
        public async Task<IActionResult> UpdateChoice([FromRoute] int questionId, [FromRoute] int choiceId, [FromBody] UpdateChoiceDto request)
        {

            var company = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (company == null) return Unauthorized();

            var choice = _choiceRepository.GetOne(expression:e=>e.QuestionId== questionId && e.Id==choiceId);
            if (choice == null) return NotFound();

             request.Adapt(choice);

            await _choiceRepository.EditAsync(choice);

            var choices = _choiceRepository.Get(expression: e => e.QuestionId == questionId).AsNoTracking();
            if (!await choices.AnyAsync(e => e.IsCorrect))
                return BadRequest("Must be has one correct answer");

            return Ok(choice.Adapt<GetChoicesForQuestionResponse>());
        }
    }
}
