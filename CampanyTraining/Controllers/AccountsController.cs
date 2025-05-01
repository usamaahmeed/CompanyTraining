using CompanyTraining.DTOs.Request;
using CompanyTraining.Models;
using CompanyTraining.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.IdentityModel.Tokens;
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
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JwtOptions _jwtOptions;


        public AccountsController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, JwtOptions jwtOptions)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._jwtOptions = jwtOptions;
        }

        private bool IsValidFile(IFormFile formFile) => (formFile != null && formFile.Length > 0);

        private string GenerateToken(ApplicationUser user)
        {
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
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Email, user.Email)
                }),
            };

            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(securityToken);
        }

        private async Task<string> SaveFileAsync(IFormFile file,string folderPath)
        {
            var fileName = Guid.NewGuid().ToString()+Path.GetExtension(file.FileName);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), folderPath, fileName);
            using (var stream = System.IO.File.Create(filePath))
            {
                await file.CopyToAsync(stream);
            }
            return fileName;
        }
       

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromForm] RegisterDTO registerDTO)
        {
            ApplicationUser applicationUser = registerDTO.Adapt<ApplicationUser>();

            var result = await _userManager.CreateAsync(applicationUser, registerDTO.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);
            if (IsValidFile(registerDTO.MainImgFile) && IsValidFile(registerDTO.CoverImgFile))
            {
                var mainImgFileName =await SaveFileAsync(registerDTO.MainImgFile, "images/company/mainimgs");
                var coverImgFileName = await SaveFileAsync(registerDTO.CoverImgFile, "images/company/coverimgs");


                applicationUser.MainImg = mainImgFileName;

                applicationUser.CoverImg = coverImgFileName;

                await _userManager.UpdateAsync(applicationUser);
            }
            await _signInManager.SignInAsync(applicationUser, false);
            await _userManager.AddToRoleAsync(applicationUser, "Company");

            return Ok(new
            {
                Token = GenerateToken(applicationUser),
                User = new
                {
                    applicationUser.Id,
                    applicationUser.Email,
                    applicationUser.UserName,
                    applicationUser.MainImg,
                    applicationUser.CoverImg,
                }
            });
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
                Token = GenerateToken(user),
                User = new
                {
                    user.Id,
                    user.Email,
                    user.UserName
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
        [Authorize()]
        public async Task<IActionResult> GetProfileInfo()
        {
            var appUser = await _userManager.GetUserAsync(User);
            if(appUser == null )
                return NotFound();
            var profile = appUser.Adapt<ProfileResponse>();
            return Ok(profile);
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
