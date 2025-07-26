using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Secretaria.Dominio.Models;

namespace Secretaria.Infra.Initiazer
{
    public static class DbInitializer
    {
        public static async Task SeedRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new ApplicationRole { Name = "Admin" });
            }
        }
    }
}
