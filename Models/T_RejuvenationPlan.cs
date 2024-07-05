using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace OBTEST.Models
{
    public class T_RejuvenationPlan
    {
        /// <summary>
        /// 自動編號
        /// </summary>
        public int? ID { get; set; }

        /// <summary>
        /// 社區既有核定案件編號
        /// </summary>
        public int? OBJECTID { get; set; } //主要依據核定案件編號進行填寫
        /// <summary>
        /// 社區名稱
        /// </summary>
        public string COMMUNITY { get; set; }
        /// <summary>
        /// 核定日期
        /// </summary>
        public string APPDATE { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public string REPORTDATE { get; set; }
        /// <summary>
        /// 核定文號
        /// </summary>
        public string APPLIC { get; set; }
        /// <summary>
        /// 社區計畫名稱
        /// </summary>
        public string PROJECTPLAN { get; set; }
        /// <summary>
        /// 社區計畫年度
        /// </summary>
        public string PROJECTYEAR { get; set; }
        /// <summary>
        /// 備註
        /// </summary>
        public string NOTE { get; set; }
        /// <summary>
        /// 社區範圍Geojson格式
        /// </summary>
        public string Wkt { get; set; }
        /// <summary>
        /// 創建時間
        /// </summary>
        public DateTime? CREATETIME { get; set; } //須注意測試案例是否為NULL
        /// <summary>
        /// 更新時間
        /// </summary>
        public DateTime? UPDATETIME { get; set; } //須注意測試案例是否為NULL
        /// <summary>
        /// 創建人員
        /// </summary>
        public string CreateBy { get; set; }
        /// <summary>
        /// 更新人員
        /// </summary>
        public string UpdateBy { get; set; }
        /// <summary>
        /// 是否刪除
        /// </summary>
        public bool Disable { get; set; }  //須注意測試案例是否為NULL
    }

    public class T_RejuvenationPlanAdd
    {
        
        /// <summary>
        /// 社區既有核定案件編號
        /// </summary>
        public int? OBJECTID { get; set; } //主要依據核定案件編號進行填寫
        /// <summary>
        /// 社區名稱
        /// </summary>
        public string COMMUNITY { get; set; }
        /// <summary>
        /// 核定日期
        /// </summary>
        public string APPDATE { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public string REPORTDATE { get; set; }
        /// <summary>
        /// 核定文號
        /// </summary>
        public string APPLIC { get; set; }
        /// <summary>
        /// 社區計畫名稱
        /// </summary>
        public string PROJECTPLAN { get; set; }
        /// <summary>
        /// 社區計畫年度
        /// </summary>
        public string PROJECTYEAR { get; set; }
        /// <summary>
        /// 備註
        /// </summary>
        public string NOTE { get; set; }
        /// <summary>
        /// 社區範圍Geojson格式
        /// </summary>
        public string Wkt { get; set; }
        /// <summary>
        /// 創建時間
        /// </summary>
        public DateTime? CREATETIME { get; set; } //須注意測試案例是否為NULL
        /// <summary>
        /// 更新時間
        /// </summary>
        public DateTime? UPDATETIME { get; set; } //須注意測試案例是否為NULL

    }

}
