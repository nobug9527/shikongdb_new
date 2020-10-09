using Sk_B2BAPI.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sk_B2BAPI.Controllers
{
    /// <summary>
    /// 小组相关功能
    /// </summary>
    public class GroupController : Controller
    {
        #region 创建小组
        
        public JsonResult AddUserGroup()
        {
            try
            {
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Home/AddUserGroup", ex.Message.ToString());
                return Json(new { success = false, message = "创建小组失败！" });
            }
        }
        #endregion

        #region 编辑小组

        #endregion

        #region 解散小组

        #endregion

        #region 邀请组队

        #endregion

        #region 同意组队

        #endregion

        #region 拒绝组队

        #endregion

        #region 开放小组登录

        #endregion

        #region 开放小组流水共享

        #endregion

        #region 获取小组信息及成员

        #endregion

        #region 踢出小组成员

        #endregion
    }
}