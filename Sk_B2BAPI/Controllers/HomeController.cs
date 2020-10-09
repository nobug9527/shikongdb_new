using Sk_B2BAPI.App_Code;
using Sk_B2BAPI.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sk_B2BAPI.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        #region 网站配置
        [HttpPost]
        public JsonResult GetWebConfig(string entId,string imgType="")
        {
            try
            {
                if (string.IsNullOrEmpty(entId))
                {
                    entId = BaseConfiguration.EntId;
                }
                DAL.ImgInfoDal dal = new DAL.ImgInfoDal();
                var list = dal.GetIndexConfig(entId,imgType);
                return Json(new { success = true, list = list });
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Home/GetWebConfig", ex.Message.ToString());
                return Json(new { success = false, message = "网站配置接口获取失败！" });
            }
        }
        #endregion

        #region 网站广告图
        [HttpPost]
        public JsonResult GetIndexAdvert(string entId)
        {
            try
            {
                if (string.IsNullOrEmpty(entId))
                {
                    entId = BaseConfiguration.EntId;
                }
                DAL.ImgInfoDal dal = new DAL.ImgInfoDal();
                var model = dal.GetIndexAdvert(entId);
                if (model != null)
                {
                    return Json(new { success = true, list = model });
                }
                else
                {
                    return Json(new { success = false, list = model });
                }

            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Home/GetWebConfig", ex.Message.ToString());
                return Json(new { success = false, message = "网站广告图获取失败！" });
            }
        }
        #endregion


    }
}
