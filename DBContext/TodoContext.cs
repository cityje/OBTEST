//using OBTEST.Helpers;
using OBTEST.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OBTEST.Controllers;


namespace OBTEST.DBContext
{
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ORG_ACCOUNT>()
                .HasKey(o => o.ID_NO);

            base.OnModelCreating(modelBuilder);
        }

        #region 屬性表
        /// <summary>
        /// 使用者資訊
        /// </summary>
        public virtual DbSet<ORG_ACCOUNT> ORG_ACCOUNT { get; set; }


        /// <summary>
        /// 公務計畫表單
        /// </summary>
        public DbSet<GRP_PROPOSAL> GRP_PROPOSALS { get; set; }

        #endregion
    }
}
