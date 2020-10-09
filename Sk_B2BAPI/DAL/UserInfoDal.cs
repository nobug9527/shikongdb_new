using Sk_B2BAPI.App_Code;
using Sk_B2BAPI.Models;
using Sk_B2BAPI.Tool;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Providers.Entities;

namespace Sk_B2BAPI.DAL
{
    public class UserInfoDal
    {
        #region 用户登陆
        /// <summary>
        /// 用户登陆
        /// </summary>
        /// <param name="UserName">登陆名</param>
        /// <param name="passWord">密码</param>
        /// <returns></returns>
        public DataTable UserLoginDal(string UserName, string loginType, string passWord)
        {
            StringBuilder strSql = new StringBuilder();
            if (loginType == "account")
            {
                if (UserName.Contains("DWI") || UserName.Contains("dwi"))
                {
                    strSql.Append("select userid,entid,status,password,role_type from dt_users where businessid=@UserName and password=@PassWord and role_id=1");
                }
                else
                {
                    strSql.Append("select userid,entid,status,password,role_type from dt_users where UserName=@UserName and password=@PassWord and role_id=1");
                }
            }
            else
            {
                strSql.Append("select userid,entid,status,password,role_type from dt_users where telphone=@UserName and password=@PassWord and role_id=1");
            }
            SqlParameter[] param=new SqlParameter[]{
                new SqlParameter("@UserName",UserName),
                new SqlParameter("@PassWord",passWord)
            };

            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataTable dt = new DataTable();
            dt = sql.RunSqlDataTable(strSql.ToString(), param);

            return dt;
        }
        #endregion

