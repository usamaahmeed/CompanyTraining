using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CompanyTraining.Areas.User
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    [Authorize(Roles = "User")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmplyeeRepository _employeeRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IUserCourseRepository _userCourseRepository;
        private readonly ILessonRepository _lessonRepository;
        private readonly IModuleRepository _moduleRepository;
        private readonly IQuizRepository _quizRepository;

        public EmployeeController(IEmplyeeRepository employeeRepository, 
            ICourseRepository courseRepository,IUserCourseRepository userCourseRepository,
            ILessonRepository lessonRepository,
            IModuleRepository moduleRepository,
            IQuizRepository quizRepository
            )
        {
            this._employeeRepository = employeeRepository;
            this._courseRepository = courseRepository;
            this._userCourseRepository = userCourseRepository;
            this._lessonRepository = lessonRepository;
            this._moduleRepository = moduleRepository;
            this._quizRepository = quizRepository;
        }
        [HttpGet]
        public IActionResult GetCoursesForEmployee()
        {
            var employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var employee = _employeeRepository.GetOne(e => e.Id == employeeId);
            if(employee == null) return Unauthorized();
            var courses = _courseRepository.Get(expression:e=>e.isActive && e.ApplicationUserId == employee.CompanyId, includes: [
                e=>e.Category
                ]);
            return Ok(courses.Adapt<IEnumerable<GetCoursesForUserResponse>>());
        }
        [HttpPost("Course/{courseId}")]
        public async Task<IActionResult> AssignCourseForUser([FromRoute] int courseId)
        {
            var course = _courseRepository.GetOne(e=>e.Id == courseId);
            if(course == null) return Forbid();

            var employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(employeeId == null) return Unauthorized();

            var userCourse = _userCourseRepository.GetOne(expression: e => e.CourseId == courseId && e.ApplicationUserId == employeeId);
            if(userCourse != null)
                return NoContent();

            await _userCourseRepository.CreateAsync(new UserCourse
            {
                CourseId = courseId,
                ApplicationUserId = employeeId,
            });
            return Ok();
        }
        [HttpGet("Course/{courseId}")]
        public IActionResult ViewCourseModules([FromRoute]int courseId) 
        {
            var course = _courseRepository.GetOne(e => e.Id == courseId);
            if (course == null) return Forbid();

            var employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (employeeId == null) return Unauthorized();

            var modulesForCourse = _moduleRepository.Get(e=>e.CourseId==courseId);
            if (modulesForCourse == null) return NotFound();

            return Ok(modulesForCourse.Adapt<IEnumerable<ViewCourseModulesResponse>>());
        }

        [HttpGet("Course/{courseId}/Modules/{moduleId}")]
        public IActionResult ViewLessons([FromRoute] int moduleId, [FromRoute] int courseId)
        {
            var employee = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (employee == null) return Unauthorized();

            var module = _moduleRepository.GetOne(expression: e => e.Id == moduleId && e.CourseId == courseId);
            if (module == null) return NotFound();

            var lessons = _lessonRepository.Get(e => e.ModuleId == module.Id);
            if (lessons == null) return NotFound();

            return Ok(lessons.Adapt<IEnumerable<GetLessonsResponse>>());
        }


        [HttpGet("Courses/{courseId}/Exams/{quizId}")]
        public async Task<IActionResult> TakeExam([FromRoute] int courseId, [FromRoute] int quizId)
        {
            var employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (employeeId == null) return NotFound();


            var assigned = await _userCourseRepository.Get().AnyAsync(e=>e.CourseId == courseId && e.ApplicationUserId == employeeId);
            if (!assigned) return Forbid();

            var quiz =  await _employeeRepository.GetQuizWithQuetionsWithChoices(quizId,courseId);
            if (quiz == null) return NotFound();

            return Ok(quiz.Adapt<GetExamWithQuestionsResponse>());
        }

    }
}
