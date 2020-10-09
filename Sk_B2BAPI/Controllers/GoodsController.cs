using Newtonsoft.Json;
using Sk_B2BAPI.App_Code;
using Sk_B2BAPI.DAL;
using Sk_B2BAPI.Models;
using Sk_B2BAPI.Models.Admin;
using Sk_B2BAPI.Tool;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Web.Mvc;

namespace Sk_B2BAPI.Controllers
{
    public class GoodsController : Controller
    {
        #region 获取搜索页商品列表
        /// <summary>
        /// 获取搜索页商品列表
        /// </summary>
        /// <param name="userId">客户Id</param>
        /// <param name="searchValue">搜索类容</param>
        /// <param name="letter">厂家首字母</param>
        /// <param name="tags">排序</param>
        /// <param name="isKc">是否有货</param>
        /// <param name="CategoryId">分类Id</param>
        /// <param name="Login_Id">登陆Id</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页容量</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SearchGoodsList(string userId, string entId, string searchValue, string letter, string tags, string isKc, string CategoryId, string Login_Id, int pageIndex, int pageSize)
        {
            try
            {
                if (string.IsNullOrEmpty(entId))
                {
                    entId = BaseConfiguration.EntId;
                }
                ///获取商品分类列表
                ///
                ////获取当前商品分类
                List<Category> clist = new List<Category>();
                if (CategoryId != "")
                {
                    ImgInfoDal idal = new ImgInfoDal();
                    clist = idal.GetCategory(CategoryId, entId);
                }
                ///商品信息
                GoodsInfoDal dal = new GoodsInfoDal();
                List<GoodsList> list = dal.GetGoodsList(userId, Server.UrlDecode(searchValue.Trim()), letter, tags, isKc, CategoryId, Login_Id, pageIndex, pageSize, entId);
                ///商品分类信息
                return Json(new { success = true, list, clist });
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Search/SearchGoodsList", ex.Message.ToString());
                return Json(new { success = false, message = "商品列表加载失败！" });
            }
        }
        #endregion

        #region 获取商品详情页
        /// <summary>
        /// 获取商品详情页
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="articleId">商品ID</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GoodsDetails(string entId, string userId, string articleId)
        {
            try
            {
                if (string.IsNullOrEmpty(entId))
                {
                    entId = BaseConfiguration.EntId;
                }
                if (string.IsNullOrEmpty(articleId))
                {
                    return Json(new { success = false, message = "参数获取失败！" });
                }
                ///商品信息
                GoodsInfoDal dal = new GoodsInfoDal();
                ///商品当月销量排行
                List<GoodsStatistical> slist = dal.GetGoodsRanking(userId, entId, "GoodsSale", 10);
                ///商品当月点击量量排行
                List<GoodsStatistical> clist = dal.GetGoodsRanking(userId, entId, "GoodsClick", 3);
                ///商品详情
                List<GoodsList> list = dal.GetGoodDetail(userId, articleId, entId);

                return Json(new { success = true, list, slist, clist });
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Goods/GoodsDetails", ex.Message.ToString());

                return Json(new { success = false, message = "商品详情加载失败！" });
            }
        }
        #endregion

