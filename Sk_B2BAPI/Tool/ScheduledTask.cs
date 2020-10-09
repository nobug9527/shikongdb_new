using Sk_B2BAPI.App_Code;
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Web.Script.Serialization;

namespace Sk_B2BAPI.Tool
{
    public class ScheduledTask
    {
        public ScheduledTask()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }
        private static readonly ScheduledTask _ScheduledTask = null;
        private System.Threading.Timer UpdateTimer = null;
        private int Interval = BaseConfiguration.Interval; //间隔时间，这里设置为15分钟
        private int _IsRunning;//上一个时间间隔触发的任务是否运行完成
        private SqlRun db = new SqlRun(SqlRun.sqlstr);
        static ScheduledTask()
        {
            _ScheduledTask = new ScheduledTask();
        }
        public static ScheduledTask Instance()
        {
            return _ScheduledTask;
        }
        /// <summary>
        /// timer启动
        /// </summary>
        public void Start()
        {
            if (UpdateTimer == null)
            {
                UpdateTimer = new System.Threading.Timer(new TimerCallback(seachnewmessage), null, Interval, Interval);
            }
        }
        /// <summary>
        /// 时钟callback事件
        /// </summary>
        /// <param name="sender"></param>
        private void UpdateTimerCallback(string sender, string alter, string menu, string sort)
        {
            string obj = sender.ToString();
            if (Interlocked.Exchange(ref _IsRunning, 1) == 0)
            {
                try
                {
                    //要处理后台任务
                    JpushV3 util = new JpushV3();

                    string reqParams;
                    if (sort == "message" && obj == "")//自定义消息  广播  暂定轮播图使用
                    {
                        reqParams = "{\"platform\": \"all\"," +
                                     "\"audience\":\"all\"," +
                                     "\"message\":{\"msg_content\":\"" + alter + "\"}," +
                                     "\"options\":{\"apns_production\":false}}";
                    }
                    else if (sort == "message" && obj != "")//自定义消息 目标：obj  暂定修改密码使用
                    {
                        reqParams = "{\"platform\": \"all\"," +
                                     "\"audience\":{" + obj + "}," +
                                     "\"message\":{\"msg_content\":\"" + alter + "\"}," +
                                     "\"options\":{\"apns_production\":true}}";
                    }
                    else//通知  目标：obj
                    {
                        reqParams = "{\"platform\":\"all\",\"audience\":{"
                                         + obj + "},\"notification\":{"
                                         //+"{\"alert\":\"测试测试测试测试啊\"}"
                                         + "\"android\":{\"alert\":\"" + alter + "\",\"extras\":" + menu + "},"
                                         + "\"ios\":{\"alert\":\"" + alter + "\",\"sound\":\"Bloom.caf\",\"badge\" : 1,\"extras\":" + menu + "}}"
                                         + ",\"options\":{\"apns_production\":true}}";
                    }
                    Log.Debug("callback", reqParams);
                    string jsonString = util.SendPostRequest(reqParams);

                    // Log.Debug("推送消息日志",jsonString + "\r\n");

                    string errorMessage = "";

                    string errorCode = "";

                    string sendno = "";

                    string msg_id = "";

                    bool processResult = util.IsSuccess(jsonString, out errorMessage, out errorCode, out sendno, out msg_id);
                    Start();


                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    Interlocked.Exchange(ref _IsRunning, 0);
                }
            }
        }
        /// <summary>
        ///timer停止
        /// </summary>
        public void Stop()
        {
            if (UpdateTimer != null)
            {
                UpdateTimer.Dispose();
                UpdateTimer = null;
            }
        }
        /// <summary>
        /// 获取新推送
        /// <summary>
        private void seachnewmessage(object sender)
        {
            Stop();

            //获取推送编号
            SqlParameter[] parameter_xh = new SqlParameter[]
            {
                new SqlParameter("@type","查询新单据"),
            };
            DataTable dt_xh = db.RunProcedureDR("SK_App_GetNewPsd", parameter_xh);
            if (dt_xh.Rows.Count == 0)
            {
                Log.Debug("推送获取编号详情", "无推送信息！");
            }
            else
            {
                foreach (DataRow dataRow in dt_xh.Rows)
                {
                    string xh = dataRow["djbh"].ToString();
                    Log.Debug("推送获取编号详情", xh);
                    SeachNewOrder(xh);

                }
            }
            Start();
        }



