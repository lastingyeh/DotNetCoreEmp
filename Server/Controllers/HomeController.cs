using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;


namespace Server.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<HomeController> logger;

        public HomeController(IConfiguration configuration, ILogger<HomeController> logger)
        {
            this.configuration = configuration;
            this.logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Secret()
        {
            return View();
        }

        public IActionResult GenToken()
        {
            try
            {
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, "some_id"),
                    new Claim("role","user")
                };

                // Get Vars from appsettings
                var secret = configuration.GetValue<string>(Constants.SECRET);
                var issuer = configuration.GetValue<string>(Constants.ISSUER);
                var audiance = configuration.GetValue<string>(Constants.AUDIANCE);

                // Encoding secret 
                var secretBytes = Encoding.UTF8.GetBytes(secret);
                // Get Symmetric key
                var symmKey = new SymmetricSecurityKey(secretBytes);
                // Set Encrypt Algorithm
                var algorithm = SecurityAlgorithms.HmacSha256;

                // Generate credentials
                var signInCredentials = new SigningCredentials(symmKey, algorithm);

                // Generate token
                var token = new JwtSecurityToken(
                    issuer,
                    audiance,
                    claims,
                    notBefore: DateTime.Now,
                    expires: DateTime.Now.AddDays(1),
                    signInCredentials
                    );

                // Serialize token
                var tokenJson = new JwtSecurityTokenHandler().WriteToken(token);

                return Ok(new { access_token = tokenJson });
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);

                return BadRequest();
            }
        }

        // Decode base64 body info
        public IActionResult DeToken(string token)
        {
            //var bytes = Convert.FromBase64String(token);

            var jsonToken = new JwtSecurityTokenHandler().ReadJwtToken(token);

            var claims = jsonToken.Claims.Select(claim =>
            {
                var obj = new Dictionary<string, string>();
                obj.Add(claim.Type, claim.Value);

                return obj;
            });

            return Json(new { userInfo = claims });
        }
    }
}