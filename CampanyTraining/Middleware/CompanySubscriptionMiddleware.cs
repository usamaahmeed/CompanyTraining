using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace CompanyTraining.Middleware
{
    public class CompanySubscriptionMiddleware
    {
        private readonly RequestDelegate _next;

        public CompanySubscriptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context,ISubscribeRepository subscribeRepository, ICompanyRepository companyRepo, UserManager<ApplicationUser> userManager)
        {
            var path = context.Request.Path.ToString().ToLower();
            if (path.Contains("/login") || path.Contains("/register") || path.Contains("/upgrade"))
            {
                await _next(context);
                return;
            }

            var user = context.User;
            if (user.Identity?.IsAuthenticated == true)
            {
                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!string.IsNullOrEmpty(userId))
                {
                    var company = await companyRepo.GetCompanyByUserId(userId);
                    if (company != null)
                    {
                        var companyId = company.Id;

                        var subscripe = subscribeRepository.GetOne(e => e.ApplicationUserId == companyId);

                        if (subscripe != null && subscripe.SubscriptionEndDate < DateTime.Now)
                        {
                            context.Response.StatusCode = StatusCodes.Status403Forbidden;
                            await context.Response.WriteAsync("🚫 Your company's subscription has expired. Please contact your admin.");
                            return;
                        }
                    }
                }
            }

            await _next(context);
        }
    }
}
