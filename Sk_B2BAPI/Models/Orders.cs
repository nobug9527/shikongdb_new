using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.Models
{
    public class Orders
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
        /// 用户Id
        /// </summary>
        public string UserId { get; set; } = "";
        /// <summary>
        /// 客户内码
        /// </summary>
        public string BusinessId { get; set;} = "";
        /// <summary>
        /// 支付方式Id
        /// </summary>
        public string PaymentId{ get; set;} = "";
        /// <summary>
        /// 线上支付发起时间
        /// </summary>
        public string Initiationtime { get; set; } = "";
        /// <summary>
        /// 线上支付临时单号
        /// </summary>
        public string Generate { get; set; } = "";
        /// <summary>
        /// 线上支付第三方交易号
        /// </summary>
        public string Thirdparty { get; set; } = "";
        /// <summary>
        /// 二维码
        /// </summary>
        public string Url { get; set; } = "";
        /// <summary>
        /// 在线支付金额
        /// </summary>
        public decimal RefundFee { get; set; }
        /// <summary>
        /// 支付方式名称
        /// </summary>
        public string PaymentName { get; set;} = "";
        /// <summary>
        /// 支付状态 0未支付/1已支付/2线下支付
        /// </summary>
        public int PaymentStatus { get; set;}
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
        public decimal Real_Amount { get; set; }
        /// <summary>
        /// 订单金额
        /// </summary>
        public decimal Order_Amount { get; set; }
        /// <summary>
        /// 积分
        /// </summary>
        public int Point { get; set; }
        /// <summary>
        /// 订单状态（0未审核/1已审核/2已开票审核/3已出库/4删除（取消））
        /// </summary>
        public int status { get; set; }
        /// <summary>
        //日期
        /// </summary>
        public string AddTime{ get; set; } = "";
        /// <summary>
        /// 订单来源（PC/APP）
        /// </summary>
        public string Source { get; set; } = "";
        /// <summary>
        /// 订单商品明细
        /// </summary>
        public List<OrderGoods> OrderGoods { get; set; }
        /// <summary>
        ///  支付发生在APP或PC
        /// </summary>
        public string equipment { get; set; }
    }
}