        #region 获取用户信息
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public List<UserInfo> GetUserInfo(string userId,string entId)
        {
            DataSet ds = new DataSet();
            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@type","UserInfo"),
                new SqlParameter("@userId",userId),
                new SqlParameter("@entId",entId)
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            ds = sql.RunProDataSet("Proc_UserInfo", param);
            List<UserInfo> Ulist=new List<UserInfo>();
            if (ds.Tables[0].Rows.Count > 0)
            {
                Ulist = SetUserInfo(ds.Tables[0], ds.Tables[1]);
            }
            return Ulist;
        }
        /// <summary>
        /// 填充Models(UserInfo)数据
        /// </summary>
        /// <returns></returns>
        private List<UserInfo> SetUserInfo(DataTable dt,DataTable data)
        {
            //获取用户所有绑定证书存入键值对myDic
            Dictionary<string, string> myDic = new Dictionary<string, string>();
            foreach (DataRow item in data.Rows)
            {
                myDic.Add(item["fdname"].ToString().Trim(), item["materialName"].ToString().Trim());
            }
            //用户信息字段名
            DataColumnCollection dataColumn = dt.Columns;

            string webUrl = BaseConfiguration.SercerIp.ToString();
            List<UserInfo> list = new List<UserInfo>();
            foreach (DataRow dr in dt.Rows)
            {
                UserInfo User = new UserInfo() {
                    EntId = dr["entid"].ToString().Trim(),
                    UserId = dr["userid"].ToString().Trim(),
                    UserName = dr["name"].ToString().Trim(),
                    Sex = dr["sex"].ToString().Trim(),
                    Telphone = dr["telphone"].ToString().Trim(),
                    Province = dr["province"].ToString().Trim(),
                    City = dr["city"].ToString().Trim(),
                    Balance=dr["balance"].ToString(),
                    Point = int.Parse(dr["point"].ToString().Trim()),
                    Status=int.Parse(dr["status"].ToString()),
                    BusinessId = dr["businessid"].ToString().Trim(),
                    BusinessScope=dr["businesscont"].ToString().Trim(),
                    BusinessName = dr["businessname"].ToString().Trim(),
                    Longitude = dr["Longitude"].ToString().Trim(),
                    Latitude = dr["Latitude"].ToString().Trim(),
                    Pricelevel = dr["shortname"].ToString().Trim(),
                    PassWord = dr["password"].ToString().Trim(),
                    KhType = dr["TypeID"].ToString().Trim(),
                    ClientType=dr["clienttype"].ToString().Trim(),
                    CustomerTypeId=dr["CustomerTypeId"].ToString().Trim(),
                    ClientLimit = dr["clientlimit"].ToString().Trim(),
                    CouponCount =int.Parse(dr["CouponCount"].ToString())
                };
                if (dr["img_url"].ToString().Trim()!="")
                {
                    User.Img_Url = webUrl + dr["img_url"].ToString().Trim();
                }
                else
                {
                    User.Img_Url = "";
                }

                /*证书效期提示*/
                string message = "";
                /*证书过期数*/
                int number = 0;
                bool staleDated = false;
                /*提前day天开始提示*/
                int day = BaseConfiguration.Day;
                /*当前日期*/
                var nowDate = DateTime.Now;

                List<Certificate> Certificates = new List<Certificate>();
                //遍历字段名
                foreach (DataColumn item in dataColumn)
                {
                    foreach (var key in myDic.Keys)
                    {
                        if (item.ColumnName.Contains(key))
                        {
                            if (dr[key].ToString() != "")
                            {
                                bool xq = DateTime.TryParse(dr[key].ToString().Trim(), out DateTime resxq);
                                if (xq)
                                {
                                    var diff = BasisConfig.DateDiff(nowDate, resxq);
                                    if (diff < day && diff > 0)
                                    {
                                        message += myDic[key] + "即将过期！\r\n";
                                    }
                                    else if (diff < day && diff < 0)
                                    {
                                        staleDated = true;
                                        number++;
                                        message += myDic[key] + "过期！\r\n";
                                    }
                                }
                            }
                            Certificate certificate = new Certificate()
                            {
                                CertificateName = myDic[key],
                                FieldName = key,
                                ExpiryDate = dr[key].ToString(),
                                TimeExpiration = staleDated
                            };
                            Certificates.Add(certificate);
                        }
                    }
                }
                User.Certificates = Certificates;
                User.OverdueNumber = number;
                #region _弃用【登录时提示证书到期时间】
                //#region 医疗器械经营许可证 
                //if (dr["ylqxjyxkzxq"].ToString().Split('&')[1] == "Y" && dr["ylqxjyxkzxq"].ToString().Split('&')[0].Trim() != "")
                //{
                //    bool FLAGYLQXJYXKZXQ = DateTime.TryParse(dr["ylqxjyxkzxq"].ToString().Split('&')[0].Trim(), out DateTime RESYLQXJYXKZXQ);
                //    if (FLAGYLQXJYXKZXQ)
                //    {
                //        var diff = BasisConfig.DateDiff(nowDate, RESYLQXJYXKZXQ);
                //        if (diff < day && diff > 0)
                //        {
                //            message += "医疗器械经营许可证即将过期！\r\n";
                //        }
                //        else if (diff < day && diff < 0)
                //        {
                //            staleDated = true;
                //            message += "医疗器械经营许可证过期！\r\n";
                //        }
                //    }
                //}
                //User.YLQXJYXKZXQ = dr["ylqxjyxkzxq"].ToString().Split('&')[0].Trim();/*医疗器械经营许可证*/
                //#endregion
                //#region 药品生产许可证效期
                //if (dr["ypscxkzxq"].ToString().Split('&')[1] == "Y" && dr["ypscxkzxq"].ToString().Split('&')[0].Trim() != "")
                //{
                //    bool FLAGYPSCXKZXQ = DateTime.TryParse(dr["ypscxkzxq"].ToString().Split('&')[0].Trim(), out DateTime RESYPSCXKZXQ);
                //    if (FLAGYPSCXKZXQ)
                //    {
                //        var diff = BasisConfig.DateDiff(nowDate, RESYPSCXKZXQ);
                //        if (diff < day && diff > 0)
                //        {
                //            message += "药品生产许可证即将过期！\r\n";
                //        }
                //        else if (diff < day && diff < 0)
                //        {
                //            staleDated = true;
                //            message += "药品生产许可证过期！\r\n";
                //        }
                //    }
                //}
                //User.YPSCXKZXQ = dr["ypscxkzxq"].ToString().Split('&')[0].Trim();/*药品生产许可证*/
                //#endregion
                //#region 医疗器械生产许可证效期
                //if (dr["ylqxscxkzxq"].ToString().Split('&')[1] == "Y" && dr["ylqxscxkzxq"].ToString().Split('&')[0].Trim() != "")
                //{
                //    bool FLAGYLQXSCXKZXQ = DateTime.TryParse(dr["ylqxscxkzxq"].ToString().Split('&')[0].Trim(), out DateTime RESYLQXSCXKZXQ);
                //    if (FLAGYLQXSCXKZXQ)
                //    {
                //        var diff = BasisConfig.DateDiff(nowDate, RESYLQXSCXKZXQ);
                //        if (diff < day && diff > 0)
                //        {
                //            message += "医疗器械生产许可证即将过期！\r\n";
                //        }
                //        else if (diff < day && diff < 0)
                //        {
                //            staleDated = true;
                //            message += "医疗器械生产许可证过期！\r\n";
                //        }
                //    }
                //}
                //User.YLQXSCXKZXQ = dr["ylqxscxkzxq"].ToString().Split('&')[0].Trim();/*医疗器械生产许可证*/
                //#endregion
                //#region 医疗机构执业许可证效期
                //if (dr["yljgzyxkzxq"].ToString().Split('&')[1] == "Y" && dr["yljgzyxkzxq"].ToString().Split('&')[0].Trim() != "")
                //{
                //    bool FLAGYLJGZYXKZXQ = DateTime.TryParse(dr["yljgzyxkzxq"].ToString().Split('&')[0].Trim(), out DateTime RESYLJGZYXKZXQ);
                //    if (FLAGYLJGZYXKZXQ)
                //    {
                //        var diff = BasisConfig.DateDiff(nowDate, RESYLJGZYXKZXQ);
                //        if (diff < day && diff > 0)
                //        {
                //            message += "医疗机构执业许可证即将过期！\r\n";
                //        }
                //        else if (diff < day && diff < 0)
                //        {
                //            staleDated = true;
                //            message += "医疗机构执业许可证过期！\r\n";
                //        }
                //    }
                //}
                //User.YLJGZYXKZXQ = dr["yljgzyxkzxq"].ToString().Split('&')[0].Trim();/*医疗机构执业许可证效期*/
                //#endregion
                //#region 年度报告效期
                //if (dr["ndbgxq"].ToString().Split('&')[1] == "Y" && dr["ndbgxq"].ToString().Split('&')[0].Trim() != "")
                //{
                //    bool FLAGNDBGXQ = DateTime.TryParse(dr["ndbgxq"].ToString().Split('&')[0].Trim(), out DateTime RESNDBGXQ);
                //    if (FLAGNDBGXQ)
                //    {
                //        var diff = BasisConfig.DateDiff(nowDate, RESNDBGXQ);
                //        if (diff < day && diff > 0)
                //        {
                //            message += "年度报告即将过期！\r\n";
                //        }
                //        else if (diff < day && diff < 0)
                //        {
                //            staleDated = true;
                //            message += "年度报告过期！\r\n";
                //        }
                //    }
                //}
                //User.NDBGXQ = dr["ndbgxq"].ToString().Split('&')[0].Trim();/*年度报告效期*/
                //#endregion
                //#region 质量保证协议效期
                //if (dr["zlbzxyxq"].ToString().Split('&')[1] == "Y" && dr["zlbzxyxq"].ToString().Split('&')[0].Trim() != "")
                //{
                //    bool FLAGZLBZXYXQ = DateTime.TryParse(dr["zlbzxyxq"].ToString().Split('&')[0].Trim(), out DateTime RESZLBZXYXQ);
                //    if (FLAGZLBZXYXQ)
                //    {
                //        var diff = BasisConfig.DateDiff(nowDate, RESZLBZXYXQ);
                //        if (diff < day && diff > 0)
                //        {
                //            message += "质量保证协议即将过期！\r\n";
                //        }
                //        else if (diff < day && diff < 0)
                //        {
                //            staleDated = true;
                //            message += "质量保证协议过期！\r\n";
                //        }
                //    }
                //}
                //User.ZLBZXYXQ = dr["zlbzxyxq"].ToString().Split('&')[0].Trim();/*质量保证协议效期*/
                //#endregion
                //#region 母婴保健许可证
                //if (dr["mybjxkzxq"].ToString().Split('&')[1] == "Y" && dr["mybjxkzxq"].ToString().Split('&')[0].Trim() != "")
                //{
                //    bool FLAGMYBJXKZXQ = DateTime.TryParse(dr["mybjxkzxq"].ToString().Split('&')[0].Trim(), out DateTime RESMYBJXKZXQ);
                //    if (FLAGMYBJXKZXQ)
                //    {
                //        var diff = BasisConfig.DateDiff(nowDate, RESMYBJXKZXQ);
                //        if (diff < day && diff > 0)
                //        {
                //            message += "母婴保健许可证即将过期！\r\n";
                //        }
                //        else if (diff < day && diff < 0)
                //        {
                //            staleDated = true;
                //            message += "母婴保健许可证过期！\r\n";
                //        }
                //    }
                //}
                //User.MYBJXKZXQ = dr["mybjxkzxq"].ToString().Split('&')[0].Trim();/*母婴保健许可证*/
                //#endregion
                //#region 母婴保健技术许可证
                //if (dr["mybjjsxkzxq"].ToString().Split('&')[1] == "Y" && dr["mybjjsxkzxq"].ToString().Split('&')[0].Trim() != "")
                //{
                //    bool FLAGMYBJJSXKZXQ = DateTime.TryParse(dr["mybjjsxkzxq"].ToString().Split('&')[0].Trim(), out DateTime RESMYBJJSXKZXQ);
                //    if (FLAGMYBJJSXKZXQ)
                //    {
                //        var diff = BasisConfig.DateDiff(nowDate, RESMYBJJSXKZXQ);
                //        if (diff < day && diff > 0)
                //        {
                //            message += "母婴保健技术许可证即将过期！\r\n";
                //        }
                //        else if (diff < day && diff < 0)
                //        {
                //            staleDated = true;
                //            message += "母婴保健技术许可证过期！\r\n";
                //        }
                //    }
                //}
                //User.MYBJJSXKZXQ = dr["mybjjsxkzxq"].ToString().Split('&')[0].Trim();/*母婴保健技术许可证*/
                //#endregion
                //#region gsp证书效期
                //if (dr["gspxq"].ToString().Split('&')[1] == "Y" && dr["gspxq"].ToString().Split('&')[0].Trim() != "")
                //{
                //    bool FLAGGSPZSYXQ = DateTime.TryParse(dr["gspxq"].ToString().Split('&')[0].Trim(), out DateTime RESGSPZSYXQ);
                //    if (FLAGGSPZSYXQ)
                //    {
                //        var diff = BasisConfig.DateDiff(nowDate, RESGSPZSYXQ);
                //        if (diff < day && diff > 0)
                //        {
                //            message += "gsp证书即将过期！\r\n";
                //        }
                //        else if (diff < day && diff < 0)
                //        {
                //            staleDated = true;
                //            message += "gsp证书过期！\r\n";
                //        }
                //    }
                //}
                //User.GSPZSYXQ = dr["gspzsyxq"].ToString().Split('&')[0].Trim();/*gsp证书效期*/
                //#endregion
                //#region 药品经营许可证效期
                //if (dr["ypjyxkzxq"].ToString().Split('&')[1] == "Y" && dr["ypjyxkzxq"].ToString().Split('&')[0].Trim() != "")
                //{
                //    bool FLAGYPJYXKZXQ = DateTime.TryParse(dr["ypjyxkzxq"].ToString().Split('&')[0].Trim(), out DateTime RESYPJYXKZXQ);
                //    if (FLAGYPJYXKZXQ)
                //    {
                //        var diff = BasisConfig.DateDiff(nowDate, RESYPJYXKZXQ);
                //        if (diff < day && diff > 0)
                //        {
                //            message += "药品经营许可证即将过期！\r\n";
                //        }
                //        else if (diff < day && diff < 0)
                //        {
                //            staleDated = true;
                //            message += "药品经营许可证过期！\r\n";
                //        }
                //    }
                //}
                //User.YPJYXKZXQ = dr["ypjyxkzxq"].ToString().Split('&')[0].Trim();/*药品经营许可证效期*/
                //#endregion
                //#region 营业执照效期
                //if (dr["yyzzxq"].ToString().Split('&')[1] == "Y" && dr["yyzzxq"].ToString().Split('&')[0].Trim() != "")
                //{
                //    bool FLAGYYZZXQ = DateTime.TryParse(dr["yyzzxq"].ToString().Split('&')[0].Trim(), out DateTime RESYYZZXQ);
                //    if (FLAGYYZZXQ)
                //    {
                //        var diff = BasisConfig.DateDiff(nowDate, RESYYZZXQ);
                //        if (diff < day && diff > 0)
                //        {
                //            message += "营业执照即将过期！\r\n";
                //        }
                //        else if (diff < day && diff < 0)
                //        {
                //            staleDated = true;
                //            message += "营业执照过期！\r\n";
                //        }
                //    }
                //}
                //User.YYZZXQ = dr["yyzzxq"].ToString().Split('&')[0].Trim();/*营业执照效期*/
                //#endregion
                //#region 食品流通许可证效期
                //if (dr["spltxkzxq"].ToString().Split('&')[1] == "Y" && dr["spltxkzxq"].ToString().Split('&')[0].Trim() != "")
                //{
                //    bool FLAGSPLTXKZXQ = DateTime.TryParse(dr["spltxkzxq"].ToString().Split('&')[0].Trim(), out DateTime RESSPLTXKZXQ);
                //    if (FLAGSPLTXKZXQ)
                //    {
                //        var diff = BasisConfig.DateDiff(nowDate, RESSPLTXKZXQ);
                //        if (diff < day && diff > 0)
                //        {
                //            message += "食品流通许可证即将过期！\r\n";
                //        }
                //        else if (diff < day && diff < 0)
                //        {
                //            staleDated = true;
                //            message += "食品流通许可证过期！\r\n";
                //        }
                //    }
                //}
                //User.SPLTXKZXQ = dr["spltxkzxq"].ToString().Split('&')[0].Trim();/*食品流通许可证效期*/
                //#endregion
                //#region 委托书
                //if (dr["wtsxq"].ToString().Split('&')[1] == "Y" && dr["wtsxq"].ToString().Split('&')[0].Trim() != "")
                //{
                //    bool FLAGWTSXQ = DateTime.TryParse(dr["wtsxq"].ToString().Split('&')[0].Trim(), out DateTime RESWTSXQ);
                //    if (FLAGWTSXQ)
                //    {
                //        var diff = BasisConfig.DateDiff(nowDate, RESWTSXQ);
                //        if (diff < day && diff > 0)
                //        {
                //            message += "委托书即将过期！\r\n";
                //        }
                //        else if (diff < day && diff < 0)
                //        {
                //            staleDated = true;
                //            message += "委托书过期！\r\n";
                //        }
                //    }
                //}
                //User.WTSXQ = dr["wtsxq"].ToString().Split('&')[0].Trim();/*委托书*/
                //#endregion
                //#region 卫生许可证
                //if (dr["wsxkzxq"].ToString().Split('&')[1] == "Y" && dr["wsxkzxq"].ToString().Split('&')[0].Trim() != "")
                //{
                //    bool FLAGWSXKZXQ = DateTime.TryParse(dr["wsxkzxq"].ToString().Split('&')[0].Trim(), out DateTime RESWSXKZXQ);
                //    if (FLAGWSXKZXQ)
                //    {
                //        var diff = BasisConfig.DateDiff(nowDate, RESWSXKZXQ);
                //        if (diff < day && diff > 0)
                //        {
                //            message += "卫生许可证即将过期！\r\n";
                //        }
                //        else if (diff < day && diff < 0)
                //        {
                //            staleDated = true;
                //            message += "卫生许可证过期！\r\n";
                //        }
                //    }
                //}
                //User.WSXKZXQ = dr["wsxkzxq"].ToString().Split('&')[0].Trim();/*卫生许可证*/
                //#endregion
                //#region 公示许可证
                //if (dr["gsyxq"].ToString().Split('&')[1] == "Y" && dr["gsyxq"].ToString().Split('&')[0].Trim() != "")
                //{
                //    bool FLAGGSYXQ = DateTime.TryParse(dr["gsyxq"].ToString().Split('&')[0].Trim(), out DateTime RESGSYXQ);
                //    if (FLAGGSYXQ)
                //    {
                //        var diff = BasisConfig.DateDiff(nowDate, RESGSYXQ);
                //        if (diff < day && diff > 0)
                //        {
                //            message += "公示许可证即将过期！\r\n";
                //        }
                //        else if (diff < day && diff < 0)
                //        {
                //            staleDated = true;
                //            message += "公示许可证过期！\r\n";
                //        }
                //    }
                //}
                //User.GSYXQ = dr["gsyxq"].ToString().Split('&')[0].Trim();/*公示许可证*/
                //#endregion
                //#region 许可证效期
                //if (dr["xkzxq"].ToString().Split('&')[1] == "Y" && dr["xkzxq"].ToString().Split('&')[0].Trim() != "")
                //{
                //    bool FLAGXKZXQ = DateTime.TryParse(dr["xkzxq"].ToString().Split('&')[0].Trim(), out DateTime RESXKZXQ);
                //    if (FLAGXKZXQ)
                //    {
                //        var diff = BasisConfig.DateDiff(nowDate, RESXKZXQ);
                //        if (diff < day && diff > 0)
                //        {
                //            message += "许可证即将过期！\r\n";
                //        }
                //        else if (diff < day && diff < 0)
                //        {
                //            staleDated = true;
                //            message += "许可证过期！\r\n";
                //        }
                //    }
                //}
                //User.XKZXQ = dr["xkzxq"].ToString().Split('&')[0].Trim();/*许可证效期*/
                //#endregion
                #endregion

                User.StaleDated = staleDated;
                User.StaleMsg = message;

                list.Add(User);
            }
            return list;
        }

