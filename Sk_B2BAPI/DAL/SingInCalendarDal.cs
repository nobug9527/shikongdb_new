using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sk_B2BAPI.Models;
using System.Data;
using System.Data.SqlClient;
using Sk_B2BAPI.App_Code;
using Sk_B2BAPI.Tool;

namespace Sk_B2BAPI.DAL
{
    public class SingInCalendarDal
    {
        public string Rule(string entId)
        {
            string Json = "";
            SqlParameter[] sqljyfw = new SqlParameter[]
            {
               new SqlParameter("@Type","GetRule"),
               new SqlParameter("@entid",entId)
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataSet ds = sql.RunProDataSet("Proc_SignLog", sqljyfw);
            if (ds.Tables.Count >= 3)
            {
                if (ds.Tables[1].Rows.Count > 0)
                {
                    Json = JsonHelper.GetDataJson(0, 0, 0, ds.Tables[1]);
                }
                else
                {
                    string error = "无数据";
                    Json = JsonHelper.GetErrorJson(1, 0, error);
                }
            }
            else
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    int recordCount = 0;
                    int pageCount = 0;
                    Json = JsonHelper.GetDataJson(0, recordCount, pageCount, ds.Tables[0]);
                }
                else
                {
                    string error = "无数据";
                    Json = JsonHelper.GetErrorJson(1, 0, error);
                }
            }
            return Json;
        }

        public string QSign(string entId, string userId)
        {
            string Json = "";
            SqlParameter[] sqls = new SqlParameter[]
            {
               new SqlParameter("@Type","GetUserSign"),
               new SqlParameter("@UserId",userId),
               new SqlParameter("@entid",entId)
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataSet ds = sql.RunProDataSet("Proc_SignLog", sqls);
            if (ds.Tables.Count >= 3)
            {
                if (ds.Tables[1].Rows.Count > 0)
                {
                    Json = JsonHelper.GetTablesDataJson(0, 0, 0, ds.Tables[0], ds.Tables[2], ds.Tables[1]);
                }
                else
                {
                    string error = "无数据";
                    Json = JsonHelper.GetErrorJson(1, 0, error);
                }
            }
            else
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    int recordCount = 0;
                    int pageCount = 0;
                    Json = JsonHelper.GetDataJson(0, recordCount, pageCount, ds.Tables[0]);
                }
                else
                {
                    string error = "无数据";
                    Json = JsonHelper.GetErrorJson(1, 0, error);
                }
            }
            return Json;
        }

        public string ReturnSign(string entId,string userId)
        {

            string Json = "";
            SqlParameter[] sqls = new SqlParameter[]
            {
               new SqlParameter("@Type","InsertUserSign"),
               new SqlParameter("@UserId",userId),
               new SqlParameter("@entid",entId)
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataSet ds = sql.RunProDataSet("Proc_SignLog", sqls);
            if (ds.Tables.Count >= 3)
            {
                if (ds.Tables[1].Rows.Count > 0)
                {
                    Json = JsonHelper.GetDataJson(0, 0, 0, ds.Tables[1]);
                }
                else
                {
                    string error = "无数据";
                    Json = JsonHelper.GetErrorJson(1, 0, error);
                }
            }
            else
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    int recordCount = 0;
                    int pageCount = 0;
                    Json = JsonHelper.GetDataJson(0, recordCount, pageCount, ds.Tables[0]);
                }
                else
                {
                    string error = "无数据";
                    Json = JsonHelper.GetErrorJson(1, 0, error);
                }
            }
            return Json;
        }

        public string RecordSign(string entId,string userId)
        {
            SqlParameter[] parameter = new SqlParameter[]{
                 new SqlParameter("@type","GetUserSign"),
                 new SqlParameter("@UserId",userId),
                 new SqlParameter("@entid",entId)
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataSet ds = sql.RunProDataSet("Proc_SignLog", parameter);
            string jsonStr;
            if (ds.Tables.Count>0)
            {
                DataTable dt_hz = ds.Tables[0];
                DataTable dt_jf = ds.Tables[1];
                DataTable dt3 = ds.Tables[3];
                string hz = "";
                string gifts = "";
                string dates = "";
                string history = "";
                //签到汇总
                bool is_sign = (dt_hz.Rows[0]["flag"].ToString().Trim().Equals("1"));
                hz = "\"is_sign\":\"" + is_sign + "\",\"even_sign_num\":\"" + dt_hz.Rows[0]["days"] + "\",\"cumulative_sign\":\"" + dt3.Rows.Count + "\",";
                //奖励列表
                string gift = "";
                foreach (DataRow dw in dt_jf.Rows)
                {
                    gift += "{\"id\":\"" + dw["ID"] + "\",\"name\":\"" + dw["RewardForm"] + "\",\"reward\":\"" + dw["SignReward"] + "\"},";
                }
                if (gift.Length > 0)
                {
                    gift = gift.Remove(gift.Length - 1, 1);
                }
                gifts = "\"gifts\":[" + gift + "],";
                //签到日期记录
                DataTable dt_date = new DataTable();
                dt_date.Columns.Add(new DataColumn("time", typeof(string)));
                dt_date.Columns.Add(new DataColumn("is_sign", typeof(bool)));
                for (int i = 6; i >= -1; i--)
                {
                    DataRow dw = dt_date.NewRow();
                    dw["time"] = DateTime.Now.AddDays(-i).ToString("yyyy-MM-dd");
                    dt_date.Rows.Add(dw);
                }
                foreach (DataRow dw_date in dt_date.Rows)
                {
                    bool is_qd = false;
                    foreach (DataRow dw3 in dt3.Rows)
                    {

                        DateTime d_time1 = DateTime.Parse(dw3["addtime"].ToString().Substring(0, 10));
                        DateTime d_time2 = DateTime.Parse(dw_date["time"].ToString());
                        if (d_time1 == d_time2)
                        {
                            //表示这个日期已经签到
                            is_qd = true;
                            break;
                        }
                    }
                    dw_date["is_sign"] = is_qd;
                }
                dates = "\"dates\":" + JsonHelper.ToJson(dt_date) + ",";
                //本月全部签到历史
                DataTable dtHistory = new DataTable();
                dtHistory.Columns.Add(new DataColumn("date", typeof(string)));
                dtHistory.Columns.Add(new DataColumn("time", typeof(string)));
                dtHistory.Columns.Add(new DataColumn("from", typeof(string)));
                dtHistory.Columns.Add(new DataColumn("reward", typeof(string)));
                string date;
                foreach (DataRow dw in dt3.Rows)
                {
                    date = dw["addtime"].ToString();
                    DataRow dw_date = dtHistory.NewRow();
                    dw_date["from"] = dw["RewardForm"];
                    dw_date["reward"] = dw["SignReward"];
                    dw_date["date"] = date.Substring(0, 10);
                    dw_date["time"] = date.Remove(0, 11);
                    dtHistory.Rows.Add(dw_date);
                }

                history = "\"history\":" + JsonHelper.ToJson(dtHistory);
                jsonStr = "{\"return_code\":0,\"data\":{" + hz + gifts + dates + history + "}}";
            }
            else
            {
                jsonStr = "{\"return_code\":1,\"data\":{}}";
            }
            return jsonStr;
        }
    }
}