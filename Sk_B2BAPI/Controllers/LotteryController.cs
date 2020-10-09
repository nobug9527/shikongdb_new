using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using SqlSugar;
using Sk_B2BAPI.Tool;
using Sk_B2BAPI.Models;

namespace Sk_B2BAPI.Controllers
{
    public class LotteryController : SqlSugarHelper
    {
        /// <summary>
        /// 抽奖次数
        /// </summary>
        /// <param name="userId">用户</param>
        /// <param name="entId">机构</param>
        /// <returns>返回抽奖次数</returns>
        [HttpPost]
        public JsonResult LotteryCount(string userId,string entId)
        {
            var result = new ResponseModel<object>();
            try
            {
                result.Source = Db.Queryable<SC_T_LuckyHis>().Count(d => d.UserID == userId && d.EntID==entId && SqlFunc.IsNullOrEmpty(d.PrizeBH));
                result.Code = EReturnCode.Success;
            }
            catch (Exception ex)
            {
                result.Code = EReturnCode.SystemError;
                result.Message = $"异常：{ex.Message}";
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LotteryHis(string keyword, string selectType, int pageIndex, int pageSize, string start, string end)
        {
            var result = new ResponseModel<object>();
            keyword = keyword ?? "";

            if (pageIndex <= 0 || pageSize <= 0)
            {
                result.Code = EReturnCode.ParamError;
                result.Message = "无效的参数:页码相关的参数必须为正整数";
            }
            else
            {
                try
                {
                    var startTime = DateTime.Parse(start);

                    var endTime = DateTime.Parse(end + " 23:59:59");
                    var total = 0;
                    List<SC_T_LuckyHis> list;
                    if (selectType == "All")
                    {
                        list = Db.Queryable<SC_T_LuckyHis>().Where(d => (d.PrizeName.Contains(keyword) || d.UserName.Contains(keyword)) && d.LuckyTime >= startTime && d.LuckyTime <= endTime).ToPageList(pageIndex, pageSize, ref total).ToList();
                    }
                    else
                    {
                        list = Db.Queryable<SC_T_LuckyHis>().Where(d => d.PrizeType != "空奖" && (d.PrizeName.Contains(keyword) || d.UserName.Contains(keyword)) && d.LuckyTime >= startTime && d.LuckyTime <= endTime).ToPageList(pageIndex, pageSize, ref total).ToList();
                    }
                    result.Source = list.Select(d => new { d.PrizeBH, d.ImgUrl, LuckyTime = d.LuckyTime.Value.ToString("yyyy-MM-dd HH:mm:ss"), d.PrizeCount, d.PrizeName, d.PrizeType, d.UserName, d.UserID, d.ID });
                    result.PageIndex = pageIndex;
                    result.PageSize = pageSize;
                    result.TotalCount = total;
                    result.Code = EReturnCode.Success;
                }
                catch (Exception ex)
                {
                    result.Code = EReturnCode.SystemError;
                    result.Message = $"异常：{ex.Message}";
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 获奖名单
        /// </summary>
        /// <returns>返回中奖名单</returns>
        [HttpPost]
        public JsonResult LotteryHisPre()
        {
            var result = new ResponseModel<object>();
            try
            {
                var model1 = Db.Queryable<SC_T_Lottery>().First(d => d.isActive == "是" && SqlFunc.IsNullOrEmpty(d.UserID) && SqlFunc.IsNullOrEmpty(d.EntID));
                //LogQueue.Write(LogType.Debug, "model1", model1.ExpirationTime.ToString());
                if (model1 != null)
                {
                    var model = Db.Queryable<SC_T_LuckyDraw>().First();
                    var list = (from info in model.GetType().GetProperties() where info.Name.Length == 6 select info.GetValue(model, null).ToString()).ToList();

                    //var a = Db.Queryable<SC_T_LuckyHis>().Where(d => d.PrizeType == "优惠券" && d.PrizeBH == "JP4").First();
                    //LogQueue.Write(LogType.Debug, "a", a.LuckyTime.ToString());

                    //result.Source = Db.Queryable<SC_T_LuckyHis>().Where(d => d.PrizeType != "空奖" && list.Contains(d.PrizeBH) && d.LuckyTime >= model1.UpdateTime).ToList();
                    result.Source = Db.Queryable<SC_T_LuckyHis>().Where(d => d.PrizeType != "空奖" && list.Contains(d.PrizeBH) && d.LuckyTime <= model1.ExpirationTime).ToList();
                    result.Code = EReturnCode.Success;
                }
                else
                {
                    result.Code = EReturnCode.SystemError;
                    result.Message = $"暂无活动";
                }
            }
            catch (Exception ex)
            {
                result.Code = EReturnCode.SystemError;
                result.Message = $"异常：{ex.Message}";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 用户抽奖记录
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="entId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult LotteryRecord(string userId,string entId)
        {
            var result = new ResponseModel<object>();
            try
            {
                var model = Db.Queryable<SC_T_LuckyHis>().Where(d=>d.UserID==userId && d.EntID==entId && d.PrizeType!="空奖" && d.PrizeType!=null ).ToList();
                result.Code = EReturnCode.Success;
                result.Source = model;
            }
            catch (Exception ex)
            {
                result.Code = EReturnCode.SystemError;
                result.Message = $"异常：{ex.Message}";
            }
            return Json(result,JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 抽奖
        /// </summary>
        /// <param name="userId">用户</param>
        /// <param name="entId">机构</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Lottery(string userId,string entId)
        {
            var result = new ResponseModel<object>();

            try
            {
                //LogQueue.Write(LogType.Debug, "进入1", userId.ToString());
                var userModel = Db.Queryable<dt_users>().First(d => d.userid.ToString() == userId && d.entid.ToString()==entId);
                //LogQueue.Write(LogType.Debug, "dt_users2", userId.ToString());
                if (userModel == null)
                {
                    result.Code = EReturnCode.SystemError;
                    result.Message = $"无效的用户";
                    return Json(result);
                }
                Db.Ado.BeginTran();
                var count = Db.Queryable<SC_T_LuckyHis>().Count(d => d.UserID == userId && d.EntID==entId && SqlFunc.IsNullOrEmpty(d.PrizeBH));
                //LogQueue.Write(LogType.Debug, "SC_T_LuckyHis3", userId.ToString());
                if (count <= 0)
                {
                    result.Code = EReturnCode.SystemError;
                    result.Message = $"可抽奖次数不足";
                }
                else
                {
                    var model = Db.Queryable<SC_T_Lottery>().First(d => d.isActive == "是" && SqlFunc.IsNullOrEmpty(d.UserID) && SqlFunc.IsNullOrEmpty(d.EntID));
                    //LogQueue.Write(LogType.Debug, "SC_T_Lottery4", userId.ToString());
                    if (model == null)
                    {
                        result.Code = EReturnCode.SystemError;
                        result.Message = $"可抽奖次数不足";
                    }
                    else
                    {
                        var lucky =
                            Db.Queryable<SC_T_LuckyHis>()
                                .First(d => d.UserID == userId && d.EntID==entId && SqlFunc.IsNullOrEmpty(d.PrizeBH));
                        //LogQueue.Write(LogType.Debug, "SC_T_LuckyHis5", userId.ToString());
                        if (lucky == null)
                        {
                            result.Code = EReturnCode.SystemError;
                            result.Message = $"抽奖活动已结束";
                        }
                        else
                        {
                            var prize = Db.Queryable<SC_T_Prize>().First(d => d.BH == model.PrizeBH);
                            //LogQueue.Write(LogType.Debug, "SC_T_Prize6", userId.ToString());
                            model.isActive = "否";
                            model.UpdateTime = DateTime.Now;
                            model.UserID = userId;
                            model.EntID = entId;

                            if (prize.PrizeType == "会员积分")
                            {
                                //LogQueue.Write(LogType.Debug, "会员积分1", prize.PrizeType.ToString());
                                userModel.point += int.Parse(prize.PrizeCount);
                                //LogQueue.Write(LogType.Debug, "会员积分2", prize.PrizeCount.ToString());
                                Db.Updateable(userModel).ExecuteCommand();
                                //LogQueue.Write(LogType.Debug, "会员积分3", "更新");
                                Db.Insertable(new dt_JFLS()
                                {
                                    rq = DateTime.Now.ToString("yyyy-MM-dd"),
                                    ontime = DateTime.Now.ToString("HH:mm:ss"),
                                    userid = userId,
                                    lcjf = prize.PrizeCount,
                                    jyjf = userModel.point.ToString(),
                                    beizhu = $"抽奖获得{prize.PrizeCount}积分"
                                }).ExecuteCommand();
                                //LogQueue.Write(LogType.Debug, "会员积分4", "插入");
                            }
                            if (prize.PrizeType == "优惠券")
                            {
                                //LogQueue.Write(LogType.Debug, "优惠券1", prize.PrizeType.ToString());
                                var coupon = Db.Queryable<Zzsk_Coupons>().First(d=>d.couponCode==int.Parse(prize.PrizeCount));
                                //LogQueue.Write(LogType.Debug, "Zzsk_Coupons7", userId.ToString());
                                if (coupon==null)
                                {
                                    result.Code = EReturnCode.ParamError;
                                    result.Message = "无效的优惠券编号";
                                    Db.Ado.RollbackTran();
                                    return Json(result);
                                }

                                var userCoupon = new Zzsk_UserCoupon()
                                {
                                    CouponId=coupon.couponCode,
                                    ReceiveTime=DateTime.Now.ToString(),
                                    EndTIme=coupon.endTime,
                                    UseTime="",
                                    OrderId="",
                                    Status=0,
                                    Remarks="",
                                    UserId=userModel.userid.ToString(),
                                    entid=userModel.entid.ToString()
                                };
                                Db.Insertable(userCoupon).ExecuteCommand();
                            }

                            lucky.PrizeBH = model.PrizeBH;
                            lucky.ImgUrl = prize.ImgUrl;
                            lucky.LuckyTime = DateTime.Now;
                            lucky.PrizeCount = prize.PrizeCount;
                            lucky.PrizeName = prize.PrizeName;
                            lucky.PrizeType = prize.PrizeType;
                            lucky.UserName = userModel.businessname;

                            Db.Updateable(model).ExecuteCommand();
                            Db.Updateable(lucky).ExecuteCommand();

                            result.Source = prize.BH;
                            result.Code = EReturnCode.Success;
                        }
                    }
                }

                Db.Ado.CommitTran();
            }
            catch (Exception ex)
            {
                Db.Ado.RollbackTran();
                result.Code = EReturnCode.SystemError;
                result.Message = $"异常：{ex.Message}";
            }

            return Json(result);
        }


    }
}