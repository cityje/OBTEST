using OBTEST.Helpers;
using OBTEST.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OBTEST.Controllers;
using OBTEST.Models;

namespace OBTEST.DBContext
{
    //繼承DbContext
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options) : base(options)
        {
        }



        #region 屬性表
        /// <summary>
        /// 圖資管理_社區範圍資料
        /// </summary>
        public virtual DbSet<T_RejuvenationPlan> T_RejuvenationPlan { get; set; }


        /// <summary>
        /// 使用者資訊
        /// </summary>
        public virtual DbSet<Sys_users> Sys_users { get; set; }

        #endregion
    }
}
