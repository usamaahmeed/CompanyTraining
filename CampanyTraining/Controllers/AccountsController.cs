
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


namespace CompanyTraining.Controllers
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

        public AccountsController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
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
        private async Task<string> GenerateToken(ApplicationUser user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>
              {
                   new Claim(JwtRegisteredClaimNames.Sub, user.Id), // Subject (user ID)
                   new Claim(ClaimTypes.NameIdentifier, user.Id),
                   new Claim(JwtRegisteredClaimNames.Email, user.Email),                  
                   new Claim(JwtRegisteredClaimNames.Name, user.UserName), // For User.Identity.Name
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
            var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), folderPath);

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

            var baseUrl = $"http://{Request.Host}";
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
        private async Task HandleFileUploadAndUpdateUser(ApplicationUser applicationUser, IFormFile mainImgFile)
        {
            if (IsValidFile(mainImgFile))
            {
                var mainImgFileName = await SaveFileAsync(mainImgFile, "Images/company/mainimgs");
                applicationUser.MainImg = mainImgFileName;
            }
            await _userManager.UpdateAsync(applicationUser);
        }

        // [HttpPost("Register")]
        // public async Task<IActionResult> Register([FromForm] RegisterDTO registerDTO)
        // {
        //     var allowedRoles = new List<string> { "Company", "User" };
        //     if (!allowedRoles.Contains(registerDTO.Role))
        //     {
        //         return BadRequest("Invalid role selected.");
        //     }

        //     else
        //     {
        //         ApplicationUser ApplicationUser = registerDTO.Adapt<ApplicationUser>();

        //         if (registerDTO.Role == "User")
        //         {
        //             var company = await _userManager.GetUserAsync(User);

        //             if(company==null)
        //                 return NotFound();

        //             ApplicationUser.CompanyId = company.Id;

        //             var result = await _userManager.CreateAsync(ApplicationUser, registerDTO.Password);

        //             if (!result.Succeeded)
        //                 return BadRequest(result.Errors);

        //              await HandleFileUploadAndUpdateUser(ApplicationUser,registerDTO.MainImgFile);

        //             await _userManager.AddToRoleAsync(ApplicationUser, "User");

        //             var roles = await _userManager.GetRolesAsync(ApplicationUser);
        //             var role = roles.FirstOrDefault();
        //             return Ok(new
        //             {
        //                 Message = "User Created Successfully",
        //                 Success = true,
        //                 Data = new
        //                 {
        //                     Token = await GenerateToken(ApplicationUser),
        //                     ApplicationUser.Id,
        //                     ApplicationUser.Email,
        //                     ApplicationUser.UserName,
        //                     ApplicationUser.Address,
        //                     ApplicationUser.MainImg,
        //                     Role = role,
        //                 },
        //             });
        //         }
        //         using var transaction = _applicationDbContext.Database.BeginTransaction();
        //         try
        //         {
        //             var package = _packageRepository.GetOne(expression: e => e.Id == registerDTO.PackageId);
        //             var result = await _userManager.CreateAsync(ApplicationUser, registerDTO.Password);
        //             if (!result.Succeeded || package == null)
        //                 return BadRequest(result.Errors);

        //             await HandleFileUploadAndUpdateUser(ApplicationUser, registerDTO.MainImgFile);


        //             await _userManager.AddToRoleAsync(ApplicationUser, "Company");

        //             var subscribe = new Subscribe
        //             {
        //                 ApplicationUserId = ApplicationUser.Id,
        //                 PackageId = package.Id,
        //                 SubscriptionStartDate = DateTime.UtcNow,
        //                 SubscriptionEndDate = DateTime.Today.AddDays(package.DurationDay),
        //             };

        //             var session = await CreateStripeSession(package);
        //             subscribe.SessionId = session.Id;
        //             await _subscribeRepository.CreateAsync(subscribe);
        //             await _subscribeRepository.CommitAsync();

        //             await _signInManager.SignInAsync(ApplicationUser, false);
        //             var roles = await _userManager.GetRolesAsync(ApplicationUser);
        //             var role = roles.FirstOrDefault();

        //             await transaction.CommitAsync();

        //             return Ok(new
        //             {
        //                 Message = "Company Created Successfully",
        //                 Success = true,
        //                 Data = new
        //                 {
        //                     Token = await GenerateToken(ApplicationUser),
        //                     ApplicationUser.Id,
        //                     ApplicationUser.Email,
        //                     ApplicationUser.UserName,
        //                     ApplicationUser.Address,
        //                     ApplicationUser.MainImg,
        //                     Role = role,
        //                     Stripe = session.Url
        //                 },
        //             });
        //         }
        //         catch (Exception ex)
        //         {
        //             await transaction.RollbackAsync();
        //             return BadRequest(ex.Message);
        //         }
        //     }

        //}
        [HttpPost("CreateUser")]
        [Authorize]

        public async Task<IActionResult> CreateUser([FromForm] UserRegisterDTO registerDTO)
        {
            if (registerDTO.Role != "User")
                return BadRequest("Invalid role.");

            var currentCompany = await _userManager.GetUserAsync(User);
            if (currentCompany == null)
                return NotFound("Authenticated company not found.");

            if (currentCompany.Id != registerDTO.CompanyId)
                return Unauthorized("You can only register users under your own company.");

            var applicationUser = registerDTO.Adapt<ApplicationUser>();
            applicationUser.CompanyId = registerDTO.CompanyId;

            var result = await _userManager.CreateAsync(applicationUser, registerDTO.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            await HandleFileUploadAndUpdateUser(applicationUser, registerDTO.MainImgFile);
            await _userManager.AddToRoleAsync(applicationUser, "User");

            var roles = await _userManager.GetRolesAsync(applicationUser);
            var role = roles.FirstOrDefault();

            return Ok(new
            {
                Message = "User Created Successfully",
                Success = true,
                Data = new
                {
                    Token = await GenerateToken(applicationUser),
                    applicationUser.Id,
                    applicationUser.Email,
                    user_name = applicationUser.UserName,
                    applicationUser.Address,
                    main_img = applicationUser.MainImg,
                    company_id=applicationUser.CompanyId,
                    Role = role,
                },
            });
        }


        [HttpPost("RegisterCompany")]
        public async Task<IActionResult> RegisterCompany([FromForm] RegisterDTO registerDTO)
        {
            if (registerDTO.Role != "Company")
                return BadRequest("Invalid role for this endpoint.");

            using var transaction = _applicationDbContext.Database.BeginTransaction();
            try
            {
                var package = _packageRepository.GetOne(expression: e => e.Id == registerDTO.PackageId);
                var applicationUser = registerDTO.Adapt<ApplicationUser>();

                var result = await _userManager.CreateAsync(applicationUser, registerDTO.Password);
                if (!result.Succeeded || package == null)
                    return BadRequest(result.Errors);

                await HandleFileUploadAndUpdateUser(applicationUser, registerDTO.MainImgFile);
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

                await transaction.CommitAsync();

                return Ok(new
                {
                    Message = "Company Created Successfully",
                    Success = true,
                    Data = new
                    {
                        Token = await GenerateToken(applicationUser),
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


            if (loginRequestDto.Role == "Company")
            {
                return Ok(new
                {
                    Message = "Login Successfully",
                    Success = true,
                    Data = new
                    {
                        Token = await GenerateToken(user),
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
                        Token = await GenerateToken(user),
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
                        Token = await GenerateToken(user),
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
                Data =  new
                {
                    id=  profile.Id,
                    email=profile.Email,
                    user_name= profile.UserName,
                    address=profile.Address,
                    main_img= profile.MainImg,
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
            if (IsValidFile(profileRequest.MainImgFile))
            {
                if (!string.IsNullOrEmpty(userApp.MainImg))
                {
                    var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "Images/company/mainimgs", userApp.MainImg);
                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }
                }
                userApp.MainImg = await SaveFileAsync(profileRequest.MainImgFile, "Images/company/mainimgs");
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
