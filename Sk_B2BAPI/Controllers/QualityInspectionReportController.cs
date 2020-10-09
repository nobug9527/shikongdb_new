using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sk_B2BAPI.Tool;
using Sk_B2BAPI.DAL;
using Sk_B2BAPI.Models;
using ICSharpCode.SharpZipLib.Zip;
using System.IO;
using System.Data;
using Newtonsoft.Json;
using Aop.Api.Domain;

namespace Sk_B2BAPI.Controllers
{
    /// <summary>
    /// 质检报告
    /// </summary>
    public class QualityInspectionReportController : Controller
    {
        #region _弃用
        #region 订单汇总下载质检
        /// <summary>
        /// 订单汇总下载质检
        /// </summary>
        /// <param name="userId">用户</param>
        /// <param name="entId">企业</param>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="order">单据编号</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">容量</param>
        /// <returns></returns>
        public ActionResult OrdersQuality(string userId,string entId,string startDate,string endDate,string order,int pageIndex=1,int pageSize=30)
        {
            try
            {
                if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(entId))
                {
                    return Json(new { success = false, message = "用户未登录，请先登录" });
                }
                QualityInspectionReport report = new QualityInspectionReport();
                var list = report.OrdersQuality(startDate, endDate, order,pageIndex,pageSize,out int pageCount,out int recordCount);
                return Json(new { success = true, message = "数据获取成功", list, pageIndex,pageSize,pageCount,recordCount });
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "QualityInspectionReportController/OrdersQuality", ex.Message.ToString());
                return Json(new { success = false, message = ex.Message.ToString() });
            }
        }
        #endregion

        #region 订单商品质检图片查看
        /// <summary>
        /// 订单商品质检图片
        /// </summary>
        /// <param name="userId">用户</param>
        /// <param name="entId">企业</param>
        /// <param name="order">订单编号</param>
        /// <param name="goodsno">商品编号【非必填】</param>
        /// <returns></returns>
        public ActionResult ProductQualityPicturesShow(string userId, string entId, string order,string goodsno="")
        {
            try
            {
                if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(entId))
                {
                    return Json(new { success = false, message = "用户未登录，请先登录" });
                }
                if (string.IsNullOrEmpty(order))
                {
                    return Json(new { success = false, message = "订单编号不能为空" });
                }
                QualityInspectionReport report = new QualityInspectionReport();
                var list = report.ProductQualityPictures(order, goodsno);
                return Json(new { success = true, message = "数据获取成功", list });
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "QualityInspectionReportController/ProductQualityPicturesShow", ex.Message.ToString());
                return Json(new { success = false, message = ex.Message.ToString() });
            }
        }
        #endregion

        #region 订单商品质检图片下载
        public ActionResult ProductQualityPicturesDownLoad(string userId, string entId, string order, string goodsno = "")
        {
            try
            {
                if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(entId))
                {
                    return Json(new { success = false, message = "用户未登录，请先登录" });
                }
                if (string.IsNullOrEmpty(order))
                {
                    return Json(new { success = false, message = "订单编号不能为空" });
                }
                //获取源数据
                QualityInspectionReport report = new QualityInspectionReport();
                var list = report.ProductQualityPictures(order, goodsno);
                
                //打包
                byte[] bytePDF = null;
                byte[] result = null;
                using (MemoryStream ms = new MemoryStream())
                {
                    using (ZipOutputStream zipStream = new ZipOutputStream(ms))
                    {
                        ZipEntry entry = new ZipEntry("文件名");
                        entry.DateTime = DateTime.Now;//创建时间
                        zipStream.PutNextEntry(entry);
                        zipStream.Write(bytePDF, 0, bytePDF.Length);
                        zipStream.CloseEntry();
                        zipStream.IsStreamOwner = false;
                        zipStream.Finish();
                        zipStream.Close();
                        ms.Position = 0;
                        //压缩后的数据被保存到了byte[]数组中。
                        result = ms.ToArray();
                    }
                }
                return File(result, "application/zip", "文件名.zip");
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "QualityInspectionReportController/ProductQualityPicturesDownLoad", ex.Message.ToString());
                return Json(new { success = false, message = ex.Message.ToString() });
            }
        }
        #endregion

        #region 首营商品
        /// <summary>
        /// 首营商品
        /// </summary>
        /// <param name="userId">用户</param>
        /// <param name="entId">企业</param>
        /// <param name="goods">商品</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">容量</param>
        /// <returns></returns>
        public ActionResult ProductFirstCamp(string userId, string entId,string goods, int pageIndex = 1, int pageSize = 30)
        {
            try
            {
                if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(entId))
                {
                    return Json(new { success = false, message = "用户未登录，请先登录" });
                }
                QualityInspectionReport report = new QualityInspectionReport();
                var list = report.ProductFirstCamps(goods, pageIndex, pageSize,out int pageCount,out int recordCount);
                return Json(new { success = true, message = "数据获取成功", list, pageIndex, pageSize, pageCount, recordCount });
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "QualityInspectionReportController/ProductFirstCamp", ex.Message.ToString());
                return Json(new { success = false, message = ex.Message.ToString() });
            }
        }
        #endregion

        #region 商品首营图片
        /// <summary>
        /// 商品首营图片
        /// </summary>
        /// <param name="userId">用户</param>
        /// <param name="entId">企业</param>
        /// <param name="goodsno">商品编号</param>
        /// <returns></returns>
        public ActionResult ProductFirstCampPictures(string userId, string entId, string goodsno)
        {
            try
            {
                if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(entId))
                {
                    return Json(new { success = false, message = "用户未登录，请先登录" });
                }
                if (string.IsNullOrEmpty(goodsno))
                {
                    return Json(new { success = false, message = "商品编号不能为空" });
                }
                QualityInspectionReport report = new QualityInspectionReport();
                var list = report.ProductFirstCampPictures(goodsno);
                return Json(new { success = true, message = "数据获取成功", list });
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "QualityInspectionReportController/ProductFirstCampPictures", ex.Message.ToString());
                return Json(new { success = false, message = ex.Message.ToString() });
            }
        }
        #endregion
        #endregion

        #region 列表数据
        /// <summary>
        /// 列表数据
        /// </summary>
        /// <param name="userId">用户</param>
        /// <param name="entId">企业</param>
        /// <param name="json">参数数据</param>
        /// <returns></returns>
        public ActionResult TabularData(string userId, string entId,string json)
        {
            try
            {
                if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(entId))
                {
                    return Json(new { success = false, message = "用户未登录，请先登录" });
                }
                QualityInspectionReport report = new QualityInspectionReport();
                var result=report.TabularData(json.ToString(), userId, entId);
                return Content(result, "json");
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "QualityInspectionReport/TabularData", ex.Message.ToString());
                return Json(new { success = false, message = ex.Message.ToString() });
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
        public ActionResult GetQualityList(string type,string userId,string entId, string sqlValue, int pageIndex,int pageSize)
        {
            try
            {
                if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(entId))
                {
                    return Json(new { success = false, message = "用户未登录，请先登录" });
                }
                QualityInspectionReport qualityInspectionReport = new QualityInspectionReport();
                DataSet ds = qualityInspectionReport.GetQualityList(type, userId, entId, sqlValue, pageIndex, pageSize);

                DataTable data = ds.Tables[1];
                int recordCount = Convert.ToInt32(ds.Tables[2].Rows[0]["recordCount"]);
                int pageCount = Convert.ToInt32(ds.Tables[2].Rows[0]["pageCount"]);

                string result = App_Code.JsonMethod.GetDataTable(1, recordCount, pageCount, data);

                //var datalist = new { data = data.AsEnumerable().ToList(), recordCount = recordCount, pageCount = pageCount, success = true };
                //JsonSerializerSettings setting = new JsonSerializerSettings()
                //{
                //    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                //};
                //string result = JsonConvert.SerializeObject(datalist,setting);
                return Content(result);
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "QualityInspectionReport/GetQualityList", ex.Message.ToString());
                return Json(new { success = false, message = ex.Message.ToString() });
            }
        }
        #endregion

        #region 华烁质检报告打包下载
        /// <summary>
        /// 华烁质检报告打包下载
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="entId"></param>
        /// <param name="picUrls"></param>
        /// <returns></returns>
        public ActionResult QualityZipDown(string userId, string entId, List<string> picUrls)
        {
            try
            {
                if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(entId))
                {
                    return Json(new { success = false, message = "用户未登录，请先登录" });
                }
                QualityInspectionReport report = new QualityInspectionReport();
                string filePath = "";
                report.QualityZipDown(picUrls, ref filePath);
                string zipName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                var by = report.DlZipDir(filePath, zipName);
                return File(by, "application/zip", zipName + ".zip");
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "QualityInspectionReport/QualityZipDown", ex.Message.ToString());
                return Json(new { success = false, message = ex.Message.ToString() });
            }
        }
        #endregion

        #region 图片数据
        /// <summary>
        /// 图片数据
        /// </summary>
        /// <param name="userId">用户</param>
        /// <param name="entId">企业</param>
        /// <param name="sqlvalue"></param>
        /// <param name="type"></param>
        /// <param name="arguments"></param>
        /// <param name="imgUrl"></param>
        /// <returns></returns>
        public ActionResult PicturesData(string userId, string entId, string sqlvalue,string type,string arguments, string imgUrl)
        {
            try
            {
                if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(entId))
                {
                    return Json(new { success = false, message = "用户未登录，请先登录" });
                }
                QualityInspectionReport report = new QualityInspectionReport();
                var result = report.PicturesData(userId, entId, sqlvalue, type, arguments, imgUrl);
                return Content(result, "json");
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "QualityInspectionReport/PicturesData", ex.Message.ToString());
                return Json(new { success = false, message = ex.Message.ToString() });
            }
        }
        #endregion

        #region 图片下载
        /// <summary>
        /// 图片数据
        /// </summary>
        /// <param name="userId">用户</param>
        /// <param name="entId">企业</param>
        /// <param name="sqlvalue"></param>
        /// <param name="type"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public ActionResult PicturesDownLoad(string userId, string entId, string sqlvalue, string type, string arguments="")
        {
            try
            {
                if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(entId))
                {
                    return Json(new { success = false, message = "用户未登录，请先登录" });
                }
                string str = HttpRuntime.AppDomainAppPath;
                string path, fileName;
                if (!string.IsNullOrEmpty(arguments))
                {
                    //同一批号图片进行处理
                    path = str + "FPImgupload\\" + type + "\\" + sqlvalue + "\\" + arguments;
                    fileName = arguments;
                }
                else
                {
                    path = str + "FPImgupload\\" + type + "\\" + sqlvalue;
                    fileName = sqlvalue;
                }
                QualityInspectionReport report = new QualityInspectionReport();
                var by=report.DlZipDir(path, fileName);
                return File(by, "application/zip", ""+ fileName + ".zip");
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "QualityInspectionReport/PicturesData", ex.Message.ToString());
                return Json(new { success = false, message = ex.Message.ToString() });
            }
        }
        #endregion


    }
}