using Sk_B2BAPI.App_Code;
using Sk_B2BAPI.DAL;
using Sk_B2BAPI.Models;
using Sk_B2BAPI.Tool;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Data;

namespace Sk_B2BAPI.Controllers
{
    public class ArticleController : Controller
    {
        //
        // GET: /Article/

        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 获取文章/咨询
        /// </summary>
        /// <param name="channel_Id">标识(11)</param>
        /// <param name="entId"></param>
        /// <param name="type">类型(Title/Detail)</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetArticle(int channel_Id, string entId, string type)
        {
            if (channel_Id <= 0)
            {
                return Json(new { success = false, message = "参数无效" });
            }
            else
            {
                try
                {
                    if (entId == null || entId == "")
                    {
                        entId = BaseConfiguration.EntId;
                    }
                    ArticleDal dal = new ArticleDal();
                    List<Article> list = dal.GetArticle(entId, channel_Id, type);
                    return Json(new { success = true, list = list });
                }
                catch (Exception ex)
                {
                    LogQueue.Write(LogType.Error, "Article/GetArticle", ex.Message);
                    return Json(new { success = false, message = "文章/咨询加载失败！" });
                }
            }
        }
        /// <summary>
        /// 获取资讯
        /// </summary>
        /// <param name="entId">机构</param>
        /// <param name="channel">为6时查询：新闻动态/医药资讯/监管信息/公告/招贤纳士</param>
        /// <param name="category">新闻动态/医药资讯/监管信息/公告/招贤纳士</param>
        /// <param name="num">前num 条，为0时，分页返回所有</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="articleId">谋篇文章ID</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetTopArticle(string entId, string channel, string category, int num = 0, int pageIndex = 1, int pageSize = 15, string articleId = "")
        {
            try
            {
                if (string.IsNullOrEmpty(entId))
                {
                    entId = BaseConfiguration.EntId;
                }
                ArticleDal dal = new ArticleDal();
                List<Article> list = dal.GetTopArticle(entId, num, pageIndex, pageSize, channel, category, articleId, out int pageCount, out int recordCount);
                return Json(new { success = true, pageCount = pageCount, recordCount = recordCount, list = list });
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Article/GetTopArticle", ex.Message);
                return Json(new { success = false, message = "文章/咨询加载失败！" });
            }
        }

        /// <summary>
        /// 新闻资讯存盘
        /// </summary>
        /// <param name="json"></param>
        /// <param name="proc"></param>
        /// <param name="UserId"></param>
        /// <param name="entid"></param>
        /// <returns></returns>
        [ValidateInput(false)]
        [HttpPost]
        public JsonResult SaveArticle(string title, string tags, string newsType, string sortId, string dates, string zhaiYao, string content, int id = 0)
        {
            try
            {
                ArticleDal article = new ArticleDal();
                string message = article.SaveArticle(title, tags, newsType, sortId, dates, zhaiYao, content, out bool flag, id);
                int bs = flag == true ? 0 : 1;
                if (flag)
                {
                    return Json(new { flag = bs, message = message });
                }
                else
                {
                    return Json(new { flag = bs, message = message });
                }
            }
            catch (Exception ex)
            {
                Log.Error("错误:新闻资讯存盘", ex.Message);
                LogQueue.Write(LogType.Error, "Article/SaveArticle", ex.Message);
                return Json(new { success = false, message = "新闻资讯存盘失败！" });
            }

        }

        /// <summary>
        /// 帮助资讯存盘
        /// </summary>
        /// <param name="json"></param>
        /// <param name="proc"></param>
        /// <param name="UserId"></param>
        /// <param name="entid"></param>
        /// <returns></returns>
        [ValidateInput(false)]
        [HttpPost]
        public JsonResult SaveHelpCategory(string proc, string type, string class_layer, string parent_id, string sort_id, string title, string content, string id)
        {
            try
            {
                if (id == "") {
                    id = "0";
                }
                SqlRun sql = new SqlRun(SqlRun.sqlstr);
               
                DataTable dt = sql.RunProcedureDR(proc, new SqlParameter[] {
                    new SqlParameter("type",type),
                    new SqlParameter("class_layer",class_layer),
                    new SqlParameter("parent_id",parent_id),
                    new SqlParameter("sort_id",sort_id),
                    new SqlParameter("title",title),
                    new SqlParameter("channel_id","11"),
                    new SqlParameter("call_index",""),
                    new SqlParameter("seo_title",""),
                    new SqlParameter("content",content),
                    new SqlParameter("id",id)
                });
                if (dt.Rows.Count > 0 && (int.Parse(dt.Rows[0]["flag"].ToString())) > 0)
                {
                    return Json(new { flag = true, message = "添加成功" });
                }
                else
                {
                    return Json(new { flag = false, message = "添加失败" });
                }
            }
            catch (Exception ex)
            {
                Log.Error("错误:帮助资讯存盘", ex.Message);
                LogQueue.Write(LogType.Error, "Article/SaveArticle", ex.Message);
                return Json(new { success = false, message = "帮助资讯存盘失败！" });
            }

        }

    }

}
