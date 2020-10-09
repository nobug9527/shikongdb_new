using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace Sk_B2BAPI.App_Code
{
    public class JsonHelper
    {
        /// <summary>
        /// 动态解析json生成List<SqlParameter>
        /// </summary>
        /// <returns></returns>
        public static List<SqlParameter> ListParameter(string json)
        {
            List<SqlParameter> listPrmt = new List<SqlParameter>();
            JObject o = JObject.Parse(json);
            IEnumerable<JProperty> properties = o.Properties();
            foreach (JProperty item in properties)
            {
                if (item.Name.Trim() != "")
                {

                    listPrmt.Add(new SqlParameter("@" + item.Name, item.Value.ToString().Trim() ?? ""));
                }
            }
            return listPrmt;
        }
        /// <summary>
        /// 返回多张表
        /// </summary>
        /// <param name="type"></param>
        /// <param name="recordCount"></param>
        /// <param name="pageCount"></param>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static string GetDataSetJson(int type, int recordCount, int pageCount, DataSet ds)
        {
            StringBuilder jsonString = new StringBuilder();
            try
            {
                jsonString.Append("{");
                jsonString.Append("\"return_code\":" + type + ",");
                jsonString.Append("\"recordCount\":" + recordCount + ",");
                jsonString.Append("\"pageCount\":" + pageCount + ",");
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    DataTable dt = ds.Tables[i];
                    jsonString.Append("\"data" + i + "\":");
                    if (dt.Rows.Count > 0)
                    {
                        jsonString.Append("[");
                        foreach (DataRow dr in dt.Rows)
                        {
                            jsonString.Append("{");
                            for (int j = 0; j < dt.Columns.Count; j++)
                            {
                                string strKey = dt.Columns[j].ColumnName;
                                string strValue = dr[j].ToString().Trim();
                                jsonString.Append("\"" + strKey + "\":");
                                if (j < dt.Columns.Count - 1)
                                {
                                    jsonString.Append("\"" + strValue.ToString().Trim() + "\",");
                                }
                                else
                                {
                                    jsonString.Append("\"" + strValue.ToString().Trim() + "\"");
                                }
                            }
                            jsonString.Append("},");
                        }
                        jsonString.Remove(jsonString.Length - 1, 1);
                        jsonString.Append("],");
                    }
                    else
                    {
                        jsonString.Append("\"\",");
                        if (ds.Tables.Count == i)
                        {
                            jsonString.Remove(jsonString.Length - 1, 1);
                        }
                    }
                }
                jsonString.Remove(jsonString.Length - 1, 1);
                jsonString.Append("}");
            }
            catch (Exception e)
            {
                throw (e);
            }
            return jsonString.ToString();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">返回值类型(0返回正确信息，1返回错误信息)</param>
        /// <param name="count">count</param>
        /// <param name="msg">返回的信息</param>
        /// <returns></returns>
        public static string GetErrorJson(int type, int count, string msg)
        {
            StringBuilder jsonString = new StringBuilder();
            try
            {
                jsonString.Append("{");
                jsonString.Append("\"return_code\":" + type + ",");
                jsonString.Append("\"count\":" + count + ",");
                jsonString.Append("\"data\":");
                jsonString.Append("[");
                jsonString.Append("{");
                jsonString.Append("\"message\":" + "\"" + msg + "\"");
                jsonString.Append("}");
                jsonString.Append("]");
                jsonString.Append("}");
            }
            catch (Exception e)
            {
                throw (e);
            }
            return jsonString.ToString();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">返回值类型(0返回正确信息，1返回错误信息)</param>
        /// <param name="count">总记录数</param>
        /// <param name="Page">页数</param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string GetDataJson(int type, int recordCount, int pageCount, DataTable dt)
        {
            StringBuilder jsonString = new StringBuilder();
            try
            {
                jsonString.Append("{");
                jsonString.Append("\"return_code\":" + type + ",");
                jsonString.Append("\"recordCount\":" + recordCount + ",");
                jsonString.Append("\"pageCount\":" + pageCount + ",");
                jsonString.Append("\"data\":");
                jsonString.Append("[");
                DataRowCollection drc = dt.Rows;
                for (int i = 0; i < drc.Count; i++)
                {
                    jsonString.Append("{");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        string strKey = dt.Columns[j].ColumnName;
                        string strValue = drc[i][j].ToString().Trim();
                        jsonString.Append("\"" + strKey + "\":");
                        if (j < dt.Columns.Count - 1)
                        {
                            jsonString.Append("\"" + strValue.ToString().Trim() + "\",");
                        }
                        else
                        {
                            jsonString.Append("\"" + strValue.ToString().Trim() + "\"");
                        }
                    }
                    jsonString.Append("},");
                }
                jsonString.Remove(jsonString.Length - 1, 1);
                jsonString.Append("]");
                jsonString.Append("}");
            }
            catch (Exception e)
            {
                throw (e);
            }
            return jsonString.ToString();
        }

        #region DataTable/DataSet 转换为Json 字符串
        /// <summary>
        /// DataTable 对象 转换为Json 字符串
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToJson(DataTable dt)
        {
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            javaScriptSerializer.MaxJsonLength = Int32.MaxValue; //取得最大数值
            ArrayList arrayList = new ArrayList();
            foreach (DataRow dataRow in dt.Rows)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>(); //实例化一个参数集合
                foreach (DataColumn dataColumn in dt.Columns)
                {
                    dictionary.Add(dataColumn.ColumnName, dataRow[dataColumn.ColumnName].ToString());
                }
                arrayList.Add(dictionary); //ArrayList集合中添加键值
            }
            return javaScriptSerializer.Serialize(arrayList);  //返回一个json字符串

        }
        /// <summary>
        ///  DataSet 对象 转换为Json 字符串
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static string ToJson(DataSet ds)
        {
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            javaScriptSerializer.MaxJsonLength = Int32.MaxValue; //取得最大数值
            List<ArrayList> arryLists = new List<ArrayList>();
            foreach (DataTable dt in ds.Tables)
            {
                ArrayList arrayList = new ArrayList();
                foreach (DataRow dataRow in dt.Rows)
                {
                    Dictionary<string, object> dictionary = new Dictionary<string, object>(); //实例化一个参数集合
                    foreach (DataColumn dataColumn in dt.Columns)
                    {
                        dictionary.Add(dataColumn.ColumnName, dataRow[dataColumn.ColumnName].ToString());
                    }
                    arrayList.Add(dictionary); //ArrayList集合中添加键值
                }
                arryLists.Add(arrayList);
            }
            return javaScriptSerializer.Serialize(arryLists);  //返回一个json字符串
        }

        #endregion

    }
}