using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using OBTEST.Models;
using OBTEST.DBContext;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace OBTEST.Controllers
{
    /// <summary>
    /// 使用者資訊
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserinfoController : ControllerBase
    {
        private readonly TodoContext _context;
        private readonly UserInfo _userInfo;

        public UserinfoController(TodoContext context, UserInfo userInfo)
        {
            _context = context;
            _userInfo = userInfo;
        }

        /// <summary>
        /// 取得使用者資訊
        /// </summary>
        /// <returns>使用者資訊</returns>
        [HttpGet("UserInfo")]
        [Authorize]
        [OpenApiTags("使用者資訊")]
        public async Task<IActionResult> GetUserInfo()
        {
            try
            {
                var userId = _userInfo.UserId;
                var username = _userInfo.Username;
                var departid = _userInfo.DepartId;
                var canread = _userInfo.CanRead;
                var canwrite = _userInfo.CanWrite;
                var isadmin = _userInfo.IsAdmin;
                var mail = _userInfo.Mail;

                //// 檢查是否存在名為 "Canwrite" 的聲明，並取得其值
                //var canWriteClaim = User.Claims.FirstOrDefault(c => c.Type == "Canwrite");
                //var canWrite = canWriteClaim != null && bool.TryParse(canWriteClaim.Value, out var canWriteValue) && canWriteValue;

                // 返回基本使用者資訊和 CanWrite 的值
                return Ok(new
                {
                    UserId = userId,
                    UserName = username,
                    DepartId = departid,
                    CanRead = canread,
                    CanWrite = canwrite,
                    Isadmin = isadmin,
                    Mail = mail,
                });
            }
            catch (Exception ex)
            {
                // 錯誤處理
                return StatusCode(500, $"伺服器發生錯誤: {ex.Message}");
            }
        }
    }
}
