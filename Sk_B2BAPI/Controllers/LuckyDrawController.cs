using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Sk_B2BAPI.Models;
using Sk_B2BAPI.Tool;

namespace Sk_B2BAPI.Controllers
{
    public class LuckyDrawController : SqlSugarHelper
    {
        /// <summary>
        /// 抽奖商品及规则
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetLuckyDraw()
        {
            var result = new ResponseModel<object>();

            try
            {
                var model = Db.Queryable<SC_T_LuckyDraw>().First();
                if (model == null) {
                    throw new Exception("请先添加抽奖商品");
                }
                result.Code = EReturnCode.Success;
                result.Source = new
                {
                    ExpirationTime = model.ExpirationTime.Value.ToString("yyyy-MM-dd"),
                    model.ID,
                    model.Information,
                    model.LotteryName,
                    model.Prize1,
                    model.Prize2,
                    model.Prize3,
                    model.Prize4,
                    model.Prize5,
                    model.Prize6,
                    model.Prize7,
                    model.Prize8,
                    model.PrizeCount1,
                    model.PrizeCount2,
                    model.PrizeCount3,
                    model.PrizeCount4,
                    model.PrizeCount5,
                    model.PrizeCount6,
                    model.PrizeCount7,
                    model.PrizeCount8
                };
            }
            catch (Exception ex)
            {
                result.Code = EReturnCode.SystemError;
                result.Message = ex.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CreateDraw()
        {
            var result = new ResponseModel<object>();
            try
            {
                if (Db.Queryable<SC_T_Lottery>().Count() > 0)
                {
                    result.Code = EReturnCode.SystemError;
                    result.Message = "已经生效！无效的操作";
                    return Json(result);
                }
                var last = Db.Queryable<SC_T_LuckyDraw>().First();
                if (last == null)
                {
                    result.Code = EReturnCode.SystemError;
                    result.Message = "抽奖方案无效，请先编辑抽奖方案！";
                    return Json(result);
                    //Db.Insertable(model).ExecuteCommand();
                }

                var prizeList = Db.Queryable<SC_T_Prize>().ToList();
                // Db.Updateable(model).ExecuteCommand();
                var list = new List<SC_T_Lottery>();
                if (last.GetType().GetProperties().Where(d => d.Name != "ID").Any(info => string.IsNullOrWhiteSpace(info.GetValue(last)?.ToString())))
                {
                    result.Code = EReturnCode.SystemError;
                    result.Message = "不允许出现空值";
                    return Json(result);
                }
                var prize1 = prizeList.First(d => d.BH == last.Prize1);
                for (var i = 0; i < last.PrizeCount1.Value; i++)
                {
                    list.Add(new SC_T_Lottery()
                    {
                        ExpirationTime = last.ExpirationTime,
                        isActive = "是",
                        LotteryBh = last.ID.ToString(),
                        PrizeBH = prize1.BH
                    });
                }
                var prize2 = prizeList.First(d => d.BH == last.Prize2);
                for (var i = 0; i < last.PrizeCount2.Value; i++)
                {
                    list.Add(new SC_T_Lottery()
                    {
                        ExpirationTime = last.ExpirationTime,
                        isActive = "是",
                        LotteryBh = last.ID.ToString(),
                        PrizeBH = prize2.BH
                    });
                }
                var prize3 = prizeList.First(d => d.BH == last.Prize3);
                for (var i = 0; i < last.PrizeCount3.Value; i++)
                {
                    list.Add(new SC_T_Lottery()
                    {
                        ExpirationTime = last.ExpirationTime,
                        isActive = "是",
                        LotteryBh = last.ID.ToString(),
                        PrizeBH = prize3.BH
                    });
                }
                var prize4 = prizeList.First(d => d.BH == last.Prize4);
                for (var i = 0; i < last.PrizeCount4.Value; i++)
                {
                    list.Add(new SC_T_Lottery()
                    {
                        ExpirationTime = last.ExpirationTime,
                        isActive = "是",
                        LotteryBh = last.ID.ToString(),
                        PrizeBH = prize4.BH
                    });
                }
                var prize5 = prizeList.First(d => d.BH == last.Prize5);
                for (var i = 0; i < last.PrizeCount5.Value; i++)
                {
                    list.Add(new SC_T_Lottery()
                    {
                        ExpirationTime = last.ExpirationTime,
                        isActive = "是",
                        LotteryBh = last.ID.ToString(),
                        PrizeBH = prize5.BH
                    });
                }
                var prize6 = prizeList.First(d => d.BH == last.Prize6);
                for (var i = 0; i < last.PrizeCount6.Value; i++)
                {
                    list.Add(new SC_T_Lottery()
                    {
                        ExpirationTime = last.ExpirationTime,
                        isActive = "是",
                        LotteryBh = last.ID.ToString(),
                        PrizeBH = prize6.BH
                    });
                }
                var prize7 = prizeList.First(d => d.BH == last.Prize7);
                for (var i = 0; i < last.PrizeCount7.Value; i++)
                {
                    list.Add(new SC_T_Lottery()
                    {
                        ExpirationTime = last.ExpirationTime,
                        isActive = "是",
                        LotteryBh = last.ID.ToString(),
                        PrizeBH = prize7.BH
                    });
                }
                var prize8 = prizeList.First(d => d.BH == last.Prize8);
                for (var i = 0; i < last.PrizeCount8.Value; i++)
                {
                    list.Add(new SC_T_Lottery()
                    {
                        ExpirationTime = last.ExpirationTime,
                        isActive = "是",
                        LotteryBh = last.ID.ToString(),
                        PrizeBH = prize8.BH
                    });
                }

                var copyArray = new SC_T_Lottery[list.Count];
                list.CopyTo(copyArray);

                //Add range
                var copyList = new List<SC_T_Lottery>();
                copyList.AddRange(copyArray);

                //Set outputList and random
                var outputList = new List<SC_T_Lottery>();
                var rd = new Random(DateTime.Now.Millisecond);

                while (copyList.Count > 0)
                {
                    //Select an index and item
                    int rdIndex = rd.Next(0, copyList.Count - 1);
                    var remove = copyList[rdIndex];

                    //remove it from copyList and add it to output
                    copyList.Remove(remove);
                    outputList.Add(remove);
                }

                Db.Insertable(outputList).ExecuteCommand();
            }
            catch (Exception ex)
            {
                result.Code = EReturnCode.SystemError;
                result.Message = $"异常{ex.Message}";
            }
            return Json(result);
        }

        [HttpPost]
        public JsonResult BackOut()
        {
            var result = new ResponseModel<object>();

            try
            {
                Db.Deleteable<SC_T_Lottery>().ExecuteCommand();
            }
            catch (Exception ex)
            {
                result.Code = EReturnCode.SystemError;
                result.Message = $"异常{ex.Message}";
            }

            return Json(result);
        }

        [HttpPost]
        public JsonResult UpdateLuckDraw(SC_T_LuckyDraw model)
        {
            var result = new ResponseModel<object>();
            try
            {
                if (model.GetType().GetProperties().Where(d => d.Name != "ID").Any(info => string.IsNullOrWhiteSpace(info.GetValue(model)?.ToString())))
                {
                    result.Code = EReturnCode.SystemError;
                    result.Message = "不允许出现空值";
                    return Json(result);
                }
                if (Db.Queryable<SC_T_Lottery>().Count() > 0)
                {
                    result.Code = EReturnCode.SystemError;
                    result.Message = "生效中，不允许修改";
                    return Json(result);
                }
                var last = Db.Queryable<SC_T_LuckyDraw>().First();
                if (last == null)
                {
                    Db.Insertable(model).ExecuteCommand();
                }
                else
                {
                    Db.Updateable(model).ExecuteCommand();
                }
                return Json(result);
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