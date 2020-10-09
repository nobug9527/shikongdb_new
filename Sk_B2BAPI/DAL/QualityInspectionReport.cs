using ICSharpCode.SharpZipLib.Zip;
using Newtonsoft.Json;
using Sk_B2BAPI.App_Code;
using Sk_B2BAPI.Models;
using Sk_B2BAPI.Tool;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace Sk_B2BAPI.DAL
{
    public class QualityInspectionReport
    {
        #region _弃用

        #region 订单汇总下载质检
        /// <summary>
        /// 订单汇总下载质检
        /// </summary>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="order">单据编号</param>
        /// <returns></returns>
        public List<OrdersQuality> OrdersQuality(string startDate, string endDate, string order, int pageIndex, int pageSize,out int pageCount,out int recordCount)
        {
            SqlParameter[] sqls = new SqlParameter[]
            {
                new SqlParameter("@type","OrdersQuality"),
                new SqlParameter("@ksrq",startDate),
                new SqlParameter("@jsrq",endDate),
                new SqlParameter("@sqlvalue",order),
                new SqlParameter("@PageIndex",pageIndex),
                new SqlParameter("@pageSize",pageSize)
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataSet set = sql.RunProDataSet("Proc_QualityInspection", sqls);
            pageCount = 0;
            recordCount = 0;
            if (set.Tables.Count>=3)
            {
                pageCount = int.Parse(set.Tables[2].Rows[0]["pageCount"].ToString());
                recordCount = int.Parse(set.Tables[2].Rows[0]["recordCount"].ToString());
                return OrdersQualityFill(set.Tables[1]);
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 订单汇总下载质检数据填充
        /// </summary>
        /// <param name="table">数据源表</param>
        /// <returns></returns>
        public List<OrdersQuality> OrdersQualityFill(DataTable table)
        {
            List<OrdersQuality> orders = new List<OrdersQuality>();
            foreach (DataRow item in table.Rows)
            {
                OrdersQuality ordersQuality = new OrdersQuality()
                {
                    OrderNo=item["djbh"].ToString().Trim(),
                    Date=item["rq"].ToString().Trim(),
                    Businesscode =item["businesscode"].ToString().Trim(),
                    Businessname=item["businessname"].ToString().Trim(),
                    Pictures=int.Parse(item["pictures"].ToString().Trim())
                };
                orders.Add(ordersQuality);
            }
            return orders;
        }
        #endregion

        #region 订单商品质检图片
        /// <summary>
        /// 订单商品质检图片
        /// </summary>
        /// <param name="order">订单编号</param>
        /// <param name="goodsno">商品编号</param>
        /// <returns></returns>
        public List<ProductQualityPictures> ProductQualityPictures(string order, string goodsno)
        {
            SqlParameter[] sqls = new SqlParameter[]
             {
                new SqlParameter("@type","ProductQualityPictures"),
                new SqlParameter("@sqlvalue",order),
                new SqlParameter("@arguments",goodsno),
                new SqlParameter("@sqlvalue",order)
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataTable data = sql.RunProcedureDR("Proc_QualityInspection", sqls);
            if (data.Rows.Count>0)
            {
                return ProductQualityPicturesFill(data);
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 订单商品质检图片数据填充
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public List<ProductQualityPictures> ProductQualityPicturesFill(DataTable data)
        {
            List<ProductQualityPictures> productQualityPictures = new List<ProductQualityPictures>();
            foreach (DataRow item in data.Rows)
            {
                ProductQualityPictures productQuality = new ProductQualityPictures()
                {
                    GoodsNo=item["spid"].ToString().Trim(),
                    OrderNo=item["OrderNo"].ToString().Trim(),
                    BatchNumber=item["pihao"].ToString().Trim(),
                    PicName=item["phurl"].ToString().Trim(),
                    ImgPath=item["wzlj"].ToString().Trim(),
                    KeyWord=item["grp"].ToString().Trim()
                };
                productQualityPictures.Add(productQuality);
            }
            return productQualityPictures;
        }
        #endregion

        #region 首营商品
        /// <summary>
        /// 首营商品
        /// </summary>
        /// <param name="goods">商品</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">容量</param>
        /// <param name="pageCount">页数</param>
        /// <param name="recordCount">条目数</param>
        /// <returns></returns>
        public List<ProductFirstCamp> ProductFirstCamps(string goods, int pageIndex, int pageSize, out int pageCount, out int recordCount)
        {
            SqlParameter[] sqls = new SqlParameter[]
            {
                new SqlParameter("@type","OrdersQuality"),
                new SqlParameter("@sqlvalue",goods),
                new SqlParameter("@PageIndex",pageIndex),
                new SqlParameter("@pageSize",pageSize)
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataSet set = sql.RunProDataSet("Proc_QualityInspection", sqls);
            pageCount = 0;
            recordCount = 0;
            if (set.Tables.Count>=3)
            {
                pageCount = int.Parse(set.Tables[2].Rows[0]["pageCount"].ToString());
                recordCount = int.Parse(set.Tables[2].Rows[0]["recordCount"].ToString());
                return ProductFirstCampsFill(set.Tables[1]);
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 首营商品数据填充
        /// </summary>
        /// <param name="data">数据源</param>
        /// <returns></returns>
        public List<ProductFirstCamp> ProductFirstCampsFill(DataTable data)
        {
            List<ProductFirstCamp> productFirstCamps = new List<ProductFirstCamp>();
            foreach (DataRow item in data.Rows)
            {
                ProductFirstCamp productFirstCamp = new ProductFirstCamp()
                {
                    GoodsNo=item["goods_no"].ToString().Trim(),
                    GoodsName=item["sub_title"].ToString().Trim(),
                    Specifications=item["drug_spec"].ToString().Trim(),
                    Unit=item["package_unit"].ToString().Trim(),
                    DosageForms=item["dosage_form"].ToString().Trim(),
                    Manufacturer=item["drug_factory"].ToString().Trim(),
                    Pictures=int.Parse(item["pictures"].ToString())
                };
                productFirstCamps.Add(productFirstCamp);
            }
            return productFirstCamps;
        }
        #endregion

        #region 商品首营图片
        /// <summary>
        /// 商品首营图片
        /// </summary>
        /// <param name="goodsno">商品编号</param>
        /// <returns></returns>
        public List<ProductFirstCampPictures> ProductFirstCampPictures(string goodsno)
        {
            SqlParameter[] sqls = new SqlParameter[]
             {
                new SqlParameter("@type","ProductFirstCampPictures"),
                new SqlParameter("@sqlvalue",goodsno)
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataTable data = sql.RunProcedureDR("Proc_QualityInspection", sqls);
            if (data.Rows.Count > 0)
            {
                return ProductFirstCampPicturesFill(data);
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 商品首营图片数据填充
        /// </summary>
        /// <param name="data">数据源</param>
        /// <returns></returns>
        public List<ProductFirstCampPictures> ProductFirstCampPicturesFill(DataTable data)
        {
            List<ProductFirstCampPictures> productFirstCampPictures = new List<ProductFirstCampPictures>();
            foreach (DataRow item in data.Rows)
            {
                ProductFirstCampPictures productFirstCamp = new ProductFirstCampPictures()
                {
                    GoodsNo=item["spid"].ToString().Trim(),
                    PicName=item["phurl"].ToString().Trim(),
                    ImgPath=item["wzlj"].ToString().Trim(),
                    KeyWord=item["grp"].ToString().Trim()
                };
                productFirstCampPictures.Add(productFirstCamp);
            }
            return productFirstCampPictures;
        }
        #endregion
        #endregion

        #region 列表数据
        public string TabularData(string json, string userId, string entId)
        {
            string JSON = "{\"status\":";
            string message = "", status = "0";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataTable dt_fd = new DataTable();
            SqlRun sqlRun = new SqlRun(SqlRun.sqlstr);
            try
            {
                ds = AcquireData(json, userId, entId);
                int ReCount = 0, pgCount = 0;
                message = "{\"fdname\":{";
                if (ds.Tables.Count > 2)
                {
                    dt = ds.Tables[1];
                    dt_fd = sqlRun.GetChinaName(dt);
                    ReCount = Convert.ToInt32(ds.Tables[2].Rows[0]["recordCount"].ToString().Trim());
                    pgCount = Convert.ToInt32(ds.Tables[2].Rows[0]["PageCount"].ToString().Trim());
                }
                else
                {
                    dt = ds.Tables[0];
                    dt_fd = sqlRun.GetChinaName(dt);
                    ReCount = dt.Rows.Count;
                }
                if (dt_fd != null)
                {
                    foreach (DataColumn dc in dt.Columns)
                    {
                        if (dc.ColumnName.Contains("ROWSTAT"))
                            continue;
                        DataRow[] dr = dt_fd.Select("fdname = '" + dc.ColumnName + "'");
                        string chnname = dr.Length == 1 ? dr[0]["chnname"].ToString() : dc.ColumnName;
                        message += "\"" + dc.ColumnName.Trim() + "\":\"" + chnname + "\",";
                    }
                    message = message.Substring(0, message.Length - 1);
                    message += "},";
                }
                else
                {
                    throw new Exception("未获取字段");
                }
                message += "\"ReCount\":" + ReCount + ",\"pgCount\":" + pgCount + ",\"Content\":";
                if (dt.Rows.Count > 0)
                {
                    message += "[";
                    foreach (DataRow dr in dt.Rows)
                    {
                        message += "{";
                        foreach (DataColumn dc in dt.Columns)
                        {
                            if (dc.ColumnName.Contains("ROWSTAT"))
                                continue;
                            message += "\"" + dc.ColumnName.Trim() + "\":\"" + dr[dc.ColumnName].ToString().Trim() + "\",";
                        }
                        message = message.Substring(0, message.Length - 1);
                        message += "},";
                    }
                    message = message.Substring(0, message.Length - 1);
                    message += "]";
                }
                else
                {
                    message += "\"无记录\"";
                }
                message += "}";
            }
            catch (Exception e)
            {
                status = "2";
                message = "\"" + e.Message.Trim().Replace("\"", "\\\"").Replace("\r", "").Replace("\n", "") + "\"";
            }
            JSON += "\"" + status + "\"";
            JSON += ",\"message\":" + message + "}";
            return JSON;
        }
        #endregion

        #region 图片数据
        public string PicturesData(string userId, string entId, string sqlvalue, string type, string arguments,string imgUrl)
        {
            try
            {
                SqlParameter[] sqls = new SqlParameter[]
                {
                   new SqlParameter("@type",type),
                   new SqlParameter("@userId",userId),
                   new SqlParameter("@entId",entId),
                   new SqlParameter("@sqlvalue",sqlvalue),
                   new SqlParameter("@arguments",arguments)
                 };
                SqlRun sql = new SqlRun(SqlRun.sqlstr);
                DataTable data = sql.RunProcedureDR("Proc_QualityInspection", sqls);
                UplaodImg(sqlvalue, type, data, imgUrl, arguments);
                return JsonHelper.GetDataJson(0, data.Rows.Count, data);
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "QualityInspectionReport/UplaodImg", ex.ToString());
                return JsonHelper.GetErrorJson(1, 0, ex.ToString());
            }
        }
        #endregion

        #region 华烁获取质检报告数据列表
        /// <summary>
        /// 华烁获取质检报告数据列表
        /// </summary>
        /// <param name="type"></param>
        /// <param name="userId"></param>
        /// <param name="entId"></param>
        /// <param name="sqlValue"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public DataSet GetQualityList(string type, string userId, string entId, string sqlValue, int pageIndex, int pageSize)
        {
            SqlRun sqlhelper = new SqlRun(SqlRun.sqlstr);
            SqlParameter[] sqlParams = new SqlParameter[] {
                new SqlParameter("@type",type),
                new SqlParameter("@userId",userId),
                new SqlParameter("@entId",entId),
                new SqlParameter("@sqlValue",sqlValue),
                new SqlParameter("@pageIndex",pageIndex),
                new SqlParameter("@pageSize",pageSize)
            };
            DataSet dataSet = sqlhelper.RunProDataSet("Proc_QualityInspectionDown", sqlParams);
            return dataSet;
        }
        #endregion

        public static ZipOutputStream zos;
        #region 图片打包下载
        // <summary>
        /// 文件夹打包方法
        /// </summary>
        /// <param name="strPath"></param>
        /// <param name="strFileName"></param>
        public byte[] DlZipDir(string strPath, string strFileName)
        {
            try
            {
                MemoryStream ms = null;
                strFileName = HttpUtility.UrlEncode(strFileName).Replace('+', ' ');
                ms = new MemoryStream();
                zos = new ZipOutputStream(ms);
                AddZipEntry(strPath);
                zos.Finish();
                zos.Close();
                return ms.ToArray();
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "QualityInspectionReport/DlZipDir", ex.ToString());
                return null;
            }
        }
        /// <summary>
        /// 递归取文件打包
        /// </summary>
        /// <param name="PathStr"></param>
        protected void AddZipEntry(string PathStr)
        {
            DirectoryInfo di = new DirectoryInfo(PathStr);
            foreach (DirectoryInfo item in di.GetDirectories())
            {
                AddZipEntry(item.FullName);
            }
            foreach (FileInfo item in di.GetFiles())
            {
                FileStream fs = File.OpenRead(item.FullName);
                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                string strEntryName = item.FullName.Replace(PathStr, "");
                string fileName = strEntryName.Substring(strEntryName.LastIndexOf("\\") + 1);
                ZipEntry entry = new ZipEntry(fileName);
                zos.PutNextEntry(entry);
                zos.Write(buffer, 0, buffer.Length);
                fs.Close();
            }
        }
        #endregion

        #region 请求数据
        /// <summary>
        /// 请求数据
        /// </summary>
        /// <param name="json">存储过程参数数据</param>
        /// <param name="userId">用户</param>
        /// <param name="entId">企业</param>
        /// <returns></returns>
        public DataSet AcquireData(string json, string userId, string entId)
        {
            string sql = "";
            SqlRun sqlRun = new SqlRun(SqlRun.sqlstr);
            JsonReader reader = new JsonTextReader(new StringReader(json));
            string t = "";
            string v = "";
            while (reader.Read())
            {
                if (reader.TokenType.ToString() == "PropertyName")
                {
                    t = t + "," + reader.Value.ToString();
                }
                else if (reader.TokenType.ToString() == "String")
                {
                    v = v + "," + reader.Value.ToString();
                }
            }
            string[] type = t.Split(',');
            string[] value = v.Split(',');
            List<SqlParameter> ilistStr = new List<SqlParameter>();
            for (int i = 1; i < type.Length; i++)
            {
                if (type[i] != "" && type[i] != "sql")
                {
                    ilistStr.Add(new SqlParameter("@" + type[i], value[i]));
                }
                else if (type[i] == "sql")
                {
                    sql = value[i].ToString();
                }
            }
            ilistStr.Add(new SqlParameter("@userid", userId));
            ilistStr.Add(new SqlParameter("@entid", entId));
            SqlParameter[] param = ilistStr.ToArray();
            DataSet ds = sqlRun.RunProDataSet(sql, param);
            return ds;
        }
        #endregion

        #region 加水印下载至本地
        /// <summary>
        /// 将图片加水印下载至本地
        /// </summary>
        /// <param name="sqlvalue">一级（客户、商品或其它）</param>
        /// <param name="SqlType">类型</param>
        /// <param name="dt">数据源</param>
        /// <param name="ImgUrl">水印图片路径</param>
        /// <param name="arguments">二级（批号或其它）</param>
        private void UplaodImg(string sqlvalue, string SqlType, DataTable dt, string ImgUrl, string arguments)
        {
            try
            {
                if (dt.Rows.Count > 0)
                {
                    string str = HttpRuntime.AppDomainAppPath;
                    //存放质检图片路径
                    string filePath = str + "FPImgupload";
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    //创建临时文件夹（一级文件夹）
                    string url = str+"FPImgupload" + "\\" + SqlType + "\\" + sqlvalue;
                    if (!FileExists(url))
                    {
                        LogQueue.Write(LogType.Error, "QualityInspectionReport/UplaodImg", "一级文件夹创建失败");
                        throw new Exception("一级文件夹创建失败");
                    }
                    Create_YZ YZ = new Create_YZ();
                    string path = filePath + "\\" + SqlType + "\\" + sqlvalue;//带印章质检报告图片路径
                    string yzpath = str + ImgUrl;
                    System.Drawing.Image image1 = new System.Drawing.Bitmap(829, 1169);
                    if (!dt.Columns.Contains("grp"))
                    {
                        throw new Exception("数据源表缺少必须分组列：grp");
                    }
                    //根据二级将图片分组 grp为数据库分组字段，不可删除
                    var query = from g in dt.AsEnumerable()
                                group g by new { g1 = g.Field<string>("grp") } into source
                                select new { pihao = source.Key.g1 };
                    //重置路径
                    string reset = path;
                    foreach (var item in query)
                    {
                        ////每次循环重置路径
                        path = reset;
                        url = str + "FPImgupload" + "\\" + SqlType + "\\" + sqlvalue + "\\" + item.pihao;
                        if (!FileExists(url))
                        {
                            LogQueue.Write(LogType.Error, "QualityInspectionReport/UplaodImg", "二级文件夹创建失败");
                            throw new Exception("二级文件夹创建失败");
                        }
                        //拼接上分组字段
                        path = path + "\\" + item.pihao;
                        //Log.Debug("path+", path);
                        var columns = dt.AsEnumerable().Where<DataRow>(r => r["grp"].ToString() == item.pihao.ToString());
                        DataTable table = dt.Clone();
                        foreach (DataRow dr in columns)
                        {
                            table.ImportRow(dr);
                        }
                        SaveNewPicture(path, table, YZ, yzpath);
                    }
                }
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "QualityInspectionReport/UplaodImg", ex.ToString());
                throw new Exception(ex.ToString());
            }
        }
        #endregion

        #region 不存在重新创建文件
        /// <summary>
        /// 不存在重新创建文件
        /// </summary>
        /// <param name="filePath">路径</param>
        /// <returns></returns>
        protected bool FileExists(string filePath)
        {
            bool flag = true;
            try
            {
                if (Directory.Exists(filePath) == false)//如果不存在就创建file文件夹
                {
                    Directory.CreateDirectory(filePath);
                }
            }
            catch (Exception)
            {
                flag = false;
            }
            return flag;
        }
        #endregion

        #region 保存新图片
        /// <summary>
        /// 保存新图片
        /// </summary>
        /// <param name="path">新图片保存路径</param>
        /// <param name="dt">数据源</param>
        /// <param name="yZ"></param>
        /// <param name="yzpath">水印图片</param>
        public void SaveNewPicture(string path, DataTable dt, Create_YZ yZ, string yzpath)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string newpath = path + "\\" + dt.Rows[i]["phurl"].ToString().Trim();//新图片的物理路径
                if (!System.IO.File.Exists(newpath))
                {
                    try
                    {
                        string urlpt = dt.Rows[i]["wzlj"].ToString().Trim();
                        //Log.Debug("出库单图片", "图片完整路径:" + urlpt);
                        int begin = urlpt.LastIndexOf("/") + 1;
                        //Log.Debug("出库单图片", "begin:" + begin);
                        int length = urlpt.Length - begin;
                        //Log.Debug("出库单图片", "length:" + length);
                        string cutout = urlpt.Substring(begin, length);
                        //Log.Debug("出库单图片", "截取:" + cutout);
                        cutout = HttpUtility.UrlEncode(cutout);
                        cutout = cutout.Replace("+", "%20");
                        //Log.Debug("出库单图片", "截取转码:" + cutout);
                        urlpt = urlpt.Substring(0, begin) + cutout;
                        //Sk_B2BAPI.App_Code.Log.Debug("出库单图片", "重新拼接:" + urlpt);
                        WebRequest wr = WebRequest.Create(urlpt);
                        HttpWebResponse wresp = (HttpWebResponse)wr.GetResponse();
                        Stream s = wresp.GetResponseStream();
                        System.Drawing.Image img = System.Drawing.Image.FromStream(s);
                        
                        //Sk_B2BAPI.App_Code.Log.Debug("出库单图片", "新图片的物理路径:" + newpath);
                        //Sk_B2BAPI.App_Code.Log.Debug("出库单图片", "加水印之前:" + yzpath);
                        yZ.BuildWatermark(img, yzpath, "", newpath);
                    }
                    catch (Exception ex)
                    {
                        LogQueue.Write(LogType.Error, "QualityInspectionReport/SaveNewPicture", ex.ToString());
                        continue;
                    }
                }
            }
        }
        #endregion

        #region 华烁打包质检报告下载
        /// <summary>
        /// 华烁打包质检报告下载
        /// </summary>
        /// <param name="picUrls"></param>
        public void QualityZipDown(List<string> picUrls ,ref string filePath)
        {
            string newpath = HttpContext.Current.Server.MapPath("/FPImgUpload/") + Guid.NewGuid().ToString("N") + "/";//新图片的物理路径
            if (!Directory.Exists(newpath))
            {
                Directory.CreateDirectory(newpath);
            }
            filePath = newpath;
            foreach (string url in picUrls)
            {
                try
                {
                    WebRequest wr = WebRequest.Create(url);
                    HttpWebResponse wresp = (HttpWebResponse)wr.GetResponse();
                    Stream s = wresp.GetResponseStream();
                    System.Drawing.Image img = System.Drawing.Image.FromStream(s);
                    img.Save(newpath + url.Substring(url.LastIndexOf("/")));
                }
                catch (Exception ex)
                {
                    LogQueue.Write(LogType.Error, "QualityInspectionReport/QualityZipDown", ex.ToString());
                    continue;
                }
            }
        }
        #endregion
    }
}