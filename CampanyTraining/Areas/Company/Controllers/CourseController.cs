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
            _categoryRepository = categoryRepository;
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CourseRequest courseRequest)
        {
            var companyApp = _userManager.GetUserAsync(User);
            var categories = _categoryRepository.GetOne(expression: e => e.Id == courseRequest.CategoryId);
            if (categories == null)
                return NotFound(new
                {
                    Message = "Category Not Found",
                    Success = false,
                });
            if (companyApp == null)
                return NotFound();
            var course = courseRequest.Adapt<Course>();
            await _courseRepository.CreateAsync(course);

            return Ok(new
            {
                Message = "Course Created Successfully",
                Success = true,

            });
        }




    }
}
