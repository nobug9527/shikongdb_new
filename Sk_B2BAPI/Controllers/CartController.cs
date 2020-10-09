using Sk_B2BAPI.App_Code;
using Sk_B2BAPI.DAL;
using Sk_B2BAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.Mvc;
using Sk_B2BAPI.Tool;

namespace Sk_B2BAPI.Controllers
{
    public class CartController : Controller
    {
        #region 获取购物车列表
        /// <summary>
        /// 获取购物车列表
        /// </summary>
        /// <param name="entId">企业Id</param>
        /// <param name="userId">用户id</param>
        /// <param name="goodsList">选中的商品id</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CartList(string entId,string userId,string goodsList,string ywyId = "")
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { success = false, message = "用户未登录，请先登录" });
                }
                ///获取用户信息
                UserInfoDal dal = new UserInfoDal();
                List<UserInfo> user = dal.GetUserInfo(userId,entId);
                if (user.Count <= 0)
                {
                    return Json(new { success = false, message = "E002" });
                }
                CartDal cdal = new CartDal();
                List<CartEenDoc> cartDoc = new List<CartEenDoc>();
                ///获取购物车商品机构列表
                DataTable dtCartEnt = cdal.GetCartEntList(userId);
                ///获取购物车信息
                if (dtCartEnt == null || dtCartEnt.Rows.Count <= 0)
                {
                    return Json(new { success = false, messageCode = "E002" , message="无数据"});
                }
                foreach (DataRow dr in dtCartEnt.Rows)
                {
                    CartEenDoc model = new CartEenDoc();
                    model.EntId = dr["entId"].ToString();
                    model.EntName= dr["entname"].ToString();
                    model.cartlist = cdal.GetCartList(dr["entId"].ToString(), userId, goodsList, user[0].Pricelevel, user[0].KhType, ywyId);
                    cartDoc.Add(model);
                }
                return Json(new { success = true, list = cartDoc });
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Cart/CartList", ex.Message.ToString());
                return Json(new { success = false, message = ex.Message.ToString() });
            }

        }
        #endregion

        #region 获取购物车金额
        /// <summary>
        /// 获取购物车金额
        /// </summary>
        /// <param name="entId">企业Id</param>
        /// <param name="userId">用户id</param>
        /// <param name="goodsList">选中的商品id</param>
        /// <returns></returns>
        public JsonResult CartAmount(string entId, string userId, string goodsList,string ywyId="")
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { success = false, message = "用户未登录，请先登录" });
                }
                ///获取用户信息
                UserInfoDal dal = new UserInfoDal();
                List<UserInfo> user = dal.GetUserInfo(userId,entId);
                if (user.Count <= 0)
                {
                    return Json(new { success = false, message = "E002" });
                }
                ///获取购物车信息
                CartDal cdal = new CartDal();
                List<CartList> list = cdal.GetCartAmount(user[0].EntId, userId, goodsList, user[0].Pricelevel,user[0].KhType, ywyId);
                return Json(new { success = true, list = list });
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Cart/CartAmount", ex.Message.ToString());
                return Json(new { success = false, message = ex.Message.ToString() });
            }
        }
        #endregion

        #region 商品添加购物车
        /// <summary>
        /// 商品添加购物车
        /// </summary>
        /// <param name="entId">企业id</param>
        /// <param name="userId">用户id</param>
        /// <param name="article_Id">商品id</param>
        /// <param name="quantity">数量</param>
        /// <param name="cartType">购物车类型</param>
        /// <param name="fabh">促销方案编号</param>
        /// <param name="bs">'XQ'商品详情/''购物车</param>
        /// <returns></returns>
        public JsonResult CartAdd(string entId, string userId, string article_Id, decimal quantity, string cartType,string fabh,string bs="", string ywyId = "")
        {
            try
            {
                ///获取用户信息
                UserInfoDal userInfo = new UserInfoDal();
                List<UserInfo> user = userInfo.GetUserInfo(userId ?? "", entId);

                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { success = false, message = "用户未登录，请先登录" });
                }
                else if (quantity <= 0)
                {
                    return Json(new { success = false, message = "购买数量不能小于0" });
                }
                else if (user[0].Status==1)
                {
                    return Json(new { success = false, message = "该账号未通过审核，无法购买商品" });
                }
                ////客户经营范围拦截
                CartDal dal=new CartDal ();
                bool flag = dal.CartAdd(entId,userId,article_Id,quantity,cartType,fabh,bs,ywyId);
                if (flag)
                {
                    ///获取购物车条目数
                    int num = dal.GetCartCount(entId,userId,ywyId);
                    return Json(new { success = true,num=num,message = "加入购物车成功" });
                }
                else
                {
                    return Json(new { success = true, message = "加入失败" });
                }
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Cart/CartAdd", ex.Message.ToString());
                return Json(new { success = false, message = ex.Message.ToString() });
            }
        }
        #endregion

        #region 删除购物车商品
        /// <summary>
        /// 删除购物车商品
        /// </summary>
        /// <param name="entId">企业Id</param>
        /// <param name="userId">用户id</param>
        /// <param name="dltType">删除类型（单条one/全部all）</param>
        /// <param name="cartId">购物车Id</param>
        /// <returns></returns>
        public JsonResult DeleteCartGoods(string entId, string userId, string dltType, string cartId, string cartType = "", string ywyId = "")
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { success = false, message = "用户未登录，请先登录" });
                }
                if (dltType == "" || (dltType == "one" && cartId == ""))
                {
                    return Json(new { success = false, message = "必传参数不能为空" });
                }
                CartDal dal=new CartDal ();
                bool flag = dal.DeleteCart(entId,userId,dltType,cartId,cartType,ywyId);
                if (flag)
                {
                    return Json(new { success = true, message = "操作成功" });
                }
                else
                {
                    return Json(new { success = false, message = "E006" });
                }
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Cart/DeleteCartGoods", ex.Message.ToString());
                return Json(new { success = false, message = ex.Message.ToString() });
            }
        }
        #endregion

        #region 组合商品加入购物车
        /// <summary>
        /// 组合商品加入购物车
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="entId">企业Id</param>
        /// <param name="fabh">方案编号</param>
        /// <param name="num">组数</param>
        /// <returns></returns>
        public JsonResult AddCartGroup(string userId, string entId,string cartType, string fabh, decimal num,string bs="")
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { success = false, message = "用户未登录，请先登录" });
                }
                if (num <= 0)
                {
                    return Json(new { success = false, message = "购买组数不能小于0" });
                }
                //获取组合商品信息
                PromotionDal pdal = new PromotionDal();
                DataTable dt = pdal.GetGroupInfo(entId, userId, fabh);
                if (dt.Rows.Count <= 0)
                {
                    return Json(new { success = true, message = "该组合不存在" });
                }
                StringBuilder strSql = new StringBuilder();
                string msg = "";
                foreach (DataRow dr in dt.Rows)
                {
                    ///客户经营范围验证
                    if (decimal.Parse(dr["giftquantity"].ToString()) * num > decimal.Parse(dr["stock_quantity"].ToString()))
                    {
                        msg = dr["sub_title"].ToString() + "库存不足";
                        return Json(new { success = true, message = msg });
                    }
                }
                CartDal dal = new CartDal();
                msg = dal.CartAddGroup(entId, userId, num, fabh, out int flag,bs, cartType);
                return Json(new { success=flag==0?true:false, message = msg});
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Cart/AddCartGroup", ex.Message.ToString());
                return Json(new { success = false, message = "E005" });
            }
        }
        #endregion

        #region 再次购买
        /// <summary>
        /// 再次购买
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="entId">机构Id</param>
        /// <param name="billNo">订单序号</param>
        /// <returns></returns>
        public JsonResult OnceAgain(string userId,string entId,int billNo,string cartType, string bs = "XQ", string loginId="")
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    LogQueue.Write(LogType.Error, "Cart/OnceAgain", $"userId:{userId},entId:{entId},billNo:{billNo}");
                    return Json(new{ success=false,message= "用户未登录，请先登录！" });
                }
                ///获取用户信息
                UserInfoDal udal = new UserInfoDal();
                List<UserInfo> user = udal.GetUserInfo(userId, entId);
                string jgjb = "",khType="";
                if (user.Count >0)
                {
                    jgjb = user[0].Pricelevel;
                    khType = user[0].KhType;
                }
                CartDal dal = new CartDal();
                string result = dal.OnceAgain(userId, entId, billNo, loginId,out bool flag,jgjb,khType, cartType,bs);
                return Json(new { success=flag,message=result});
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Cart/OnceAgain", ex.Message.ToString());
                return Json(new { success = false, message = ex.Message });
            }
        }
        #endregion

        #region 快速下单商品加入购物车
        /// <summary>
        /// 快速下单商品加入购物车
        /// </summary>
        /// <param name="entId"></param>
        /// <param name="userId"></param>
        /// <param name="IdOrNum"></param>
        /// <returns></returns>
        public JsonResult SetCarProducts(string entId, string userId, string IdOrNum)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { success = false, message = "用户未登录，请先登录" });
                }
                CartDal dal = new CartDal();
                var proModel=IdOrNum.Split('|');//id,num|id,num|
                List<string> successlist = new List<string>();
                List<string> errorlist = new List<string>();
                if (proModel.Length > 0)
                {
                    foreach (var porobj in proModel)
                    {
                        if (porobj != null && porobj != "")
                        {
                            var pro = porobj.Split(',');
                            if (pro.Length == 2)
                            {
                                bool flag = dal.CartAdd(entId, userId, pro[0], decimal.Parse(pro[1]), "PC", "", "XQ");
                                if (flag)
                                {
                                    successlist.Add(pro[0]);
                                }
                                else
                                {
                                    errorlist.Add(pro[0]);
                                }
                            }
                            else
                            {
                                return Json(new { success = false, num = 0, message = "请检查下单商品的拼接！" });
                            }
                        }
                        else
                        {
                            return Json(new { success = false, num = 0, message = "请检查下单组合的拼接！" });
                        }
                    }
                }
                else {
                    return Json(new { success = false, num = 0, message = "请选择加入购物车的商品！" });
                }
                if (errorlist.Count == 0)
                {
                    return Json(new { success = true, num = successlist.Count, message = "加入购物车成功" });
                }
                else if (successlist.Count==0)
                {
                    return Json(new { success = false, num =0, message = "加入购物车全部失败" });
                }
                else
                {
                    return Json(new { success = true, num = -1, message = "未全部加入购物车，某些商品加入失败" });
                }

            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Cart/CartAdd", ex.Message.ToString());
                return Json(new { success = false, message = ex.Message.ToString() });
            }
        }
        #endregion

        #region 购物车商品条目数
        /// <summary>
        /// 购物车商品条目数
        /// </summary>
        /// <param name="entId">企业</param>
        /// <param name="userId">用户</param>
        /// <param name="ywyId">业务员</param>
        /// <returns></returns>
        public ActionResult NumberOfItems(string entId, string userId,string ywyId = "")
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { success = false, message = "用户未登录，请先登录" });
                }
                ///获取用户信息
                UserInfoDal dal = new UserInfoDal();
                List<UserInfo> user = dal.GetUserInfo(userId, entId);
                if (user.Count <= 0)
                {
                    return Json(new { success = false, message = "用户未登录，请重新登录" });
                }
                ///获取购物车信息
                CartDal cdal = new CartDal();
                int num= cdal.GetCartCount(entId, userId, ywyId);
                return Json(new { success = true,message= "购物车商品条目数获取成功", num });
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Cart/NumberOfItems", ex.Message.ToString());
                return Json(new { success = false, message = ex.Message.ToString() });
            }
        }
        #endregion
    }
} 
