using System.Linq;
using System.Threading.Tasks;
using BaseProject.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BaseProject
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using var scope = host.Services.CreateScope();

            var identityDbContext = scope.ServiceProvider.GetRequiredService<DataContext>();

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            
            await identityDbContext.Database.MigrateAsync();

            if (!userManager.Users.Any())
            {
                await userManager.CreateAsync(new AppUser
                {
                    UserName = "user1",
                    Email = "user1@test.com"
                },"Password12*");
                await userManager.CreateAsync(new AppUser
                {
                    UserName = "user2",
                    Email = "user2@test.com"
                },"Password12*");
                await userManager.CreateAsync(new AppUser
                {
                    UserName = "user3",
                    Email = "user3@test.com"
                },"Password12*");
                await userManager.CreateAsync(new AppUser
                {
                    UserName = "user4",
                    Email = "user4@test.com"
                },"Password12*");
                await userManager.CreateAsync(new AppUser
                {
                    UserName = "user5",
                    Email = "user5@test.com"
                },"Password12*");
            }
            
            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}