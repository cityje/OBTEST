using System;
using System.Linq;
using Dapper;
using System.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using GeoLibrary.Model;
using GeoLibrary.IO.GeoJson;
using OBTEST.DBContext;
using Microsoft.Data.SqlClient;


namespace OBTEST.Helpers
{
    /// <summary>
    /// 資料相關處理
    /// 1. DB相關事宜; 2.資料轉成Json格式; 3.資料轉成XML格式; 4.
    /// </summary>
    public class UtilDB_Core : ControllerBase
    {
        public UtilDB_Core() { }

        private static IConfiguration _context { set; get; }

        public UtilDB_Core(IConfiguration context)
        {
            _context = context;
        }

        internal static void GetDBIConfiguration(IConfiguration context)
        {
            _context = context;
        }

        #region DB連線相關處理
        private static string GetDBConnectionString
        {
            get
            {
                return _context.GetConnectionString("DefaultConnection");
            }
        }
        private static string GetDBNameString
        {
            get
            {
                return "LandChange";
            }
        }
        private static string GetFullDBConnctionString
        {
            get
            {
                return GetDBConnectionString; //+ ";Database=" + GetDBNameString;
            }
        }

        /// <summary>
        /// 進行SQL搜尋功能
        /// 執行SQL且帶入判斷條件內所需要的變數，回傳List的泛型資料
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="tmpSQL">SQL語法</param>
        /// <param name="tmpParameters">判斷條件所需變數</param>
        /// <returns></returns>
        public static System.Collections.Generic.List<T> GetDataList<T>(string tmpSQL)
        {
            return GetDataList<T>(tmpSQL, null);
        }

        /// <summary>
        /// 進行SQL搜尋功能
        /// 執行SQL且帶入判斷條件內所需要的變數，回傳List的泛型資料
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="tmpSQL">SQL語法</param>
        /// <param name="tmpParameters">判斷條件所需變數</param>
        /// <returns></returns>
        public static System.Collections.Generic.List<T> GetDataList<T>(string tmpSQL, object tmpParameters)
        {
            return GetDataList<T>(tmpSQL, tmpParameters, GetFullDBConnctionString);
        }

        /// <summary>
        /// 進行SQL搜尋功能
        /// 執行SQL且帶入判斷條件內所需要的變數，回傳List的泛型資料
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="tmpSQL">SQL語法</param>
        /// <param name="tmpParameters">判斷條件所需變數</param>
        /// <param name="tmpFullDBConnctionString">連線字串</param>
        /// <returns></returns>
        public static System.Collections.Generic.List<T> GetDataList<T>(string tmpSQL, object tmpParameters, string tmpFullDBConnctionString)
        {
            try
            {
                using (var tmpCon = new System.Data.SqlClient.SqlConnection(tmpFullDBConnctionString))
                {
                    tmpCon.Open();
                    return tmpCon.Query<T>(tmpSQL, tmpParameters).ToList<T>();
                }
            }
            catch (Exception ex)
            {
                return default(System.Collections.Generic.List<T>);
            }
        }



        /*
        public static DataTable ToDataTable<T>(this System.Collections.Generic.List<T> data)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                table.Columns.Add(prop.Name, prop.PropertyType);
            }
            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                table.Rows.Add(values);
            }
            return table;
        }
        public static System.Data.DataTable getTtoDataTable<T>(T tmpData) 
        {
            System.Data.DataTable table = new System.Data.DataTable();
            using (var reader = ObjectReader.Create(tmpData))
            {
                table.Load(reader);
            }
        }
         */
        /*
        public static System.Data.DataTable getDataByDataTable<T>(string tmpSQL, object tmpParameters) 
        {
            return ToDataTable<T>(GetDataList<T>(tmpSQL, tmpParameters));
        }
        public static System.Data.DataTable getDataByDataTable<T>(string tmpSQL)
        {
            return getDataByDataTable<T>(tmpSQL, null);
        }
        */

