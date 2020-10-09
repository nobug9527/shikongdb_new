using Sk_B2BAPI.App_Code;
using Sk_B2BAPI.DAL;
using Sk_B2BAPI.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Sk_B2BAPI.Tool;

namespace Sk_B2BAPI.Controllers
{
    public class CLPriceController : Controller
    {
        /// <summary>
        /// 保存大宗商品阶梯价格
        /// </summary>
        /// <param name="leveName">价格级别</param>
        /// <param name="enable">是否启用</param>
        /// <returns>是否成功</returns>
        [HttpPost]
        public JsonResult SaveCLPrice(string entid, int article_id, string pricelevel, decimal price)
        {
            try
            {
                bool flag = false;
                CLPriceDal dal = new CLPriceDal();
                string result = dal.SaveCLPrice(entid,article_id,pricelevel,price,out flag);
                return Json(new { success = flag, message = result });
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "CLPrice/SaveCLPrice", ex.Message.ToString());
                return Json(new { success = false, message = ex.Message });
            }
        }
        /// <summary>
        /// 获取大宗商品阶梯价格信息
        /// </summary>
        /// <param name="entid">机构ID</param>
        /// <param name="article_id">文章ID</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult QueryCLPrice(string entid, int article_id)
        {
            try
            {
                CLPriceDal dal = new CLPriceDal();
                List<CLPrice> cLPrices = dal.QueryCLPrice(entid, article_id);
                return Json(new { success = true, list = cLPrices });
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "CLPrice/QueryCLPrice", ex.Message.ToString());
                return Json(new { success = false, message = ex.Message });
            }
        }
        /// <summary>
        /// 维护大宗商品阶梯价格
        /// </summary>
        /// <param name="levelId">价格级别编号</param>
        /// <param name="leveName">价格级别</param>
        /// <param name="enable">是否启用</param>
        /// <returns>是否成功</returns>
        [HttpPost]
        public JsonResult UpdateCLPrice(string entid, int article_id, string pricelevel, decimal price)
        {
            try
            {
                if (string.IsNullOrEmpty(entid))
                {
                    return Json(new { success = false, message = "用户未登录，请先登录" });
                }
                else
                {
                    CLPriceDal dal = new CLPriceDal();
                    bool flag = false;
                    string result = dal.UpdateCLPrice(entid,article_id,pricelevel,price, out flag);
                    return Json(new { success = flag, message = result });
                }
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "CLPrice/UpdateCLPrice", ex.Message.ToString());
                return Json(new { success = false, message = ex.Message });
            }
        }

    }
}
