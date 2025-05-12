using CompanyTraining.DTOs.Request;
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

        private bool isUpdated(CategoryRequest categoryRequest, Category category)
        {
            bool updated = false;
            if (categoryRequest.Name != null)
            {
                category.Name = categoryRequest.Name;
                updated = true;
            }
           
            return updated;
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

            return Ok(new
            {
                Message = "Category Created Successfully",
                Success = true,
                Data = category.Adapt<CategoryResponse>(),
            });
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> Get()
        {
            var companyUser = await _userManager.GetUserAsync(User);
            if (companyUser == null)
                return NotFound();
            var categories = _categoryRepository.Get(expression: e => e.ApplicationUserId == companyUser.Id, includes: [e => e.ApplicationCompany, e => e.Courses]);

            return Ok(new
            {
                Message = "Category Return Successfully",
                Success = true,
                data = categories.Adapt<IEnumerable<CategoryResponse>>()

            });
        }
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] CategoryRequest categoryRequest)
        {
            var companyUser = await _userManager.GetUserAsync(User);
            if (companyUser == null)
                return NotFound();
            var categoryInDb = _categoryRepository.GetOne(expression: e => e.Id == id);
            if (categoryInDb == null)
                return NotFound();

            if (isUpdated(categoryRequest, categoryInDb))
            {
                categoryInDb.Id = id;
                await _categoryRepository.CommitAsync();
            }
            return Ok(new
            {
                Message = "Packages Update Successfully",
                Success = true,
                Data = categoryInDb.Adapt<CategoryRequest>()

            });
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var companyUser = await _userManager.GetUserAsync(User);
            if (companyUser == null)
                return NotFound();
            var category = _categoryRepository.GetOne(expression: e=>e.Id == id);
            if (category == null)
                return NotFound();
            await _categoryRepository.DeleteAsync(category);
            return Ok(new
            {
                Message = "Category Deleted Successfully",
                Success = true,
            });
        }
    }
}
