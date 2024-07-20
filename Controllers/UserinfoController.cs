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
    /// 登入使用者資訊
    /// </summary>
    [ApiController]
    [Route("[controller]")]
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
        /// 登入使用者資訊
        /// </summary>
        /// <returns>使用者資訊</returns>
        [HttpGet("UserInfo")]
        [Authorize]
        [OpenApiTags("登入使用者資訊")]
        public async Task<IActionResult> GetUserInfo()
        {
            try
            {
                var idno = _userInfo.ID_NO;
                var logintype = _userInfo.LOGIN_TYPE;
                var userid = _userInfo.USER_ID;
                var userpwd = _userInfo.USER_PWD;
                var ismanager = _userInfo.IS_MANAGER;

                // 返回基本使用者資訊和 CanWrite 的值
                return Ok(new
                {
                    ID_NO = idno,
                    LOGIN_TYPE = logintype,
                    USER_ID = userid,
                    USER_PW = userpwd,
                    IS_MANAGER = ismanager,
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
