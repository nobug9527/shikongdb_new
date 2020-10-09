using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.Models
{
    public class OrdersMt
    {
        /// <summary>
        /// 单据Id
        /// </summary>
        public int BillNo { get; set; }
        /// <summary>
        /// 单据编号
        /// </summary>
        public string Order_No { get; set; } = "";
        /// <summary>
        /// 支付临时订单号
        /// </summary>
        public string Generate { get; set; } = "";
        /// <summary>
        /// 第三方支付订单
        /// </summary>
        public string Thirdparty { get; set; } = "";
        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId { get; set; } = "";
        /// <summary>
        /// 客户内码
        /// </summary>
        public string BusinessId { get; set; } = "";
        /// <summary>
        /// 支付方式Id
        /// </summary>
        public string PaymentId { get; set; } = "";
        /// <summary>
        /// 在线支付发起时间
        /// </summary>
        public string Initiationtime { get; set; } = "";
        /// <summary>
        /// 支付方式名称
        /// </summary>
        public string PaymentName { get; set; } = "";
        /// <summary>
        /// 支付状态 0未支付/1已支付/2线下支付
        /// </summary>
        public int PaymentStatus { get; set; }
        /// <summary>
        /// 支付状态名称
        /// </summary>
        public string PaymentStatusName { get; set; } = "";
        /// <summary>
        /// 支付金额
        /// </summary>
        public decimal PaymentFee { get; set; }
        /// <summary>
        /// 支付时间
        /// </summary>
        public string PaymentTime { get; set; } = "";
        /// <summary>
        /// 线上支付方式
        /// </summary>
        public string PayType { get; set; } = "";
        /// <summary>
        /// 收货人
        /// </summary>
        public string Accept_Name { get; set; } = "";
        /// <summary>
        /// 联系方式
        /// </summary>
        public string Telphone { get; set; } = "";
        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; } = "";
        /// <summary>
        /// 实付金额
        /// </summary>
        public decimal Real_Amount { get; set; } = 0;
        /// <summary>
        /// 订单金额
        /// </summary>
        public decimal Order_Amount { get; set; } = 0;
        /// <summary>
        /// 优惠金额
        /// </summary>
        public decimal Discount_Amount { get; set; } = 0;
        /// <summary>
        /// 红包金额
        /// </summary>
        public decimal BonusAmount { get; set; } = 0;
        /// <summary>
        /// 积分
        /// </summary>
        public decimal Point { get; set; } = 0;
        /// <summary>
        /// 邮费
        /// </summary>
        public decimal Postage { get; set; }
        /// <summary>
        /// 订单状态（0未审核/1已审核/2已开票审核/3已出库/4删除（取消））
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 订单状态名称
        /// </summary>
        public string StatusName { get; set; } = "";
        /// <summary>
        ///创建日期
        /// </summary>
        public string AddTime { get; set; } = "";
        /// <summary>
        /// 订单来源（PC/APP）
        /// </summary>
        public string Source { get; set; } = "";
        /// <summary>
        /// 是否评论
        /// </summary>
        public string IsCriticism { get; set; } = "";
        /// <summary>
        /// 发货时间
        /// </summary>
        public string DeliveryTime { get; set; } = "";
        /// <summary>
        /// 快递公司名称
        /// </summary>
        public string ExpressName { get; set; } = "";
        /// <summary>
        /// 快递公司编号
        /// </summary>
        public string ExpressCode { get; set; } = "";
        /// <summary>
        /// 快递编号
        /// </summary>
        public string ExpressNum { get; set; } = "";
        /// <summary>
        /// 订单详情
        /// </summary>
        public List<OrderDt> orderDt { get; set; }
    }
}