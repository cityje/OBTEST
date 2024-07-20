using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OBTEST.Models
{
    [Table("GRP_PROPOSAL")]
    public class GRP_PROPOSAL
    {
        [Key]
        [Column(TypeName = "varchar(50)")]
        public string UNID { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string? PLAN_NO { get; set; }

        [Column(TypeName = "varchar(5)")]
        public string? SERIAL_NO { get; set; }

        [Column(TypeName = "nvarchar(200)")]
        public string? PLAN_CNAME { get; set; }

        [Column(TypeName = "nvarchar(300)")]
        public string? PLAN_ENAME { get; set; }

        [Column(TypeName = "varchar(3)")]
        public string? PLAN_YEAR { get; set; }

        [Column(TypeName = "decimal(20, 2)")]
        public decimal? GRANTS_AMT { get; set; }

        [Column(TypeName = "decimal(20, 2)")]
        public decimal? OWN_AMT { get; set; }

        [Column(TypeName = "decimal(20, 2)")]
        public decimal? BUDGET { get; set; }

        [Column(TypeName = "decimal(20, 2)")]
        public decimal? ACTUAL_PAY { get; set; }

        [Column(TypeName = "varchar(10)")]
        public string? NATURE { get; set; }

        [Column(TypeName = "varchar(10)")]
        public string? PLAN_TYPE { get; set; }

        [Column(TypeName = "varchar(10)")]
        public string? ITEM_TYPE { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string? LY_PLAN_NO { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string? APPLY_ORG { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        public string? SPONSOR { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        public string? CONTACT_MAN { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string? JOB_TITLE { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? TEL { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? FAX { get; set; }

        [Column(TypeName = "nvarchar(120)")]
        public string? EMAIL { get; set; }

        [Column(TypeName = "varchar(9)")]
        public string? FULL_EXEC_SDATE { get; set; }

        [Column(TypeName = "varchar(9)")]
        public string? FULL_EXEC_EDATE { get; set; }

        [Column(TypeName = "varchar(9)")]
        public string? EXEC_SDATE { get; set; }

        [Column(TypeName = "varchar(9)")]
        public string? EXEC_EDATE { get; set; }

        [Column(TypeName = "ntext")]
        public string? ACCM_RESULT { get; set; }

        [Column(TypeName = "ntext")]
        public string? SOLVE_PROB { get; set; }

        [Column(TypeName = "ntext")]
        public string? TOTAL_TARGET { get; set; }

        [Column(TypeName = "ntext")]
        public string? CURR_TARGET { get; set; }

        [Column(TypeName = "ntext")]
        public string? PROC_STEP { get; set; }

        [Column(TypeName = "ntext")]
        public string? UN_BENEFIT { get; set; }

        public int? X_COORD { get; set; }

        public int? Y_COORD { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string? EXEC_RESULT { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string? ENG_NO { get; set; }

        [Column(TypeName = "nvarchar(200)")]
        public string? APPR_DOC_NO { get; set; }

        public DateTime? APPR_DATE { get; set; }

        [Column(TypeName = "varchar(30)")]
        public string? APPR_ORG { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        public string? APPLY_MAN { get; set; }

        [Column(TypeName = "ntext")]
        public string? PLAN_ORIGIN { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string? REASON_RETURN { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string? CONTINUES_MASTER_PLAN { get; set; }

        [Column(TypeName = "varchar(1)")]
        public string? SUMMARY_FLAG { get; set; }

        public DateTime? CLOSE_DATE { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string? MODIFIER { get; set; }

        public DateTime? MODIFY_DATE { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string? SUFFIX_NO { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        public string? SPS_JOB_TITLE { get; set; }

        [Column(TypeName = "varchar(11)")]
        public string? MOBILE { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        public string? APPR_MAN { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? PLAN_LOCAL { get; set; }

        [Column(TypeName = "varchar(30)")]
        public string? CONTACT_ORG { get; set; }

        [Column(TypeName = "varchar(1)")]
        public string? PLAN_TYPE_FROM { get; set; }

        [Column(TypeName = "varchar(1)")]
        public string? ISCONTINUE { get; set; }

        [Column(TypeName = "varchar(1)")]
        public string? ISAUTO_NO { get; set; }

        public DateTime? ACTUAL_DATE { get; set; }

        [Column(TypeName = "varchar(2)")]
        public string? BUDGET_PROPERTY { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string? APPR_DOC_NO_CH { get; set; }

        [Column(TypeName = "varchar(2)")]
        public string? ISSINGLE_MULTIPLAN { get; set; }

        [Column(TypeName = "char(1)")]
        public string? ISJOINT_APPR { get; set; }

        [Column(TypeName = "varchar(1)")]
        public string? MASTER_TYPE { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? SINGEL_UNID { get; set; }

        [Column(TypeName = "varchar(1)")]
        public string? HAS_FIRST_APPR { get; set; }

        [Column(TypeName = "varchar(30)")]
        public string? FIRST_APPR_ORG { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        public string? FIRST_APPR_MAN { get; set; }

        [Column(TypeName = "varchar(3)")]
        public string? ACTUAL_YEAR { get; set; }

        [Column(TypeName = "varchar(2)")]
        public string? EXEC_SORT_FROM { get; set; }

        [Column(TypeName = "varchar(1)")]
        public string? SEASON_APPR { get; set; }

        [Column(TypeName = "money")]
        public decimal? INTERESET_INCOME { get; set; }

        [Column(TypeName = "money")]
        public decimal? FORMERLY_INCOME { get; set; }

        [Column(TypeName = "money")]
        public decimal? RESEARCH_INCOME { get; set; }

        [Column(TypeName = "money")]
        public decimal? OTHER_INCOME { get; set; }

        [Column(TypeName = "money")]
        public decimal? SUBSIDY_OVERSPEND { get; set; }

        [Column(TypeName = "varchar(1)")]
        public string? PRINT_SHOWDETAIL { get; set; }

        [Column(TypeName = "varchar(1)")]
        public string? AXIS { get; set; }

        [Column(TypeName = "ntext")]
        public string? INNO_DESC { get; set; }

        [Column(TypeName = "ntext")]
        public string? BUDGET_DESC { get; set; }
    }


}