        #endregion

        #region  用户信贷
        /// <summary>
        /// 用户信贷
        /// </summary>
        /// <param name="userId">用户</param>
        /// <param name="entId">机构</param>
        /// <returns></returns>
        public UserDebt DebtInfo(string userId, string entId)
        {
            //LogQueue.Write(LogType.Debug, "1", userId);
            SqlParameter[] sqls = new SqlParameter[]
            {
                new SqlParameter("@type","UserDebt"),
                new SqlParameter("@userId",userId),
                new SqlParameter("@entId",entId)
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataTable table = sql.RunProcedureDR("Proc_UserInfo", sqls);
            UserDebt debt = new UserDebt();
            if (table.Rows.Count>0)
            {
                debt = FillDebt(table);
            }
            return debt;
        }
        /// <summary>
        /// 用户信贷数据填充
        /// </summary>
        /// <param name="table">信贷数据源</param>
        /// <returns></returns>
        public UserDebt FillDebt(DataTable table)
        {
            UserDebt debt = new UserDebt()
            {
                Xde=decimal.Parse(table.Rows[0]["xde"].ToString().Trim()),
                Xdq=int.Parse(table.Rows[0]["xdq"].ToString().Trim()),
                DebtDate=table.Rows[0]["debtdate"].ToString().Trim(),
                DebtDays=int.Parse(table.Rows[0]["debtdays"].ToString().Trim()),
                DebtMoney=decimal.Parse(table.Rows[0]["debtmoney"].ToString().Trim())
            };
            return debt;
        }
        #endregion

        #region 修改用户信息
        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="entId"></param>
        /// <param name="userId"></param>
        /// <param name="telPhone"></param>
        /// <param name="imgUrl"></param>
        /// <returns></returns>
        public static bool UpdateUserInfo(string entId, string userId, string telPhone, string imgUrl,string email = "", string qq = "", string sex = "", string birthday = "")
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update dt_users set telphone='" + telPhone + "',img_url='" + imgUrl + "',sex='"+sex+"',email='"+email+"',birthday='"+birthday+"' where userid='" + userId + "' and entid='" + entId + "'");
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            bool flag = sql.ExecuteSql(strSql.ToString());
            return flag;
        }
        #endregion

