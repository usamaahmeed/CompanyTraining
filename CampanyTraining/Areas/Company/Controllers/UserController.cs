using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Stripe.Checkout;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace CompanyTraining.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Company")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtOptions _jwtOptions;

        public UserController(UserManager<ApplicationUser> userManager, JwtOptions jwtOptions)
        {
            this._userManager = userManager;
            this._jwtOptions = jwtOptions;
        }

        private bool IsValidFile(IFormFile formFile) => (formFile != null && formFile.Length > 0);

        private async Task<string> GenerateToken(ApplicationUser user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>
              {
                   new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                   new Claim(ClaimTypes.NameIdentifier, user.Id),
                   new Claim(JwtRegisteredClaimNames.Email, user.Email),
                   new Claim(JwtRegisteredClaimNames.Name, user.UserName), 
              };
            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
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

        private async Task HandleFileUploadAndUpdateUser(ApplicationUser applicationUser, IFormFile mainImgFile)
        {
            if (IsValidFile(mainImgFile))
            {
                var mainImgFileName = await SaveFileAsync(mainImgFile, "Images/company/mainimgs");
                applicationUser.MainImg = mainImgFileName;
            }
            await _userManager.UpdateAsync(applicationUser);
        }

        [HttpPost("CreateUser")]
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
                    company_id = applicationUser.CompanyId,
                    Role = role,
                },
            });
        }

        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            // الحصول على الشركة المسجّلة حالياً
            var currentCompany = await _userManager.GetUserAsync(User);
            if (currentCompany == null)
                return NotFound(new { message = "Authenticated company not found." });

            // جلب جميع المستخدمين الذين لديهم نفس CompanyId
            var users = await _userManager.Users
                .Where(u => u.CompanyId == currentCompany.Id)
                .ToListAsync();

            if (!users.Any())
                return NotFound(new { message = "No users found for this company." });

            // تجهيز البيانات للإرجاع
            var userResponses = users.Select(user => new
            {
                user.Id,
                user.Email,
                user.UserName,
                user.MainImg,
                user.Address,
                user.CompanyId,
            });

            return Ok(new
            {
                Message = "Users retrieved successfully",
                Success = true,
                Data = userResponses
            });
        }


        [HttpDelete("DeleteUser/{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound(new { message = "User not found" });
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            return Ok(new { message = "User deleted successfully" });
        }


    }
} 

