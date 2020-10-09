using Sk_B2BAPI.App_Code;
using Sk_B2BAPI.DAL;
using Sk_B2BAPI.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Sk_B2BAPI.Tool;
using System.Text;
using System.Data;

namespace Sk_B2BAPI.Controllers
{
    public class ImgController : Controller
    {
        #region 获取商城图片信息
        /// <summary>
        /// 获取商城图片信息
        /// </summary>
        /// <param name="ImgType"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetImgInfo(int num, string imgType, string entId,string singular="",string source="PC")
        {
            if (imgType == "")
            {
                return Json(new { success = false, message = "参数无效" });
            }
            else
            {
                try 
                {
                    if (string.IsNullOrEmpty(entId))
                    {
                        entId = BaseConfiguration.EntId;
                    }
                    DAL.ImgInfoDal IDal = new DAL.ImgInfoDal();
                    List<ImgInfo> Imglist = IDal.GetImgInfo(num, imgType, entId, source, singular);
                    return Json(new { success = true, list = Imglist });
                }
                catch (Exception ex)
                {
                    LogQueue.Write(LogType.Error, "Img/GetImgInfo", ex.Message.ToString());
                    return Json(new { success = false, message = "图片加载失败！" });
                }
            }
        }
        #endregion

        #region 获取网站导航栏/商品分类
        /// <summary>
        /// 获取网站导航栏/商品分类
        /// 5 导航栏
        /// 7 商品分类
        /// 8 商品剂型
        /// </summary>
        /// <param name="channel_Id">分组ID</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetCategory(int channel_Id, string entId)
        {
            if (channel_Id <= 0)
            {
                return Json(new { success = false, message = "参数无效" });
            }
            else
            {
                try
                {
                    if (entId == null || entId == "")
                    {
                        entId = BaseConfiguration.EntId;
                    }
                    DAL.ImgInfoDal CDal = new DAL.ImgInfoDal();
                    List<Category> Clist = CDal.GetCategory(entId,channel_Id);
                    return Json(new { success = true, list = Clist });
                }
                catch (Exception ex)
                {
                    LogQueue.Write(LogType.Error, "Img/GetCategory", ex.Message.ToString());
                    return Json(new { success = false, message = "导航/分类加载失败！" });
                }
            }
        }
        #endregion

        #region 获取用户楼层信息
        /// <summary>
        /// 获取用户楼层信息
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="entId">机构</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetFloorInfo(string userId, string entId)
        {
            try
            {
                if (string.IsNullOrEmpty(entId))
                {
                    entId = BaseConfiguration.EntId;
                }
                DAL.ImgInfoDal dal = new DAL.ImgInfoDal();
                List<IndexFloor> listF = dal.GetIndexFloor(userId, entId);
                List<IndexFloor> datalist = dal.GetZQIndexFloor(userId, entId);
                return Json(new { success = true, list = listF, data = datalist });
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Img/GetFloorInfo", ex.Message.ToString());
                return Json(new { success = false, message = "首页信息加载失败！" });
            }

        }
        #endregion

        #region App模块
        /// <summary>
        /// App模块
        /// </summary>
        /// <param name="entId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetAppModule(string entId)
        {
            try
            {
                if (string.IsNullOrEmpty(entId))
                {
                    entId = BaseConfiguration.EntId;
                }
                Information list = new Information();
                ImgInfoDal imgInfoDal = new ImgInfoDal();
                list = imgInfoDal.AppModule(entId);
                return Json(new { success = true,list=list });
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Img/GetAppModule", ex.Message.ToString());
                return Json(new { success = false, message = "App模块加载失败！" });
            }
        }
        #endregion

        #region  APP个人中心模块/积分商城首页模块
        /// <summary>
        /// APP个人中心模块/积分商城首页模块
        /// </summary>
        /// <param name="entId">机构</param>
        /// <param name="userId">用户</param>
        /// <param name="model">模块类型 PerModule[个人中心]IntegralModule[积分首页]</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GerPerModult(string entId,string userId,string model= "PerModule")
        {
            try
            {
                if (string.IsNullOrEmpty(entId))
                {
                    entId = BaseConfiguration.EntId;
                }
                List<AppModule> list = new List<AppModule>();
                ImgInfoDal imgInfoDal = new ImgInfoDal();
                list = imgInfoDal.PerModule(entId,userId, model);
                return Json(new { success = true, list = list });
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Img/GerPerModult", ex.Message.ToString());
                return Json(new { success = false, message = "App个人中心模块加载失败！" });
            }
        }
        #endregion

        #region APP自动更新接口
        /// <summary>
        /// APP自动更新接口
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AppUpdateEdition()
        {
            try
            {
                SqlRun sql = new SqlRun(SqlRun.sqlstr);
                StringBuilder strsql = new StringBuilder();
                strsql.Append(" select top 1 VersionNo,UpdateDescription,ForceUpdate,DownloadAddress,VersionName from Zzsk_AppUpdate order by UpdateTime desc");
                DataTable data = sql.RunSqlDataTable(strsql.ToString());
                if (data.Rows.Count != 1)
                {
                    throw new Exception("系统后台未配置！请暂停自动更新！");
                }
                string web_url = BaseConfiguration.SercerIp;
                return Json(new
                {
                    success = true,
                    obj = new AppUpdate(
                                        decimal.Parse(data.Rows[0]["VersionNo"].ToString()),
                                        int.Parse(data.Rows[0]["ForceUpdate"].ToString()),
                                        data.Rows[0]["UpdateDescription"].ToString(),
                                        web_url+data.Rows[0]["DownloadAddress"].ToString(),
                                        data.Rows[0]["VersionName"].ToString()
                                        )
                }
                            );
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Img/AppUpdateEdition", ex.Message.ToString());
                return Json(new { success = false, message = "系统后台未配置！请暂停自动更新！" });
            }
        }
        
        private class AppUpdate
        {
            /// <summary>
            /// App版本号
            /// </summary>
            public decimal Version_No { get; set; }
            /// <summary>
            /// App是否强制更新
            /// </summary>
            public int To_update { get; set; }
            /// <summary>
            /// App更新说明 
            /// </summary>
            public string Description { get; set; }
            /// <summary>
            /// App下载地址
            /// </summary>
            public string DownloaUrl { get; set; }
            /// <summary>
            /// App版本名
            /// </summary>
            public string VersionName { get; set; }
            public AppUpdate()
            {

            }
            public AppUpdate(decimal version_No, int to_update, string description, string downloaUrl, string versionName)
            {
                this.Version_No = version_No;
                this.To_update = to_update;
                this.Description = description;
                this.DownloaUrl = downloaUrl;
                this.VersionName = versionName;

            }
        }
        #endregion
    }
}
