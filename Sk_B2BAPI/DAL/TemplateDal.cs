using Sk_B2BAPI.App_Code;
using Sk_B2BAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace Sk_B2BAPI.DAL
{
    public class TemplateDal
    {
        #region 获取网站系统
        /// <summary>
        ///获取网站系统
        /// </summary>
        /// <param name="entId"></param>
        /// <returns></returns>
        public List<SystemBase> GetSystemBase(string entId)
        {
            DataTable dt = new DataTable();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select web_name,web_ip,company,complaints,xxjyz,xxfwz,icp,img_logo,img_app,img_left,img_right,img_service,link_service from dt_system_base where entId=@entId");
            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@entId",entId),
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            dt = sql.RunSqlDataTable(strSql.ToString(), param);
            List<SystemBase> list = new List<SystemBase>();
            if (dt.Rows.Count > 0)
            {
                list = SetSystemBase(dt);
            }
            return list;
        }
        public List<SystemBase> SetSystemBase(DataTable dt)
        {
            List<SystemBase> list = new List<SystemBase>();
            SystemBase s = new SystemBase();
            s.Title = dt.Rows[0]["web_name"].ToString();
            s.Web_Ip = dt.Rows[0]["web_ip"].ToString();
            s.Company = dt.Rows[0]["company"].ToString();
            s.Complaints = dt.Rows[0]["complaints"].ToString();
            s.Xxjyz = dt.Rows[0]["xxjyz"].ToString();
            s.Xxfwz = dt.Rows[0]["xxfwz"].ToString();
            s.ICP = dt.Rows[0]["icp"].ToString();
            s.Img_Logo = dt.Rows[0]["img_logo"].ToString();
            s.Img_App = dt.Rows[0]["img_app"].ToString();
            s.Img_Left = dt.Rows[0]["img_left"].ToString();
            s.Img_Right = dt.Rows[0]["img_right"].ToString();
            s.Img_Service = dt.Rows[0]["img_service"].ToString();
            s.Link_Service = dt.Rows[0]["link_service"].ToString();
            list.Add(s);
            return list;
        }
        #endregion

        #region 获取主题色
        /// <summary>
        /// 主题色
        /// </summary>
        /// <param name="entId">机构</param>
        /// <returns></returns>
        public string SubjectColor(string entId)
        {
            DataTable dt = new DataTable();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top 1 Value from Zzsk_ConfigValue a(nolock) join Zzsk_Configuration b(nolock) on a.ConfigId=b.Id and b.TypeCode=901 where a.Status = 1 and b.Status = 1 and b.Code=0 and a.Entid = @entId");
            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@entId",entId),
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            dt = sql.RunSqlDataTable(strSql.ToString(), param);
            string color = "";
            if (dt.Rows.Count > 0)
            {
                color = dt.Rows[0]["Value"].ToString();
            }
            return color;
        }
        #endregion
    }
}