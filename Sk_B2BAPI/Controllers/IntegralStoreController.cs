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
    /// 积分商城
    /// </summary>
    public class IntegralStoreController : Controller
    {
        /// <summary>
        /// 积分商城首页楼层及楼层所有数据
        /// </summary>
        /// <param name="entId">机构</param>
        /// <param name="flootType">楼层 Office【办公用品】/Electrical【家用电器】/Supplies【生活用品】/HotFor【热门兑换】亦可传空获取所有楼层，但传空时num>0</param>
        /// <param name="num">前n条</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="pageSize">单页条目数</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult IntegralFloor(string entId,string flootType,int num=0,int pageIndex=1,int pageSize=15)
        {
            try
            {
                if (string.IsNullOrEmpty(entId))
                {
                    entId = BaseConfiguration.EntId.ToString();
                }
                if (flootType==""&&num<=0)
                {
                    return Json(new { success = false, message = "获取所有楼层商品信息时，参数num必须大于0" });
                }
                else if (flootType != "" && num > 0)
                {
                    return Json(new { success = false, message = "获取单一楼层所有商品信息时，参数num必须等于0" });
                }
                if (num>0)
                {
                    pageIndex = 1;
                    pageSize = num;
                }
                
                IntegralStoreDal interceptDal = new IntegralStoreDal();
                List<IndexFloor> integralGoods = interceptDal.IntegralFloor(entId, flootType, num, pageIndex, pageSize, out int pageCount, out int recordCount);

                return Json(new { success = true, pageCount= pageCount, recordCount= recordCount,list= integralGoods });
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "IntegralStore/IntegralFloor", ex.Message.ToString());
                return Json(new { success = false, message = ex.Message.ToString() });
            }
        }

        /// <summary>
        /// PC楼层下一批
        /// </summary>
        /// <param name="entId"></param>
        /// <param name="flootType"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult NextGroup(string entId, string flootType,int pageIndex,int pageSize)
        {
            try
            {
                if (string.IsNullOrEmpty(entId))
                {
                    entId = BaseConfiguration.EntId.ToString();
                }
                IntegralStoreDal interceptDal = new IntegralStoreDal();
                List<ImgInfo> imgInfos = interceptDal.NextGroup(entId, flootType, pageIndex, pageSize, out int pageCount, out int recordCount);
                return Json(new { success = true, pageCount = pageCount, recordCount = recordCount, list = imgInfos });
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "IntegralStore/NextGroup", ex.Message.ToString());
                return Json(new { success = false, message = ex.Message.ToString() });
            }
        }

        /// <summary>
        /// 积分商品详情
        /// </summary>
        /// <param name="entId">机构</param>
        /// <param name="goodsId">积分商品Id</param>
        /// <returns></returns>
        public ActionResult IntegralGoods(string entId,string goodsId)
        {
            try
            {
                if (string.IsNullOrEmpty(entId))
                {
                    entId = BaseConfiguration.EntId.ToString();
                }
                if (string.IsNullOrEmpty(goodsId))
                {
                    return Content(JsonHelper.GetErrorJson(3, 0, "商品Id不能为空"), "json");
                }
                
                IntegralStoreDal integralStoreDal = new IntegralStoreDal();
                string json = integralStoreDal.IntegralGoods(entId, goodsId);
                return Content(json, "json");
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "IntegralStore/IntegralGoods", ex.Message.ToString());
                return Content(JsonHelper.GetErrorJson(1, 0, ex.Message.ToString()), "json");
            }
        }

        /// <summary>
        /// 积分购物车
        /// </summary>
        /// <param name="type">操作类型</param>
        /// <param name="userId">用户</param>
        /// <param name="entId">机构</param>
        /// <param name="goodsId">积分商品</param>
        /// <param name="num">数目</param>
        /// <returns></returns>
        public ActionResult IntegralCart()
        {
            try
            {
                string result;
                string userId = Request.Params["userId"].ToString();
                string entId = Request.Params["entId"].ToString();
                string type = Request.Params["type"].ToString();
                if (string.IsNullOrEmpty(userId))
                {
                    return Content(JsonHelper.GetErrorJson(3, 0, "请登录后再加入礼品车"), "json");
                }
                IntegralStoreDal integralStoreDal = new IntegralStoreDal();
                if (type == "addgwc" || type == "delGoods" || type == "changeNum")
                {
                    string goodsId = Request.Params["goodsId"].ToString();
                    decimal num = decimal.Parse(Request.Params["num"].ToString());
                    int Temp = integralStoreDal.AddGwc(type, userId, goodsId, entId,num);
                    string msg;
                    if (Temp > 0)
                    {
                        switch (type)
                        {
                            case "addgwc": msg = "礼品车添加成功"; break;
                            case "delGoods": msg = "礼品车商品删除成功"; break;
                            case "changeNum": msg = "礼品车商品修改成功"; break;
                            default: msg = "未知操作"; break;
                        }
                        result = JsonHelper.GetErrorJson(0, 0, msg);
                    }
                    else
                    {
                        switch (type)
                        {
                            case "addgwc": msg = "礼品车添加失败"; break;
                            case "delGoods": msg = "礼品车商品删除失败"; break;
                            case "changeNum": msg = "礼品车商品修改失败"; break;
                            default: msg = "未知操作"; break;
                        }
                        result = JsonHelper.GetErrorJson(1, 0, msg); 
                    }
                    return Content(result, "json");
                }
                if (type == "getAddress" || type == "getGwc")
                {
                    string msg = integralStoreDal.GetAddress(type, userId, entId);
                    return Content(msg, "json");
                }
                else
                {
                    result = JsonHelper.GetErrorJson(1, 0, "操作类型type无效"); 
                    return Content(result, "json");
                }
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "IntegralStore/IntegralCart", ex.Message.ToString());
                return Content(JsonHelper.GetErrorJson(1, 0, ex.Message.ToString()), "json");
            }
        }

        /// <summary>
        /// 积分个人中心
        /// </summary>
        /// userId：用户
        /// entId：机构
        /// type:getTJ【推荐商品每页四条数据】/type:getJF【获取积分】/type:getJFLS【积分流水】/type:getDD【订单】/type:ddmx【订单明细】
        /// type:getTJ/getJFLS/getDD时，page：当前页数
        /// type:ddmx时，djbh：单据编号
        /// <returns></returns>
        public ActionResult IntegralCenter()
        {
            try
            {
                string result; 
                string userId = Request.Params["userId"].ToString();
                string entId = Request.Params["entId"].ToString();
                if (string.IsNullOrEmpty(userId))
                {
                    return Content(JsonHelper.GetErrorJson(3, 0, "请登录后再加入礼品车"), "json");
                }
                string type = Request.Params["Type"].ToString().Trim();
                IntegralStoreDal integralStoreDal = new IntegralStoreDal();
                if (type == "getTJ")
                {
                    int page = int.Parse(Request.Params["page"].ToString());
                    result = integralStoreDal.GetGoods(type,entId,page);
                    return Content(result, "json");
                }
                else if (type == "getJF")
                {
                    result = integralStoreDal.GetJF( type, userId,entId);
                    return Content(result, "json");
                }
                else if (type == "getJFLS" || type == "getDD" )
                {
                    int page = int.Parse(Request.Params["page"].ToString());
                    int size = int.Parse(Request.Params["size"].ToString());
                    result = integralStoreDal.GetMsg(type, userId,entId, page,size);
                    return Content(result, "json");
                }
                else if (type == "ddmx")
                {
                    string djbh = Request.Params["djbh"].ToString().Trim();
                    result = integralStoreDal.GetOrderDetail(type, userId, entId, djbh);
                    return Content(result, "json");
                }
                else
                {
                    return Content(JsonHelper.GetErrorJson(1, 0, "操作类型type无效"), "json");
                }
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "IntegralStore/IntegralCart", ex.Message.ToString());
                return Content(JsonHelper.GetErrorJson(1, 0, ex.Message.ToString()), "json");
            }

        }

        /// <summary>
        /// 积分购车车结算
        /// </summary>
        /// userId：用户，entId：机构，goodsid ：','拼接的积分商品id字符串，num：积分商品数量，total：积分商品总积分
        /// <returns></returns>
        public ActionResult SettleAccounts()
        {
            try
            {
                string result;
                string userId = Request.Params["userId"].ToString();
                string entId = Request.Params["entId"].ToString();
                if (string.IsNullOrEmpty(userId))
                {
                    return Content(JsonHelper.GetErrorJson(1, 0, "请登录后再加入礼品车"), "json");
                }
                string address = Request.Params["address"].ToString();
                string contact = Request.Params["contact"].ToString();//联系人
                string phone = Request.Params["phone"].ToString();//联系电话
                string remarks = Request.Params["remarks"].ToString();//备注
                string goodsidStr = Request.Params["goodsid"].ToString().Trim();
                int num = int.Parse(Request.Params["num"].ToString());
                string zjf = Request.Params["total"].ToString().Trim();
                IntegralStoreDal integralStoreDal = new IntegralStoreDal();
                result = integralStoreDal.SettleAccounts(goodsidStr, num, userId, entId, zjf,address,contact,phone,remarks);
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