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
    /// 農再社區社區繪製資料匯入作業
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class T_RejuvenationPlanController : ControllerBase
    {
        private readonly TodoContext _context;
        private readonly UserInfo _userInfo;

        public T_RejuvenationPlanController(TodoContext context , UserInfo userInfo)
        {
            _context = context;
            _userInfo = userInfo;
        }


        /// <summary>
        /// 取得社區圖徵資料
        /// </summary>
        /// <param name="id">編號</param>
        /// <returns>指定ID的圖徵資料</returns>
        [HttpGet("{id}")]
        //[Authorize]
        [OpenApiTags("自訂作畫_社區繪製資料匯入作業")]
        public async Task<IActionResult> GetDataById(int id)
        {
            try
            {
                var tmpData = await _context.T_RejuvenationPlan
                    .Where(x => x.ID == id)
                    .Select(c => new T_RejuvenationPlan
                    {
                        ID = c.ID,
                        OBJECTID = c.OBJECTID,   // 主要為紀錄已核定區域編號 , 新增區域則免填
                        COMMUNITY = c.COMMUNITY,
                        APPDATE = c.APPDATE,
                        REPORTDATE = c.REPORTDATE,
                        APPLIC = c.APPLIC,
                        PROJECTPLAN = c.PROJECTPLAN,
                        PROJECTYEAR = c.PROJECTYEAR,
                        NOTE = c.NOTE,
                        Wkt = c.Wkt,
                        CREATETIME = c.CREATETIME,
                        UPDATETIME = c.UPDATETIME,
                        Disable = c.Disable
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


        /// <summary>
        /// 取得社區圖徵資料
        /// </summary>
        /// <param name="filter">查詢條件，格式為 "欄位名稱 操作符 值"</param>
        /// <returns>符合條件的圖徵資料</returns>
        [HttpGet]
        //[Authorize]
        [OpenApiTags("自訂作畫_社區繪製資料匯入作業")]
        public async Task<IActionResult> GetDataByQuery([FromQuery] string filter)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(filter))
                {
                    return BadRequest("缺少查詢條件");
                }

                IQueryable<T_RejuvenationPlan> query = _context.T_RejuvenationPlan;

                // 分析 filter 字符串，假設格式為 "欄位名稱 操作符 值"
                var conditions = filter.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var condition in conditions)
                {
                    var parts = condition.Split(new[] { ' ' }, 3, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length != 3)
                    {
                        return BadRequest("無效的查詢條件格式");
                    }

                    var field = parts[0].Trim().ToLower();
                    var operatorStr = parts[1].Trim().ToLower();
                    var value = parts[2].Trim().Trim('\'');

                    // 根據輸入的欄位名稱和對應數值動態構建查詢條件
                    switch (field)
                    {
                        case "id":
                            if (int.TryParse(value, out int id))
                            {
                                query = ApplyFilter(query, x => x.ID, operatorStr, id);
                            }
                            else
                            {
                                return BadRequest("無效的ID值");
                            }
                            break;
                        case "objectid":
                            if (int.TryParse(value, out int objectId))
                            {
                                query = ApplyFilter(query, x => x.OBJECTID, operatorStr, objectId);
                            }
                            else
                            {
                                return BadRequest("無效的OBJECTID值");
                            }
                            break;
                        case "community":
                            query = ApplyFilter(query, x => x.COMMUNITY, operatorStr, value);
                            break;
                        case "projectyear":
                            query = ApplyFilter(query, x => x.PROJECTYEAR, operatorStr, value);
                            break;
                        // 添加其他欄位查詢條件
                        // ...
                        default:
                            return BadRequest("無效的欄位名稱");
                    }
                }

                var tmpData = await query
                    .Select(c => new T_RejuvenationPlan
                    {
                        ID = c.ID,
                        OBJECTID = c.OBJECTID,
                        COMMUNITY = c.COMMUNITY,
                        APPDATE = c.APPDATE,
                        REPORTDATE = c.REPORTDATE,
                        APPLIC = c.APPLIC,
                        PROJECTPLAN = c.PROJECTPLAN,
                        PROJECTYEAR = c.PROJECTYEAR,
                        NOTE = c.NOTE,
                        Wkt = c.Wkt,
                        CREATETIME = c.CREATETIME,
                        UPDATETIME = c.UPDATETIME,
                        Disable = c.Disable
                    })
                    .ToListAsync();

                if (tmpData == null || !tmpData.Any())
                {
                    return NotFound("找不到符合條件的圖徵資料");
                }

                return Ok(tmpData);
            }
            catch (Exception ex)
            {
                // 可以根據需要記錄例外或返回詳細的錯誤信息
                return StatusCode(500, $"伺服器發生錯誤: {ex.Message}");
            }
        }

        /// <summary>
        /// 應用篩選條件的方法
        /// </summary>
        /// <typeparam name="T">實體類型</typeparam>
        /// <typeparam name="TProperty">屬性類型</typeparam>
        /// <param name="query">查詢源</param>
        /// <param name="propertySelector">屬性選擇器</param>
        /// <param name="operatorStr">操作符</param>
        /// <param name="value">值</param>
        /// <returns>應用篩選條件後的查詢源</returns>
        private IQueryable<T> ApplyFilter<T, TProperty>(IQueryable<T> query, Expression<Func<T, TProperty>> propertySelector, string operatorStr, TProperty value)
        {
            switch (operatorStr)
            {
                case "eq":
                    return query.Where(Expression.Lambda<Func<T, bool>>(Expression.Equal(propertySelector.Body, Expression.Constant(value)), propertySelector.Parameters));
                case "ne":
                    return query.Where(Expression.Lambda<Func<T, bool>>(Expression.NotEqual(propertySelector.Body, Expression.Constant(value)), propertySelector.Parameters));
                case "gt":
                    return query.Where(Expression.Lambda<Func<T, bool>>(Expression.GreaterThan(propertySelector.Body, Expression.Constant(value)), propertySelector.Parameters));
                case "ge":
                    return query.Where(Expression.Lambda<Func<T, bool>>(Expression.GreaterThanOrEqual(propertySelector.Body, Expression.Constant(value)), propertySelector.Parameters));
                case "lt":
                    return query.Where(Expression.Lambda<Func<T, bool>>(Expression.LessThan(propertySelector.Body, Expression.Constant(value)), propertySelector.Parameters));
                case "le":
                    return query.Where(Expression.Lambda<Func<T, bool>>(Expression.LessThanOrEqual(propertySelector.Body, Expression.Constant(value)), propertySelector.Parameters));
                case "contains":
                    if (propertySelector.Body.Type == typeof(string))
                    {
                        var method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                        return query.Where(Expression.Lambda<Func<T, bool>>(Expression.Call(propertySelector.Body, method, Expression.Constant(value)), propertySelector.Parameters));
                    }
                    throw new NotSupportedException($"操作符 'contains' 只適用於字串屬性。");
                default:
                    throw new NotSupportedException($"不支援的操作符 '{operatorStr}'。");
            }
        }






        /// <summary>
        /// 新增社區圖徵資料
        /// </summary>
        /// <param name="data">資料</param>
        [HttpPost("Post")]
        //[Authorize]
        [OpenApiTags("自訂作畫_社區繪製資料匯入作業")]
        public async Task<IActionResult> AddData(T_RejuvenationPlanAdd data)
        {
            try
            {
                //if (_context.T_RejuvenationPlan.Any(w => w.ID == data.ID))
                //{
                //    return BadRequest("已有相同資料，請確認資料正確性");
                //}

                //後續需要補使用者ID
                // 檢查用戶是否有寫入權限
                if (User.HasClaim(c => c.Type == "Canwrite" && c.Value == "True"))
                {

                    var tmpData = new T_RejuvenationPlan
                    {
                        OBJECTID = data.OBJECTID,
                        COMMUNITY = data.COMMUNITY,
                        APPDATE = data.APPDATE,
                        REPORTDATE = data.REPORTDATE,
                        APPLIC = data.APPLIC,
                        PROJECTPLAN = data.PROJECTPLAN,
                        PROJECTYEAR = data.PROJECTYEAR,
                        NOTE = data.NOTE,
                        Wkt = data.Wkt,
                        CREATETIME = DateTime.Now,
                        UPDATETIME = DateTime.Now
                    };

                    // 取得當前用戶的 ID
                    var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                    if (userIdClaim != null)
                    {
                        tmpData.CreateBy = userIdClaim.Value;  // 確保 UpdateBy 欄位存在於 tempData 對象
                        tmpData.UpdateBy = userIdClaim.Value;  // 確保 UpdateBy 欄位存在於 tempData 對象
                    }

                    _context.T_RejuvenationPlan.Add(tmpData);
                    await _context.SaveChangesAsync();

                    // 使用原生 SQL 更新 GEO 欄位
                    // 使用命名參數來確保參數類型
                    string sql = "UPDATE [RuralRegeneration].[dbo].[T_RejuvenationPlan] " +
                                 "SET [GEO] = geometry::STGeomFromText({0}, 4326).MakeValid() " +
                                 "WHERE [ID] = {1}";

                    await _context.Database.ExecuteSqlRawAsync(sql, tmpData.Wkt, tmpData.ID);

                    // 回傳新增ID
                    return Ok(new { message = "資料新增成功", id = tmpData.ID });

                }
                else
                {
                    return Forbid("您沒有執行此操作的權限");
                }
            }
            catch (Exception ex)
            {
                // 可以根據需要記錄例外或返回詳細的錯誤信息
                return StatusCode(500, $"伺服器發生錯誤: {ex.Message}");
            }
        }



        #region 只能整筆輸入修改沒有帶到的欄位會填空 修改可以單筆更新
        /// <summary>
        /// 編輯社區圖徵資料
        /// </summary>
        /// <param name="data">資料</param>
        [HttpPost("Put")]
        //[Authorize]
        [OpenApiTags("自訂作畫_社區繪製資料匯入作業")]
        public async Task<IActionResult> UpdateData(T_RejuvenationPlan data)
        {
            try
            {
                var tempData = _context.T_RejuvenationPlan.Where(w => w.ID == data.ID).FirstOrDefault();

                //後續需要補使用者ID
                // 檢查用戶是否有寫入權限
                if (User.HasClaim(c => c.Type == "Canwrite" && c.Value == "True"))
                {

                        if (tempData != null)
                    {
                        // 只更新輸入欄位的數值
                        if (data.OBJECTID != null) tempData.OBJECTID = data.OBJECTID;
                        if (data.COMMUNITY != null) tempData.COMMUNITY = data.COMMUNITY;
                        if (data.APPDATE != null) tempData.APPDATE = data.APPDATE;
                        if (data.APPLIC != null) tempData.APPLIC = data.APPLIC;
                        if (data.PROJECTPLAN != null) tempData.PROJECTPLAN = data.PROJECTPLAN;
                        if (data.PROJECTYEAR != null) tempData.PROJECTYEAR = data.PROJECTYEAR;
                        if (data.NOTE != null) tempData.NOTE = data.NOTE;
                        if (data.Wkt != null) tempData.Wkt = data.Wkt;
                        tempData.UPDATETIME = DateTime.Now;

                        // 手動標記修改的屬性
                        var entry = _context.Entry(tempData);
                        if (data.OBJECTID != null) entry.Property(e => e.OBJECTID).IsModified = true;
                        if (data.COMMUNITY != null) entry.Property(e => e.COMMUNITY).IsModified = true;
                        if (data.APPDATE != null) entry.Property(e => e.APPDATE).IsModified = true;
                        if (data.APPLIC != null) entry.Property(e => e.APPLIC).IsModified = true;
                        if (data.PROJECTPLAN != null) entry.Property(e => e.PROJECTPLAN).IsModified = true;
                        if (data.PROJECTYEAR != null) entry.Property(e => e.PROJECTYEAR).IsModified = true;
                        if (data.NOTE != null) entry.Property(e => e.NOTE).IsModified = true;
                        if (data.Wkt != null) entry.Property(e => e.Wkt).IsModified = true;


                        // 取得當前用戶的 ID
                        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                        if (userIdClaim != null)
                        {
                            tempData.UpdateBy = userIdClaim.Value;  // 確保 UpdateBy 欄位存在於 tempData 對象
                        }


                        await _context.SaveChangesAsync();



                        // 如果有更新Wkt欄位，執行相應的SQL語法更新GEO欄位
                        if (data.Wkt != null)
                        {
                            string sql = "UPDATE [RuralRegeneration].[dbo].[T_RejuvenationPlan] " +
                                         "SET [GEO] = geometry::STGeomFromText({0}, 4326).MakeValid() " +
                                         "WHERE [ID] = {1}";

                            await _context.Database.ExecuteSqlRawAsync(sql, tempData.Wkt, tempData.ID);

                            // 回傳空間計算更新成功
                            return Ok(new { message = "空間更新成功", id = tempData.ID });

                        }

                        // 回傳更新成功
                        return Ok(new { message = "資料更新成功", id = tempData.ID });

                    }
                    else
                    {
                        return Forbid("您沒有執行此操作的權限");
                    }
                }
                else
                {
                    return BadRequest("無此資料，請確認資料正確性");
                }
            }
            catch (Exception ex)
            {
                // 記錄例外或返回詳細的錯誤信息
                return BadRequest(ex.Message);
            }
        }
        #endregion




        #region 刪除方法資料仍保留

        ///// <summary>
        ///// 刪除資料
        ///// </summary>
        ///// <param name="id">編號</param>
        //[HttpPost("Delete")]
        //[Authorize]
        //[OpenApiTags("自訂作畫_社區繪製資料匯入作業")]
        //public Task<IActionResult> DeleteData(int id)
        //{
        //    try
        //    {
        //        //_globalVariables.GetLoginInfo(_httpHelpers, HttpContext);

        //        var tempData = _context.T_RejuvenationPlan.Where(w => w.ID == id).FirstOrDefault();
        //        if (tempData != null)
        //        {
        //            tempData.Disable = true;
        //            tempData.UPDATETIME = DateTime.Now;
        //            _context.T_RejuvenationPlan.Update(tempData);
        //            _context.SaveChanges();

        //            //後續需要補使用者ID
        //            //LoggerHelpers.AddWebSystemLog(_context, new WebSystemLog { TableName = "Project_Landno", TID = id, Type = "Delete", UserID = _globalVariables?.LoginInfo?.Id, LogTime = DateTime.Now, Action = "" });

        //            return Task.FromResult<IActionResult>(Ok());
        //        }
        //        else
        //        {
        //            return Task.FromResult<IActionResult>(BadRequest("無此資料，請確認資料正確性"));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //LoggerHelpers.logger.Error(ex.ToString());
        //        return Task.FromResult<IActionResult>(BadRequest(ex.Message));
        //    }
        //}


        /// <summary>
        /// 刪除資料
        /// </summary>
        /// <param name="id">編號</param>
        [HttpPost("Delete")]
        //[Authorize]
        [OpenApiTags("自訂作畫_社區繪製資料匯入作業")]
        public async Task<IActionResult> DeleteData(int id)
        {
            try
            {
                // 檢查用戶是否有寫入權限
                if (User.HasClaim(c => c.Type == "Canwrite" && c.Value == "True"))
                {
                    var tempData = await _context.T_RejuvenationPlan.Where(w => w.ID == id).FirstOrDefaultAsync();
                    if (tempData != null)
                    {
                        tempData.Disable = true;
                        tempData.UPDATETIME = DateTime.Now;

                        // 取得當前用戶的 ID
                        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                        if (userIdClaim != null)
                        {
                            tempData.UpdateBy = userIdClaim.Value;  // 確保 UpdateBy 欄位存在於 tempData 對象
                        }

                        _context.T_RejuvenationPlan.Update(tempData);
                        await _context.SaveChangesAsync();

                        // 記錄日誌
                        // LoggerHelpers.AddWebSystemLog(_context, new WebSystemLog { TableName = "Project_Landno", TID = id, Type = "Delete", UserID = userIdClaim?.Value, LogTime = DateTime.Now, Action = "" });

                        return Ok();
                    }
                    else
                    {
                        return BadRequest("無此資料，請確認資料正確性");
                    }
                }
                else
                {
                    return Forbid("您沒有執行此操作的權限");
                }
            }
            catch (Exception ex)
            {
                // LoggerHelpers.logger.Error(ex.ToString());
                return BadRequest(ex.Message);
            }
        }



        #endregion


        #region 刪除方法資料不保留需要綁定最高權限
        /// <summary>
        /// 刪除資料(最高權限)
        /// </summary>
        /// <param name="id">編號</param>
        [HttpPost("DeleteR")]
        //[Authorize]
        [OpenApiTags("自訂作畫_社區繪製資料匯入作業")]
        public async Task<IActionResult> DeleteRData(int id)
        {
            try
            {
                // 檢查用戶是否為管理員
                if (User.HasClaim(c => c.Type == "Isadmin" && c.Value == "True"))
                {
                    var tempData = await _context.T_RejuvenationPlan.Where(w => w.ID == id).FirstOrDefaultAsync();
                    if (tempData != null)
                    {
                        _context.T_RejuvenationPlan.Remove(tempData);
                        await _context.SaveChangesAsync();
                        // LoggerHelpers.AddWebSystemLog(_context, new WebSystemLog { TableName = "Project_Landno", TID = id, Type = "Delete", UserID = _globalVariables?.LoginInfo?.Id, LogTime = DateTime.Now, Action = "" });

                        return Ok();
                    }
                    else
                    {
                        return BadRequest("無此資料，請確認資料正確性");
                    }
                }
                else
                {
                    return Forbid("您沒有執行此操作的權限");
                }
            }
            catch (Exception ex)
            {
                // LoggerHelpers.logger.Error(ex.ToString());
                return BadRequest(ex.Message);
            }
        }


        #endregion

    }

}
