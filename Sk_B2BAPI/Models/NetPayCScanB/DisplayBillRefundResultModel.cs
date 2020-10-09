using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.Models.NetPayCScanB
{
    public class DisplayBillRefundResultModel
    {
        [Display(Name = "平台错误码")]
        public string errCode { get; set; }

        [Display(Name = "平台错误信息")]
        public string errMsg { get; set; }

        [Display(Name = "商户号")]
        public string mid { get; set; }

        [Display(Name = "终端号")]
        public string tid { get; set; }

        [Display(Name = "业务类型")]
        public string instMid { get; set; }

        [Display(Name = "账单号")]
        public string billNo { get; set; }

        [Display(Name = "支付总金额，单位为分")]
        public string totalAmount { get; set; }

        [Display(Name = "账单状态")]
        public string billStatus { get; set; }

        [Display(Name = "退款结果")]
        public string refundStatus { get; set; }
        /// <summary>
        /// 退款订单号
        /// </summary>
        public string refundOrderId { get; set; }
        /// <summary>
        /// 退款金额
        /// </summary>
        public int refundAmount { get; set; }
    }
}