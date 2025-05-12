using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CompanyTraining.Areas.Admin.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ISubscribeRepository _subscribeRepository;
        private readonly IUserRepository _userRepository;

       private void RemoveImageForCompany(string imgName)
        {
            var oldPath = Path.Combine(Directory.GetCurrentDirectory(),"Images/company/mainimgs",imgName);
            if (System.IO.File.Exists(oldPath))
            {
                System.IO.File.Delete(oldPath);
            }
        }
        private void RemoveImagesRangeForEmplyess(IEnumerable<ApplicationUser> applicationUsers)
        {
            foreach (var employee in applicationUsers) 
            {
                var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "Images/employees/mainimgs", employee.MainImg!);
                if (System.IO.File.Exists(oldPath))
                {
                    System.IO.File.Delete(oldPath);
                }
            }
        }
        public CompanyController(IUserRepository userRepository,ISubscribeRepository subscribeRepository,IUserRepository _userRepository,UserManager<ApplicationUser> userManager, ApplicationDbContext applicationDbContext )
        {
            this._subscribeRepository = subscribeRepository;
            this._userRepository = _userRepository;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var subscriptions = _subscribeRepository.Get(includes: [
                    e => e.Package,
                    e => e.ApplicationCompany
                  ]).ToList();
            var data = subscriptions.Adapt<IEnumerable<CompanyResponse>>();
            return Ok(new
            {
                Message = "تم جلب الشركات بنجاح",
                Success = true,
                Data = data
            });
        }
        [HttpDelete("DeleteCompany/{id}")]
        public async Task<IActionResult> DeleteCompany([FromRoute] string id)
        {
            var company = _userRepository.GetOne(expression: e => e.Id== id, includes: [
                e=>e.Employees,
                ]);

            if (company == null)
                return NotFound(new { message = "الشركة غير موجودة" });

            if (company.Employees != null && company.Employees.Any())
            {
                _userRepository.RemoveRange(company.Employees);
                RemoveImagesRangeForEmplyess(company.Employees);
            }
            RemoveImageForCompany(company.MainImg!);
            await _userRepository.DeleteAsync(company);
                return Ok(new { 
                    message = "تم حذف الشركة والموظفين بنجاح",
                    success = true,
                });
        }

        [HttpGet("GetCount")]
        public IActionResult GetCompanyCount()
        {
            var companyCount = _subscribeRepository.Get().Count();
            return Ok(companyCount);
        }

        [HttpGet("GetRevenue")]
        public IActionResult GetRevenue()
        {
             var total_revenue = _subscribeRepository.Get(includes: [
                e=>e.Package,
                ]).Sum(e=>e.Package.Price);

            return Ok(new
            {
                Message = "Get Revenue Successfully",
                Success = true,
                Data = new
                {
                    total_revenue
                }
            });
          
        }
    }
}