        /// <summary>
        /// 執行Insert、Update、Delete語法
        /// </summary>
        /// <param name="tmpSQL"></param>
        /// <param name="tmpParameters"></param>
        /// <returns></returns>
        public static int RunSQLCommand(string tmpSQL, object tmpParameters)
        {
            try
            {
                using (var tmpCon = new System.Data.SqlClient.SqlConnection(GetFullDBConnctionString))
                {
                    tmpCon.Open();
                    return tmpCon.Execute(tmpSQL, tmpParameters);
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        /// <summary>
        /// 同一段語法 帶入不同參數使用
        /// 執行Insert、Update、Delete語法
        /// </summary>
        /// <param name="tmpSQL"></param>
        /// <param name="tmpParameters">List的格式</param>
        /// <returns></returns>
        public static int RunSQLCommand(string tmpSQL, System.Collections.Generic.List<object> tmpParameters)
        {
            try
            {
                using (var tmpCon = new System.Data.SqlClient.SqlConnection(GetFullDBConnctionString))
                {
                    tmpCon.Open();
                    int tmpRun = 0;
                    foreach (var tmpRow in tmpParameters)
                        if (tmpCon.Execute(tmpSQL, tmpRow) > 0)
                            tmpRun++;
                    return tmpRun;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        #endregion

        #region 產生不重複的亂數(Key使用)

        /// <summary>
        /// 計算毫秒用的基數 年/月/日
        /// </summary>
        private static string GetSerialDate { get { return "1990/12/31"; } }

        /// <summary>
        /// 取得一串不重複的Key
        /// 西元年後兩碼 + N年N月N日到現在的毫秒數(11碼) + 日期兩碼 + 五碼自用號(00000)
        /// </summary>
        /// <param name="tmpPreCode">前置符</param>
        /// <returns></returns>
        public static string GetSerialCode(string tmpPreCode)
        {
            //年 12345678901 月 000
            //16 12345678901 01 000

            string tmpYear = DateTime.Now.Year.ToString();
            string tmpMonth = ((DateTime.Now.Month.ToString().Length == 1) ? "0" + DateTime.Now.Month.ToString() : DateTime.Now.Month.ToString());
            string tmpDay = ((DateTime.Now.Day.ToString().Length == 1) ? "0" + DateTime.Now.Day.ToString() : DateTime.Now.Day.ToString());

            TimeSpan tmpTS = DateTime.Now - DateTime.Parse(GetSerialDate);
            return tmpPreCode + tmpYear.Substring(2, 2) + tmpTS.TotalMilliseconds.ToString("00000000000") + tmpDay + "0000";
        }
        public static string GetSerialCode()
        {
            return GetSerialCode("");
        }
        #endregion

        #region Json相關處理內容

        /// <summary>
        /// T轉Json資料(包含DataTable也行)，如有意外發生，則回傳空字串
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="tmpData"></param>
        /// <returns></returns>
        public static string GetObjectToJson<T>(T tmpData)
        {
            try
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(tmpData, Newtonsoft.Json.Formatting.None);
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        /// <summary>
        /// 將Json資料轉成物件，如有意外發生，則回傳default(T)
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="tmpData"></param>
        /// <returns></returns>
        public static T GetJsonToObject<T>(string tmpData)
        {
            try
            {
                return (T)Newtonsoft.Json.JsonConvert.DeserializeObject(tmpData);
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }

        #endregion

        #region XML相關處理內容
        /// <summary>
        /// Class 轉 XML
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="tmpObject"></param>
        /// <param name="E"></param>
        /// <returns></returns>
        public static string GetObjectToXML<T>(T tmpObject, System.Text.Encoding E)
        {
            using (System.IO.MemoryStream tmpMemory = new System.IO.MemoryStream())
            using (System.IO.StreamWriter tmpWriter = new System.IO.StreamWriter(tmpMemory, E))
            {
                System.Xml.Serialization.XmlSerializer tmpSer = new System.Xml.Serialization.XmlSerializer(tmpObject.GetType());
                tmpSer.Serialize(tmpWriter, tmpObject);
                return E.GetString(tmpMemory.ToArray());
            }
        }

        /// <summary>
        /// XML 轉 Class
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="tmpValue"></param>
        /// <param name="E"></param>
        /// <returns></returns>
        public static T GetXMLToObject<T>(string tmpValue, System.Text.Encoding E)
        {
            System.Xml.XmlDocument tmpXmlDocument = new System.Xml.XmlDocument();
            try
            {
                byte[] tmpEncodeByte = E.GetBytes(tmpValue);
                using (System.IO.MemoryStream tmpMemory = new System.IO.MemoryStream(tmpEncodeByte))
                {
                    tmpMemory.Flush();
                    tmpMemory.Position = 0;
                    tmpXmlDocument.Load(tmpMemory);
                }
                using (System.Xml.XmlNodeReader tmpReader = new System.Xml.XmlNodeReader(tmpXmlDocument.DocumentElement))
                {
                    System.Xml.Serialization.XmlSerializer tmpSer = new System.Xml.Serialization.XmlSerializer(typeof(T));
                    object tmpObject = tmpSer.Deserialize(tmpReader);
                    return (T)tmpObject;
                }
            }
            catch
            {
                return default(T);
            }
        }
        #endregion

        #region 取得日期時間相關資料

        /// <summary>取得既定日期時間格式, 回傳日期及時間(yyyy/mm/dd HH:mm:ss)</summary>
        public static string GetDateTime()
        {
            return String.Format("{0:yyyy/MM/dd HH:mm:ss}", DateTime.Now);
        }

        #endregion

        /// <summary>
        /// 在C#內呼叫網址，並將網址內容傳回
        /// </summary>
        /// <param name="tmpURL">網址</param>
        /// <returns></returns>
        public static string GetUrlPageData(string tmpURL)
        {
            try
            {
                using (WebClient client = new WebClient() { Encoding = System.Text.Encoding.UTF8 })
                {
                    return client.DownloadString(new Uri(tmpURL));
                }
            }
            catch (Exception ex)
            {
                return "";
            }
        }


        #region GEO相關

        /// <summary>
        /// WKT2GeoJson
        /// </summary>
        /// <param name="tmpData"></param>
        /// <returns></returns>
        public static string WKT2GeoJson(List<dynamic> tmpData)
        {
            GeoJsonClass objGeoJson = new GeoJsonClass();
            List<features> fts = new List<features>();


            objGeoJson.type = "FeatureCollection";
            objGeoJson.totalFeatures = tmpData.Count();

            crs crs = new crs
            {
                type = "name",
                properties = new { name = "urn:ogc:def:crs:EPSG::4326" }
            };
            objGeoJson.crs = crs;

            int i = 0;
            foreach (var data in tmpData)
            {
                i++;
                //var geoArray = WKT2Json(data.GEO, data.GEO.Split(' ')[0])[0];
                //dynamic geojson = geoArray;
                if (data.GEO.Split(' ')[0] == "MULTILINESTRING")
                {
                    var geoArray = WKT2Json(data.GEO, data.GEO.Split(' ')[0])[0];

                    fts.Add(new features()
                    {
                        type = "Feature",
                        id = "shp_dir" + i,
                        //geometry = new geometry { type = GetGeoType(data.GEO.Split(' ')[0]), coordinates = geojson },
                        geometry = new geometry { type = GetGeoType(data.GEO.Split(' ')[0]), coordinates = Newtonsoft.Json.JsonConvert.DeserializeObject(geoArray) },
                        geometry_name = "the_geom",
                        properties = data
                    }); ;
                }
                else
                {
                    string wkt = data.GEO;
                    var point = Geometry.FromWkt(wkt);
                    //var geojson = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(point.ToGeoJson()).coordinates;

                    fts.Add(new features()
                    {
                        type = "Feature",
                        id = "shp_dir" + i,
                        geometry = new geometry { type = GetGeoType(data.GEO.Split(' ')[0]), coordinates = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(point.ToGeoJson()).coordinates },
                        //geometry = new geometry { type = GetGeoType(data.GEO.Split(' ')[0]), coordinates = Newtonsoft.Json.JsonConvert.DeserializeObject(geoArray) },
                        geometry_name = "the_geom",
                        properties = data
                    }); ;
                }
            }

            foreach (var data in tmpData)
            {
                data.GEO = null;
            }
            objGeoJson.features = fts;
            return Newtonsoft.Json.JsonConvert.SerializeObject(objGeoJson);
        }

        private static string GetGeoType(string geo_Type)
        {
            string GType = "";
            switch (geo_Type)
            {
                case "MULTIPOLYGON":
                    GType = "MultiPolygon";
                    break;
                case "POLYGON":
                    GType = "Polygon";
                    break;
                case "MULTILINESTRING":
                    GType = "MultiLineString";
                    break;
                case "LINESTRING":
                    GType = "LineString";
                    break;
                case "POINT":
                    GType = "Point";
                    break;
            }
            return GType;
        }
        private static List<string> WKT2Json(string WKT, string geo_Type)
        {
            string wktStr = "";
            List<string> wktArray = new List<string>();
            switch (geo_Type)
            {
                case "MULTIPOLYGON":
                    wktStr = WKT.Replace(geo_Type + " ", "").Replace("(((", "[[[[").Replace(")))", "]]]]").Replace("((", "[").Replace("))", "]");
                    wktArray = wktStr.Replace("), (", "_").Replace(", ", "],[").Replace(" ", ",").Split('_').ToList();
                    break;
                case "POLYGON":
                    wktStr = WKT.Replace(geo_Type + " ", "").Replace(", ", "],[").Replace(" ", ",").Replace("((", "[[[").Replace("))", "]]]");
                    wktArray.Add(wktStr);
                    break;
                case "MULTILINESTRING":
                    wktStr = WKT.Replace(geo_Type + " ", "").Replace("((", "[[[").Replace("))", "]]]").Replace("), (", "]],[[").Replace(", ", "],[").Replace(" ", ",");
                    //wktArray = wktStr;
                    wktArray.Add(wktStr);
                    break;
                case "LINESTRING":
                    wktStr = WKT.Replace(geo_Type + " ", "").Replace(", ", "],[").Replace(" ", ",").Replace("(", "[[").Replace(")", "]]");
                    wktArray.Add(wktStr);
                    break;
                case "POINT":
                    wktStr = WKT.Replace(geo_Type + " ", "").Replace(" ", ",").Replace("(", "[").Replace(")", "]");
                    wktArray.Add(wktStr);
                    break;
            }
            return wktArray;
        }


        /// <summary>
        /// 輸入右上左下座標取WKT格式
        /// </summary>
        /// <param name="PositionType">Geometry</param>
        /// <param name="URX">右上X</param>
        /// <param name="URY">右上Y</param>
        /// <param name="LLX">左下X</param>
        /// <param name="LLY">左下Y</param>
        /// <returns>WKT</returns>
        private string URLLtoWKT(string PositionType, string xmax, string ymax, string xmin, string ymin)
        {
            string WKT = "";
            double dxmax = Convert.ToDouble(xmax);
            double dymax = Convert.ToDouble(ymax);
            double dxmin = Convert.ToDouble(xmin);
            double dymin = Convert.ToDouble(ymin);
            if (PositionType.ToUpper() == "POINT")
            {
                WKT = "POINT (" + (dxmax + dxmin) / 2 + " " + (dymax + dymin) / 2 + ")";
            }
            else if (PositionType.ToUpper() == "LINESTRING")
            {
                WKT = "LINESTRING (" + dxmax + " " + dymax + ", " + dxmin + " " + dymin + ")";
            }
            else if (PositionType.ToUpper() == "POLYGON")
            {
                WKT = "POLYGON ((" + dxmax + " " + dymax + ", " + dxmax + " " + dymin + ", " + dxmin + " " + dymin + ", " + dxmin + " " + dymax + ", " + dxmax + " " + dymax + "))";
            }
            return WKT;
        }


        #region GeoJson結構
        public class GeoJsonClass
        {
            public string type { get; set; }
            public int totalFeatures { get; set; }
            public List<features> features;
            public crs crs;
        }
        public class features
        {
            public string type { get; set; }
            public string id { get; set; }
            public geometry geometry { get; set; }
            public string geometry_name { get; set; }
            public dynamic properties { get; set; }
        }
        public class geometry
        {
            public string type { get; set; }
            public dynamic coordinates { get; set; }
        }

        public class crs
        {
            public string type { get; set; }
            public dynamic properties { get; set; }
        }
        #endregion

        #endregion



        #region Base64相關
        ////編碼
        //public static void Base64SaveImage(string base64Photo, string filePath)
        //{
        //    byte[] arr = Convert.FromBase64String(base64Photo);
        //    using (MemoryStream ms = new MemoryStream(arr))
        //    {
        //        System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(ms);
        //        bmp.Save(filePath, System.Drawing.Imaging.ImageFormat.Jpeg);
        //        //bmp2.Save(filePath + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
        //        //bmp2.Save(filePath + ".bmp", System.Drawing.Imaging.ImageFormat.Bmp);
        //        //bmp2.Save(filePath + ".gif", System.Drawing.Imaging.ImageFormat.Gif);
        //        //bmp2.Save(filePath + ".png", System.Drawing.Imaging.ImageFormat.Png);
        //        bmp.Dispose();
        //    }
        //}

        ////解碼
        //public static string GetBase64String(string filePath)
        //{
        //    System.Drawing.Bitmap image = new System.Drawing.Bitmap(filePath);
        //    string base64Photo = "";
        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
        //        byte[] arr = new byte[ms.Length];
        //        ms.Position = 0;
        //        ms.Read(arr, 0, (int)ms.Length);
        //        ms.Close();
        //        base64Photo = Convert.ToBase64String(arr);
        //    }
        //    return base64Photo;
        //}

        #endregion
    }
}