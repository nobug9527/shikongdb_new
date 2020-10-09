using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Sk_B2BAPI.App_Code;
using Sk_B2BAPI.Tool;
using Sk_B2BAPI.Models;

namespace Sk_B2BAPI.DAL
{
    /// <summary>
    /// 充值
    /// </summary>
    public class RechargeDal
    {
        /// <summary>
        /// 新增充值记录
        /// </summary>
        /// <param name="userId">充值用户</param>
        /// <param name="entId">充值机构</param>
        /// <param name="orderNo">充值订单号</param>
        /// <param name="goodId">订单商品</param>
        /// <param name="fee">订单金额</param>
        /// <param name="remark">订单备注</param>
        /// <param name="payment">支付方式</param>
        /// <param name="operation">操作 0【退款】 1【充值】</param>
        /// <param name="appId">合作伙伴ID</param>
        /// <param name="ruleId">充值规则</param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool IncreaseRecharge(string userId,string entId,string orderNo,string goodId,decimal fee,string remark,string payment,int operation,string appId,string ruleId,int rechargeType, out string message)
        {
            try
            {
                message = "";
                SqlParameter[] sqls = new SqlParameter[]
                {
                    new SqlParameter("@type","IncreaseRecharge"),
                    new SqlParameter("@orderNo",orderNo),
                    new SqlParameter("@goodId",goodId),
                    new SqlParameter("@ruleId",ruleId),
                    new SqlParameter("@fee",fee),
                    new SqlParameter("@userId",userId),
                    new SqlParameter("@entId",entId),
                    new SqlParameter("@remark",remark),
                    new SqlParameter("@payment",payment),
                    new SqlParameter("@operation",operation),
                    new SqlParameter("@appId",appId),
                    new SqlParameter("@rechargeType",rechargeType)
                };
                SqlRun sql = new SqlRun(SqlRun.sqlstr);
                int number = sql.ExecuteNonQuery("Proc_OperationRecharge", sqls);
                if (number > 0)
                {
                    message = "商城充值下单成功！";
                    return true;
                }
                else
                {
                    message = "商城充值下单失败！";
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Recharge/RechargeDal", ex.Message.ToString());
                message = ex.Message.ToString();
                return false;
            }
        }

        /// <summary>
        /// 回写商城充值订单
        /// </summary>
        /// <param name="orderNo">商户充值订单单号</param>
        /// <param name="transaction_id">微信【支付宝】支付订单号</param>
        /// <param name="status">充值订单状态 0【失败/关闭】 1【申请】 2【成功】</param>
        /// <param name="refundFee">交易金额</param>
        /// <param name="refundId">微信退款单号</param>
        /// <param name="refundNo">商户退款单号</param>
        /// <param name="message">返回操作结果</param>
        /// <returns></returns>
        public bool UpdateRecharge(string orderNo, string transaction_id, int status,decimal refundFee,string refundId, string refundNo, out string message)
        {
            try
            {
                message = "";
                SqlParameter[] sqls = new SqlParameter[]
                {
                    new SqlParameter("@type","UpdateRecharge"),
                    new SqlParameter("@orderNo",orderNo),
                    new SqlParameter("@transaction_id",transaction_id),
                    new SqlParameter("@status",status),
                    new SqlParameter("@refundFee",refundFee),
                    new SqlParameter("@refundId",refundId),
                    new SqlParameter("@refundNo",refundNo)
                    
                };
                SqlRun sql = new SqlRun(SqlRun.sqlstr);
                int number = sql.ExecuteNonQuery("Proc_OperationRecharge", sqls);
                if (number > 0)
                {
                    message = "商城订单回写成功！";
                    return true;
                }
                else
                {
                    message = "商城订单回写失败！";
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Recharge/UpdateRecharge", ex.Message.ToString());
                message = ex.Message.ToString();
                return false;
            }
        }

        /// <summary>
        /// 查询商户充值订单
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public List<RechargeOrders> InquireRecharge (string orderNo)
        {
            SqlParameter[] sqls = new SqlParameter[]
            {
                new SqlParameter("@type","InquireRecharge"),
                new SqlParameter("@orderNo",orderNo)
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataSet set = sql.RunProDataSet("Proc_OperationRecharge", sqls);

            List<RechargeOrders> rechargeOrders = new List<RechargeOrders>();
            if (set.Tables[0].Rows.Count>0)
            {
                rechargeOrders = FillRechargeOrders(set.Tables[0]);
            }
            else
            {
                rechargeOrders = null;
            }
            return rechargeOrders;
        }

        /// <summary>
        /// 商城充值订单填充数据
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public List<RechargeOrders> FillRechargeOrders(DataTable table)
        {
            List<RechargeOrders> rechargeOrders = new List<RechargeOrders>();
            foreach (DataRow row in table.Rows)
            {
                RechargeOrders orders = new RechargeOrders()
                {
                    ID = int.Parse(row["id"].ToString()),
                    OrderNo = row["orderNo"].ToString(),
                    GoodId = row["goodId"].ToString(),
                    Fee = decimal.Parse(row["fee"].ToString()),
                    Status = int.Parse(row["status"].ToString()),
                    AddTime = row["addTime"].ToString(),
                    TransactionId = row["transaction_id"].ToString(),
                    UserId = row["userId"].ToString(),
                    EntId = row["entId"].ToString(),
                    Remark = row["remark"].ToString(),
                    Operation = int.Parse(row["operation"].ToString()),
                    AppId = row["appId"].ToString()
                };
                rechargeOrders.Add(orders);
            }
            return rechargeOrders;
        }

        /// <summary>
        /// 充值选项
        /// </summary>
        /// <returns></returns>
        public List<RechargeGoods> RechargeOptions(string entId,string userId)
        {
            SqlParameter[] sqls = new SqlParameter[]
            {
                new SqlParameter("@type","RechargeGoods"),
                new SqlParameter("@userId",userId),
                new SqlParameter("@entId",entId)
            };
            List<RechargeGoods> rechargeGoods = new List<RechargeGoods>();
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataSet set = sql.RunProDataSet("Proc_OperationRecharge", sqls);
            DataTable table = set.Tables[0];
            if (table.Rows.Count > 0)
            {
                rechargeGoods = FillRechargeGoods(table);
            }
            else
            {
                rechargeGoods = null;
            }
            return rechargeGoods;
        }

        /// <summary>
        /// 充值选项数据填充
        /// </summary>
        /// <param name="table">充值选项数据</param>
        /// <returns></returns>
        public List<RechargeGoods> FillRechargeGoods(DataTable table)
        {
            List<RechargeGoods> rechargeGoods = new List<RechargeGoods>();
            foreach (DataRow row in table.Rows)
            {
                RechargeGoods goods = new RechargeGoods()
                {
                    ProductId=int.Parse(row["productId"].ToString()),
                    Title=row["title"].ToString(),
                    Fee= decimal.Parse(row["fee"].ToString()),
                    Type=int.Parse(row["type"].ToString()),
                    TypeName=row["typeName"].ToString(),
                    Remark=row["remark"].ToString(),
                    BonusAmount=decimal.Parse(row["bonusAmount"].ToString()),
                    BonusNum=int.Parse(row["bonusNum"].ToString()),
                    IsActive=row["isActive"].ToString().Trim(),
                    IsAble=row["isAble"].ToString().Trim(),
                    RuleId=row["ruleId"].ToString()
                };
                rechargeGoods.Add(goods);
            }
            return rechargeGoods;
        }

        /// <summary>
        /// 充值退款记录
        /// </summary>
        /// <param name="userId">用户</param>
        /// <param name="entId">机构</param>
        /// <param name="payment">支付方式 微信/支付宝/All</param>
        /// <param name="operation">操作类型 1充值/0退款/99全部</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="pageSize">单页条目数</param>
        /// <param name="pageCount">输出参数 总页数</param>
        /// <param name="recordCount">输出参数 总条目数</param>
        /// <returns></returns>
        public List<RechargeOrders> RechargeRecord(string userId, string entId, string payment, int operation,int pageIndex,int pageSize,out int pageCount,out int recordCount)
        {
            SqlParameter[] sqls = new SqlParameter[]
            {
                new SqlParameter("@type","RechargeRecord"),
                new SqlParameter("@userId",userId),
                new SqlParameter("@entId",entId),
                new SqlParameter("@payment",payment),
                new SqlParameter("@operation",operation),
                new SqlParameter("@pageIndex",pageIndex),
                new SqlParameter("@pageSize",pageSize)
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataSet set = sql.RunProDataSet("Proc_OperationRecharge", sqls);
            List<RechargeOrders> rechargeOrders = new List<RechargeOrders>();
            if (set.Tables.Count>0)
            {
                rechargeOrders = FillRechargeRecord(set.Tables[1]);
                pageCount = int.Parse(set.Tables[2].Rows[0]["pageCount"].ToString());
                recordCount = int.Parse(set.Tables[2].Rows[0]["recordCount"].ToString());

            }
            else
            {
                pageCount = 1;
                recordCount = 0;
                rechargeOrders = null;
            }
            return rechargeOrders;
        }

        /// <summary>
        /// 充值记录数据填充
        /// </summary>
        /// <param name="table">充值记录</param>
        /// <returns></returns>
        public List<RechargeOrders> FillRechargeRecord(DataTable table)
        {
            List<RechargeOrders> rechargeOrders = new List<RechargeOrders>();
            foreach (DataRow row in table.Rows)
            {
                RechargeOrders orders = new RechargeOrders()
                {
                    ID = int.Parse(row["id"].ToString()),
                    OrderNo = row["orderNo"].ToString(),
                    GoodId = row["goodId"].ToString(),
                    Fee = decimal.Parse(row["fee"].ToString()),
                    AddTime = row["addTime"].ToString(),
                    TransactionId = row["transaction_id"].ToString(),
                    UserId = row["userId"].ToString(),
                    EntId = row["entId"].ToString(),
                    Remark = row["remark"].ToString(),
                    Payment=row["payment"].ToString(),
                    operationName=row["operationName"].ToString()
                };
                rechargeOrders.Add(orders);
            }
            return rechargeOrders;
        }

        /// <summary>
        /// 充值活动
        /// </summary>
        /// <param name="ruleId"></param>
        /// <returns></returns>
        public List<RechargeRule> RechargeRules(string ruleId)
        {
            SqlParameter[] sqls = new SqlParameter[]
            {
                new SqlParameter("@type","RechargeRules"),
                new SqlParameter("@ruleId",ruleId)
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataSet set = sql.RunProDataSet("Proc_OperationRecharge", sqls);
            List<RechargeRule> rechargeRules = new List<RechargeRule>();
            if (set.Tables[0].Rows.Count>0)
            {
                return rechargeRules = FillRechargeRules(set.Tables[0]);
            }
            else
            {
                return rechargeRules = null;
            }
        }

        public List<RechargeRule> FillRechargeRules(DataTable table)
        {
            List<RechargeRule> rechargeRules = new List<RechargeRule>();
            foreach (DataRow item in table.Rows)
            {
                RechargeRule rechargeRule = new RechargeRule()
                {
                    RuleId=item["ruleId"].ToString(),
                    ProductId=int.Parse(item["productId"].ToString()),
                    BonusAmount=decimal.Parse(item["bonusAmount"].ToString()),
                    BonusNum=int.Parse(item["bonusNum"].ToString()),
                    IsActive=item["isActive"].ToString()
                };
                rechargeRules.Add(rechargeRule);
            }
            return rechargeRules;
        }
    }
}