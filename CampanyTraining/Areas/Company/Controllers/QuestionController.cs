using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CompanyTraining.Areas.Company.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Company")]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionRepository _questionRepository;

        public QuestionController(IQuestionRepository questionRepository, UserManager<ApplicationUser> userManager)
        {
            this._questionRepository = questionRepository;
        }
        [HttpPost("CreateQuestion")]
        public async Task<IActionResult> Create([FromBody] QuestionRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();
            var question = request.Adapt<Question>();
            question.ApplicationUserId = userId;
            await _questionRepository.CreateAsync(question);
            return Created();
        }

        [HttpGet("GetAllQuestions")]
        public IActionResult Get()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var questions = _questionRepository.Get(expression: e => e.ApplicationUserId == userId);

            if (questions == null) return NotFound();

            return Ok(new
            {
                Success = true,
                Data = questions.Adapt<IEnumerable<QuestionResponse>>()
            });
        }
        [HttpDelete("DeleteQuestion/{questionId}")]
        public async Task<IActionResult> Delete([FromRoute] int questionId) 
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var question = _questionRepository.GetOne(e=>e.Id==questionId);
            if (question == null || question.ApplicationUserId!=userId) return Forbid();

            await _questionRepository.DeleteAsync(question);
            return NoContent();
        }

        [HttpGet("GetHardQuestions")]
        public IActionResult GetHardQuestion()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var hardQuestions = _questionRepository.Get(e => e.ApplicationUserId == userId && e.QuestionLevel == enQuestionLevel.Hard);
            if (hardQuestions == null) return Forbid();
            return Ok(new
            {
                Success = true,
                Data = hardQuestions.Adapt<IEnumerable<QuestionResponse>>()
            });

        }

        [HttpGet("GetMediumQuestions")]
        public IActionResult GetMediumQuestion()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var mediumQuestions = _questionRepository.Get(e => e.ApplicationUserId == userId && e.QuestionLevel == enQuestionLevel.Medium);
            if (mediumQuestions == null) return Forbid();
            return Ok(new
            {
                Success = true,
                Data = mediumQuestions.Adapt<IEnumerable<QuestionResponse>>()
            });

        }

        [HttpGet("GetSimpleQuestions")]
        public IActionResult GetSimpleQuestion()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var simpleQuestions = _questionRepository.Get(e => e.ApplicationUserId == userId && e.QuestionLevel == enQuestionLevel.Simple);
            return Ok(new
            {
                Success = true,
                Data = simpleQuestions.Adapt<IEnumerable<QuestionResponse>>()
            });

        }
    }
}
