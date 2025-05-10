using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CompanyTraining.Areas.Company.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("Company")]
    public class CourseController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICourseRepository _courseRepository;

        public CourseController(UserManager<ApplicationUser> userManager, ICourseRepository courseRepository)
        {
            this._userManager = userManager;
            this._courseRepository = courseRepository;
        }
        [HttpPost("Create")]
        public IActionResult Create([FromBody] CourseRequest courseRequest)
        {
            var companyApp = _userManager.GetUserAsync(User);
            if (companyApp == null)
                return NotFound();
            var course = courseRequest.Adapt<Course>();
            return Created();
        }
    }
}
