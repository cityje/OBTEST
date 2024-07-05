using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using OBTEST.Models;
using OBTEST.Controllers;
using OBTEST.DBContext;

namespace OBTEST.Until
{
    public class BasicAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly TodoContext _context;

        public BasicAuthHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            TodoContext context)
            : base(options, logger, encoder, clock)
        {
            _context = context;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return Task.FromResult(AuthenticateResult.Fail("Missing Authorization Header"));
            }

            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                var credentialsBytes = Convert.FromBase64String(authHeader.Parameter);
                var credentials = Encoding.UTF8.GetString(credentialsBytes).Split(':');
                var username = credentials[0];
                var password = credentials[1];

                // 從資料庫查詢使用者資訊
                var user = _context.Sys_users.FirstOrDefault(u => u.Username == username && u.Password == password);
                if (user != null)
                {
                    var claims = new[] { 
                        new Claim(ClaimTypes.Name, username),
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // 假设 user.Id 是用户的唯一标识符
                        new Claim("Isadmin", user.IsAdmin.ToString()),
                        new Claim("Canwrite", user.CanWrite.ToString()),
                        new Claim("Canread", user.CanRead.ToString()),
                        new Claim("Canlogin", user.CanLogin.ToString()),
                        new Claim("Departid", user.DepartId.ToString()),
                        new Claim("MAIL", user.Mail.ToString()),
                    };
                    var identity = new ClaimsIdentity(claims, Scheme.Name);
                    var principal = new ClaimsPrincipal(identity);
                    var ticket = new AuthenticationTicket(principal, Scheme.Name);

                    return Task.FromResult(AuthenticateResult.Success(ticket));
                }
                else
                {
                    return Task.FromResult(AuthenticateResult.Fail("Invalid Username or Password"));
                }
            }
            catch
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Header"));
            }
        }

        protected override Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Response.StatusCode = 401;
            Response.Headers["WWW-Authenticate"] = "Basic realm=\"dotnetcore\"";
            return Task.CompletedTask;
        }
    }
}
