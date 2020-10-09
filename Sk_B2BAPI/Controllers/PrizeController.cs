using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Sk_B2BAPI.Tool;
using Sk_B2BAPI.Models;

namespace Sk_B2BAPI.Controllers
{
    public class PrizeController : SqlSugarHelper
    {
        /// <summary>
        /// 返回参与促销的内容的类型
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult PrizeType()
        {
            var list = new List<string>();
            list.AddRange(Enum.GetNames(typeof(EPrizeType)));

            var result = new ResponseModel<List<string>>()
            {
                Code = EReturnCode.Success,
                Source = list
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult PrizeOnly(string code)
        {
            var result = new ResponseModel<object>();
            if (string.IsNullOrEmpty(code))
            {
                result.Code = EReturnCode.ParamError;
                result.Message = "未获取到参数";
                return Json(result);
            }
            try
            {
                result.Code = EReturnCode.Success;
                result.Source = Db.Queryable<SC_T_Prize>().Where(d => d.BH == code).First();
            }
            catch (Exception ex)
            {
                result.Code = EReturnCode.SystemError;
                result.Message = $"异常：{ex.Message}";
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetPrizeList(string keyword, int pageSize, int pageIndex)
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
                    var total = 0;
                    var list = Db.Queryable<SC_T_Prize>()
                         .Where(d => d.PrizeName.Contains(keyword))
                         .ToPageList(pageIndex, pageSize, ref total)
                         .Select(d => new { d.BH, d.ImgUrl, d.PrizeCount, d.PrizeName, d.PrizeType, LastModifyTime = d.LastModifyTime.Value.ToString("yyyy-MM-dd HH:mm:ss") })
                         .ToList();

                    result.Code = EReturnCode.Success;
                    result.PageSize = pageSize;
                    result.PageIndex = pageIndex;
                    result.TotalCount = total;
                    result.Source = list;
                }
                catch (Exception ex)
                {
                    result.Code = EReturnCode.SystemError;
                    result.Message = ex.Message;
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeletePrize(string code)
        {
            var result = new ResponseModel<object>();
            if (string.IsNullOrEmpty(code))
            {
                result.Code = EReturnCode.ParamError;
                result.Message = "未获取到参数";
                return Json(result);
            }
            try
            {
                var list =
                        Db.Queryable<SC_T_LuckyDraw>()
                            .Where(
                                d =>
                                    d.Prize1 == code || d.Prize2 == code || d.Prize3 == code || d.Prize4 == code ||
                                    d.Prize5 == code || d.Prize6 == code || d.Prize7 == code || d.Prize8 == code)
                            .ToList();
                if (list.Count == 0)
                {
                    Db.Deleteable<SC_T_Prize>(d => d.BH == code).ExecuteCommand();
                    result.Code = EReturnCode.Success;
                }
                else
                {
                    result.Code = EReturnCode.SystemError;
                    result.Message = $"该奖品存在于{list[0].LotteryName}中";
                }
            }
            catch (Exception ex)
            {
                result.Code = EReturnCode.SystemError;
                result.Message = $"异常：{ex.Message}";
            }

            return Json(result);
        }

        public JsonResult UpdatePrize(SC_T_Prize model)
        {
            var result = new ResponseModel<object>();
            if (model == null)
            {
                result.Code = EReturnCode.ParamError;
                result.Message = "未获取到参数";
                return Json(result);
            }
            try
            {
                switch (model.PrizeType)
                {
                    case "优惠券":
                        var coupon = Db.Queryable<Zzsk_Coupons>().First(d => d.couponCode.ToString() == model.PrizeCount);
                        if (coupon == null)
                        {
                            result.Code = EReturnCode.ParamError;
                            result.Message = "无效的优惠券编号";
                            return Json(result);
                        }
                        break;
                    case "会员积分":
                        var reg = new Regex(@"^\d+$");
                        if (!reg.IsMatch(model.PrizeCount))
                        {
                            result.Code = EReturnCode.ParamError;
                            result.Message = "会员积分必须为正整数";
                            return Json(result);
                        }
                        break;
                }
                model.LastModifyTime = DateTime.Now;
                if (string.IsNullOrEmpty(model.BH))
                {
                    var list = Db.Queryable<SC_T_Prize>().Count();
                    model.BH = "JP" + (list + 1);
                    Db.Insertable(model).ExecuteCommand();
                }
                else
                {
                    Db.Updateable(model).ExecuteCommand();
                }
                result.Code = EReturnCode.Success;
            }
            catch (Exception ex)
            {
                result.Code = EReturnCode.SystemError;
                result.Message = $"异常：{ex.Message}";
            }

            return Json(result);
        }
    }
}