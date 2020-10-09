using Sk_B2BAPI.App_Code;
using Sk_B2BAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Sk_B2BAPI.Tool;

namespace Sk_B2BAPI.DAL
{
    public class ArticleDal
    {
        /// <summary>
        /// 获取文章
        /// </summary>
        /// <returns></returns>
        public List<Article> GetArticle(string entId, int channel_Id, string type)
        {
            DataTable dt = new DataTable();
            StringBuilder strSql = new StringBuilder();
            string web_url = BaseConfiguration.SercerIp;
            strSql.Append($"select id,title,call_index,parent_id,class_list,class_layer,replace (content,'lib/ueditor','{web_url}/admin/lib/ueditor') as content,sort_id from dt_article_category(nolock) where channel_id=@Channel_Id and entid=@entid ");
            strSql.Append("order by sort_id asc");
            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@Channel_Id",channel_Id),
                new SqlParameter("@entid",entId)
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            dt = sql.RunSqlDataTable(strSql.ToString(), param);
            List<Article> list = new List<Article>();
            if (dt.Rows.Count > 0)
            {
                list = SetArticle(dt,type);
            }
            return list;
        }

        public List<Article> SetArticle(DataTable dt, string type)
        {
            List<Article> list = new List<Article>();
            ///加载一级分类标题
            DataRow[] dr1 = dt.Select("class_layer=1", " sort_id asc");
            foreach (DataRow dr in dr1)
            {
                Article a1 = new Article();
                a1.Id = int.Parse(dr["id"].ToString());
                a1.Title = dr["title"].ToString();
                a1.Call_Index = dr["call_index"].ToString();
                a1.Parent_Id = dr["parent_id"].ToString();
                //a1.Category = dr["category"].ToString();
                ////加载二级分类
                DataRow[] dr2 = dt.Select("class_layer=2 and parent_id=" + a1.Parent_Id + "", " sort_id asc");
                List<Article> list1 = new List<Article>();
                foreach (DataRow drs in dr2)
                {
                    Article a2 = new Article();
                    a2.Id = int.Parse(drs["id"].ToString());
                    a2.Title = drs["title"].ToString();
                    a2.Call_Index = drs["call_index"].ToString();
                    if (type == "Detail")
                    {
                        a2.Content = drs["content"].ToString();
                    }
                    list1.Add(a2);
                }
                a1.ArticleList = list1;
                list.Add(a1);
            }
            return list;
        }
        /// <summary>
        /// 获取资讯前n条或所有数据分页
        /// </summary>
        /// <param name="entId"></param>
        /// <param name="num"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="channel"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        public List<Article> GetTopArticle(string entId, int num, int pageIndex, int pageSize, string channel, string category,string articleId,out int pageCount,out int recordCount)
        {
            if (num!=0)
            {
                pageIndex = 1;
                pageSize = num;
            }
            pageCount = 0;
            recordCount = 0;
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@type","TopArticle"),
                new SqlParameter("@entId",entId),
                new SqlParameter("@pageIndex",pageIndex),
                new SqlParameter("@pageSize",pageSize),
                new SqlParameter("@channel",channel),
                new SqlParameter("@category",category),
                new SqlParameter("@articleId",articleId)
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            var ds = sql.RunProDataSet("Proc_Article", parameters);
            List<Article> list = new List<Article>();
            if (ds.Tables.Count>0)
            {
                pageCount =int.Parse(ds.Tables[2].Rows[0]["pageCount"].ToString());
                recordCount= int.Parse(ds.Tables[2].Rows[0]["recordCount"].ToString());
                list = FillList(ds.Tables[1]);
            }
            return list;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public List<Article> FillList(DataTable table)
        {
            List<Article> list = new List<Article>();
            string web_url = BaseConfiguration.SercerIp;
            foreach (DataRow item in table.Rows)
            {
                Article article = new Article() { 
                    Id=int.Parse(item["id"].ToString()),
                    Title=item["title"].ToString(),
                    Digest=item["zhaiyao"].ToString(),
                    Category = item["category"].ToString(),
                    Content = item["content"].ToString().Replace("lib/ueditor", $"{web_url}/admin/lib/ueditor"),
                    Call_Index =item["call_index"].ToString(),
                    Class_layer=item["class_list"].ToString(),
                    Parent_Id=item["parent_id"].ToString(),
                    Add_Time=item["add_time"].ToString()
                };
                list.Add(article);
            }
            return list;
        }

        public string SaveArticle(string title, string tags, string newsType, string sortId, string dates, string zhaiYao, string content,out bool flag,int id)
        {
            try
            {
                SqlParameter[] sqls = new SqlParameter[]
                {
                    new SqlParameter("@type","SaveArticle"),
                    new SqlParameter("@title",title),
                    new SqlParameter("@tags",tags),
                    new SqlParameter("@newsType",newsType),
                    new SqlParameter("@sort_id",sortId),
                    new SqlParameter("@dates",dates),
                    new SqlParameter("@zhaiYao",zhaiYao),
                    new SqlParameter("@content",content),
                    new SqlParameter("@id",id)
                };
                SqlRun sql = new SqlRun(SqlRun.sqlstr);
                int num = sql.ExecuteNonQuery("Proc_Article", sqls);
                if (num > 0)
                {
                    flag = true;
                    return "操作成功！";
                }
                else
                {
                    flag = false;
                    return "操作失败！";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                return ex.Message.ToString();
            }
        }
    }
}