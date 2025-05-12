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
        private readonly IUserRepository _userRepository;
        private readonly JwtOptions _jwtOptions;

        public UserController(UserManager<ApplicationUser> userManager,IUserRepository userRepository,JwtOptions jwtOptions)
        {
            this._userManager = userManager;
            this._userRepository = userRepository;
            this._jwtOptions = jwtOptions;
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

            var existedUser = await _userManager.FindByEmailAsync(applicationUser.Email!);
            if (existedUser != null)
                return BadRequest("Choose Another Email");

            var result = await _userManager.CreateAsync(applicationUser, registerDTO.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            await Helper.HandleFileUploadAndUpdateUser(applicationUser, registerDTO.MainImgFile, _userManager);
            await _userManager.AddToRoleAsync(applicationUser, "User");

            var roles = await _userManager.GetRolesAsync(applicationUser);
            var role = roles.FirstOrDefault();

            return Ok(new
            {
                Message = "User Created Successfully",
                Success = true,
                Data = new
                {
                    Token = await Helper.GenerateToken(applicationUser,_userManager,_jwtOptions),
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
            var users = _userRepository.Get(expression: e => e.CompanyId == currentCompany.Id, includes: [
                e=>e.Employees,
                ]);

            if (!users.Any())
                return NotFound(new { message = "No users found for this company." });

            var userResponses = users.Adapt<IEnumerable<EmployeeUserResponse>>();

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

