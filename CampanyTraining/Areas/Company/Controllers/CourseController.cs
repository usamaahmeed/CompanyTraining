using CompanyTraining.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CompanyTraining.Areas.Company.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Company")]
    public class CourseController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICourseRepository _courseRepository;
        private readonly ICategoryRepository _categoryRepository;

        public CourseController(UserManager<ApplicationUser> userManager, ICourseRepository courseRepository, ICategoryRepository categoryRepository)
        {
            this._userManager = userManager;
            this._courseRepository = courseRepository;
            this. _categoryRepository = categoryRepository;
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CourseRequest courseRequest)
        {
            var companyApp = await _userManager.GetUserAsync(User);

            if (companyApp == null)
                return NotFound();

            var category = _categoryRepository.GetOne(expression: e => e.Id == courseRequest.CategoryId && e.ApplicationUserId == companyApp.Id);

            if (category == null)
                return NotFound(new
                {
                    Message = "Category Not Found",
                    Success = false,
                });

            var course = courseRequest.Adapt<Course>();

            course.ApplicationUserId = companyApp.Id;

            await _courseRepository.CreateAsync(course);
            return Ok(new
            {
                Message = "Course Created Successfully",
                Success = true,
                Data = course.Adapt<CourseResponse>(),
            });
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> Get()
        {
            var company = await _userManager.GetUserAsync(User);
            if(company==null)
                return NotFound();
            var existedCourses = _courseRepository.Get(expression: e => e.ApplicationUserId == company.Id, includes: [
                e=>e.Category
                ]);
            if (existedCourses == null)
                return NotFound();
            return Ok(existedCourses.Adapt<IEnumerable<CourseResponse>>());
        }
        [HttpGet("Search")]
        public async Task<IActionResult> Search([FromBody] SearchCourseRequest searchCourseRequest)
        {
            if (!string.IsNullOrEmpty(searchCourseRequest.Title))
            {
                var company = await _userManager.GetUserAsync(User);
                if (company == null)
                    return NotFound();
                var existedCourses = _courseRepository.Get(expression: e => e.ApplicationUserId == company.Id && e.Title.ToLower().Contains(searchCourseRequest.Title), includes: [
                    e=>e.Category
                    ]);
                if (existedCourses == null)
                    return NotFound();
                return Ok(existedCourses.Adapt<CourseResponse>());
            }
            return BadRequest( new
                {
                    Message = "Title is required for search.",
                    Success = false
                }
                );
        }

        [HttpPut("ToggleActive/{id}")]
        public async Task<IActionResult> ToggleActive(int id)
        {
            var company = await _userManager.GetUserAsync(User);

            if (company == null) 
                return NotFound();

            var course =_courseRepository.GetOne(expression:e=>e.Id == id && e.ApplicationUserId==company.Id);

            if (course == null)
                return NotFound();

            course.isActive = !course.isActive;
            await _courseRepository.CommitAsync();
            return Ok(
                new
                {
                    message = "Changed The Status Successfully"
                }
                );


        }
    }
}
