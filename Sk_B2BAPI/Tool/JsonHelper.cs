using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Script.Serialization;


namespace Sk_B2BAPI.Tool
{

    public static class JsonHelper
    {
        /// <summary>
        /// 把json字符串转成对象
        /// </summary>
        /// <typeparam name="T">对象</typeparam>
        /// <param name="data">json字符串</param> 
        public static T Deserialize<T>(string data)
        {
            System.Web.Script.Serialization.JavaScriptSerializer json = new System.Web.Script.Serialization.JavaScriptSerializer();
            return json.Deserialize<T>(data);
        }

        /// <summary>
        /// 把对象转成json字符串
        /// </summary>
        /// <param name="o">对象</param>
        /// <returns>json字符串</returns>
        public static string Serialize(object o)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            System.Web.Script.Serialization.JavaScriptSerializer json = new System.Web.Script.Serialization.JavaScriptSerializer();
            json.Serialize(o, sb);
            return sb.ToString();
        }

        /// <summary>
        /// 把DataTable对象转成json字符串
        /// </summary> 
        public static string ToJson(DataTable dt)
        {
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            ArrayList arrayList = new ArrayList();
            foreach (DataRow dataRow in dt.Rows)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                foreach (DataColumn dataColumn in dt.Columns)
                {
                    dictionary.Add(dataColumn.ColumnName, dataRow[dataColumn.ColumnName]);
                }
                arrayList.Add(dictionary);
            }
            return javaScriptSerializer.Serialize(arrayList);
        }

        /// <summary>
        /// 给URl传数据进行解码
        /// </summary>
        /// <returns></returns>
        public static T UrlDecodeData<T>(T obj)
        {
            var type=obj.GetType();//反射对象类型
            var list= type.GetProperties();//获取所有属性
            foreach (var abc in list) {
                if (abc.PropertyType.Name=="String"|| abc.PropertyType.Name == "string") {
                    continue;//只有字符串类型进行解码
                }
                var str= type.GetProperty(abc.Name);//根据属性名获取属性值
                var value= str.GetValue(obj, null).ToString();
                type.GetProperty(abc.Name).SetValue(obj, HttpUtility.UrlDecode(value), null);//将属性解码重新赋值
            }
            return obj;
        }

        /// <summary>
        /// 返回单张表，无分页
        /// </summary>
        /// <param name="type">返回值类型(0返回正确信息，1返回错误信息)</param>
        /// <param name="count"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string GetDataJson(int type, int count, DataTable dt)
        {
            StringBuilder jsonString = new StringBuilder();
            try
            {
                jsonString.Append("{");
                jsonString.Append("\"return_code\":" + type + ",");
                jsonString.Append("\"count\":" + count + ",");
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
                            jsonString.Append("\"" + strValue + "\",");
                        }
                        else
                        {
                            jsonString.Append("\"" + strValue + "\"");
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

        /// <summary>
        /// 返回单张表，分页
        /// </summary>
        /// <param name="type">返回值类型(0返回正确信息，1返回错误信息)</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="pageCount">页数</param>
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

        ///
        ///<summary>
        ///返回多张表数据
        /// 
        public static string GetTablesDataJson(int type, int recordCount, int pageCount, DataTable dt, DataTable dR, DataTable dS)
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
                    jsonString.Append("}");
                }
                //jsonString.Remove(jsonString.Length - 1, 1);
                jsonString.Append("],");
                jsonString.Append("\"time\":[");
                DataRowCollection dr = dR.Rows;
                for (int i = 0; i < dr.Count; i++)
                {
                    jsonString.Append("{");
                    for (int j = 0; j < dR.Columns.Count; j++)
                    {
                        string strKey = dR.Columns[j].ColumnName;
                        string strValue = dr[i][j].ToString().Trim();
                        jsonString.Append("\"" + strKey + "\":");
                        if (j < dR.Columns.Count - 1)
                        {
                            jsonString.Append("\"" + strValue.ToString().Trim() + "\",");
                        }
                        else
                        {
                            jsonString.Append("\"" + strValue.ToString().Trim() + "\"");
                        }
                    }
                    if (i == dr.Count - 1)
                    {
                        jsonString.Append("}");
                    }
                    else
                    {
                        jsonString.Append("},");
                    }

                }
                //jsonString.Remove(jsonString.Length - 1, 1);
                jsonString.Append("],");
                jsonString.Append("\"Three\":[");
                DataRowCollection ds = dS.Rows;
                for (int i = 0; i < ds.Count; i++)
                {
                    jsonString.Append("{");
                    for (int j = 0; j < dS.Columns.Count; j++)
                    {
                        string strKey = dS.Columns[j].ColumnName;
                        string strValue = ds[i][j].ToString().Trim();
                        jsonString.Append("\"" + strKey + "\":");
                        if (j < dS.Columns.Count - 1)
                        {
                            jsonString.Append("\"" + strValue.ToString().Trim() + "\",");
                        }
                        else
                        {
                            jsonString.Append("\"" + strValue.ToString().Trim() + "\"");
                        }
                    }
                    if (i == ds.Count - 1)
                    {
                        jsonString.Append("}");
                    }
                    else
                    {
                        jsonString.Append("},");
                    }

                }
                //jsonString.Remove(jsonString.Length - 1, 1);
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
        /// 返回物流信息
        /// </summary>
        /// <param name="type"></param>
        /// <param name="count"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        public static string ReturnExpressMsg(string type, string message, string json)
        {
            StringBuilder stringBuilder = new StringBuilder();
            try
            {
                stringBuilder.Append("{");
                stringBuilder.Append("\"success\":" + type + ",");
                if (json!=null)
                {
                    stringBuilder.Append("\"message\":\"" + message + "\",");
                    stringBuilder.Append("\"express\":");
                    stringBuilder.Append("" + json + "");
                }
                else
                {
                    stringBuilder.Append("\"message\":\"" + message + "\"");
                }
                stringBuilder.Append("}");
            }
            catch (Exception e)
            {
                throw e;
            }
            return stringBuilder.ToString();
        }
    }

    public static class EnumerableExtension
    {
        /// <summary>
                /// 集合添加一个对象
                /// </summary> 
        public static IEnumerable<T> Add<T>(this IEnumerable<T> e, T value)
        {
            foreach (var cur in e)
            { yield return cur; }
            yield return value;
        }

        /// <summary>
        /// 把集合转成DataTable
        /// </summary> 
        public static DataTable ConvertToDataTable<T>(this IEnumerable<T> enumerable)
        {
            var dataTable = new DataTable();
            foreach (PropertyDescriptor pd in TypeDescriptor.GetProperties(typeof(T)))
            {
                dataTable.Columns.Add(pd.Name, pd.PropertyType);
            }
            foreach (T item in enumerable)
            {
                var Row = dataTable.NewRow();

                foreach (PropertyDescriptor dp in TypeDescriptor.GetProperties(typeof(T)))
                {
                    Row[dp.Name] = dp.GetValue(item);
                }
                dataTable.Rows.Add(Row);
            }
            return dataTable;
        }

       
    }

}

