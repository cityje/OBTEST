using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using OBTEST.Models;
using OBTEST.DBContext;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Data.SqlTypes;

namespace OBTEST.Until
{
    public class BasicAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly TodoContext _context;
        private readonly TimeProvider _timeProvider;
        private readonly ILogger<BasicAuthHandler> _logger;

        public BasicAuthHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            TodoContext context,
            TimeProvider timeProvider)
            : base(options, logger, encoder)
        {
            _context = context;
            _timeProvider = timeProvider;
            _logger = logger.CreateLogger<BasicAuthHandler>();
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                _logger.LogWarning("缺少 Authorization 標頭");
                return AuthenticateResult.Fail("缺少 Authorization 標頭");
            }

            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers.Authorization);
                _logger.LogInformation("Authorization 標頭解析成功: {authHeader}", authHeader);

                var credentialsBytes = Convert.FromBase64String(authHeader.Parameter);
                var credentials = Encoding.UTF8.GetString(credentialsBytes).Split(':');
                if (credentials.Length != 2)
                {
                    _logger.LogWarning("Authorization 標頭格式錯誤");
                    return AuthenticateResult.Fail("無效的 Authorization 標頭");
                }

                var USER_ID = credentials[0];
                var USER_PWD = credentials[1];
                _logger.LogInformation("解析到的 USER_ID: {USER_ID}", USER_ID);

                var user = await _context.ORG_ACCOUNT
                    .Where(u => u.USER_ID == USER_ID)
                    .Select(u => new
                    {
                        u.ID_NO,
                        u.USER_PWD,
                        u.IS_MANAGER
                    })
                    .FirstOrDefaultAsync();

                if (user == null)
                {
                    _logger.LogWarning("無效的 USER_ID: {USER_ID}", USER_ID);
                    return AuthenticateResult.Fail("無效的 USER_ID 或 USER_PWD");
                }

                if (!VerifyPassword(user.USER_PWD, USER_PWD))
                {
                    _logger.LogWarning("密碼驗證失敗: {USER_ID}", USER_ID);
                    return AuthenticateResult.Fail("無效的 USER_ID 或 USER_PWD");
                }

                _logger.LogInformation("驗證成功: {USER_ID}", USER_ID);

                var claims = new List<Claim>
        {
            new Claim("USER_ID", USER_ID),
            new Claim(ClaimTypes.NameIdentifier, user.ID_NO),
            new Claim("IS_MANAGER", user.IS_MANAGER)
        };

                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return AuthenticateResult.Success(ticket);
            }
            catch (SqlNullValueException sqlEx)
            {
                _logger.LogError(sqlEx, "資料庫查詢時發生空值異常");
                return AuthenticateResult.Fail("資料庫查詢時發生空值異常");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "處理 Authorization 標頭時出錯");
                return AuthenticateResult.Fail("無效的 Authorization 標頭");
            }
        }

        private bool VerifyPassword(string storedPassword, string enteredPassword)
        {
            using (var md5 = MD5.Create())
            {
                var inputBytes = Encoding.UTF8.GetBytes(enteredPassword);
                var hashBytes = md5.ComputeHash(inputBytes);

                var sb = new StringBuilder();
                foreach (var b in hashBytes)
                {
                    sb.Append(b.ToString("x2"));
                }

                var hashPassword = sb.ToString();
                _logger.LogInformation("計算出的哈希密碼: {hashPassword}", hashPassword);

                var isPasswordValid = storedPassword == hashPassword;
                _logger.LogInformation("密碼驗證結果: {isPasswordValid}", isPasswordValid);

                return isPasswordValid;
            }
        }
   
    }

}
