using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Stripe.Checkout;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CompanyTraining.Utility
{
    public static class Helper
    {
        public static bool IsValidFile(IFormFile formFile) => (formFile != null && formFile.Length > 0);
        public static async Task<string> GenerateToken(ApplicationUser user,UserManager<ApplicationUser> userManager, JwtOptions jwtOptions)
        {
            var userRoles = await userManager.GetRolesAsync(user);
            var claims = new List<Claim>
              {
                   new Claim(JwtRegisteredClaimNames.Sub, user.Id), // Subject (user ID)
                   new Claim(ClaimTypes.NameIdentifier, user.Id),
                   new Claim(JwtRegisteredClaimNames.Email, user.Email),
                   new Claim(JwtRegisteredClaimNames.Name, user.UserName), // For User.Identity.Name
              };
            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            // إنشاء التوكن
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = jwtOptions.Issuer,
                Audience = jwtOptions.Audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key)),
                    SecurityAlgorithms.HmacSha256
                ),
                Subject = new ClaimsIdentity(claims)
            };

            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(securityToken);
        }
        public static async Task<string> SaveFileAsync(IFormFile file, string folderPath)
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

            //var baseUrl = $"http://{Request.Host}";
            //var imageUrl = $"{baseUrl}/{folderPath}/{fileName}";
            return fileName; // Return the full URL
        }
        public static async Task HandleFileUploadAndUpdateUser(ApplicationUser applicationUser, IFormFile mainImgFile,UserManager<ApplicationUser> userManager)
        {
            if (IsValidFile(mainImgFile))
            {
                var mainImgFileName = await SaveFileAsync(mainImgFile, "Images/company/mainimgs");
                applicationUser.MainImg = mainImgFileName;
            }
            await userManager.UpdateAsync(applicationUser);
        }
    }
}