        #region 修改用户密码
        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="entId"></param>
        /// <param name="userId"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public static bool UpdateUserPwd(string entId, string userId, string pwd)
        {
            try
            {
                SqlParameter[] sqls = new SqlParameter[] { 
                    new SqlParameter("@type","UpdatePassword"),
                    new SqlParameter("@userId",userId),
                    new SqlParameter("@entId",entId),
                    new SqlParameter("@pwd",pwd)
                };
                SqlRun sql = new SqlRun(SqlRun.sqlstr);
                int num = sql.ExecuteNonQuery("Proc_UserInfo", sqls);
                if (num>0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "UserInfoDal/UpdateUserPwd", $"{ex.Message}");
                return false;
            }
        }
        #endregion

        #region 保存注册信息
        /// <summary>
        /// 保存注册信息
        /// </summary>
        /// <param name="entId">机构</param>
        /// <param name="businessName">单位</param>
        /// <param name="telphone">电话</param>
        /// <param name="clinettype">客户类型</param>
        /// <param name="address">地址</param>
        /// <param name="birthday">生日</param>
        /// <param name="email">邮件</param>
        /// <param name="password">密码</param>
        /// <param name="name">姓名</param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public string SaveUserInfo(string entId, string businessName,string clinettype, string telphone, string sex, string email, string password, string name, string province, string city,string prefecture, string address, string birthday, string material, out bool flag)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[] {
                    new SqlParameter("@type","Registered"),
                    new SqlParameter("@entId",entId),
                    new SqlParameter("@password",password),
                    new SqlParameter("@sex",sex),
                    new SqlParameter("@birthday",birthday),
                    new SqlParameter("@telphone",telphone),
                    new SqlParameter("@email",email),
                    new SqlParameter("@province",province),
                    new SqlParameter("@city",city),
                    new SqlParameter("@prefecture",prefecture),
                    new SqlParameter("@address",address),
                    new SqlParameter("@businessname",businessName),
                    new SqlParameter("@clinettype",clinettype),
                    new SqlParameter("@name",name),
                    new SqlParameter("@material",material)
                };
                SqlRun sql = new SqlRun(SqlRun.sqlstr);
                int number = sql.ExecuteNonQuery("Proc_OperationUser", parameters);
                if (number > 0)
                {
                    flag = true;
                    return "账号注册成功，请等待审核！";
                }
                else
                {
                    flag = false;
                    return "账号注册失败，请重新注册！";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                return ex.Message.ToString();
            }
        }
        #endregion

        #region 业务员开放注册
        /// <summary>
        /// 业务员开放注册
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="name"></param>
        /// <param name="telphone"></param>
        /// <returns>1:成功,0:失败，-1：用户名已存在</returns>
        public int AddSalesman(string username,string password,string name,string telphone)
        {
            SqlSugarHelper helper = new SqlSugarHelper();
            int reun= helper.Db.Queryable<Models.dt_users>().Where(m => m.username == username).Count();
            if (reun > 0)
                return -1;
            string maxbh = helper.Db.Ado.GetScalar("select maxbh from dt_Maxbh where BiaoShi='YW'").ToString() ;
            string entid = helper.Db.Ado.GetScalar("select top 1 entid from dt_entdoc").ToString();
            string userid = (Convert.ToInt32(maxbh)+1).ToString();
            while (userid.Length < 7)
            {
                userid = "0" + userid;
            }
            userid = "YW" + userid;
            Models.dt_users model = new dt_users
            {
                userid = userid,
                entid = entid,
                username = username,
                password = Encryption.GetMD5_16(password),
                name = name,
                telphone = telphone,
                role_type = 3,// 业务员
                status = 1,
                salt = "2H8H0P",
                role_id = 1,
                add_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            };       
            int flag = helper.Db.Insertable(model).ExecuteCommand();
            if (flag > 0)
            {
                int new_maxbh = Convert.ToInt32(maxbh) + 1;
                helper.Db.Ado.ExecuteCommand("update dt_Maxbh set [maxbh]=" + new_maxbh + " where BiaoShi='YW'");
            }
            return flag;
        }
        #endregion

        #region 开放注册业务员审核页，列表，带分页
        /// <summary>
        /// 获取业务员列表，带分页
        /// </summary>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="soustr"></param>
        /// <returns></returns>
        public object GetSalesmanList(int pageindex, int pagesize, string soustr,ref int total)
        {
            SqlSugarHelper helper = new SqlSugarHelper();
            var data = helper.Db.Queryable<dt_users, dt_entdoc>((u, e) => u.entid == e.entid)
                .Where((u, e) => u.role_type == 3 && u.status==1 && (e.entname.Contains(soustr) || u.username.Contains(soustr) || u.name.Contains(soustr) || u.telphone.Contains(soustr)))
                .Select((u, e) => new { entname = e.entname, entid = u.entid, userid = u.userid, username = u.username, telphone = u.telphone, name = u.name, status = u.status, add_time = u.add_time })
                .ToPageList(pageindex, pagesize, ref total);
            return data;
        }
        #endregion

        #region 批量删除未审批业务员
        /// <summary>
        /// 批量删除未审批业务员
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public int BatchDeleteSalesman(string[] userids)
        {
            SqlSugarHelper helper = new SqlSugarHelper();
            int flag = helper.Db.Deleteable<dt_users>().Where(m =>userids.Contains(m.userid) && m.role_type==3 && m.status==1).ExecuteCommand();
            return flag;
        }
        #endregion

        #region 分配审核注册业务员
        /// <summary>
        /// 分配审核注册业务员
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="entid"></param>
        /// <returns>0:此业务员已审核或userid不存在</returns>
        public int AuditSalesman(string userid, string entid)
        {
            SqlSugarHelper helper = new SqlSugarHelper();
            var user = helper.Db.Queryable<dt_users>().First(m => m.userid == userid && m.status == 1 && m.role_type == 3);
            if (user == null)
                return 0;
            int flag = helper.Db.Ado.ExecuteCommand("update dt_users set entid=@entid,[status]=2 where userid=@userid",
                new SugarParameter("@entid", entid),
                new SugarParameter("@userid", userid)
                );
            //.Updateable<dt_users>()
            //    .SetColumns(m => new dt_users() { entid = entid, status = 2 })
            //    .Where(m=>m.userid== userid)
            //    .ExecuteCommand();
            return flag;
        }
        #endregion

        #region 注册手机号验证
        /// <summary>
        /// 注册手机号验证
        /// </summary>
        /// <param name="telphone">手机号</param>
        /// <returns></returns>
        public int PhoneVerify(string telphone,out string entId)
        {
            entId = "";
            SqlParameter[] sqls = new SqlParameter[] {
                    new SqlParameter("@type","PhoneVerify"),
                    new SqlParameter("@telphone",telphone)
                };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataTable table = sql.RunProcedureDR("Proc_OperationUser", sqls);
            if (table.Rows.Count > 0)
            {
                entId = table.Rows[0]["entid"].ToString();
                return int.Parse(table.Rows[0]["status"].ToString());
            }
            else
            {
                entId = "";
                return 9999;
            }
        }
        #endregion

        #region 获取用户资料
        /// <summary>
        /// 获取用户资料
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="entId">机构Id</param>
        /// <returns></returns>
        public List<UserRegister> AcquireMaterial(string telphone, string entId)
        {
            SqlParameter[] sqls = new SqlParameter[] {
                    new SqlParameter("@type","AcquireMaterial"),
                    new SqlParameter("@telphone",telphone),
                    new SqlParameter("@entId",entId)
                };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            //DataTable table = sql.RunProcedureDR("Proc_OperationUser", sqls);
            DataSet set = sql.RunProDataSet("Proc_OperationUser", sqls);
            //LogQueue.Write(LogType.Debug, "表数目", set.Tables.Count.ToString());
            List<UserRegister> infos = new List<UserRegister>();
            if (set.Tables.Count > 0)
            {
                infos = FillList(set);
            }
            else
            {
                infos=null;
            }
            return infos;
        }

        /// <summary>
        /// 用户信息填充（包含资料）
        /// </summary>
        /// <param name="set"></param>
        /// <returns></returns>
        public List<UserRegister> FillList(DataSet set)
        {
            //string webUrl = BaseConfiguration.SercerIp.ToString();
            List<UserRegister> registers = new List<UserRegister>();
            foreach (DataRow item in set.Tables[0].Rows)
            {
                UserRegister register = new UserRegister()
                {
                    UserId = item["userid"].ToString(),
                    EntId = item["entid"].ToString(),
                    Sex = item["sex"].ToString(),
                    Birthday = item["birthday"].ToString(),
                    Telphone = item["telphone"].ToString(),
                    Email = item["email"].ToString(),
                    Province = item["province"].ToString(),
                    City = item["city"].ToString(),
                    Prefecture = item["prefecture"].ToString(),
                    Address = item["address"].ToString(),
                    Status = int.Parse(item["status"].ToString()),
                    AddTime = DateTime.Parse(item["add_time"].ToString()),
                    BusinessName = item["businessname"].ToString(),
                    ClinetType=item["clinettype"].ToString(),
                    Name = item["name"].ToString(),
                    Remark=item["remark"].ToString()
                };
                List<UserMaterial> infos = new List<UserMaterial>();
                foreach (DataRow row in set.Tables[1].Rows)
                {
                    UserMaterial material = new UserMaterial()
                    {
                        Id=int.Parse(row["id"].ToString()),
                        MaterialName = row["materialName"].ToString(),
                        Remark=row["remark"].ToString()
                    };
                    if (row["materialUrl"].ToString() != "")
                    {
                        //material.MaterialUrl = webUrl + row["materialUrl"].ToString();
                        material.MaterialUrl = row["materialUrl"].ToString();
                    }
                    else
                    {
                        material.MaterialUrl = "";
                    }
                    infos.Add(material);
                }
                register.Materials = infos;
                registers.Add(register);
            }
            return registers;
        }

        #endregion

        #region 用户充值红包
        /// <summary>
        /// 用户充值红包
        /// </summary>
        /// <param name="userId">用户</param>
        /// <param name="entId">机构</param>
        /// <param name="target">目标 unused[未使用]/used[已使用]</param>
        /// <param name="money">金额</param>
        /// <param name="normal">true[正常]/false[超期异常]</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="pageSize">单页数目</param>
        /// <param name="pageCount">返回总页数</param>
        /// <param name="recordCount">返回总条目数</param>
        /// <returns></returns>
        public List<Bonus> RedEnvelopes(string userId, string entId, string target, decimal money, string normal, int pageIndex, int pageSize,out int pageCount,out int recordCount)
        {
            SqlParameter[] sqls = new SqlParameter[]
            {
                new SqlParameter("@type","rechargeBonus"),
                new SqlParameter("@userId",userId),
                new SqlParameter("@entId",entId),
                new SqlParameter("@target",target),
                new SqlParameter("@money",money),
                new SqlParameter("@normal",normal),
                new SqlParameter("@pageIndex",pageIndex),
                new SqlParameter("@pageSize",pageSize)
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            pageCount = 1;
            recordCount = 0;
            DataSet set = sql.RunProDataSet("Proc_OperationRecharge", sqls);
            List<Bonus> bonus = new List<Bonus>();
            if (set.Tables.Count>0)
            {
                bonus = FillBonus(set.Tables[1]);
                pageCount = int.Parse(set.Tables[2].Rows[0]["pageCount"].ToString());
                recordCount = int.Parse(set.Tables[2].Rows[0]["recordCount"].ToString());
            }
            else
            {
                bonus = null;
            }
            return bonus;
        }

        /// <summary>
        /// 用户红包信息填充
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public List<Bonus> FillBonus(DataTable table)
        {
            List<Bonus> bonus = new List<Bonus>();
            foreach (DataRow item in table.Rows)
            {
                Bonus bonus1 = new Bonus()
                {
                    Id = int.Parse(item["Id"].ToString()),
                    BonusAmount = decimal.Parse(item["bonusAmount"].ToString()),
                    BonusNum = int.Parse(item["bonusNum"].ToString()),
                    ReceiveTime = item["receiveTime"].ToString(),
                    Source=item["source"].ToString()
                };
                bonus.Add(bonus1);
            }
            return bonus;
        }
        #endregion

        #region 资质证书
        /// <summary>
        /// 修改已经过期的资质
        /// </summary>
        /// <param name="entId"></param>
        /// <param name="userId"></param>
        /// <param name="imgurl"></param>
        /// <param name="materialId"></param>
        /// <returns></returns>
        public static bool UpdateUsersMaterial(string entId, string userId, string imgurl,string materialId, string materialname, string customertypeid)
        {
            StringBuilder strSql = new StringBuilder();
            if (int.Parse(materialId) != 0)
            {
                strSql.Append("update dt_users_material Set materialUrl='" + imgurl + "'  where id=" + materialId + "");
            }
            else {
                //strSql.Append("INSERT INTO [dbo].[dt_users_material] VALUES('"+userId+"','" + entId + "',"+ customertypeid + ",'"+ materialname + "','"+ imgurl + "','','"+DateTime.Now+"')");
                strSql.Append("insert into [dbo].[dt_users_material](userId,entId,CustomerTypeId,materialName,materialUrl,remark,lastmodifytime) select top 1 userid,entid," + customertypeid + ",'"+ materialname + "','"+ imgurl + "','','"+ DateTime.Now.ToString() + "' from dt_register where dtuserid='"+userId+"' and entid='"+entId+"'");
            }
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            bool flag = sql.ExecuteSql(strSql.ToString());
            return flag;
        }
        #endregion

        #region 服务器信息
        public void ServerVariables(string entId, string userId,HttpRequest request)
        {
            //发出请求的远程主机的IP地址
            var addr = request.ServerVariables["Remote_Addr"].ToString();
            //发出请求的远程主机名称
            var host = "";
            //发出请求的远程主机端口
            var port = request.ServerVariables["SERVER_PORT"].ToString();

            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into ServerVariables(UserID,EntID,Host,Addr,Port,RecordDate)values(@userId,@entId,@host,@addr,@port,@date)");
            SqlParameter[] sqls = new SqlParameter[] {
                    new SqlParameter("@userId",userId),
                    new SqlParameter("@entId",entId),
                    new SqlParameter("@host",host),
                    new SqlParameter("@addr",addr),
                    new SqlParameter("@port",port),
                    new SqlParameter("@date",DateTime.Now)
             };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            sql.RunSqlNumber(strSql.ToString(), sqls);
        }
        #endregion

        #region 个人中心角标
        /// <summary>
        /// 个人中心角标
        /// </summary>
        /// <param name="entId">企业</param>
        /// <param name="userId">用户</param>
        /// <returns></returns>
        public CornerMark CornerMark(string entId, string userId)
        {
            DataTable table = new DataTable();
            SqlParameter[] sql = new SqlParameter[]
            {
                new SqlParameter("@type","CornerMark"),
                new SqlParameter("@userId",userId),
                new SqlParameter("@entId",entId)
            };
            SqlRun sqlRun = new SqlRun(SqlRun.sqlstr);
            table = sqlRun.RunSqlDataTable("Proc_UserInfo", sql);

            CornerMark cornerMark = new CornerMark();
            if (table.Rows.Count>0)
            {
                cornerMark = CornerMarkFill(table.Rows[0]);
            }
            return cornerMark;
        }
        /// <summary>
        /// 个人中心角标数据填充
        /// </summary>
        /// <param name="entId">企业</param>
        /// <param name="userId">用户</param>
        /// <returns></returns>
        private CornerMark CornerMarkFill(DataRow row)
        {
             CornerMark cornerMark = new CornerMark()
             {
                 Coupon=int.Parse(row["coupon"].ToString()),
                 RechargeRule=int.Parse(row["rechargeRule"].ToString()),
                 Bonus=int.Parse(row["bonus"].ToString()),
                 Message=int.Parse(row["message"].ToString()),
                 VersionNo=row["versionNo"].ToString(),
                 NoPay=int.Parse(row["NoPay"].ToString()),
                 NotReceiving=int.Parse(row["NotReceiving"].ToString()),
                 NoEvaluation= int.Parse(row["NoEvaluation"].ToString()),
                 AfterSale=int.Parse(row["AfterSale"].ToString())
             };
            return cornerMark;
        }
        #endregion
    }
}