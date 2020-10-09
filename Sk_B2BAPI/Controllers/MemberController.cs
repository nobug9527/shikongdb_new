using Sk_B2BAPI.DAL;
using Sk_B2BAPI.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sk_B2BAPI.Controllers
{
    public class MemberController : Controller
    {
        #region======================前台会员管理======================================
        /// <summary>
        /// 会员申请机构
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="entId">机构Id</param>
        /// <param name="status">状态 0/1</param>
        /// <returns></returns>
        public JsonResult SaveMerchants(string userId, string entIdList, int status)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { success = false, message = "请登陆后再提交申请" });
                }
                else if (string.IsNullOrEmpty(entIdList))
                {
                    return Json(new { success = false, message = "请选择机构" });
                }
                if (status != 0 && status != 1)
                {
                    return Json(new { success = false, message = "status参数错误" });
                }
                MemberDal dal = new MemberDal();
                bool flag = dal.SaveMerchantsDal(userId, entIdList, status);
                string msg = "存盘失败";
                if (flag)
                {
                    msg = "存盘成功";
                }
                return Json(new { success = flag, message = msg });
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Member/SaveMerchants", ex.Message.ToString());
                return Json(new { success = false, message = ex.Message.ToString() });
            }
        }
        ///查询已申请机构
        ///
        #endregion
    }
}