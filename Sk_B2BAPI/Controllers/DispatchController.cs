using Sk_B2BAPI.App_Code;
using Sk_B2BAPI.DAL;
using Sk_B2BAPI.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Sk_B2BAPI.Tool;

namespace Sk_B2BAPI.Controllers
{
    public class DispatchController : Controller
    {
        
        /// <summary>
        /// 获取配送方式信息
        /// </summary>
        /// <param name="enable">是否启用</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">单页条目</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult QueryDispatch(string enable="是")
        {
            try
            {
                DispatchInfoDal dal = new DispatchInfoDal();
                List<Dispatch> dispatch = dal.QueryPriceLevel(enable);
                return Json(new { success = true, list = dispatch});
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Dispatch/QueryDispatch", ex.Message.ToString());
                return Json(new { success = false, message = ex.Message });
            }
        }


        #region------已弃用------
        /*
        /// <summary>
        /// 保存配送方式
        /// </summary>
        /// <param name="dispatchName">配送方式</param>
        /// <param name="enable">是否启用</param>
        /// <returns>是否成功</returns>
        [HttpPost]
        public JsonResult SaveDispatcht(string dispatchName, string enable)
        {
            try
            {
                bool flag = false;
                DispatchInfoDal dal = new DispatchInfoDal();
                string result = dal.SavePriceLevel(dispatchName, enable, out flag);
                return Json(new { success = flag, message = result });
            }
            catch (Exception ex)
            {
                Log.Error("错误：保存配送方式", ex.Message);
                return Json(new { success = false, message = ex.Message });
            }
        }
        /// <summary>
        /// 维护配送方式
        /// </summary>
        /// <param name="dispatchId">配送方式编号</param>
        /// <param name="dispatchName">配送方式</param>
        /// <param name="enable">是否启用</param>
        /// <returns>是否成功</returns>
        [HttpPost]
        public JsonResult UpdateDispatch(string dispatchId, string dispatchName, string enable)
        {
            try
            {
                if (string.IsNullOrEmpty(dispatchId))
                {
                    return Json(new { success = false, message = "用户未登录，请先登录" });
                }
                else
                {
                    DispatchInfoDal dal = new DispatchInfoDal();
                    bool flag = false;
                    string result = dal.UpdatePriceLevel(dispatchId, dispatchName, enable, out flag);
                    return Json(new { success = flag, message = result });
                }
            }
            catch (Exception ex)
            {
                Log.Error("错误：维护配送方式", ex.Message);
                return Json(new { success = false, message = ex.Message });
            }
        }
        */
        #endregion

    }
}
