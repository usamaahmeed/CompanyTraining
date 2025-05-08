using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CompanyTraining.Areas.Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ISubscribeRepository _subscribeRepository;

        public CompanyController(IUserRepository userRepository,ISubscribeRepository subscribeRepository)
        {
            this._subscribeRepository = subscribeRepository;
        }
        [HttpGet("Get")]
        public IActionResult Get()
        {
            var subscribesWithComapny = _subscribeRepository.Get(includes: [
                e=>e.Package,
                e=>e.ApplicationCompany,
                ]);
            return Ok(subscribesWithComapny.Adapt<IEnumerable<CompanyResponse>>());
        }

        [HttpGet("GetRevenue")]
        public IActionResult GetRevenue()
        {
             var totalRevenue = _subscribeRepository.Get(includes: [
                e=>e.Package,
                ]).Sum(e=>e.Package.Price);
            return Ok(totalRevenue);
        }
    }
}
