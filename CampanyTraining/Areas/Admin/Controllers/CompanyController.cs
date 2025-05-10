using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CompanyTraining.Areas.Admin.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ISubscribeRepository _subscribeRepository;

        public CompanyController(IUserRepository userRepository,ISubscribeRepository subscribeRepository)
        {
            this._subscribeRepository = subscribeRepository;
        }
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var subscribesWithComapny = _subscribeRepository.Get(includes: [
                e=>e.Package,
                e=>e.ApplicationCompany,
                ]);
             return Ok(new
            {
                Message = "Get company Successfully",
                Success = true,
                Data = subscribesWithComapny.Adapt<IEnumerable<CompanyResponse>>()

             });
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
