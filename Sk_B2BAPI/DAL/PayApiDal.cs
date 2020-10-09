using Sk_B2BAPI.App_Code;
using Sk_B2BAPI.Models;
using Sk_B2BAPI.Tool;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Sk_B2BAPI.DAL
{
    public class PayApiDal
    {
        /// <summary>
        /// 获知支付配置信息
        /// </summary>
        /// <param name="payType">支付方式</param>
        /// <param name="entId">企业id</param>
        /// <returns></returns>
        public List<PayApi> GetPayInfo(string payType,string entId)
        {
            DataTable dt = new DataTable();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select payid,AppId,Merchants,AppKey,AppSecert,SSlCertPath,SSlCertPassword,NotifyUrl,RefundUrl,Ip,ProxyUrl,ReportLevel,LogLevel,PayType,Beactive,Web_Url,EntId,Sort_Id from dt_payapi (NOLOCK) ");
            strSql.Append(" where Beactive='Y' and  Entid=@Entid");
            if (payType != "全部")
            {
                strSql.Append(" and PayType='" + payType + "'");
            }
            strSql.Append(" order by Sort_Id asc ");
            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@Entid",entId)
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            dt = sql.RunSqlDataTable(strSql.ToString(), param);
            List<PayApi> PList = new List<PayApi>();
            if (dt.Rows.Count > 0)
            {
                PList = SetPayInfo(dt);
            }
            return PList;
        }
        /// <summary>
        /// 填充Model(PayApi)
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private List<PayApi> SetPayInfo(DataTable dt)
        {
            List<PayApi> PList = new List<PayApi>();
            foreach (DataRow dr in dt.Rows)
            {
                PayApi pay = new PayApi();
                pay.PayId = dr["payid"].ToString().Trim();
                pay.AppId = dr["AppId"].ToString().Trim();
                pay.Merchants = dr["Merchants"].ToString().Trim();
                pay.AppKey = dr["AppKey"].ToString().Trim();
                pay.AppSecert = dr["AppSecert"].ToString().Trim();
                pay.SSlCertPath = dr["SSlCertPath"].ToString().Trim();
                pay.SSlCertPassword = dr["SSlCertPassword"].ToString().Trim();
                pay.NotifyUrl = dr["NotifyUrl"].ToString().Trim();
                pay.RefundUrl = dr["RefundUrl"].ToString().Trim();
                pay.Ip = dr["Ip"].ToString().Trim();
                pay.ProxyUrl = dr["ProxyUrl"].ToString().Trim();
                pay.ReportLevel = int.Parse(dr["ReportLevel"].ToString());
                pay.LogLevel = int.Parse(dr["LogLevel"].ToString());
                pay.PayType = dr["PayType"].ToString().Trim();
                pay.Web_Url = dr["Web_Url"].ToString().Trim();
                PList.Add(pay);
            }
            return PList;
        }

        /// <summary>
        /// 充值方式
        /// </summary>
        /// <param name="entId">机构</param>
        /// <returns></returns>
        public List<Payment> PayTypes(string entId)
        {
            SqlParameter[] sqls = new SqlParameter[]
            {
                new SqlParameter("@type","PayTypes"),
                new SqlParameter("@entId",entId)
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataTable table = sql.RunProcedureDR("Proc_OperationRecharge", sqls);
            List<Payment> payApis = new List<Payment>();
            if (table.Rows.Count>0)
            {
                return payApis = FillPayApi(table);
            }
            else
            {
                return payApis = null;
            }
        }

        /// <summary>
        /// 填充充值方式
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        private static List<Payment> FillPayApi(DataTable table)
        {
            string webUrl = BaseConfiguration.SercerIp;
            List<Payment> payments = new List<Payment>();
            foreach (DataRow item in table.Rows)
            {
                Payment payment = new Payment()
                {
                    PayId=item["PayId"].ToString(),
                    PayType=item["PayType"].ToString()
                };
                if (item["LogoUrl"].ToString()=="")
                {
                    payment.LogoUrl = "";
                }
                else
                {
                    payment.LogoUrl = webUrl + item["LogoUrl"].ToString();
                }
                payments.Add(payment);
            }
            return payments;
        }
    }
}