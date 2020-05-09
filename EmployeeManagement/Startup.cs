using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EmployeeManagement
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        private IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // DB set
            services.AddDbContextPool<AppDbContext>(options =>
                options.UseSqlServer(_config.GetConnectionString("EmployeeDBConnection")));

            //Identity Set #2
            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 10;
                options.Password.RequiredUniqueChars = 3;

            }).AddEntityFrameworkStores<AppDbContext>();

            services.AddMvc().AddXmlDataContractSerializerFormatters();

            // services.AddSingleton<IEmployeeRepository, MockEmployeeRepository>();

            // kept the same instance of SQLEmployeeRepository during one request 
            services.AddScoped<IEmployeeRepository, SQLEmployeeRepository>();

            // new instance each request
            // services.AddScoped<IEmployeeRepository, MockEmployeeRepository>();
            // new instance each render view
            // services.AddTransient<IEmployeeRepository, MockEmployeeRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //set Properties/launchSettings.json non 'Development'

                //default status code page
                //app.UseStatusCodePages();
                // StatusCode (302) redirect to Error/400 (200)
                // app.UseStatusCodePagesWithRedirects("/Error/{0}");
                // StatusCode 404
                app.UseStatusCodePagesWithReExecute("/Error/{0}");

                // Handle server exception
                app.UseExceptionHandler("/Error");
            }

            // StatusCode 404
            app.UseStatusCodePagesWithReExecute("/Error/{0}");

            // Handle server exception
            app.UseExceptionHandler("/Error");

            app.UseStaticFiles();
            //Identity Set #3
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
