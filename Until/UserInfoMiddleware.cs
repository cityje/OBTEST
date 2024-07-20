using Microsoft.AspNetCore.Http;
using OBTEST.Models;
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
            var idNoClaim = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            var userIdClaim = context.User.Claims.FirstOrDefault(c => c.Type == "USER_ID");

            if (idNoClaim != null)
            {
                userInfo.ID_NO = idNoClaim.Value;
            }

            if (userIdClaim != null)
            {
                userInfo.USER_ID = userIdClaim.Value;
            }

            var isManagerClaim = context.User.Claims.FirstOrDefault(c => c.Type == "IS_MANAGER");
            if (isManagerClaim != null)
            {
                userInfo.IS_MANAGER = isManagerClaim.Value;
            }

            await _next(context);
        }
    }
}
