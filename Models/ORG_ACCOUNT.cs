using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OBTEST.Models
{
    public class ORG_ACCOUNT
    {
        /// <summary>
        /// 自動編號
        /// </summary>
            public string ID_NO { get; set; }
            public string LOGIN_TYPE { get; set; }
            public string USER_ID { get; set; }
            public string? USER_PWD { get; set; }
            public DateTime? LAST_LOGIN { get; set; }
            public DateTime? LAST_CHANGE_PWD { get; set; }
            public string AUTH_CODE { get; set; }
            public string STATUS { get; set; }
            public string IS_SYSTEM { get; set; }
            public string IS_SYNC { get; set; }
            public string CRE_USER { get; set; }
            public DateTime? CRE_DATETIME { get; set; }
            public string UPD_USER { get; set; }
            public DateTime? UPD_DATETIME { get; set; }
            public string IS_MANAGER { get; set; }
            public string APPROVED_FLAG { get; set; }
            public string APPROVED_USER { get; set; }
            public DateTime? APPROVED_DATETIME { get; set; }
            public int? LOGIN_ERROR { get; set; }
            public string USER_PWD_HIS1 { get; set; }
            public string USER_PWD_HIS2 { get; set; }
            public string SHOW_ALERT { get; set; }
            public DateTime? LAST_SURVEY { get; set; }
            public string SHOW_MARQUEE { get; set; }
        }
    /// <summary>
    /// 
    /// </summary>
    public class UserInfo
    {
        public string ID_NO { get; set; }
        public string LOGIN_TYPE { get; set; }
        public string USER_ID { get; set; }
        public string USER_PWD { get; set; }
        public string IS_MANAGER { get; set; }
    }
}