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
                var newUser = new AppUser
                {
                    UserName = "user1",
                    Email = "user1@test.com"
                };
                await userManager.CreateAsync(newUser,"Password12*");
                
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
                
                var newCategory1 = new Category { Name = "Suç romanları", ReferenceId = 0, UserId = newUser.Id };
                var newCategory2 = new Category { Name = "Cinayet romanları", ReferenceId = 0, UserId = newUser.Id };
                var newCategory3 = new Category { Name = "Polisiye romanları", ReferenceId = 0, UserId = newUser.Id };

                await identityDbContext.Categories.AddRangeAsync(newCategory1, newCategory2, newCategory3);

                await identityDbContext.SaveChangesAsync();

                var subCategory1 = new Category { Name = "Suç romanları 1", ReferenceId = newCategory1.Id, UserId = newUser.Id };
                var subCategory2 = new Category { Name = "Cinayet romanları 1", ReferenceId = newCategory2.Id, UserId = newUser.Id };
                var subCategory3 = new Category { Name = "Polisiye romanları 1", ReferenceId = newCategory3.Id, UserId = newUser.Id };

                await identityDbContext.Categories.AddRangeAsync(subCategory1, subCategory2, subCategory3);
                await identityDbContext.SaveChangesAsync();

                var subCategory4 = new Category { Name = "Cinayet romanları 1.1", ReferenceId = subCategory2.Id, UserId = newUser.Id };

                await identityDbContext.Categories.AddRangeAsync(subCategory4);
                await identityDbContext.SaveChangesAsync();
            }
            
            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}