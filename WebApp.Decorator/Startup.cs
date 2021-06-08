using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseProject.Models;
using BaseProject.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WebApp.Decorator.Repositories.Decorator;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

namespace BaseProject
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();

            //2. yol Scrutor kütüphanesi ile decorator pattern kullanımı.
            services.AddScoped<IProductRepository, ProductRepository>()
                .Decorate<IProductRepository, ProductRepositoryCacheDecorator>()
                .Decorate<IProductRepository, ProductRepositoryLoggingDecorator>();
            
            //1. yol
            /*services.AddScoped<IProductRepository>(sp =>
            {
                var context = sp.GetRequiredService<DataContext>();
                var memoryCache = sp.GetRequiredService<IMemoryCache>();
                var logService = sp.GetRequiredService<ILogger<ProductRepositoryLoggingDecorator>>();
                
                var productRepository = new ProductRepository(context);

                var cacheDecorator = new ProductRepositoryCacheDecorator(productRepository, memoryCache);
                var logDecorator = new ProductRepositoryLoggingDecorator(cacheDecorator, logService); //aynı zamanda hem loglama hem cacheleme yapıyor.
                
                return logDecorator;
            });*/
            
            //3. yol runtime zamanında kullanıcıya seçenek olarak log ve cache yaptırma.
            /*services.AddScoped<IProductRepository>(sp =>
            {
                var httpContextAccessor = sp.GetRequiredService<IHttpContextAccessor>();
                var context = sp.GetRequiredService<DataContext>();
                var memoryCache = sp.GetRequiredService<IMemoryCache>();
                var logService = sp.GetRequiredService<ILogger<ProductRepositoryLoggingDecorator>>();
                var productRepository = new ProductRepository(context);
                
                //user1 kullanıcısı giriş yaptıysa sadece cache verilerini görebilir değilse sadece loglama çalışır.
                if (httpContextAccessor.HttpContext.User.Identity.Name == "user1")
                {
                    var cacheDecorator = new ProductRepositoryCacheDecorator(productRepository, memoryCache);
                    return cacheDecorator;
                }
                
                var logDecorator = new ProductRepositoryLoggingDecorator(productRepository, logService);
                
                return logDecorator;
            });*/
            
            services.AddDbContext<DataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("SqlServer")));

            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<DataContext>();
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}