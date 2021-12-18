using AgencyApp.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace AgencyApp
{
    public class RoleInitializer
    {
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            string adminEmail = "admin@gmail.com";
            string password = "123qwe";
            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }
            if (await roleManager.FindByNameAsync("agent") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("agent"));
            }
            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                User admin = new() { Email = adminEmail, UserName = adminEmail };
                IdentityResult result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");
                    await userManager.AddToRoleAsync(admin, "agent");
                }
            }
        }
    }
}