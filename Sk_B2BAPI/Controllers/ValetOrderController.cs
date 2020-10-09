using Sk_B2BAPI.Models;
using Sk_B2BAPI.Tool;
using Sk_B2BAPI.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sk_B2BAPI.App_Code;

namespace Sk_B2BAPI.Controllers
{
    public class ValetOrderController : Controller
    {
        /// <summary>
        /// 代客下单获取客户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public JsonResult GetMemberList(string ywyId, string strWhere, int pageIndex, int pageSize)
        {
            ValetOrderModel<ValetOrder_MemberList> model = new ValetOrderModel<ValetOrder_MemberList>();
            try
            {
                if (string.IsNullOrEmpty(ywyId))
                {
                    model.Success = "002";
                    model.Message = "用户未登录，请先登录";
                }
                else
                {
                    ValetOrderDal dal = new ValetOrderDal();
                    model = dal.GetMemberList(ywyId, strWhere, pageIndex, pageSize);
                }
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "ValetOrder/GetMemberList", ex.Message.ToString());
                model.Success = "002";
                model.Message = ex.Message.ToString();

            }
            return Json(new { models = model });
        }
        /// <summary>
        /// 代客下单获取搜索页商品列表
        /// </summary>
        /// <param name="userId">客户Id</param>
        /// <param name="searchValue">搜索类容</param>
        /// <param name="letter">厂家首字母</param>
        /// <param name="tags">排序</param>
        /// <param name="isKc">是否有货</param>
        /// <param name="CategoryId">分类Id</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页容量</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetGoodsList(string ywyId, string userId, string entId, string searchValue, string letter, string tags, string isKc, string CategoryId, int pageIndex, int pageSize)
        {
            ValetOrderModel<GoodsInfo> model = new ValetOrderModel<GoodsInfo>();
            try
            {
                if (string.IsNullOrEmpty(entId))
                {
                    entId = BaseConfiguration.EntId;
                }
                else if (ywyId == null || ywyId == "")
                {
                    model.Success = "002";
                    model.Message = "业务员Id获取失败";
                }
                else
                {
                    if (searchValue == null)
                    {
                        searchValue = "";
                    }
                    ///商品信息
                    ValetOrderDal dal = new ValetOrderDal();
                    model = dal.GetGoodsList(ywyId, userId, searchValue, letter, tags, isKc, CategoryId, pageIndex, pageSize, entId);
                }
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "ValetOrder/GetGoodsList", ex.Message.ToString());
                model.Success = "002";
                model.Message = ex.Message.ToString();
            }
            return Json(new { models = model });
        }
    }
}