        #region 商品评价
        /// <summary>
        /// 商品评价
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="entId">机构Id</param>
        /// <param name="articleId">商品Id</param>
        /// <param name="orderBy">排序方式</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页条目数</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GoodsCriticisms(string userId, string entId, string articleId, string orderBy = "desc", int pageIndex = 1, int pageSize = 30)
        {
            try
            {
                if (string.IsNullOrEmpty(entId))
                {
                    entId = BaseConfiguration.EntId;
                }
                if (string.IsNullOrEmpty(articleId))
                {
                    return Json(new { success = false, message = "参数获取失败！" });
                }
                else if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { success = false, message = "请先登录！" });
                }
                ///商品信息
                GoodsInfoDal dal = new GoodsInfoDal();
                ///商品评价
                List<StairCriticisms> list = dal.GetCriticisms(userId, entId, articleId, pageIndex, pageSize, orderBy, out int pageCount, out int recordCount, out int passSum, out decimal raveReviews);
                return Json(new { success = true, list, pageCount, recordCount, passSum, raveReviews });
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Goods/GoodsEvaluate", ex.Message.ToString());
                return Json(new { success = false, message = "商品评价失败！" });
            }
        }
        #endregion

        #region 编辑评论
        /// <summary>
        /// 编辑评论
        /// </summary>
        /// <param name="userId">用户</param>
        /// <param name="entId">机构</param>
        /// <param name="orderNo">订单编号</param>
        /// <param name="id">订单商品序号</param>
        /// <param name="articleId">商品</param>
        /// <param name="content">内容</param>
        /// <param name="starLevel">星级</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RedactCriticism(string userId, string entId, string orderNo, int id, string articleId, string content, decimal starLevel, int superiorId = 0, int replayId = 0)
        {
            try
            {
                if (string.IsNullOrEmpty(entId))
                {
                    entId = BaseConfiguration.EntId;
                }
                if (string.IsNullOrEmpty(articleId) && string.IsNullOrEmpty(orderNo))
                {
                    return Json(new { success = false, message = "商品和订单编号不能为空" });
                }
                else if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { success = false, message = "请先登录！" });
                }
                ///商品信息
                GoodsInfoDal dal = new GoodsInfoDal();
                int flag = dal.RedactCriticism(userId, entId, orderNo, id, articleId, content, starLevel, superiorId, replayId, out string message);
                return Json(new { success = flag == 1 ? true : false, message });
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Goods/RedactCriticism", ex.Message.ToString());
                return Json(new { success = false, message = "商品评价编辑失败！" });
            }
        }
        #endregion

        #region 评论点赞
        /// <summary>
        /// 评论点赞
        /// </summary>
        /// <param name="userId">用户</param>
        /// <param name="entId">机构</param>
        /// <param name="criticismsId">评论Id</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult LikeCriticism(string userId, string entId, int criticismsId)
        {
            try
            {
                if (string.IsNullOrEmpty(entId))
                {
                    entId = BaseConfiguration.EntId;
                }
                if (criticismsId == 0)
                {
                    return Json(new { success = false, message = "参数获取异常！" });
                }
                else if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { success = false, message = "请先登录！" });
                }
                ///商品信息
                GoodsInfoDal dal = new GoodsInfoDal();
                int flag = dal.LikeCriticism(userId, entId, criticismsId, out string message);
                return Json(new { success = flag == 1 ? true : false, message });
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Goods/RedactCriticism", ex.Message.ToString());
                return Json(new { success = false, message = "商品评价点赞失败！" });
            }
        }
        #endregion

        #region 更多回复
        /// <summary>
        /// 更多回复
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="entId"></param>
        /// <param name="criticismsId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public JsonResult MoreRepaly(string userId, string entId, int criticismsId, int pageIndex = 1, int pageSize = 30)
        {
            try
            {
                if (string.IsNullOrEmpty(entId))
                {
                    entId = BaseConfiguration.EntId;
                }
                if (criticismsId == 0)
                {
                    return Json(new { success = false, message = "参数获取异常！" });
                }
                else if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { success = false, message = "请先登录！" });
                }
                ///商品信息
                GoodsInfoDal dal = new GoodsInfoDal();
                ///商品评价回复
                List<StairCriticisms> list = dal.GetReplay(userId, entId, criticismsId, pageIndex, pageSize, out int pageCount, out int recordCount);
                return Json(new { success = true, list, pageCount, recordCount });
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Goods/MoreRepaly", ex.Message.ToString());
                return Json(new { success = false, message = "商品评价回复获取失败！" });
            }
        }
        #endregion

        #region 添加商品收藏
        /// <summary>
        /// 添加商品收藏
        /// </summary>
        /// <param name="entId"></param>
        /// <param name="userId"></param>
        /// <param name="article_Id">商品Id</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddGoodsCollection(string entId, string userId, string article_Id)
        {
            try
            {
                ///获取用户收藏商品
                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { success = false, message = "用户未登录，请先登录" });
                }
                if (string.IsNullOrEmpty(article_Id))
                {
                    return Json(new { success = false, message = "必传参数不能为空" });
                }
                bool flag = StatisticalDal.AddGoodsCollection(entId, userId, article_Id);
                if (flag)
                {
                    return Json(new { success = true, message = "操作成功" });
                }
                else
                {
                    return Json(new { success = false, message = "操作失败" });
                }

            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Search/AddGoodsCollection", ex.Message.ToString());
                return Json(new { success = false, message = "商品收藏添加失败！" });
            }
        }
        #endregion

        #region 用户商品收藏查询
        /// <summary>
        /// 用户商品收藏查询
        /// </summary>
        /// <param name="entId"></param>
        /// <param name="userId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GoodsCollectionQuery(string entId, string userId, int pageIndex, int pageSize)
        {
            try
            {
                ///获取用户收藏商品
                if (userId == null || userId == "")
                {
                    return Json(new { success = false, message = "用户未登录，请先登录" });
                }
                StatisticalDal dal = new StatisticalDal();
                List<GoodsStatistical> list = dal.GoodsCollectionQuery(entId, userId, pageIndex, pageSize, out int recordCount, out int pageCount);
                ///商品分类信息
                return Json(new { success = true, list, RecordCount = recordCount, PageCount = pageCount });
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Search/GoodsCollectionQuery", ex.Message.ToString());
                return Json(new { success = false, message = "商品列表加载失败！" });
            }
        }
        #endregion

        #region 删除收藏商品
        /// <summary>
        /// 删除收藏商品
        /// </summary>
        /// <param name="id">收藏Id</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DltCollectionQuery(int id)
        {
            try
            {
                ///获取用户收藏商品
                bool flag = StatisticalDal.DltGoodsCollection(id);
                if (flag)
                {
                    return Json(new { success = true, message = "操作成功" });
                }
                else
                {
                    return Json(new { success = false, message = "操作失败" });
                }
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Search/DltCollectionQuery", ex.Message.ToString());
                return Json(new { success = false, message = "商品收藏删除失败！" });
            }
        }
        #endregion

        #region 促销信息
        /// <summary>
        /// 促销信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="entId"></param>
        /// <param name="articleId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetActivity(string userId, string entId, string articleId, string fabh = "")
        {
            try
            {
                if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(entId))
                {
                    return Json(new { success = false, message = "用户未登录，请先登录" });
                }
                GoodsInfoDal dal = new GoodsInfoDal();
                List<Promotion> list = new List<Promotion>();
                list = dal.GetActivity(userId, entId, articleId, fabh);
                return Json(new { success = true, message = list });
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Search/GetActivity", ex.Message.ToString());
                return Json(new { success = false, message = "商品活动信息获取失败！" });
            }
        }
        #endregion

        #region 搜索栏索引
        /// <summary>
        /// 搜索栏索引
        /// </summary>
        /// <param name="entid"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SearchIndex(string entid, string parameter)
        {
            try
            {
                if (string.IsNullOrEmpty(entid))
                {
                    entid = BaseConfiguration.EntId;
                }
                GoodsInfoDal infoDal = new GoodsInfoDal();
                List<SearchIndex> indices = new List<SearchIndex>();
                indices = infoDal.SearchIndex(entid, parameter);
                return Json(new { success = true, list = indices });
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Search/SearchIndex", ex.Message.ToString());
                return Json(new { success = false, message = "搜索栏索引获取失败！" });
            }
        }
        #endregion

        #region 搜索快速下单商品
        /// <summary>
        /// 搜索快速下单商品
        /// </summary>
        /// <param name="entid">机构</param>
        /// <param name="userId">用户</param>
        /// <param name="searchValue">参数</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SearchFastGoods(string entid, string userId, string searchValue)
        {
            try
            {
                if (string.IsNullOrEmpty(entid) || string.IsNullOrEmpty(userId))
                {
                    return Json(new { flag = 0, message = "请先登录！" });
                }
                //获取用户信息
                UserInfoDal dal = new UserInfoDal();
                List<UserInfo> user = new List<UserInfo>();
                if (!string.IsNullOrEmpty(userId))
                {
                    user = dal.GetUserInfo(userId, entid);
                }
                string jgjb = "", clientlimit = "", KhType = "";
                bool landing = false;
                bool staleDated = false;
                if (user.Count > 0)
                {
                    entid = user[0].EntId;
                    jgjb = user[0].Pricelevel;
                    clientlimit = user[0].ClientLimit;
                    KhType = user[0].KhType;
                    staleDated = user[0].StaleDated;
                    landing = true;
                }

                GoodsInfoDal infoDal = new GoodsInfoDal();
                if (searchValue.Trim().Contains(" "))
                {
                    int length = searchValue.Length;
                    int index = searchValue.IndexOf(" ");
                    string goodsValue, factoryValue;
                    goodsValue = searchValue.Substring(0, index).Trim();
                    factoryValue = searchValue.Substring(index, length - index).ToString().Trim();
                    searchValue = goodsValue + ',' + factoryValue;
                }
                List<GoodsList> lists = new List<GoodsList>();
                lists = infoDal.SearchFastGoods(entid, searchValue, jgjb, KhType, landing, staleDated);
                if (lists == null)
                {
                    return Json(new { flag = 2, message = "无符合商品！" });
                }
                else
                {
                    return Json(new { flag = 2, message = "商品查询成功！", lists });
                }
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Search/SearchFastGoods", ex.Message.ToString());
                return Json(new { flag = 99, message = "商品获取失败！" });
            }
        }
        #endregion

        #region 添加商品到货提醒
        /// <summary>
        /// 添加商品到货提醒
        /// </summary>
        /// <param name="entId"></param>
        /// <param name="userId"></param>
        /// <param name="article_Id">商品Id</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddGoodsArrival(string entId, string userId, string article_Id)
        {
            try
            {
                ///获取用户到货提醒
                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { success = false, message = "用户未登录，请先登录" });
                }
                if (string.IsNullOrEmpty(article_Id))
                {
                    return Json(new { success = false, message = "必传参数不能为空" });
                }
                if (StatisticalDal.CheckGoodsArrival(entId, userId, article_Id))
                {
                    return Json(new { success = false, message = "该商品已添加过到货提醒!" });
                }
                else
                {
                    bool flag = StatisticalDal.AddGoodsArrival(entId, userId, article_Id);
                    if (flag)
                    {
                        return Json(new { success = true, message = "操作成功" });
                    }
                    else
                    {
                        return Json(new { success = false, message = "操作失败" });
                    }
                }
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Search/AddGoodsCollection", ex.Message.ToString());
                return Json(new { success = false, message = "商品到货提醒失败！" });
            }
        }
        #endregion

        #region 用户商品到货提醒查询
        /// <summary>
        /// 用户商品到货提醒查询
        /// </summary>
        /// <param name="entId"></param>
        /// <param name="userId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GoodsArrivalQuery(string entId, string userId, int pageIndex, int pageSize)
        {
            try
            {
                ///获取用户到货提醒
                if (userId == null || userId == "")
                {
                    return Json(new { success = false, message = "用户未登录，请先登录" });
                }
                StatisticalDal dal = new StatisticalDal();
                List<GoodsArrival> list = dal.GoodsArrivalQuery(entId, userId, pageIndex, pageSize, out int recordCount, out int pageCount);
                ///到货提醒信息
                return Json(new { success = true, list, RecordCount = recordCount, PageCount = pageCount });
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Search/GoodsCollectionQuery", ex.Message.ToString());
                return Json(new { success = false, message = "商品列表加载失败！" });
            }
        }
        #endregion

        #region 删除到货提醒
        /// <summary>
        /// 删除到货提醒
        /// </summary>
        /// <param name="id">到货Id</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DltArrivalQuery(int id)
        {
            try
            {
                ///获取用户到货提醒
                bool flag = StatisticalDal.DltGoodsCollection(id);
                if (flag)
                {
                    return Json(new { success = true, message = "操作成功" });
                }
                else
                {
                    return Json(new { success = false, message = "操作失败" });
                }
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Search/DltCollectionQuery", ex.Message.ToString());
                return Json(new { success = false, message = "商品到货提醒删除失败！" });
            }
        }
        #endregion

        #region 到货提醒
        /// <summary>
        /// 到货提醒
        /// </summary>
        /// <param name="id">到货Id</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ReadGoodsArrival(int id)
        {
            try
            {
                ///获取用户到货提醒
                bool flag = StatisticalDal.ReadGoodsArrival(id);
                if (flag)
                {
                    return Json(new { success = true, message = "操作成功" });
                }
                else
                {
                    return Json(new { success = false, message = "操作失败" });
                }
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Search/DltCollectionQuery", ex.Message.ToString());
                return Json(new { success = false, message = "商品到货提醒修改失败！" });
            }
        }
        #endregion

        #region 求购专区
        /// <summary>
        /// 发起求购专区
        /// </summary>
        /// <param name="userId">用户</param>
        /// <param name="entId">机构</param>
        /// <param name="buyName">求购人</param>
        /// <param name="buyTel">求购人联系方式</param>
        /// <param name="buyGoods">求购药品名称</param>
        /// <param name="productName">求购药品厂家</param>
        /// <param name="buySpec">求购药品规格</param>
        /// <param name="buyNumber">求购药品数量</param>
        /// <param name="buyPrice">求购药品价格</param>
        /// <param name="message">求购备注</param>
        /// <returns></returns>
        public ActionResult InsertReply(string userId, string entId, string buyName, string buyTel, string buyGoods, string productName, string buySpec, decimal buyNumber, decimal buyPrice, string message)
        {
            try
            {
                GoodsInfoDal goodsInfoDal = new GoodsInfoDal();
                if (string.IsNullOrEmpty(buyName) || string.IsNullOrEmpty(buyGoods) || string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(entId) || string.IsNullOrEmpty(productName) || string.IsNullOrEmpty(buyTel) || string.IsNullOrEmpty(buySpec))
                {
                    LogQueue.Write(LogType.Debug, "", $"求购人:{buyName},求购人联系方式:{buyTel},求购药品名称:{buyGoods},用户:{userId},机构:{entId},求购药品厂家:{productName},求购药品规格:{buySpec}");
                    return Json(new { success = false, message = "参数异常" });
                }
                else
                {
                    bool flag = goodsInfoDal.InsertReply(buyName, buyTel, buyGoods, productName, buySpec, buyNumber, buyPrice, userId, entId, message, out string msg);
                    return Json(new { success = flag, message = msg });
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message.ToString().Trim();
                LogQueue.Write(LogType.Error, "GoodsInfoDal/InsertReply", msg);
                return Json(new { success = false, message = msg });
            }
        }
        /// <summary>
        /// 请求求购信息
        /// </summary>
        /// <param name="userId">用户</param>
        /// <param name="entId">机构</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">容量</param>
        /// <returns></returns>
        public ActionResult QueryReply(string userId, string entId, int pageIndex, int pageSize)
        {
            try
            {
                GoodsInfoDal goodsInfoDal = new GoodsInfoDal();
                RequrementList requrementList = new RequrementList();
                requrementList = goodsInfoDal.QueryReply(userId, entId, pageIndex, pageSize);
                return Json(new { success = true, message = "求购信息获取成功", requrementList });
            }
            catch (Exception ex)
            {
                string msg = ex.Message.ToString().Trim();
                LogQueue.Write(LogType.Error, "GoodsInfoDal/QueryReply", msg);
                return Json(new { success = false, message = msg });
            }
        }
        #endregion

        #region 常购商品
        /// <summary>
        /// 常购商品
        /// </summary>
        /// <param name="userId">用户</param>
        /// <param name="entId">机构</param>
        /// <param name="searchValue">检索条件</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">容量</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult OftenBuy(string userId, string entId, string searchValue, int pageIndex = 1, int pageSize = 15)
        {
            try
            {
                if (string.IsNullOrEmpty(entId) || string.IsNullOrEmpty(userId))
                {
                    return Json(new { success = false, message = "请先登录" });
                }
                //获取用户信息
                UserInfoDal dal = new UserInfoDal();
                List<UserInfo> user = new List<UserInfo>();
                if (!string.IsNullOrEmpty(userId))
                {
                    user = dal.GetUserInfo(userId, entId);
                }
                string jgjb = "", clientlimit = "", KhType = "";
                bool landing = false;
                bool staleDated = false;
                if (user.Count > 0)
                {
                    entId = user[0].EntId;
                    jgjb = user[0].Pricelevel;
                    clientlimit = user[0].ClientLimit;
                    KhType = user[0].KhType;
                    staleDated = user[0].StaleDated;
                    landing = true;
                }

                GoodsInfoDal infoDal = new GoodsInfoDal();
                if (searchValue.Trim().Contains(" "))
                {
                    int length = searchValue.Length;
                    int index = searchValue.IndexOf(" ");
                    string goodsValue, factoryValue;
                    goodsValue = searchValue.Substring(0, index).Trim();
                    factoryValue = searchValue.Substring(index, length - index).ToString().Trim();
                    searchValue = goodsValue + ',' + factoryValue;
                }
                List<GoodsList> lists = new List<GoodsList>();
                lists = infoDal.OftenBuy(userId, entId, searchValue, jgjb, KhType, landing, staleDated, pageIndex, pageSize, out int pageCount, out int recordCount);
                return Json(new { success = true, message = "常购商品获取成功", lists, pageCount, recordCount });
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Search/OftenBuy", $"{ex.Message}");
                return Json(new { success = false, message = "常购商品获取失败！" });
            }
        }
        #endregion

        #region 同类商品推荐
        /// <summary>
        /// 同类商品推荐
        /// </summary>
        /// <param name="entId">企业</param>
        /// <param name="userId">用户</param>
        /// <param name="articleId">商品ID</param>
        /// <param name="num">条目数</param>
        /// <returns></returns>
        public ActionResult Commendation(string entId, string userId, string articleId, int num)
        {
            try
            {
                if (string.IsNullOrEmpty(entId))
                {
                    entId = BaseConfiguration.EntId;
                }
                if (string.IsNullOrEmpty(articleId))
                {
                    return Json(new { success = false, message = "参数异常" });
                }
                ///获取用户信息
                UserInfoDal dal = new UserInfoDal();
                List<UserInfo> user = new List<UserInfo>();
                if (!string.IsNullOrEmpty(userId))
                {
                    user = dal.GetUserInfo(userId, entId);
                }
                string jgjb = "", clientlimit = "", KhType = "";
                bool landing = false;
                bool staleDated = false;
                //获取客户价格级别
                if (user.Count > 0)
                {
                    entId = user[0].EntId;
                    jgjb = user[0].Pricelevel;
                    clientlimit = user[0].ClientLimit;
                    KhType = user[0].KhType;
                    staleDated = user[0].StaleDated;
                    landing = true;
                }
                GoodsInfoDal infoDal = new GoodsInfoDal();
                ///商品详情
                List<GoodsList> list = infoDal.GetGoodDetail(userId, articleId, entId);
                var category = list[0].GoodsInfo[0].Category;
                //同类商品推荐
                List<Models.GoodsInfo> klist = infoDal.Commendation(entId, category, num, jgjb, KhType, landing, staleDated);
                return Json(new { success = true, message = "商品推荐获取成功", klist });
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Goods/Commendation", ex.Message.ToString());
                return Json(new { success = false, message = "商品推荐加载失败！" });
            }
        }
        #endregion

        #region 获取推荐商品
        /// <summary>
        /// 获取推荐商品
        /// </summary>
        /// <param name="count"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetArticleRecommend(string entId, string userId, int count, int type)
        {
            if (string.IsNullOrEmpty(entId))
            {
                entId = BaseConfiguration.EntId;
            }
            ///获取用户信息
            UserInfoDal dal = new UserInfoDal();
            List<UserInfo> user = new List<UserInfo>();
            if (!string.IsNullOrEmpty(userId))
            {
                user = dal.GetUserInfo(userId, entId);
            }
            string jgjb = "", clientlimit = "", KhType = "";
            bool staleDated = false;
            //获取客户价格级别
            if (user.Count > 0)
            {
                entId = user[0].EntId;
                jgjb = user[0].Pricelevel;
                clientlimit = user[0].ClientLimit;
                KhType = user[0].KhType;
                staleDated = user[0].StaleDated;
            }
            GoodsInfoDal goodsInfoDal = new GoodsInfoDal();
            DataTable dt = goodsInfoDal.GetGetArticleRecommend(entId, jgjb, KhType, count, type);

            JsonSerializerSettings setting = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            var data = JsonConvert.SerializeObject(dt, setting);

            return Json(data);
        }
        #endregion

        #region 上传评论图片
        /// <summary>
        /// 上传评论图片
        /// </summary>
        /// <returns></returns>
        public JsonResult UploadCriticismPic()
        {
            try
            {
                string dataFile;
                string filePath = string.Empty;
                string fileNewName = string.Empty;
                var file = Request.Files;
                if (file.Count <= 0)
                {
                    return Json(new { success = false, message = "请选择要上传的文件！" });//请选择要上传的文件  
                }
                var ufile = file[0];
                string fileName = ufile.FileName;/*获取文件名： C:\Documents and Settings\Administrator\桌面\123.jpg*/
                //Log.Error("错误",$"上传文件名：{fileName}");//上传文件名：201912261054561.jpg
                string suffix = fileName.Substring(fileName.LastIndexOf(".") + 1).ToLower();/*获取后缀名并转为小写： jpg*/
                int bytes = ufile.ContentLength;//获取文件的字节大小  
                if (suffix != "jpg" && suffix != "png")
                {
                    return Json(new { success = false, message = "上传文件格式错误！" });//只能上传JPG格式图片  
                }
                else if (bytes > 1024 * 1024 * 3)
                {
                    return Json(new { success = false, message = "图片不能大于3M！" }); //图片不能大于1M 
                }
                /*判断文件夹是否存在*/
                if (Directory.Exists(Server.MapPath("/UploadFile/Criticism/")) == false)
                {
                    //Log.Error("打印", $"文件夹Material不存在，重新创建");
                    Directory.CreateDirectory(Server.MapPath("/UploadFile/Criticism/"));
                }
                string path = Server.MapPath("/UploadFile/Criticism/");
                //Log.Error("打印", $"物理文件路径path：{path}");
                string year = DateTime.Now.Year.ToString();
                string month = DateTime.Now.Month.ToString();
                string date = DateTime.Now.ToFileTimeUtc().ToString();
                string newfileName = date + "." + suffix;
                string newfile = year + "_" + month + "/" + newfileName;
                string newPath = path + year + "_" + month;
                //Log.Error("打印", $"上传文件路径newPath：{newPath}");//  /UploadFile/Users/System.Web.HttpPostedFileWrapper
                //Log.Error("打印", $"newPath文件是否存在：{System.IO.Directory.Exists(newPath)}");
                if (false == System.IO.Directory.Exists(newPath))
                {
                    //Log.Error("打印", $"文件夹newPath不存在，重新创建");
                    //创建文件夹
                    System.IO.Directory.CreateDirectory(newPath);
                }
                ufile.SaveAs(newPath + "/" + newfileName);//保存图片  
                                                          //保存图片路径
                dataFile = "/UploadFile/Criticism/" + newfile;
                string webUrl = BaseConfiguration.SercerIp.ToString();
                //if (webUrl.Contains("/b2b_api"))
                //{
                //    var index = webUrl.LastIndexOf('/');
                //    webUrl = webUrl.Substring(0, index);
                //}
                return Json(new { success = true, message = "评论图片上传成功！", imgUrl = webUrl + dataFile });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message, imgUrl = "" });
            }
        }
        #endregion
        
        #region 编辑商品
        [ValidateInput(false)]
        [HttpPost]
        public JsonResult UpdateProduct(string category_id, string sub_title, string img_url, string left_pic, string content, string brand_img_url, string sort_id, string article_id,
            string generic, string drug_factory, string approval_number, string drug_spec, string big_package, string package_unit, string min_package, string mnemonic_code, string Storage_conditions,
            string BrandCode, string rate, string min_package_astrict, string price, string category, string dosage_form, string recommendList,string isZbz,string isDbz,string zhaiyao)
        {
            Dt_User user = RoleFuns.IsLoginAdmin(HttpContext.Session["user"]);
            if (user != null)//登录检测，权限检测 context.Session["user"]
            {
                try
                {
                    StringBuilder strSql = new StringBuilder();
                    //修改商品主表
                    strSql.Append("update dt_article set category_id='" + category_id + "',title='" + sub_title + "',img_url='" + img_url + "',");
                    strSql.Append("left_pic='" + left_pic + "',content='" + content + "',brand_img_url='" + brand_img_url + "',sort_id='" + sort_id + "' ");
                    strSql.Append(" ,zhaiyao='"+ zhaiyao + "' where id='" + article_id + "' and entid='" + user.entId + "' ;");
                    //修改属性表
                    strSql.Append("update dt_article_attribute set sub_title='" + sub_title.ToString().Trim() + "',generic='" + generic + "',drug_factory='" + drug_factory + "',"); 
                    strSql.Append("approval_number='" + approval_number + "',drug_spec='" + drug_spec + "',big_package='" + big_package + "',min_package='" + min_package + "',");
                    strSql.Append("package_unit='" + package_unit + "',Storage_conditions='" + Storage_conditions + "',category='" + category + "',mnemonic_code='" + mnemonic_code + "',");
                    strSql.Append("dosage_form='" + dosage_form + "',rate='" + rate + "',brandId='" + BrandCode + "',price='" + price + "',min_package_astrict='" + min_package_astrict + "'");
                    strSql.Append(",scattered='"+isZbz+ "',packControl='"+isDbz+"' where article_id='" + article_id + "' and entid='" + user.entId + "'");
                    //status = " + obj["status"].ToString().Trim() + "
                    SqlRun sql = new SqlRun(SqlRun.sqlstr);
                    bool flag = sql.ExecuteSql(strSql.ToString());
                    if (flag)
                    {
                        SqlParameter[] prmt = new SqlParameter[] {
                                                                new SqlParameter("@type","GoodsRecommend"),
                                                                new SqlParameter("@recommendList",recommendList),
                                                                new SqlParameter("@article_id",article_id),
                                                                new SqlParameter("@BrandCode",BrandCode),
                                                                new SqlParameter("@entId",user.entId),
                                                                };
                        int n = sql.ExecuteNonQuery("Proc_Admin_GoodsList", prmt);
                        return Json(JsonMethod.GetError(0, "操作成功"));
                    }
                    else
                    {
                        return Json(JsonMethod.GetError(1, "操作失败"));
                    }
                }
                catch (Exception ex)
                {
                    Log.Error("错误:商品编辑失败", ex.Message);
                    LogQueue.Write(LogType.Error, "Goods/UpdateProduct", ex.Message);
                    return Json(JsonMethod.GetError(1, "操作失败"));
                }
            }
            else
            {
                return Json(JsonMethod.GetError(2, "登录超时"));
            }

        }
        #endregion
        #region==============专区商品===============================================
        /// <summary>
        /// 专区商品
        /// </summary>
        /// <param name="entId">机构Id</param>
        /// <param name="userId">会员Id</param>
        /// <param name="zqType">专区类型</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页容</param>
        /// <returns></returns>
        public JsonResult GetZqGoodsList(string entId, string userId, string zqType, int pageIndex, int pageSize)
        {
            try
            {
                if (string.IsNullOrEmpty(entId))
                {
                    entId = BaseConfiguration.EntId;
                }
                if (string.IsNullOrEmpty(zqType))
                {
                    return Json(new { success = false, msgCode = "E002", message = "专区类型获取失败" });
                }
                ///获取商品分类列表
                GoodsInfoDal dal = new GoodsInfoDal();
                List<GoodsList> list = dal.GetGoodsList(userId,"", "", "cxbs", "", "", "", pageIndex, pageSize, entId, zqType);
                ///商品分类信息
                return Json(new { success = true, list });
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Search/SearchGoodsList", ex.Message.ToString());
                return Json(new { success = false, message = "商品列表加载失败！" });
            }
        }
        #endregion

    }
}
