using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OBTEST.Helpers
{
    /// <summary>
    /// 取得欄位對應中文名稱
    /// </summary>
    public static class ColumnName
    {
        /// <summary>
        /// 取得對應Dictionary
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, Dictionary<string, string>> GetDictionaryNames()
        {
            return new Dictionary<string, Dictionary<string, string>>
            {
                { "歷史災例", GetDisasterNames()},
                { "農路", GetFramRoadNames()},
                { "農塘", GetFramPondNames()},
                { "土石流潛勢溪流", GetPotentialDebrisFlowsNames()},
                { "河川界點", GetRiverSplitPointNames()},
                { "林班地界", GetForestAreaNames()},
                { "山坡地界", GetHillsideAreaNames()},
                { "崩塌潛勢區", GetPotentialCollapseAreaNames()}
             };
        }


        /// <summary>
        /// 歷史災害
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetDisasterNames()
        {
            return new Dictionary<string, string>
            {
                { "Year", "年度"},
                { "DisasterName", "災害名稱"},
                { "County", "縣市"},
                { "Town", "鄉鎮"},
                { "Vill", "村里"},
                { "LandType", "災害範圍"},
                { "DebrisNo", "鄰近潛勢溪流"},
                { "Location", "災害位置"},
                { "House", "受損民宅"},
                { "Road", "鄰近道路"},
                { "DisasterTime", "發生時間"},
                { "DisasterType", "災害類型"},
                { "DisasterDescription", "災害描述"},
                { "DisasterCause", "災害原因"},
                { "wgs84_x", "經度"},
                { "wgs84_y", "緯度"}
             };
        }

        /// <summary>
        /// 農路
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetFramRoadNames()
        {
            return new Dictionary<string, string>
            {
                { "NO", "農路編號"},
                { "NAME", "農路名稱"},
                { "縣市別", "縣市別"},
                { "起點鄉鎮別", "起點鄉鎮別"},
                { "起點村里別", "起點村里別"},
                { "終點鄉鎮別", "終點鄉鎮別"},
                { "終點村里別", "終點村里別"},
                { "連接農村", "連接農村"},
                { "聯外公路", "聯外公路"},
                { "農路長度M", "農路長度"},
                { "農路寬度", "農路寬度"}
             };
        }

        /// <summary>
        /// 農塘
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetFramPondNames()
        {
            return new Dictionary<string, string>
            {
                { "農塘編號", "農塘編號"},
                { "所屬分局", "所屬分局"},
                { "集水區名稱", "集水區名稱"},
                { "縣市", "縣市"},
                { "鄉鎮", "鄉鎮"},
                { "村里", "村里"},
                { "使用狀況", "使用狀況"},
                { "農塘型式", "農塘型式"},
                { "型式說明", "型式說明"},
                { "蓄水面積", "蓄水面積"},
                { "淤積程度", "淤積程度"},
                { "勘查單位", "勘查單位"},
                { "勘查時間", "勘查時間"},
                { "勘查人員", "勘查人員"},
                { "wgs84_x", "經度"},
                { "wgs84_y", "緯度"}
             };
        }

        /// <summary>
        /// 土石流潛勢溪流
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetPotentialDebrisFlowsNames()
        {
            return new Dictionary<string, string>
            {
                { "Debrisno", "編號"},
                { "Type", "型式"},
                { "Basin", "流域"},
                { "Sub_basin", "子集水區"},
                { "Full_", "位置"},
                { "County01", "縣市"},
                { "Town01", "鄉鎮"},
                { "Vill01", "村里"},
                { "Mark", "鄰近地標"},
                { "Potential", "風險程度"},
                { "Length", "長度"},
                { "Owner", "位於"},
                { "Roadname", "鄰近道路"},
                { "DW_number", "影響戶數"}
             };
        }

        /// <summary>
        /// 河川界點
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetRiverSplitPointNames()
        {
            return new Dictionary<string, string>
            {
                { "BASIN_NAME", "流域名稱"},
                { "RV_NAME", "河川名稱"},
                { "FULL_", "行政區"},
                { "NAME", "位置"},
                { "Full_", "位置"},
                { "RVB_NO", "河川局"},
                { "WGS84_X", "經度"},
                { "WGS84_Y", "緯度"},
                { "TM2_X97", "TWD97 X"},
                { "TM2_Y97", "TWD97 Y"}
             };
        }

        /// <summary>
        /// 林班地界
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetForestAreaNames()
        {
            return new Dictionary<string, string>
            {
                { "DIST_C", "管理處"},
                { "WKNG_C", "事業區"},
                { "AREA", "面積"}
             };
        }

        /// <summary>
        /// 山坡地界
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetHillsideAreaNames()
        {
            return new Dictionary<string, string>
            {
                { "COUN_NAME", "縣市"},
                { "new_area", "面積(公頃)"}
             };
        }

        /// <summary>
        /// 崩塌潛勢區
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetPotentialCollapseAreaNames()
        {
            return new Dictionary<string, string>
            {
                { "新編號", "編號"},
                { "COUNTYNAME", "縣市"},
                { "TOWNNAME", "鄉鎮區"},
                { "VILLAGENAM", "村里"},
                { "山坡地", "是否為山坡地"},
                { "林班地", "是否為林班地"},
                { "危險度", "危險度"},
                { "潛勢溪流", "鄰近潛勢溪流"},
                { "坡度", "坡度"},
                { "area", "面積"},
                { "住戶", "住戶"},
                { "note", "備註"},
                { "優先辦理區", "優先辦理區"}
             };
        }
    }
}