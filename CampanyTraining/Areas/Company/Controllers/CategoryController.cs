using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CompanyTraining.Areas.Company.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles ="Company")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(UserManager<ApplicationUser> userManager,ICategoryRepository categoryRepository) 
        {
            this._userManager = userManager;
            this._categoryRepository = categoryRepository;
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CategoryRequest categoryRequest)
        {
            var companyApp = await _userManager.GetUserAsync(User);
            if(companyApp == null)
                return NotFound();
            var category = categoryRequest.Adapt<Category>();
            category.ApplicationUserId = companyApp.Id;
            await _categoryRepository.CreateAsync(category);
            return Created();
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> Get()
        {
            var companyUser = await _userManager.GetUserAsync(User);
            if(companyUser == null)
                return NotFound();
            var categories = _categoryRepository.Get(expression: e => e.ApplicationUserId == companyUser.Id, includes: [e=>e.ApplicationCompany]);
            return Ok(categories.Adapt<IEnumerable<CategoryResponse>>());
        }
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var companyUser = await _userManager.GetUserAsync(User);
            if (companyUser == null)
                return NotFound();
            var category = _categoryRepository.GetOne(expression: e=>e.Id == id);
            if (category == null)
                return NotFound();
            await _categoryRepository.DeleteAsync(category);
            return NoContent();
        }
    }
}
