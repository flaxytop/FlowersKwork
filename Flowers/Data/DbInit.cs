using Flowers.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.PowerBI.Api.Models;
using System.Security.Claims;

namespace Flowers.Data
{
    public class DbInit
    {
        public static async Task SetupIdentityAdmin(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Create admin role if it doesn't exist
            if (!await roleManager.RoleExistsAsync("admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }

            var user = await userManager.FindByEmailAsync("admin@gmail.com");
            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = "admin@gmail.com",
                    Email = "admin@gmail.com",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, "Admin@123");
                if (result.Succeeded)
                {
                    // Add user to admin role
                    await userManager.AddToRoleAsync(user, "admin");
                    
                    // Add admin claim
                    var claim = new Claim(ClaimTypes.Role, "admin");
                    await userManager.AddClaimAsync(user, claim);
                }
            }
        }
    }
}



    
