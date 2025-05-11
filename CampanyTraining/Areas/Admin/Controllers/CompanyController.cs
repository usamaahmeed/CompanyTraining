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
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _applicationDbContext;

        public CompanyController(IUserRepository userRepository,ISubscribeRepository subscribeRepository , UserManager<ApplicationUser> userManager, ApplicationDbContext applicationDbContext )
        {
            this._subscribeRepository = subscribeRepository;
            _userManager = userManager;
            _applicationDbContext = applicationDbContext;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var subscriptions = _subscribeRepository.Get(includes: [
                e => e.Package,
        e => e.ApplicationCompany
            ]).ToList();

            var companyResponses = subscriptions
                .Where(sub => sub.ApplicationCompany != null)
                .Select(sub => new CompanyResponse
                {
                    Id = sub.ApplicationCompany!.Id,
                    UserName = sub.ApplicationCompany.UserName ?? string.Empty,
                    PackageName = sub.Package?.Name ?? string.Empty,
                    SubscriptionStartDate = sub.SubscriptionStartDate,
                    SubscriptionEndDate = sub.SubscriptionEndDate
                })
                .DistinctBy(c => c.Id)
                .ToList();

            return Ok(new
            {
                Message = "تم جلب الشركات بنجاح",
                Success = true,
                Data = companyResponses
            });
        }


        [HttpDelete("DeleteCompany/{id}")]
        public async Task<IActionResult> DeleteCompany([FromRoute] string id)
        {
            var company = await _applicationDbContext.Users
                .Include(u => u.Employees)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (company == null)
                return NotFound(new { message = "الشركة غير موجودة" });

            if (company.Employees != null && company.Employees.Any())
            {
                _applicationDbContext.Users.RemoveRange(company.Employees);
            }

            _applicationDbContext.Users.Remove(company);

            try
            {
                await _applicationDbContext.SaveChangesAsync();
                return Ok(new { 
                    message = "تم حذف الشركة والموظفين بنجاح",
                    success = true,

                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "حدث خطأ أثناء الحذف", error = ex.Message });
            }
        }



 





        //[HttpGet("GetAll")]
        //public IActionResult GetAll()
        //{
        //    var subscribesWithComapny = _subscribeRepository.Get(includes: [
        //        e=>e.Package,
        //        e=>e.ApplicationCompany,
        //        ]);

        //    var data = subscribesWithComapny.Adapt<IEnumerable<CompanyResponse>>();
        //     return Ok(new
        //    {
        //        Message = "Get company Successfully",
        //        Success = true,
        //        Data = new
        //        {
        //            id = subscribesWithComapny,
        //            data
        //        }

        //     });
        //}


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
