using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace OBTEST.Helpers
{
    public static class DataTableExtensions
    {
        public static DataTable ListToDataTable<TResult>(this IEnumerable<TResult> ListValue) where TResult : class, new()
        {
            //建立一個回傳用的 DataTable
            DataTable dt = new DataTable();

            //取得映射型別
            Type type = typeof(TResult);

            //宣告一個 PropertyInfo 陣列，來接取 Type 所有的共用屬性
            PropertyInfo[] PI_List = null;

            foreach (var item in ListValue)
            {
                //判斷 DataTable 是否已經定義欄位名稱與型態
                if (dt.Columns.Count == 0)
                {
                    //取得 Type 所有的共用屬性
                    PI_List = item.GetType().GetProperties();

                    //將 List 中的 名稱 與 型別，定義 DataTable 中的欄位 名稱 與 型別
                    foreach (var item1 in PI_List)
                    {
                        dt.Columns.Add(item1.Name, item1.PropertyType);
                    }
                }

                //在 DataTable 中建立一個新的列
                DataRow dr = dt.NewRow();

                //將資料足筆新增到 DataTable 中
                foreach (var item2 in PI_List)
                {
                    dr[item2.Name] = item2.GetValue(item, null);
                }

                dt.Rows.Add(dr);
            }

            dt.AcceptChanges();

            return dt;
        }


        /// <summary>
        /// 將DataTable 轉換成 List dynamic
        /// reverse 反轉：控制返回结果中是只存在 FilterField 指定的字段,還是排除.
        /// [flase 返回FilterField 指定的字段]|[true 返回结果剔除 FilterField 指定的字段]
        /// FilterField  字段過濾，FilterField 為空 忽略 reverse 參數；返回DataTable中的全部數
        /// </summary>
        /// <param name="table">DataTable</param>
        /// <param name="reverse">
        /// 反轉：控制返回结果中是只存在 FilterField 指定的字段,還是排除.
        /// [flase 返回FilterField 指定的字段]|[true 返回结果剔除 FilterField 指定的字段]
        ///</param>
        /// <param name="FilterField">字段過濾，FilterField 為空 忽略 reverse 參數；返回DataTable中的全部數據</param>
        /// <returns>List-dynamic-</returns>
        public static List<dynamic> ToDynamicList(this DataTable table, bool reverse = true, params string[] FilterField)
        {
            var modelList = new List<dynamic>();
            foreach (DataRow row in table.Rows)
            {
                dynamic model = new ExpandoObject();
                var dict = (IDictionary<string, object>)model;
                foreach (DataColumn column in table.Columns)
                {
                    if (FilterField.Length != 0)
                    {
                        if (reverse == true)
                        {
                            if (!FilterField.Contains(column.ColumnName))
                            {
                                dict[column.ColumnName] = row[column];
                            }
                        }
                        else
                        {
                            if (FilterField.Contains(column.ColumnName))
                            {
                                dict[column.ColumnName] = row[column];
                            }
                        }
                    }
                    else
                    {
                        dict[column.ColumnName] = row[column];
                    }
                }
                modelList.Add(model);
            }
            return modelList;
        }
    }
}
