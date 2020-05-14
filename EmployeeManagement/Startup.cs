using EmployeeManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.CodeAnalysis.Options;
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
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 10;
                options.Password.RequiredUniqueChars = 3;

            }).AddEntityFrameworkStores<AppDbContext>();

            services.AddMvc(
                config =>
                {
                    //Add Filter policy of authorization
                    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                    config.Filters.Add(new AuthorizeFilter(policy));

                }).AddXmlDataContractSerializerFormatters();

            //Change AccessDeniedPath from Account/AccessDenied to Administration/AccessDenied
            services.ConfigureApplicationCookie(options =>
            {

                options.AccessDeniedPath = new PathString("/Administration/AccessDenied");
            });

            // Claims Policy authorization
            services.AddAuthorization(options =>
            {
                //As applied 'DeleteRolePolicy' => require two Claims ['Delete Role', 'Create Role']
                //options.AddPolicy("DeleteRolePolicy",
                //    policy => policy.RequireClaim("Delete Role").RequireClaim("Create Role"));

                options.AddPolicy("DeleteRolePolicy",
                   policy => policy.RequireAssertion(context => AuthorizeAccess(context, "Delete Role")));

            });

            // Roles Policy
            services.AddAuthorization(options =>
            {
                //options.AddPolicy("EditRolePolicy", policy => policy.RequireClaim("Edit Role", "true"));
                // Policy match rules ["Admin" & "Edit Role", "Super Admin"]
                options.AddPolicy("EditRolePolicy", policy => policy.RequireAssertion(context => AuthorizeAccess(context, "Edit Role")));
            });

            // Roles Policy
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminRolePolicy", policy => policy.RequireRole("Admin"));
            });

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

        // AuthorizeAccess
        private bool AuthorizeAccess(AuthorizationHandlerContext context, string policy) =>
            context.User.IsInRole("Admin") && context.User.HasClaim(claim => claim.Type == policy && claim.Value == "true")
            || context.User.IsInRole("Super Admin");

    }
}
