using Microsoft.AspNetCore.Http;
using OBTEST.Controllers;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OBTEST.Until
{
    public class UserInfoMiddleware
    {
        private readonly RequestDelegate _next;

        public UserInfoMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, UserInfo userInfo)
        {
            var userIdClaim = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            var userNameClaim = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);

            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                userInfo.UserId = userId;
            }

            if (userNameClaim != null)
            {
                userInfo.Username = userNameClaim.Value;
            }

            // Check for the "Departid" claim and set its value in userInfo if it exists
            var DepartidClaim = context.User.Claims.FirstOrDefault(c => c.Type == "Departid");
            if (DepartidClaim != null && int.TryParse(DepartidClaim.Value, out var DepartidValue))
            {
                userInfo.DepartId = DepartidValue;
            }


            // Check for the "Canread" claim and set its value in userInfo if it exists
            var canReadClaim = context.User.Claims.FirstOrDefault(c => c.Type == "Canread");
            if (canReadClaim != null && bool.TryParse(canReadClaim.Value, out var canReadValue))
            {
                userInfo.CanRead = canReadValue;
            }

            // Check for the "Canwrite" claim and set its value in userInfo if it exists
            var canWriteClaim = context.User.Claims.FirstOrDefault(c => c.Type == "Canwrite");
            if (canWriteClaim != null && bool.TryParse(canWriteClaim.Value, out var canWriteValue))
            {
                userInfo.CanWrite = canWriteValue;
            }

            // Check for the "Canwrite" claim and set its value in userInfo if it exists
            var isAdminClaim = context.User.Claims.FirstOrDefault(c => c.Type == "Isadmin");
            if (isAdminClaim != null && bool.TryParse(isAdminClaim.Value, out var isAdminValue))
            {
                userInfo.IsAdmin = isAdminValue;
            }

            // Check for the "Departid" claim and set its value in userInfo if it exists
            var MAILClaim = context.User.Claims.FirstOrDefault(c => c.Type == "MAIL");
            if (MAILClaim != null)
            {
                userInfo.Mail = MAILClaim.Value; ;
            }
            await _next(context);
        }
    }
}
