using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace Server
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication("OAuth")
                .AddJwtBearer("OAuth", config =>
                {
                    // Get Vars from appsettings
                    var secret = configuration.GetValue<string>(Constants.SECRET);
                    var issuer = configuration.GetValue<string>(Constants.ISSUER);
                    var audiance = configuration.GetValue<string>(Constants.AUDIANCE);

                    // Encoding secret 
                    var secretBytes = Encoding.UTF8.GetBytes(secret);
                    // Get Symmetric key
                    var symmKey = new SymmetricSecurityKey(secretBytes);

                    // Bind event for authorization from querystring
                    config.Events = new JwtBearerEvents()
                    {
                        OnMessageReceived = context =>
                        {
                            if (context.Request.Query.ContainsKey("access_token"))
                            {
                                context.Token = context.Request.Query["access_token"];
                            }
                            return Task.CompletedTask;
                        }
                    };

                    // Token Authorization
                    config.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidIssuer = issuer,
                        ValidAudience = audiance,
                        IssuerSigningKey = symmKey
                    };
                });

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
