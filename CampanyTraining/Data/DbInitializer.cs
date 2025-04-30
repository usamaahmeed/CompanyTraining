using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CompanyTraining.Data
{
    public class DbInitializer
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> identityRole;

        public DbInitializer(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> identityRole)
        {
            this.userManager = userManager;
            this.identityRole = identityRole;
        }
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            // Create a new service scope (since we're outside the HTTP request lifecycle)
            using var scope = serviceProvider.CreateScope();

            // Get the database context (used to apply migrations)
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // Get RoleManager to create and manage roles
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Get UserManager to create and manage users of type ApplicationUser
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // Apply any pending migrations to the database
            await context.Database.MigrateAsync();

            // List of default roles we want to ensure exist
            var roles = new[] { "SuperAdmin", "Admin", "Company", "User" };

            // Check if each role exists; if not, create it
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Define the default admin's email and password
            var adminEmail = "admin@system.com";
            var adminPassword = "Admin@123!"; // Must meet Identity password policy

            // Check if the admin user already exists by email
            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                // Create a new ApplicationUser object
                var adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true, // You can set this to false if you want email confirmation
                };

                // Try to create the admin user with the provided password
                var result = await userManager.CreateAsync(adminUser, adminPassword);

                // If creation is successful, assign the user to the Admin role
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "SuperAdmin");
                }
                else
                {
                    // Log any errors that occurred during user creation
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"Error creating admin user: {error.Description}");
                    }
                }
            }
        }
    }
}
