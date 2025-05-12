using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CompanyTraining.Areas.Company.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Company")]
    public class ModuleController : ControllerBase
    {
        private readonly IModuleRepository _moduleRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICourseRepository _courseRepository;
        private readonly ILessonRepository _lessonRepository;

        public ModuleController(IModuleRepository moduleRepository,
            UserManager<ApplicationUser> userManager,
            ICourseRepository courseRepository,
            ILessonRepository lessonRepository
            )
        {
            this._moduleRepository = moduleRepository;
            this._userManager = userManager;
            this._courseRepository = courseRepository;
            this._lessonRepository = lessonRepository;
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] ModuleRequest moduleRequest)
        {
            var company = await _userManager.GetUserAsync(User);

            if (company == null)
                return NotFound();

            var course = _courseRepository.GetOne(expression: e => e.Id == moduleRequest.CourseId && e.ApplicationUserId == company.Id);

            if (course == null)
                return NotFound();

            var module = moduleRequest.Adapt<Module>();

            await _moduleRepository.CreateAsync(module);
            return Ok(
                new
                {
                    Message = "Module Created Successfully",
                    Success = true,
                    Data = module.Adapt<ModuleResponse>(),
                });
        }

        [HttpGet("GetAll/{id}")]

        public async Task<IActionResult> Get(int id)
        {
            var company = await _userManager.GetUserAsync(User);
            if (company == null)
                return NotFound();
            var modules = _moduleRepository.Get(expression: e => e.CourseId == id && e.Course.ApplicationUserId == company.Id, includes: [
                e=>e.Course
                ]);
            if (modules == null || !modules.Any())
                return NotFound();
            return Ok(
                new
                {
                    Message = "Get All Modules For this course successfully",
                    Success = true,
                    Data = modules.Adapt<IEnumerable<ModuleResponse>>(),
                }
                );
        }


        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateModuleRequest updateModuleRequest)
        {
            var company = await _userManager.GetUserAsync(User);
            if(company==null) return NotFound();
            var module = _moduleRepository.GetOne(expression:e=>e.Course.ApplicationUserId==company.Id && e.Id == id, includes: [
                e=>e.Course
                ]);
            if(module==null)
                return NotFound();
            if (!string.IsNullOrEmpty(updateModuleRequest.Title)) 
            {
                module.Title = updateModuleRequest.Title;
                await _moduleRepository.CommitAsync();
                return Ok(new
                {
                    message = "Updated The Module Successfully",
                    Success = true
                });
            }
           return NoContent();
        }


        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var company = await _userManager.GetUserAsync(User);
            if (company == null) return NotFound();
            var module =  _moduleRepository.GetOne(expression:e=>e.Id==id && e.Course.ApplicationUserId == company.Id, includes: [
                e=>e.Course,
                e=>e.Lessons
                ]);
              if(module == null) return NotFound();
              if(module.Lessons.Any())
                 {
                _lessonRepository.RemoveRange(module.Lessons);
                   }
             await  _moduleRepository.DeleteAsync(module);
            return Ok(new
            {
                message = "Deleted Module Successfully",
                Success=true
            });
        }
    }
}
