using CompanyTraining.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CompanyTraining.Areas.Company.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Company")]
    public class QuizController : ControllerBase
    {
        private readonly IQuizRepository _quizRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly ICourseRepository _courseRepository;

        public QuizController(IQuizRepository quizRepository,IQuestionRepository questionRepository, ICourseRepository courseRepository)
        {
            this._quizRepository = quizRepository;
            this._questionRepository = questionRepository;
            this._courseRepository = courseRepository;
        }


        [HttpPost("AssignQuestion/{quizId}")]
        public async Task<IActionResult> AssignQuestion([FromRoute] int quizId, [FromBody] AssignQuestionRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var quiz = _quizRepository.GetOne(expression: e => e.Id == quizId, includes: [
                e=>e.Course,
                e=>e.Questions,
                ]);

            if (quiz == null || quiz.Course.ApplicationUserId != userId) return Forbid();

            if (request.Questions is null || quiz.NumberOfQuestions < request.Questions.Count())
                return BadRequest("Number of questions exceeds the allowed limit.");

            var questions = await  _questionRepository.Get(expression: e => e.ApplicationUserId==userId &&e.QuizId==null&&request.Questions.Contains(e.Id)).ToListAsync();

            if (questions.Count() != request.Questions.Count())
                return BadRequest("Some Questions are Invalid");

            foreach (var question in questions)
            {
                question.QuizId = quizId;
                quiz.Questions.Add(question);
            }

            await _quizRepository.CommitAsync();

            return Ok(new
            {
                Message = "Questions assigned successfully.",
                Success = true,
                QuizId = quiz.Id,
                TotalAssigned = quiz.Questions.Count
            });
        }

        [HttpPost("CreateQuiz")]
        public async Task<IActionResult> Create([FromBody] QuizCreationRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var course = _courseRepository.GetOne(e => e.Id == request.CourseId && e.ApplicationUserId == userId);
            if (course == null) return Forbid();

            var quiz = request.Adapt<Quiz>();
            
            if (request.IsGenerated == true)
            {
               bool result = await AddQuestionsGenerated(quiz);
                if (!result) return BadRequest("There is enough types of questions to fill quiz");
            }
            await _quizRepository.CreateAsync(quiz);
            return Created();
        }

        [HttpGet("GetQuestionsPerQuiz/{quizId}")]
        public  IActionResult GetQuestionsPerQuiz([FromRoute] int quizId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();
            var quiz = _quizRepository.GetOne(expression: e => e.Id == quizId, includes: [
                e=>e.Course
                ]);
            if (quiz == null) return NotFound();
            if (quiz.Course.ApplicationUserId != userId) return Forbid();
            var questions = _questionRepository.Get(expression:e=>e.QuizId==quizId);
            return Ok(new
            {
                Data = questions.Adapt<IEnumerable<QuestionResponse>>()
            });
        }
     
        private async Task<bool> AddQuestionsGenerated(Quiz quiz)
        {
            int totalQuestions = quiz.NumberOfQuestions;

            double simpleRatio = 0.5;
            double mediumRatio = 0.3;

            int simpleQuestionsExpected = Convert.ToInt32((totalQuestions * simpleRatio));
            int mediumQuestionsExpected = Convert.ToInt32((totalQuestions * mediumRatio));
            int hardCountQuestions = totalQuestions - (simpleQuestionsExpected + mediumQuestionsExpected);

            var simpleQuestionList = await _questionRepository.GetSimpleQuestions(simpleQuestionsExpected);
            var mediumQuestionList = await _questionRepository.GetMediumQuestions(mediumQuestionsExpected);
            var hardQuestionList = await  _questionRepository.GetHardQuestions(hardCountQuestions);
            if (simpleQuestionList.Count() < simpleQuestionsExpected || mediumQuestionList.Count() < mediumQuestionsExpected || hardQuestionList.Count() < hardCountQuestions)
                return false;

            var allQuestions = simpleQuestionList.Concat(mediumQuestionList).Concat(hardQuestionList);
            foreach (var question in allQuestions) 
            {
                question.QuizId = quiz.Id;
                quiz.Questions.Add(question);
            }
            return true;
        }
    }
}
