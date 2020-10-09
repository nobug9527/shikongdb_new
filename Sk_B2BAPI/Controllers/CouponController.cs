using Sk_B2BAPI.App_Code;
using Sk_B2BAPI.DAL;
using Sk_B2BAPI.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Sk_B2BAPI.Tool;

namespace Sk_B2BAPI.Controllers
{
	public class CouponController : Controller
	{
		#region 用户优惠券列表
		/// <summary>
		/// 用户优惠券列表
		/// </summary>
		/// <param name="entId">机构</param>
		/// <param name="userId">用户</param>
		/// <returns></returns>
		[HttpPost]
		public JsonResult CouponList(string entId, string userId, string source, int pageIndex = 1, int pageSize = 15, int enable = 0)
		{
			try
			{
				var list = new List<Coupon>();
				CouponDal dal = new CouponDal();
				list = dal.UserCouponList(entId, userId, source,pageIndex, pageSize, enable, out int recordCount, out int pageCount);
				return Json(new { success = true, list = list, recordCount = recordCount, pageCount = pageCount });
			}
			catch (Exception ex)
			{
				LogQueue.Write(LogType.Error, "Coupon/CouponList", ex.Message.ToString());
				return Json(new { success = false, message = "用户优惠券列表加载失败！" });
			}
		}
		#endregion

		#region 领券中心优惠券列表
		/// <summary>
		/// 领券中心优惠券列表
		/// </summary>
		/// <param name="pageIndex">当前页</param>
		/// <param name="pageSize">每页条目</param>
		/// <returns></returns>
		[HttpPost]
		public JsonResult CouponRedemptionCentre(string userId,string entId,string source, int pageIndex = 1, int pageSize = 15)
		{
			try
			{
				if (string.IsNullOrEmpty(userId))
				{
					return Json(new { success = false, message = "请求优惠券列表,异常空参！" });
				}
				var list = new List<CouponCentre>();
				CouponDal dal = new CouponDal();
				list = dal.CouponList(pageIndex, pageSize,userId,entId,source, out int pageCount, out int recordCount);
				return Json(new { success=true,list=list, recordCount = recordCount, pageCount = pageCount });
			}
			catch (Exception ex)
			{
				LogQueue.Write(LogType.Error, "Coupon/CouponRedemptionCentre", ex.Message.ToString());
				return Json(new { success = false, message ="领券中心优惠券列表加载失败！" });
			}
		}
		#endregion

		#region 领取优惠券
		/// <summary>
		/// 领取优惠券
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="entId"></param>
		/// <param name="couponCode"></param>
		/// <returns></returns>
		public JsonResult GetCoupon(string userId,string entId,int couponCode)
		{
			try
			{
				if (string.IsNullOrEmpty(userId))
				{
					return Json(new { success=false,message= "用户未登录，请先登录" });
				}
				CouponDal dal = new CouponDal();
				string message = dal.GetCoupon(userId, entId, couponCode, out bool flag);
				return Json(new {success=flag,message=message });
			}
			catch (Exception ex)
			{
				LogQueue.Write(LogType.Error, "Coupon/GetCoupon", ex.Message.ToString());
				return Json(new { success = false, message = "用户领取优惠券失败！" });
			}
		}
		#endregion

		#region 结算可用优惠券
		/// <summary>
		/// 结算可用优惠券
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="entId"></param>
		/// <param name="goodsList"></param>
		/// <returns></returns>
		public JsonResult UsableCoupon(string userId,string entId,string goodsList,int pageIndex,int pageSize,string channelName,string ywyId="")
		{
			try
			{
				if (string.IsNullOrEmpty(userId))
				{
					return Json(new { success = false, message = "用户未登录，请先登录" });
				}
				///获取用户信息
				UserInfoDal udal = new UserInfoDal();
				List<UserInfo> user = udal.GetUserInfo(userId, entId);
				if (user.Count <= 0)
				{
					return Json(new { success = false, message = "E002" });
				}
				///获取金额信息
				OrderInfoDal cdal = new OrderInfoDal();
				OrderAmount Amount = cdal.OrderOriginalAmount(user[0].EntId, userId, goodsList, user[0].Pricelevel,user[0].KhType,"","", ywyId)[0];
				
				decimal OrdersAmount = Amount.OrdersAmount;
				decimal RealAmount = Amount.RealAmount;
				decimal DiscountAmount = Amount.DiscountAmount;
				decimal PtAmount = Amount.PtAmount;
				var dal = new CouponDal();
				List<UserCoupon> list = dal.UsableCoupon(userId, entId, goodsList,pageIndex,pageSize, ywyId, out int pageCount, out int recordCount, channelName, user[0].Pricelevel,user[0].KhType, OrdersAmount, RealAmount,DiscountAmount,PtAmount);
				return Json(new {success=true,List= list,pageCount=pageCount,recordCount=recordCount });
			}
			catch (Exception ex)
			{
				LogQueue.Write(LogType.Error, "Coupon/UsableCoupon", ex.Message.ToString());
				return Json(new { success = false, message = "结算可用优惠券请求失败！" });
			}
		}
        #endregion
    }
}