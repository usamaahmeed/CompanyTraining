using CompanyTraining.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Stripe;
using Stripe.Checkout;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace CompanyTraining.Areas.Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly JwtOptions _jwtOptions;
        private readonly ISubscribeRepository _subscribeRepository;
        private readonly IPackageRepository _packageRepository;
        private readonly IEmailSender _emailSender;

        public AccountsController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext applicationDbContext,
            JwtOptions jwtOptions, ISubscribeRepository subscribeRepository, 
            IPackageRepository packageRepository,IEmailSender emailSender)

        {
            _userManager = userManager;
            _signInManager = signInManager;
            _applicationDbContext = applicationDbContext;
            _jwtOptions = jwtOptions;
            _subscribeRepository = subscribeRepository;
            _packageRepository = packageRepository;
            this._emailSender = emailSender;
        }
        private async Task<Session> CreateStripeSession(Package package)
        {
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = $"{Request.Scheme}://{Request.Host}/payment/Success",
                CancelUrl = $"{Request.Scheme}://{Request.Host}/payment/Cancel",
            };

            options.LineItems.Add
                (
                   new SessionLineItemOptions()
                   {
                       PriceData = new SessionLineItemPriceDataOptions()
                       {
                           Currency = "egp",
                           ProductData = new SessionLineItemPriceDataProductDataOptions
                           {
                               Description = package.Description,
                               Name = package.Name,
                           },
                           UnitAmount = (long)package.Price * 100,

                       },
                       Quantity = 1
                   }
                );

            var service = new SessionService();
            return await service.CreateAsync(options);
        }

        [HttpPost("RegisterCompany")]
        public async Task<IActionResult> RegisterCompany([FromForm] RegisterDTO registerDTO)
        {
            string subject = "Welcome On Board";

            if (registerDTO.Role != "Company")
                return BadRequest("Invalid role for this endpoint.");

            using var transaction = _applicationDbContext.Database.BeginTransaction();
            try
            {
                var package = _packageRepository.GetOne(expression: e => e.Id == registerDTO.PackageId);
                var applicationUser = registerDTO.Adapt<ApplicationUser>();

                var existedUser = await _userManager.FindByEmailAsync(applicationUser.Email!);

                if (existedUser != null)
                    return BadRequest("Sorry Something is wrong");

                var result = await _userManager.CreateAsync(applicationUser, registerDTO.Password);
                if (!result.Succeeded || package == null)
                    return BadRequest(result.Errors);

                await Helper.HandleFileUploadAndUpdateUser(applicationUser, registerDTO.MainImgFile,_userManager);
                await _userManager.AddToRoleAsync(applicationUser, "Company");

                var subscribe = new Subscribe
                {
                    ApplicationUserId = applicationUser.Id,
                    PackageId = package.Id,
                    SubscriptionStartDate = DateTime.UtcNow,
                    SubscriptionEndDate = DateTime.Today.AddDays(package.DurationDay),
                };

                var session = await CreateStripeSession(package);
                subscribe.SessionId = session.Id;

                await _subscribeRepository.CreateAsync(subscribe);
                await _subscribeRepository.CommitAsync();

                await _signInManager.SignInAsync(applicationUser, false);
                var roles = await _userManager.GetRolesAsync(applicationUser);
                var role = roles.FirstOrDefault();
                string emailTemplete = EmailTemplete.WelcomeEmailBody(applicationUser.UserName!, Request.Host, Request.Scheme);
                await _emailSender.SendEmailAsync(applicationUser.Email!, subject, emailTemplete);
                await transaction.CommitAsync();
                return Ok(new
                {
                    Message = "Company Created Successfully",
                    Success = true,
                    Data = new
                    {
                        Token = await Helper.GenerateToken(applicationUser,_userManager,_jwtOptions),
                       applicationUser.Id,
                       applicationUser.Email,
                       user_name= applicationUser.UserName,
                        applicationUser.Address,
                        main_img=applicationUser.MainImg,
                        Role = role,
                        Stripe = session.Url
                    },
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginRequestDto)

        {
            var user = await _userManager.FindByEmailAsync(loginRequestDto.Email);

            if (user == null) return NotFound(new { Message = "User not found" });

            if (user.Email != loginRequestDto.Email || !await _userManager.CheckPasswordAsync(user, loginRequestDto.Password))
                return Unauthorized(new { Message = "Invalid email or password" });
            if (!await _userManager.IsInRoleAsync(user, loginRequestDto.Role))
                return Unauthorized(new { Message = "User is not found" });

              var result = await _userManager.CheckPasswordAsync(user,loginRequestDto.Password);
            if (result)
            {
                await _signInManager.SignInAsync(user, isPersistent: true);
                if (loginRequestDto.Role == "Company")
                {
                    return Ok(new
                    {
                        Message = "Login Successfully",
                        Success = true,
                        Data = new
                        {
                            Token = await Helper.GenerateToken(user,_userManager,_jwtOptions),
                            id = user.Id,
                            email = user.Email,
                            user_name = user.UserName,
                            address = user.Address,
                            main_img = user.MainImg,
                            role = loginRequestDto.Role
                        }
                    });
                }
                else if (loginRequestDto.Role == "User")
                {
                    return Ok(new
                    {
                        Message = "Login Successfully",
                        Success = true,
                        Data = new
                        {
                            Token = await Helper.GenerateToken(user, _userManager, _jwtOptions),
                            id = user.Id,
                            company_id = user.CompanyId,
                            email = user.Email,
                            user_name = user.UserName,
                            address = user.Address,
                            main_img = user.MainImg,
                            role = loginRequestDto.Role
                        }
                    });
                }
                else
                {

                    return Ok(new
                    {
                        Message = "Login Successfully",
                        Success = true,
                        Data = new
                        {
                            Token = await Helper.GenerateToken(user, _userManager, _jwtOptions),
                            id = user.Id,
                            email = user.Email,
                            user_name = user.UserName,
                            address = user.Address,
                            main_img = user.MainImg,
                            role = loginRequestDto.Role
                        }
                    });
                }
            }
            return NotFound();
        }
      


        [HttpGet("Logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var user = _userManager.GetUserId(User);
            if (user == null)
                return NotFound();
            await _signInManager.SignOutAsync();
            return NoContent();
        }


        [HttpGet("Profile")]
        [Authorize]
        public async Task<IActionResult> GetProfileInfo()
        {
            var appUser = await _userManager.GetUserAsync(User);
            if (appUser == null)
                return NotFound();
            var profile = appUser.Adapt<ProfileResponse>();

            return Ok(new
            {
                Message = "Profile Loaded Successfully",
                Success = true,
                Data = new
                {
                    id = profile.Id,
                    email = profile.Email,
                    user_name = profile.UserName,
                    address = profile.Address,
                    main_img = profile.MainImg,
                }

            }

           );
        }

        [HttpPut("EditProfile")]
        [Authorize]
        public async Task<IActionResult> EditProfile([FromForm] ProfileRequest profileRequest)
        {
            bool isUpdated = false;
            var userApp = await _userManager.GetUserAsync(User);
            if (userApp == null)
                return NotFound();
            if (profileRequest.UserName != null)
            {
                userApp.UserName = profileRequest.UserName;
                isUpdated = true;
            }

            if (profileRequest.Address != null)
            {
                userApp.Address = profileRequest.Address;
                isUpdated = true;
            }
            if (profileRequest.Email != null)
            {
                userApp.Email = profileRequest.Email;
                isUpdated = true;
            }
            if (Helper.IsValidFile(profileRequest.MainImgFile!))
            {
                if (!string.IsNullOrEmpty(userApp.MainImg))
                {
                    var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "Images/company/mainimgs", userApp.MainImg);
                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }
                }
                userApp.MainImg = await Helper.SaveFileAsync(profileRequest.MainImgFile!, "Images/company/mainimgs");
                isUpdated = true;
            }
            if (isUpdated)
            {
                await _userManager.UpdateAsync(userApp);
                return Ok(new
                {
                    Message = "Profile Is Updated Successfully",
                    Success = true,
                    Data = new
                    {
                        id = userApp.Id,
                        email = userApp.Email,
                        user_name = userApp.UserName,
                        address = userApp.Address,
                        main_img = userApp.MainImg,
                    }

                });
            }
            return NoContent();
        }

        [HttpPut("ChangePassword")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO changePasswordDTO)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user is not null)
            {

                var result = await _userManager.ChangePasswordAsync(user, changePasswordDTO.OldPassword, changePasswordDTO.NewPassword);

                if (result.Succeeded)
                {
                    // Success Register
                    await _signInManager.SignInAsync(user, false);
                    return NoContent();
                }

                return BadRequest(result.Errors);

            }

            return NotFound();
        }
    }
}
