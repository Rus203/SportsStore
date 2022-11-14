using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SportsStore.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;


namespace SportsStore
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration) => Configuration = configuration;
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
                Configuration["Data:SportStoreProducts:ConnectionString"]));
            services.AddTransient<IProductRepository, EFProductRepository>();
            services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IOrderRepository, EFOrderRepository>();
            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddMemoryCache();
            services.AddSession();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseStatusCodePages();   // try to test this method later
            }
            
            app.UseStaticFiles();
                app.UseSession();
                app.UseMvc(routes =>
                {
                    routes.MapRoute(
                        name: null,
                        template: "{category}/page{productPage:int}",
                        defaults: new { controller = "Product", action = "List" }
                        );

                    routes.MapRoute(
                        name: null,
                        template: "page{productPage:int}",
                        defaults: new { controller = "Product", action = "List", productPage = 1 }
                        );

                    routes.MapRoute(
                      name: null,
                      template: "{category}",
                      defaults: new { controller = "Product", action = "List", productPage = 1 }
                      );

                    routes.MapRoute(
                      name: null,
                      template: "/",
                      defaults: new { controller = "Product", action = "List", productPage = 1 }
                      );

                    routes.MapRoute(name: null, template: "{controller}/{action}/{id?}");
                });

                app.UseRouting();
                SeedData.EnsurePopulated(app);
        }
    }
}