        /// <summary>
        ///汇总推送信息订单
        /// </summary>
        private void SeachNewOrder(string sender)
        {
            //SqlRun db = new SqlRun(SqlRun.sqlstr);
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            javaScriptSerializer.MaxJsonLength = Int32.MaxValue; //取得最大数值

            //取alert
            string alert_xx, sort;
            SqlParameter[] parameter_alert_xx = new SqlParameter[]
            {
                new SqlParameter("@type","获取信息"),
                new SqlParameter("@djbh",sender)
            };
            DataTable dt_alert_xx = db.RunProcedureDR("SK_App_GetNewPsd", parameter_alert_xx);
            alert_xx = dt_alert_xx.Rows[0][0].ToString().Trim();
            sort = dt_alert_xx.Rows[0][1].ToString().Trim();
            Log.Debug("推送获取alert内容", alert_xx + "-" + sort);

            //获取其他字段 用于跳转APP菜单
            string menu;
            SqlParameter[] param = new SqlParameter[] {
                 new SqlParameter("@type","获取APP菜单"),
                 new SqlParameter("@djbh",sender),
        };
            DataTable dt = db.RunProcedureDR("SK_App_GetNewPsd", param);
            if (dt.Rows.Count > 0)
            {
                var cdbh = dt.Rows[0][0].ToString();
                Log.Debug("推送获取menu内容", cdbh);
                menu = "{\"menu\":\"" + cdbh + "\"}";
            }
            else
            {
                menu = "{}";
                Log.Debug("推送获取menu空内容", "");
            }

            //取tag
            string tag;
            SqlParameter[] parameter_tag = new SqlParameter[]
            {
             new SqlParameter("@type","tag"),
             new SqlParameter("@djbh",sender),
                //new SqlParameter("p_cur",ParameterDirection.Output)
            };
            DataTable dt_tag = db.RunProcedureDR("SK_App_GetNewPsd", parameter_tag);
            ArrayList arrayList_tag = new ArrayList();

            int count_tag = dt_tag.Rows.Count;
            if (count_tag == 1)
            {
                string tag_z1 = dt_tag.Rows[0][0].ToString().Trim();
                if (tag_z1 != "无")
                {
                    tag = "\"tag\":[\"" + tag_z1 + "\"]";
                    Log.Debug("推送获取tag内容", tag_z1);
                }
                else
                {
                    tag = "";
                    Log.Debug("推送获取tag空内容", "");
                }
            }
            else if (count_tag > 0)
            {
                foreach (DataRow dataRow in dt_tag.Rows)
                {
                    arrayList_tag.Add(dataRow[0].ToString());
                }
                string tag_z2 = javaScriptSerializer.Serialize(arrayList_tag).ToString().Trim();
                tag = "\"tag\":" + tag_z2;
            }
            else
            {
                tag = "";
            }

            //取tag_and
            string tag_and;
            SqlParameter[] parameter_tag_and = new SqlParameter[]
            {
                new SqlParameter("@type","tag_and")
            };
            DataTable dt_tag_and = db.RunProcedureDR("SK_App_GetNewPsd", parameter_tag_and);
            ArrayList arrayList_tag_and = new ArrayList();

            int count_tag_and = dt_tag_and.Rows.Count;
            if (count_tag_and == 1)
            {
                string tag_z1_and = dt_tag_and.Rows[0]["tag_and"].ToString().Trim();
                if (tag_z1_and != "")
                {
                    tag_and = "\"tag_and\":[\"" + tag_z1_and + "\"]";
                    Log.Debug("推送获取tag_and内容", tag_z1_and);
                }
                else
                {
                    tag_and = "";
                    Log.Debug("推送获取tag_and空内容", "");
                }
            }
            else if (count_tag_and > 0)
            {
                foreach (DataRow dataRow in dt_tag_and.Rows)
                {
                    arrayList_tag_and.Add(dataRow["tag_and"].ToString());
                }
                string tag_z2_and = javaScriptSerializer.Serialize(arrayList_tag_and).ToString().Trim();
                tag_and = "\"tag_and\":" + tag_z2_and;
            }
            else
            {
                tag_and = "";
            }

            //取tag_not
            string tag_not;
            SqlParameter[] parameter_tag_not = new SqlParameter[]
            {
                new SqlParameter("@type","tag_not")
            };
            DataTable dt_tag_not = db.RunProcedureDR("SK_App_GetNewPsd", parameter_tag_not);
            ArrayList arrayList_tag_not = new ArrayList();

            int count_tag_not = dt_tag_not.Rows.Count;
            if (count_tag_not == 1)
            {
                string tag_z1_not = dt_tag_not.Rows[0]["tag_not"].ToString().Trim();
                if (tag_z1_not != "无")
                {
                    tag_not = "\"tag_not\":[\"" + tag_z1_not + "\"]";
                    Log.Debug("推送获取tag_not内容", tag_z1_not);
                }
                else
                {
                    tag_not = "";
                    Log.Debug("推送获取tag_not空内容", "");
                }
            }
            else if (count_tag_not > 0)
            {
                foreach (DataRow dataRow in dt_tag_not.Rows)
                {
                    arrayList_tag_not.Add(dataRow["tag_not"].ToString());
                }
                string tag_z2_not = javaScriptSerializer.Serialize(arrayList_tag_not).ToString().Trim();
                tag_not = "\"tag_not\":" + tag_z2_not;
            }
            else
            {
                tag_not = "";
            }

            //取alias
            string alias;
            SqlParameter[] parameter_alias = new SqlParameter[]
            {
                new SqlParameter("@type","alias"),
                new SqlParameter("@djbh",sender)
            };
            DataTable dt_alias = db.RunProcedureDR("SK_App_GetNewPsd", parameter_alias);
            ArrayList arrayList_alias = new ArrayList();

            int count_alias = dt_alias.Rows.Count;
            if (count_alias == 1)
            {
                string alias_z1 = dt_alias.Rows[0]["alias"].ToString().Trim();
                if (alias_z1 != "无")
                {
                    alias = "\"alias\":[\"" + alias_z1 + "\"]";
                    Log.Debug("推送获取alias内容", alias_z1);
                }
                else
                {
                    alias = "";
                    Log.Debug("推送获取alias空内容", "");
                }
            }
            else if (count_alias > 0)
            {
                foreach (DataRow dataRow in dt_alias.Rows)
                {
                    arrayList_alias.Add(dataRow["alias"].ToString());
                }
                string alias_z2 = javaScriptSerializer.Serialize(arrayList_alias).ToString().Trim();
                alias = "\"alias\":" + alias_z2;
            }
            else
            {
                alias = "";
            }

            ArrayList arrayList = new ArrayList();

            if (tag != "")
            {
                arrayList.Add(tag);
                Log.Debug("判断tag是否有值", tag);
            }
            if (tag_and != "")
            {
                arrayList.Add(tag_and);
                Log.Debug("判断tag_and是否有值", tag_and);
            }
            if (tag_not != "")
            {
                arrayList.Add(tag_not);
                Log.Debug("判断tag_not是否有值", tag_not);
            }
            if (alias != "")
            {
                arrayList.Add(alias);
                Log.Debug("判断alias是否有值", alias);
            }
            Log.Debug("判断arrayList.Count是否有值", arrayList.Count.ToString());
            if (arrayList.Count > 0)
            {
                try
                {
                    string obj = string.Join(",", (string[])arrayList.ToArray(typeof(string)));
                    Log.Debug("通知", sort + "-" + alert_xx);
                    UpdateTimerCallback(obj, alert_xx, menu, sort);

                    //回写，先写这里
                    SqlParameter[] parameter_hx = new SqlParameter[]
                    {
                        new SqlParameter("@type","回写"),
                        new SqlParameter("@djbh",sender)
                    };
                    DataTable dt_hx = db.RunProcedureDR("SK_App_GetNewPsd", parameter_hx);
                }
                catch (Exception e)
                {
                    //Log.Debug("推送出错", e.Message.ToString().Trim());
                    throw e;
                }
            }
            else if (sort == "message" && arrayList.Count == 0)//自定义消息 广播
            {
                try
                {
                    string obj = "";
                    Log.Debug("自定义", sort + "-" + alert_xx);
                    UpdateTimerCallback(obj, alert_xx, menu, sort);

                    //回写，先写这里
                    SqlParameter[] parameter_hx = new SqlParameter[]
                    {
                        new SqlParameter("@type","回写"),
                        new SqlParameter("@djbh",sender)

                    };
                    DataTable dt_hx = db.RunProcedureDR("SK_App_GetNewPsd", parameter_hx);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}