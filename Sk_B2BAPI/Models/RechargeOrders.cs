using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.Models
{
    /// <summary>
    /// 充值订单
    /// </summary>
    public class RechargeOrders
    {
        /// <summary>
        /// 充值订单序号
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 商城充值订单号
        /// </summary>
        public string OrderNo { get; set; } = "";

        /// <summary>
        /// 充值订单商品Id
        /// </summary>
        public string GoodId { get; set; } = "";

        /// <summary>
        /// 充值订单金额
        /// </summary>
        public decimal Fee { get; set; }

        /// <summary>
        /// 充值订单状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 充值订单生成日期
        /// </summary>
        public string AddTime { get; set; } = "";

        /// <summary>
        /// 微信支付订单号
        /// 支付宝系统交易流水号
        /// </summary>
        public string TransactionId { get; set; } = "";

        /// <summary>
        /// 充值订单用户
        /// </summary>
        public string UserId { get; set; } = "";

        /// <summary>
        /// 充值订单机构
        /// </summary>
        public string EntId { get; set; } = "";

        /// <summary>
        /// 充值订单备注
        /// </summary>
        public string Remark { get; set; } = "";

        /// <summary>
        /// 充值方式
        /// </summary>
        public string Payment { get; set; } = "";

        /// <summary>
        /// 操作（1充值/0退款）
        /// </summary>
        public int? Operation { get; set; }

        /// <summary>
        /// 操作名称 （充值/退款）
        /// </summary>
        public string operationName { get; set; } = "";

        /// <summary>
        /// 合作伙伴身份
        /// </summary>
        public string AppId { get; set; } = "";
    }
}