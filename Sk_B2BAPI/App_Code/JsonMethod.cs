using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Sk_B2BAPI.App_Code
{
    public class JsonMethod
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">返回值类型(0返回正确信息，1返回错误信息)</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="pageCount">页数</param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string GetDataTable(int type, int recordCount, int pageCount, DataTable dt)
        {
            StringBuilder jsonString = new StringBuilder();
            try
            {
                jsonString.Append("{");
                jsonString.Append("\"flag\":" + type + ",");
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
                            jsonString.Append("\"" + strValue.Replace("\"", "'") + "\",");
                        }
                        else
                        {
                            jsonString.Append("\"" + strValue.Replace("\"", "'") + "\"");
                        }
                    }
                    jsonString.Append("},");
                }
                if (drc.Count > 0)
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
        /// 
        /// </summary>
        /// <param name="type">返回值类型(0返回正确信息，1返回错误信息)</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="pageCount">页数</param>
        /// <param name="table">表头</param>
        /// <param name="data">数据</param>
        /// <returns></returns>
        public static string GetDataTable(int type, int recordCount, int pageCount, DataTable table, DataTable data)
        {
            StringBuilder jsonString = new StringBuilder();
            try
            {
                jsonString.Append("{");
                jsonString.Append("\"flag\":" + type + ",");
                jsonString.Append("\"recordCount\":" + recordCount + ",");
                jsonString.Append("\"pageCount\":" + pageCount + ",");
                jsonString.Append("\"table\":");
                jsonString.Append("{");

                foreach (DataColumn dc in data.Columns)
                {
                    if (dc.ColumnName.Contains("ROWSTAT"))
                        continue;
                    DataRow[] dr = table.Select("fdname = '" + dc.ColumnName + "'");
                    string chnname = dr.Length == 1 ? dr[0]["chnname"].ToString() : dc.ColumnName;
                    jsonString.Append("\"" + dc.ColumnName.Trim() + "\":\"" + chnname + "\",");
                }
                jsonString.Remove(jsonString.Length - 1, 1);
                jsonString.Append("},");
                jsonString.Append("\"data\":");
                jsonString.Append("[");
                DataRowCollection drc = data.Rows;
                for (int i = 0; i < drc.Count; i++)
                {
                    jsonString.Append("{");
                    for (int j = 0; j < data.Columns.Count; j++)
                    {
                        string strKey = data.Columns[j].ColumnName;
                        string strValue = drc[i][j].ToString().Trim();
                        jsonString.Append("\"" + strKey + "\":");
                        if (j < data.Columns.Count - 1)
                        {
                            jsonString.Append("\"" + strValue.Replace("\"", "'") + "\",");
                        }
                        else
                        {
                            jsonString.Append("\"" + strValue.Replace("\"", "'") + "\"");
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
        /// 
        /// </summary>
        /// <param name="type">返回值类型(0返回正确信息，1返回错误信息)</param>
        /// <param name="count">count</param>
        /// <param name="msg">返回的信息</param>
        /// <returns></returns>
        public static string GetError(int type, string msg)
        {
            StringBuilder jsonString = new StringBuilder();
            try
            {
                jsonString.Append("{");
                jsonString.Append("\"flag\":" + type + ",");
                jsonString.Append("\"message\":\"" + msg + "\"");
                jsonString.Append("}");
            }
            catch (Exception e)
            {
                throw (e);
            }
            return jsonString.ToString();
        }
        /// <summary>
        /// 动态解析json生成List<SqlParameter>
        /// </summary>
        /// <returns></returns>
        public static List<SqlParameter> ListParameter(string json, string userId, string entId)
        {
            List<SqlParameter> listPrmt = new List<SqlParameter>();
            JObject o = JObject.Parse(json);
            IEnumerable<JProperty> properties = o.Properties();
            bool flag = true;
            foreach (JProperty item in properties)
            {
                if (item.Name.Trim() != "")
                {
                    if (item.Name.ToString() == "Entid")
                    {
                        flag = false;
                    }

                    if (item.Name.ToString() == "PassWord")//密码md5加密
                    {
                        listPrmt.Add(new SqlParameter("@" + item.Name, Encryption.GetMD5_16(item.Value.ToString())));
                    }
                    else if (item.Name.ToString() == "XPassWord")//密码md5加密
                    {
                        listPrmt.Add(new SqlParameter("@" + item.Name, Encryption.GetMD5_16(item.Value.ToString())));
                    }
                    else
                    {
                        listPrmt.Add(new SqlParameter("@" + item.Name, item.Value.ToString().Trim() ?? ""));
                    }
                }
            }
            if (userId != "")
            {
                listPrmt.Add(new SqlParameter("@UserId", userId));
            }
            if (entId != "" && flag == true)
            {
                listPrmt.Add(new SqlParameter("@entId", entId));
            }
            return listPrmt;
        }


        /// <summary>
        /// DataSet转Json
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static string DataSetToJson(string type, DataSet ds)
        {
            int n = 0;
            StringBuilder json = new StringBuilder();
            json.Append("{");
            json.Append("\"flag\":" + type + ",");
            foreach (System.Data.DataTable dt in ds.Tables)
            {
                json.Append("\"table" + n + "\":");
                if (dt.Rows.Count > 0)
                {
                    json.Append("[");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        json.Append("{");
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            json.Append("\"");
                            json.Append(dt.Columns[j].ColumnName);
                            json.Append("\":\"");
                            if (dt.Columns[j].ColumnName == "content")
                            {
                                json.Append(Convert.ToBase64String(Encoding.UTF8.GetBytes(dt.Rows[i][j].ToString())));
                            }
                            else
                            {
                                json.Append(dt.Rows[i][j].ToString().Replace("\"", "\\\"").Replace("\r", "").Replace("\n", ""));
                            }
                            json.Append("\",");
                        }
                        json.Remove(json.Length - 1, 1);
                        json.Append("},");
                    }
                    json.Remove(json.Length - 1, 1);
                    json.Append("]");
                }
                else
                {
                    json.Append("\"\"");
                }
                json.Append(",");
                n++;
            }
            json.Remove(json.Length - 1, 1);
            json.Append("}");
            return json.ToString();
        }
        /// <summary>
        /// table转json
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DataTableToJson(DataTable dt)
        {
            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.Append("{\"name\":\"" + dt.TableName + "\",\"data");
            jsonBuilder.Append("\":[");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                jsonBuilder.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    jsonBuilder.Append("\"");
                    jsonBuilder.Append(dt.Columns[j].ColumnName);
                    jsonBuilder.Append("\":\"");
                    jsonBuilder.Append(dt.Rows[i][j].ToString().Replace("\"", "\\\""));
                    jsonBuilder.Append("\",");
                }
                jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                jsonBuilder.Append("},");
            }
            jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            jsonBuilder.Append("]");
            jsonBuilder.Append("}");
            return jsonBuilder.ToString();
        }

        public static string DataTableToJson(string type, DataTable dt)
        {
            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.Append("{\"flag\":" + type + ",\"data");
            jsonBuilder.Append("\":[");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                jsonBuilder.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    jsonBuilder.Append("\"");
                    jsonBuilder.Append(dt.Columns[j].ColumnName);
                    jsonBuilder.Append("\":\"");
                    jsonBuilder.Append(dt.Rows[i][j].ToString().Replace("\"", "\\\""));
                    jsonBuilder.Append("\",");
                }
                jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                jsonBuilder.Append("},");
            }
            jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            jsonBuilder.Append("]");
            jsonBuilder.Append("}");
            return jsonBuilder.ToString();
        }

        public static string DataColumnToJson(DataSet ds)
        {
            ///获取字段中文名字
            DataTable dtmc = GetColumnName.GetChinaName(ds.Tables[0]);
            //数据集
            DataTable dt = null;
            int reCount = 0;
            int pgCount = 0;
            int height = 400;
            int width = 1610;
            if (ds.Tables.Count >= 3)
            {
                if (ds.Tables.Count == 4)
                {
                    height = Convert.ToInt32(ds.Tables[3].Rows[0]["height"].ToString().Trim());
                    width = Convert.ToInt32(ds.Tables[3].Rows[0]["width"].ToString().Trim());
                }
                reCount = Convert.ToInt32(ds.Tables[2].Rows[0]["recordCount"].ToString().Trim());
                pgCount = Convert.ToInt32(ds.Tables[2].Rows[0]["PageCount"].ToString().Trim());
                dt = ds.Tables[1];

            }
            else
            {
                dt = ds.Tables[0];
                reCount = dt.Rows.Count;
                pgCount = 1;
            }
            ///生成json字符串
            StringBuilder jsonString = new StringBuilder();
            jsonString.Append("{");
            jsonString.Append("\"flag\":" + 0 + ",");
            jsonString.Append("\"recordCount\":" + reCount + ",");
            jsonString.Append("\"pageCount\":" + pgCount + ",");
            jsonString.Append("\"height\":" + height + ",");
            jsonString.Append("\"width\":" + width + ",");
            ///表头
            if (dtmc != null)
            {
                jsonString.Append("\"fdname\":");
                jsonString.Append("{");
                foreach (DataColumn dc in dt.Columns)
                {
                    if (dc.ColumnName.Contains("ROWSTAT"))
                        continue;
                    DataRow[] dr = dtmc.Select("fdname = '" + dc.ColumnName + "'");
                    string chnname = dr.Length == 1 ? dr[0]["chnname"].ToString() : dc.ColumnName;
                    jsonString.Append("\"" + dc.ColumnName.Trim() + "\":\"" + chnname + "\",");
                }
                jsonString.Remove(jsonString.Length - 1, 1);
                jsonString.Append("},");
            }
            if (dt.Rows.Count > 0)
            {
                //内容
                jsonString.Append("\"content\":");
                jsonString.Append("[");
                DataRowCollection drc = dt.Rows;
                for (int i = 0; i < drc.Count; i++)
                {
                    jsonString.Append("{");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        string strKey = dt.Columns[j].ColumnName;
                        if (strKey != "ROWSTAT")
                        {
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
                        else
                        {
                            jsonString.Remove(jsonString.Length - 1, 1);
                        }
                    }
                    jsonString.Append("},");
                }
                jsonString.Remove(jsonString.Length - 1, 1);
                jsonString.Append("]");
            }
            else
            {
                jsonString.Append("\"content\":");
                jsonString.Append("[");
                jsonString.Append("]");
            }
            jsonString.Append("}");
            return jsonString.ToString();
        }
        /// <summary>
        /// 编辑用户信息Json
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string UserToJson(DataTable dt)
        {
            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.Append("{");
            for (int j = 0; j < dt.Columns.Count; j++)
            {
                jsonBuilder.Append("\"");
                jsonBuilder.Append(dt.Columns[j].ColumnName);
                jsonBuilder.Append("\":\"");
                jsonBuilder.Append(dt.Rows[0][j].ToString().Replace("\"", "\\\""));
                jsonBuilder.Append("\",");
            }
            jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            jsonBuilder.Append("}");
            return jsonBuilder.ToString();
        }
        /// <summary>
        /// 编辑商品分类
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string GoodsCategory(DataTable dt)
        {
            StringBuilder jsonString = new StringBuilder();
            try
            {
                jsonString.Append("{");
                jsonString.Append("\"flag\":\"0\",");
                ////获取一级分类
                DataRow[] dt_1 = dt.Select("class_layer=1", "sort_id asc");
                jsonString.Append("\"data\":");
                jsonString.Append("[");
                foreach (DataRow dr_1 in dt_1)
                {
                    jsonString.Append("{");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        jsonString.Append("\"");
                        jsonString.Append(dt.Columns[j].ColumnName);
                        jsonString.Append("\":\"");
                        jsonString.Append(dr_1[j].ToString().Replace("\"", "\\\""));
                        jsonString.Append("\",");
                    }
                    //加载二级列表
                    DataRow[] dt_2 = dt.Select("class_layer=2 and parent_id='" + dr_1["parent_id"] + "'", "sort_id asc");
                    jsonString.Append("\"list\":");
                    if (dt_2.Length > 0)
                    {
                        jsonString.Append("[");
                        foreach (DataRow dr_2 in dt_2)
                        {
                            jsonString.Append("{");
                            for (int j = 0; j < dt.Columns.Count; j++)
                            {
                                jsonString.Append("\"");
                                jsonString.Append(dt.Columns[j].ColumnName);
                                jsonString.Append("\":\"");
                                jsonString.Append(dr_1[j].ToString().Replace("\"", "\\\""));
                                jsonString.Append("\",");
                            }
                            jsonString.Remove(jsonString.Length - 1, 1);
                            jsonString.Append("},");
                        }
                        jsonString.Remove(jsonString.Length - 1, 1);
                        jsonString.Append("]");
                    }
                    else
                    {
                        jsonString.Append("\"\"");
                    }
                    jsonString.Append("},");
                }
                ///加载二级分类
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
    }
}