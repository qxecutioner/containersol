using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication4.Models;
using WebApplication4.Extensions;
using Microsoft.Extensions.Options;
using Fluid;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebApplication4.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly AzureAd _azureAd;

        public AuthController(IOptions<AzureAd> azureAd)
        {
            this._azureAd = azureAd.Value;
        }

        [HttpPost]
        public IActionResult Index([FromBody] Login user)
        {
            if (user.Username == "admin" && user.Password == "password")
            {
                var token = GenerateJwtToken(user.Username);

                HttpContext.Session.SetString(nameof(Login.Username), user.Username);

                return Ok(new { HttpContext.Session.Id, token });
            }
            return Unauthorized();
        }

        [HttpPost(nameof(GetLiquidOut))]
        public IActionResult GetLiquidOut()
        {
            string templateContent = "Some liquid {{ model }}";
            string output = string.Empty;
            var parser = new FluidParser();
            if (parser.TryParse(templateContent,
                                out IFluidTemplate template,
                                out string error))
            {
                TemplateOptions options = new TemplateOptions();
                options.MemberAccessStrategy = new UnsafeMemberAccessStrategy();
                var ctx = new TemplateContext(
                                   new { model = "data" }, options, true);
                try
                {
                    output = template.Render(ctx);
                }
                catch (Exception ex)
                {
                    // handling of parser error
                }
            }
            else
            {
                // handling of the template parsing error
            }

            return Ok(output);
        }

        private string GenerateJwtToken(string username)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_azureAd.IssuerKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _azureAd.Issuer,
                audience: _azureAd.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
