using CompanyTraining.DTOs.Request;
using CompanyTraining.Models;
using CompanyTraining.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.IdentityModel.Tokens;
using Stripe.Checkout;
using Stripe.Climate;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CompanyTraining.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<ApplicationCompany> _userManager;
        private readonly SignInManager<ApplicationCompany> _signInManager;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly JwtOptions _jwtOptions;
        private readonly ISubscribeRepository _subscribeRepository;
        private readonly IPackageRepository _packageRepository;

        public AccountsController(UserManager<ApplicationCompany> userManager,
            SignInManager<ApplicationCompany> signInManager,
            ApplicationDbContext applicationDbContext,
            JwtOptions jwtOptions, ISubscribeRepository subscribeRepository, IPackageRepository packageRepository)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._applicationDbContext = applicationDbContext;
            this._jwtOptions = jwtOptions;
            this._subscribeRepository = subscribeRepository;
            this._packageRepository = packageRepository;
        }

        private bool IsValidFile(IFormFile formFile) => (formFile != null && formFile.Length > 0);

        private async Task<string> GenerateToken(ApplicationCompany user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>
              {
                   new Claim(ClaimTypes.NameIdentifier, user.Id),
                   new Claim(ClaimTypes.Email, user.Email),
              };
            foreach (var role in userRoles) {
                claims.Add(new Claim(ClaimTypes.Role,role));
            }
            // إنشاء التوكن
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _jwtOptions.Issuer,
                Audience = _jwtOptions.Audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key)),
                    SecurityAlgorithms.HmacSha256
                ),
                Subject = new ClaimsIdentity(claims)
            };

            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(securityToken);
        }
        private async Task<string> SaveFileAsync(IFormFile file, string folderPath)
        {
            var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folderPath);

            // Ensure the directory exists
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(directoryPath, fileName);

            using (var stream = System.IO.File.Create(filePath))
            {
                await file.CopyToAsync(stream);
            }

            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var imageUrl = $"{baseUrl}/{folderPath}/{fileName}";
            return imageUrl; // Return the full URL
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

        [HttpPost("Register")]
        //public async Task<IActionResult> Register([FromForm] RegisterDTO registerDTO)
        //{
        //    ApplicationCompany ApplicationCompany = registerDTO.Adapt<ApplicationCompany>();

        //    var package = _packageRepository.GetOne(expression: e => e.Id == registerDTO.PackageId);
        //    var result = await _userManager.CreateAsync(ApplicationCompany, registerDTO.Password);

        //    if (!result.Succeeded || package == null)
        //        return BadRequest(result.Errors);
        //    if (IsValidFile(registerDTO.MainImgFile) && IsValidFile(registerDTO.CoverImgFile))
        //    {
        //        var mainImgFileName = await SaveFileAsync(registerDTO.MainImgFile, "images/company/mainimgs");
        //        var coverImgFileName = await SaveFileAsync(registerDTO.CoverImgFile, "images/company/coverimgs");


        //        ApplicationCompany.MainImg = mainImgFileName;

        //        ApplicationCompany.CoverImg = coverImgFileName;

        //        await _userManager.UpdateAsync(ApplicationCompany);
        //    }

        //    var options = new SessionCreateOptions
        //    {
        //        PaymentMethodTypes = new List<string> { "card" },
        //        LineItems = new List<SessionLineItemOptions>(),
        //        Mode = "payment",
        //    };



        //    var subscribe = new Subscribe
        //    {
        //        ApplicationCompanyId = ApplicationCompany.Id,
        //        PackageId = package.Id,
        //        SubscriptionStartDate = DateTime.UtcNow,
        //        SubscriptionEndDate = DateTime.Today.AddDays(package.DurationDay),
        //    };

        //    options.LineItems.Add
        //        (
        //           new SessionLineItemOptions()
        //           {
        //               PriceData = new SessionLineItemPriceDataOptions()
        //               {
        //                   Currency = "egp",
        //                   ProductData = new SessionLineItemPriceDataProductDataOptions
        //                   {
        //                       Description = package.Description,
        //                       Name = package.Name,
        //                   },
        //                   UnitAmount = (long) package.Price * 100
        //               },
        //           }
        //        );

        //    var service = new SessionService();
        //    var session = service.Create(options);
        //    subscribe.SessionId = session.Id;

        //    await _signInManager.SignInAsync(ApplicationCompany, false);
        //    await _userManager.AddToRoleAsync(ApplicationCompany, "Company");

        //    return Ok(new
        //    {

        //        Message = "User Created Successfully",
        //        Success = true,
        //        Data = new
        //        {
        //            Token = GenerateToken(ApplicationCompany),
        //            ApplicationCompany.Id,
        //            ApplicationCompany.Email,
        //            ApplicationCompany.UserName,
        //            ApplicationCompany.Address,
        //            ApplicationCompany.MainImg,
        //            ApplicationCompany.CoverImg,
        //        }
        //    });
        //}
        public async Task<IActionResult> Register([FromForm] RegisterDTO registerDTO)
        {
            ApplicationCompany ApplicationCompany = registerDTO.Adapt<ApplicationCompany>();

            var package = _packageRepository.GetOne(expression: e => e.Id == registerDTO.PackageId);
            var result = await _userManager.CreateAsync(ApplicationCompany, registerDTO.Password);
            using var transaction = _applicationDbContext.Database.BeginTransaction();
            try
            {
                if (!result.Succeeded || package == null)
                    return BadRequest(result.Errors);
                if (IsValidFile(registerDTO.MainImgFile) && IsValidFile(registerDTO.CoverImgFile))
                {
                    var mainImgFileName = await SaveFileAsync(registerDTO.MainImgFile, "images/company/mainimgs");
                    var coverImgFileName = await SaveFileAsync(registerDTO.CoverImgFile, "images/company/coverimgs");


                    ApplicationCompany.MainImg = mainImgFileName;

                    ApplicationCompany.CoverImg = coverImgFileName;

                    await _userManager.UpdateAsync(ApplicationCompany);
                }

                var subscribe = new Subscribe
                {
                    ApplicationCompanyId = ApplicationCompany.Id,
                    PackageId = package.Id,
                    SubscriptionStartDate = DateTime.UtcNow,
                    SubscriptionEndDate = DateTime.Today.AddDays(package.DurationDay),
                };
                var session = await CreateStripeSession(package);
                subscribe.SessionId = session.Id;
                await _subscribeRepository.CreateAsync(subscribe);
                await _subscribeRepository.CommitAsync();
                await _signInManager.SignInAsync(ApplicationCompany, false);
                await _userManager.AddToRoleAsync(ApplicationCompany, "Company");
                await  transaction.CommitAsync();
                return Ok(new
                {

                    Message = "User Created Successfully",
                    Success = true,
                    Data = new
                    {
                        Token = await GenerateToken(ApplicationCompany),
                        ApplicationCompany.Id,
                        ApplicationCompany.Email,
                        ApplicationCompany.UserName,
                        ApplicationCompany.Address,
                        ApplicationCompany.MainImg,
                        ApplicationCompany.CoverImg,
                    },
                    session.Url
                });
            }catch(Exception ex)
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

            await _signInManager.SignInAsync(user, loginRequestDto.RememberMe);

            return Ok(new
            {

                Message = "Login Successfully",
                Success = true,
                Data = new
                {
                    Token = GenerateToken(user),
                    user.Id,
                    user.Email,
                    user.UserName,
                    user.Address,
                    user.MainImg,
                    user.CoverImg,
                }
            });
        }
        //[HttpPost("Login")]
        //public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        //{
        //    var appUser = await _userManager.FindByEmailAsync(loginDTO.Email);

        //    if (appUser != null)
        //    {
        //        var result = await _userManager.CheckPasswordAsync(appUser, loginDTO.Password);

        //        if (result)
        //        {
        //            // Login
        //            await _signInManager.SignInAsync(appUser, loginDTO.RememberMe);

        //            return NoContent();
        //        }
        //        else
        //        {
        //            ModelStateDictionary keyValuePairs = new();
        //            keyValuePairs.AddModelError("Error", "Invalid user name or password");
        //            return BadRequest(keyValuePairs);
        //        }
        //    }

        //    return NotFound();




        [HttpGet("Logout")]
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
                Data = profile,

            }

           );
        }

        [HttpPut("EditProfile")]
        public async Task<IActionResult> EditProfile([FromForm] ProfileRequest profileRequest)
        {
            bool isUpdated = false;
            var userApp = await _userManager.GetUserAsync(User);
            if (userApp == null)
                return NotFound();
            if (profileRequest.CompanyName != null)
            {
                userApp.UserName = profileRequest.CompanyName;
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
            if (IsValidFile(profileRequest.MainImgFile))
            {
                if (!string.IsNullOrEmpty(userApp.MainImg))
                {
                    var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "images/company/mainimgs", userApp.MainImg);
                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }
                }
                userApp.MainImg = await SaveFileAsync(profileRequest.MainImgFile, "images/company/mainimgs");
                isUpdated = true;
            }

            if (IsValidFile(profileRequest.CoverImgFile))
            {
                if (!string.IsNullOrEmpty(userApp.CoverImg))
                {
                    var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "images/company/coverimgs", userApp.CoverImg);
                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }
                }
                userApp.CoverImg = await SaveFileAsync(profileRequest.CoverImgFile, "images/company/coverimgs");
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
                        userApp.Id,
                        userApp.Email,
                        userApp.UserName,
                        userApp.Address,
                        userApp.MainImg,
                        userApp.CoverImg,
                    }
                });
            }
            return NoContent();
        }

        [HttpPut("ChangePassword")]
        [Authorize()]

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
