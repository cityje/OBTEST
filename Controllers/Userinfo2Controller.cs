using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using OBTEST.Models;
using OBTEST.DBContext;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Data.Common;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Linq.Expressions;


namespace OBTEST.Controllers
{
    /// <summary>
    /// 取得個別使用者資料
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [OpenApiTags("取得使用者資料")]
    public class Userinfo2Controller : ControllerBase
    {
        private readonly TodoContext _context;

        public Userinfo2Controller(TodoContext context)
        {
            _context = context;
        }


        /// <summary>
        /// 取得使用者資料
        /// </summary>
        /// <param name="id">編號</param>
        /// <returns>指定ID的使用者資料</returns>
        [HttpGet("{id}")]
        //[Authorize]
        [OpenApiTags("取得使用者資料")]
        public async Task<IActionResult> GetDataById(string id)
        {
            try
            {
                var tmpData = await _context.ORG_ACCOUNT
                    .Where(x => x.ID_NO == id)
                    .Select(c => new ORG_ACCOUNT
                    {
                        ID_NO = c.ID_NO,
                        LOGIN_TYPE = c.LOGIN_TYPE,
                        USER_ID = c.USER_ID,
                        USER_PWD = c.USER_PWD
                    })
                    .ToListAsync();
                if (tmpData == null)
                {
                    return NotFound("找不到指定ID的圖徵資料");
                }

                return Ok(tmpData);
            }
            catch (Exception ex)
            {
                // 可以根據需要記錄例外或返回詳細的錯誤信息
                return StatusCode(500, $"伺服器發生錯誤: {ex.Message}");
            }
        }

    }
}
