using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sk_B2BAPI.App_Code;
using Sk_B2BAPI.Tool;
using Sk_B2BAPI.Models;
using Sk_B2BAPI.DAL;

namespace Sk_B2BAPI.Controllers
{
    /// <summary>
    /// 签到
    /// </summary>
    public class SignInController : Controller
    {
        /// <summary>
        /// 签到
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SingInCalendar()
        {
            try
            {
                string result;
                string entId = Request.Params["entId"].ToString();
                
                string type = Request.Params["type"].ToString().Trim();
                SingInCalendarDal singInCalendarDal = new SingInCalendarDal();
                if (type == "ReturnSign")
                {
                    string userId = Request.Params["userId"].ToString();
                    if (string.IsNullOrEmpty(userId))
                    {
                        return Content(JsonHelper.GetErrorJson(3, 0, "请先登录后再签到"), "json");
                    }
                    else
                    {
                        result = singInCalendarDal.ReturnSign(entId, userId);
                    }
                }
                else if (type == "QSign")
                {
                    string userId = Request.Params["userId"].ToString();
                    if (string.IsNullOrEmpty(userId))
                    {
                        return Content(JsonHelper.GetErrorJson(3, 0, "请先登录"), "json");
                    }
                    else
                    {
                        result = singInCalendarDal.QSign(entId, userId);
                    }
                }
                else if (type == "Rule")
                {
                    result = singInCalendarDal.Rule(entId);
                }else if (type == "RecordSign")
                {
                    string userId = Request.Params["userId"].ToString();
                    if (string.IsNullOrEmpty(userId))
                    {
                        return Content(JsonHelper.GetErrorJson(3, 0, "请先登录"), "json");
                    }
                    else
                    {
                        result = singInCalendarDal.RecordSign(entId, userId);
                    }
                }
                else
                {
                    return Content(JsonHelper.GetErrorJson(1, 0, "操作类型type无效"), "json");
                }
                return Content(result, "json");
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "IntegralStore/IntegralCart", ex.Message.ToString());
                return Content(JsonHelper.GetErrorJson(1, 0, ex.Message.ToString()), "json");
            }
        }
    }
}