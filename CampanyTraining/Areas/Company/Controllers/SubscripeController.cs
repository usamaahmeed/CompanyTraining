using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CompanyTraining.Areas.Company.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class SubscripeController : ControllerBase
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IPackageRepository _packageRepository;
        private readonly ISubscribeRepository _subscribeRepository;

        public SubscripeController(
            ICompanyRepository companyRepository,
            IPackageRepository packageRepository
            ,ISubscribeRepository subscribeRepository)
        {
            this._companyRepository = companyRepository;
            this._packageRepository = packageRepository;
            this._subscribeRepository = subscribeRepository;
        }


        [HttpPut("/Packages/{packageId}/Upgrade")]

        public async Task<IActionResult> UpgradeSubscription([FromRoute] int packageId)
        {
            
            var companyId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (companyId == null)
                return NotFound("Company not found.");

            var subscripe = _subscribeRepository.GetOne(e => e.ApplicationUserId == companyId);
            if (subscripe == null) return NotFound("Not Found this subscription");

            var package = _packageRepository.GetOne(e => e.Id == packageId);
            if (package == null) return NotFound("This package is not found");

            if (subscripe.SubscriptionEndDate < DateTime.Now)
                subscripe.SubscriptionEndDate = DateTime.Now.AddDays(package.DurationDay);
            else
                subscripe.SubscriptionEndDate = subscripe.SubscriptionEndDate.AddDays(package.DurationDay);

            await _companyRepository.CommitAsync();

            return Ok(new
            {
                message = "Subscription upgraded successfully.",
                newExpiry = subscripe.SubscriptionEndDate.ToString("yyyy-MM-dd")
            });
        }
    }
}
