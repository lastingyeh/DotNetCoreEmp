using EmployeeManagement.Models;
using EmployeeManagement.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Security.Claims;

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
            // See secret path (*EmployeeManagement.csproj)
            //< UserSecretsId > d7506c45 - 5a03 - 4443 - a64b - 1c27d62e5607 </ UserSecretsId >

            // DB set
            services.AddDbContextPool<AppDbContext>(options =>
                options.UseSqlServer(_config.GetConnectionString("EmployeeDBConnection")));

            //Identity Set #2
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 10;
                options.Password.RequiredUniqueChars = 3;

                //Add email confirm
                options.SignIn.RequireConfirmedEmail = true;

                //Add custom token provider options
                options.Tokens.EmailConfirmationTokenProvider = "CustomEmailConfirmation";

                //Account lockout 5 times/ 15 mins
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);

            })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders()
                .AddTokenProvider<CustomEmailConfirmationTokenProvider<ApplicationUser>>("CustomEmailConfirmation");

            services.Configure<DataProtectionTokenProviderOptions>(options => options.TokenLifespan = TimeSpan.FromHours(5));

            services.Configure<CustomEmailConfirmationTokenProviderOptions>(options => options.TokenLifespan = TimeSpan.FromDays(3));

            services.AddMvc(
                config =>
                {
                    //all controller actions which are not marked with [AllowAnonymous] will require the user 
                    // is authenticated with the default authentication scheme.
                    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                    config.Filters.Add(new AuthorizeFilter(policy));

                }).AddXmlDataContractSerializerFormatters();

            services.AddAuthentication().AddGoogle(options =>
            {
                options.ClientId = _config.GetValue<string>("GoogleAuth:ID");
                options.ClientSecret = _config.GetValue<string>("GoogleAuth:Secret");

                //issues modify by https://github.com/dotnet/aspnetcore/issues/6486
                options.UserInformationEndpoint = "https://www.googleapis.com/oauth2/v2/userinfo";
                options.ClaimActions.Clear();
                options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
                options.ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
                options.ClaimActions.MapJsonKey(ClaimTypes.GivenName, "given_name");
                options.ClaimActions.MapJsonKey(ClaimTypes.Surname, "family_name");
                options.ClaimActions.MapJsonKey("urn:google:profile", "link");
                options.ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
                // default callbackPath 'https://localhost:44382/signin-google'
                //options.CallbackPath = "";

            }).AddFacebook(options =>
            {
                options.ClientId = _config.GetValue<string>("FacebookAuth:ID");
                options.ClientSecret = _config.GetValue<string>("FacebookAuth:Secret");
            });

            //Change AccessDeniedPath from Account/AccessDenied to Administration/AccessDenied
            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = new PathString("/Administration/AccessDenied");
            });


            // Roles Policy
            services.AddAuthorization(options =>
            {
                //options.AddPolicy("EditRolePolicy", policy => policy.RequireClaim("Edit Role", "true"));
                // Policy match rules ["Admin" & "Edit Role", "Super Admin"]
                //options.AddPolicy("EditRolePolicy", policy => policy.RequireAssertion(context => AuthorizeAccess(context, "Edit Role")));
                options.AddPolicy("EditRolePolicy", policy =>
                    policy.AddRequirements(new ManageAdminRolesAndClaimsRequirement()));

                //As applied 'DeleteRolePolicy' => require two Claims ['Delete Role', 'Create Role']
                //options.AddPolicy("DeleteRolePolicy",
                //    policy => policy.RequireClaim("Delete Role").RequireClaim("Create Role"));

                options.AddPolicy("DeleteRolePolicy",
                   policy => policy.RequireAssertion(context => AuthorizeAccess(context, "Delete Role")));

                options.AddPolicy("AdminRolePolicy", policy => policy.RequireRole("Admin"));

                //As False, it's return on fail as return 'context.Fail()';
                //options.InvokeHandlersAfterFailure = false;
            });

            ;

            // services.AddSingleton<IEmployeeRepository, MockEmployeeRepository>();

            // kept the same instance of SQLEmployeeRepository during one request 
            services.AddScoped<IEmployeeRepository, SQLEmployeeRepository>();

            // new instance each request
            // services.AddScoped<IEmployeeRepository, MockEmployeeRepository>();
            // new instance each render view
            // services.AddTransient<IEmployeeRepository, MockEmployeeRepository>();

            // Custom AuthorizationHandler
            services.AddSingleton<IAuthorizationHandler, CanEditOnlyOtherAdminRoleAndClaimHandler>();
            services.AddSingleton<IAuthorizationHandler, SuperAdminHandler>();
            services.AddSingleton<DataProtectionPurposeStrings>();
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
