using Sk_B2BAPI.App_Code;
using Sk_B2BAPI.DAL;
using Sk_B2BAPI.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Sk_B2BAPI.Tool;

namespace Sk_B2BAPI.Controllers
{
    public class EntdocController : Controller
    {
        /// <summary>
        /// 获取机构
        /// </summary>
        /// <returns></returns>
        public JsonResult GetEntdoc()
        {
            try
            {
                //获取数据
                EntdocDal dal = new EntdocDal();
                List<Entdoc> entdocs = dal.EntdocList();
                return Json(new { success = true, list = entdocs});
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Entdoc/GetEntdoc", ex.Message.ToString());
                return Json(new { success = false, message = "机构列表加载失败！" });
            }
        }
        /// <summary>
        /// 获取所有的机构，带分页
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetEntdocListPager(int PageIndex, int PageSize, string SouStr)
        {
            EntdocDal dal = new EntdocDal();
            int total = 0;
            var list = dal.GetEntdocListPager(PageIndex, PageSize, SouStr, ref total);
            var datalist = new { data = list, total = total };
            return Json(datalist);
        }

    }
}
