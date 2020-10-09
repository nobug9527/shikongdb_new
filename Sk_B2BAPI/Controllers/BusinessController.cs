using Sk_B2BAPI.App_Code;
using Sk_B2BAPI.DAL;
using Sk_B2BAPI.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Sk_B2BAPI.Tool;

namespace Sk_B2BAPI.Controllers
{
    public class BusinessController : Controller
    {
        /// <summary>
        /// 获取单位
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="entId"></param>
        /// <param name="strWhere"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="isjh"></param>
        /// <param name="isxs"></param>
        /// <returns></returns>
        public JsonResult GetBusiness(string userId,string entId,string strWhere, int pageIndex, int pageSize,string isjh,string isxs)
        {
            try
            {
                //获取数据
                BusinessDal dal = new BusinessDal();
                if (userId=="")//注册获取单位
                {
                    //所有单位列表
                    List<Business> businesses = dal.BusnessList(userId, entId, isjh, isxs, strWhere, pageIndex, pageSize, out int pageCount, out int recordCount);

                    return Json(new { success = true, list = businesses, pageCount = pageCount, recordCount = recordCount });
                }
                else//代课下单获取单位
                {
                    //代课下单客户
                    List<Business> client= dal.ClientList(userId, entId, isjh, isxs, strWhere, pageIndex, pageSize, out int pageCount1, out int recordCount1);
                    var objClient = new { list = client, pageCount = pageCount1, recordCount = recordCount1 };

                    //代课下单购物车有商品客户
                    List<Business> cartClient= dal.CartClientList(userId, entId, isjh, isxs, strWhere, pageIndex, pageSize, out int pageCount2, out int recordCount2);
                    var objCartClient = new { list = cartClient, pageCount = pageCount2, recordCount = recordCount2 };

                    return Json(new { success = true, client= objClient, cartClient= objCartClient });
                }                
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Business/GetBusinessDal", ex.Message);
                return Json(new { success = false, message = "客户列表加载失败！" });
            }
        }

        /// <summary>
        /// 获取客户类型
        /// </summary>
        /// <returns></returns>
        public JsonResult ClientType()
        {
            try
            {
                BusinessDal businessDal = new BusinessDal();
                List<ClintType> list = businessDal.ClientType();
                return Json(new { success = true, list = list });
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Business/ClientType", ex.Message.ToString());
                return Json(new { success = false, message = "客户类型获取失败！" });
            }
        }

        /// <summary>
        /// 获取资料类型
        /// </summary>
        /// <param name="customerType">客户类型</param>
        /// <returns></returns>
        public JsonResult MaterialType(string customerType)
        {
            try
            {
                BusinessDal businessDal = new BusinessDal();
                List<UserMaterial> list = businessDal.MaterialType(customerType);
                return Json(new { success = true, list = list });
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Business/MaterialType", ex.Message.ToString());
                return Json(new { success = false, message = "资料类型获取失败！" });
            }
        }
    }
}
