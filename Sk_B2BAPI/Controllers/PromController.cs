using Sk_B2BAPI.App_Code;
using Sk_B2BAPI.DAL;
using Sk_B2BAPI.Models;
using Sk_B2BAPI.Tool;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace Sk_B2BAPI.Controllers
{
    public class PromController : Controller
    {
        #region 组合商品
        /// <summary>
        /// 获取组合信息
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="promType">类型（GZH）</param>
        /// <returns></returns>
        public JsonResult GetGroup(string userId, string promType,string entId)
        {
            try
            {
                if (string.IsNullOrEmpty(promType))
                {
                    return Json(new { success = false, message = "必传参数不能为空" });
                }
                if (string.IsNullOrEmpty(entId))
                {
                    entId = BaseConfiguration.EntId;
                }
                ///获取用户信息
                UserInfoDal dal = new UserInfoDal();
                List<UserInfo> user = dal.GetUserInfo(userId,entId);
                string Pricelevel = "";
                string KhType = "";
                bool landing = false;
                bool staleDated = false;
                if (user.Count > 0)
                {
                    entId = user[0].EntId;
                    Pricelevel = user[0].Pricelevel;
                    KhType = user[0].KhType;
                    staleDated = user[0].StaleDated;
                    landing = true;
                }
                PromotionDal pdal = new PromotionDal();
                List<PromMt> list = pdal.GetGroupInfo(entId, userId, promType, Pricelevel, KhType, staleDated,landing);
                return Json(new { success =true, list = list });
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Prom/GetGroup", ex.Message.ToString());
                return Json(new { success = false, message = "E005" });
            }
        }
        #endregion

        #region 促销商品
        /// <summary>
        /// 促销专区
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="promType">方案类型(DHG,DZK,DMZ,DXQ)</param>
        /// <param name="imgType">图片类型ProFloor</param>
        /// <param name="num">显示条目数（8）</param>
        /// <returns></returns>
        public JsonResult GetTopPromSingle(string userId, string promType,string imgType/*,int num*/,string entId, int pageIndex = 1, int pageSize = 8)
        {
            try
            {
                if(string.IsNullOrEmpty(promType))
                {
                    return Json(new { success = false, message = "缺少必传参数方案类型" });
                }
                if (string.IsNullOrEmpty(entId))
                {
                    entId = BaseConfiguration.EntId;
                }
                ///获取用户信息
                UserInfoDal dal = new UserInfoDal();
                List<UserInfo> user = dal.GetUserInfo(userId,entId);
                string Pricelevel = "";
                string KhType = "";
                bool landing = false;
                bool staleDated = false;
                if (user.Count>0)
                {
                    entId = user[0].EntId;
                    Pricelevel = user[0].Pricelevel;
                    KhType = user[0].KhType;
                    staleDated = user[0].StaleDated;
                    landing = true;
                }
                //获取促销信息
                PromotionDal pdal = new PromotionDal();
                string[] Array = promType.Split(new char[] { ',' });
                List<PromModel> pList = new List<PromModel>();
                int record = pageSize;
                int page = pageIndex;
                for (int i = 0; i < Array.Length; i++)
                {
                    PromModel p = new PromModel();
                    if (Array[i] != "")
                    {
                        p.Name = Array[i];
                        p.PromList = pdal.GetTopPromSingle(entId, userId, Array[i], Pricelevel, KhType/*, num*/, pageIndex, pageSize,landing,staleDated, out int recordCount, out int pageCount);
                        pList.Add(p);
                        if (Array.Length > 1)
                        {
                            record = pageSize;
                            page = pageIndex;
                        }
                        else
                        {
                            record = recordCount;
                            page = pageCount;
                        }
                    }
                }
                //获取图片信息
                ImgInfoDal idal = new ImgInfoDal();
                List<ImgInfo> ilist = idal.GetImgInfo(4, imgType, entId,"PC","");

                return Json(new { success = true, list = pList, imgList = ilist, recordCount = record, pageCount = page });
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Prom/GetTopPromSingle", ex.Message.ToString());
                return Json(new { success = false, message = ex.Message.ToString() });
            }
        }
        #endregion

        #region 限时抢购
        /// <summary>
        /// 限时抢购
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="promType"></param>
        /// <param name="entId"></param>
        /// <returns></returns>
        public JsonResult FlashSale(string userId, string promType, string entId/*,string isStart,int pageIndex=1,int pageSize=15*/)
        {
            try
            {
                if (string.IsNullOrEmpty(entId))
                {
                    entId = BaseConfiguration.EntId;
                }
                ///获取用户信息
                UserInfoDal dal = new UserInfoDal();
                List<UserInfo> user = dal.GetUserInfo(userId, entId);
                string Pricelevel = "";
                string KhType = "";
                bool landing = false;
                bool staleDated = false;
                if (user.Count > 0)
                {
                    entId = user[0].EntId;
                    Pricelevel = user[0].Pricelevel;
                    KhType = user[0].KhType;
                    staleDated = user[0].StaleDated;
                    landing = true;
                }
                PromotionDal pdal = new PromotionDal();
                List<PromFlashSale> list = pdal.GetPromFlashSale(entId, userId, promType, Pricelevel, KhType,landing,staleDated/*, isStart, pageIndex, pageSize*/);
                
                //获取图片信息
                ImgInfoDal idal = new ImgInfoDal();
                List<ImgInfo> ilist = idal.GetImgInfo(1, "DQG", entId, "PC","");
                ///获取当前日期
                string date = DateTime.Now.ToLocalTime().ToString();
                return Json(new { success = true, list = list, ilist = ilist, date = date });
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Prom/FlashSale", ex.Message.ToString());
                return Json(new { success = false, message = "E005" });
            }
        }
        #endregion

        #region 主题页
        /// <summary>
        /// 修改或添加活动主题页
        /// </summary>
        /// <returns></returns>
        [ValidateInput(false)]
        [HttpPost]
        public JsonResult SaveTemplate(string title, string content, string strwhere)
        {
            try
            {
                if (!StringHelper.IsNull(title))
                {
                    return Json(new { success = false, message = "参数不完整" });
                }

                SqlRun sql = new SqlRun(SqlRun.sqlstr);
                int num = sql.ExecuteNonQuery("Pc_Template", new SqlParameter[] {
                new SqlParameter("@type", "PC_AddTemplate"),
                new SqlParameter("@TemplateName",title),
                new SqlParameter("@TemplateText",content),
                 new SqlParameter("@TemplateCode",strwhere)
                });
                if (num > 0)
                {
                    return Json(new { success = true, message = "操作成功！" });
                }
                else
                {
                    return Json(new { success = false, message = "操作失败！" });
                }
            }
            catch (Exception ex)
            {

                Log.Error("错误：生成活动主题页", ex.Message);
                return Json(new { success = false, message = "呀，出现问题了，请及时联系攻城狮" });
            }
        }
        #endregion

    }
}
