using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using core2Bowling.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace core2Bowling
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
            services.AddDbContext<BowlingContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));


            services.AddDbContext<AccountContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("UserAccConnection")));

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminGroup", policy => policy.RequireClaim("Role", "Manager", "Admin"));
            });


            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
             .AddCookie(options => {
                 options.LoginPath = "/Login/UserLogin/";
                 options.AccessDeniedPath = "/Login/Forbidden/";
             });



            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Games}/{action=Index}/{id?}");
            });
        }
    }
}
