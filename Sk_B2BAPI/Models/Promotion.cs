using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.Models
{
    public class Promotion
    {
        /// <summary>
        /// 方案编号
        /// </summary>
        public string Fabh { get; set;} = "";
        /// <summary>
        /// 方案标识
        /// </summary>
        public string Fabs { get; set; } = "";
        /// <summary>
        /// 客户类型
        /// </summary>
        public string ClientType { get; set; } = "";
        /// <summary>
        /// 促销模式
        /// </summary>
        public string ContentType { get; set; } = "";
        /// <summary>
        /// 促销开始时间
        /// </summary>
        public string StartDate { get; set; } = "";
        /// <summary>
        /// 促销结束时间
        /// </summary>
        public string EndDate { get; set; } = "";
        /// <summary>
        /// 促销详情
        /// </summary>
        public string Describe { get; set; } = "";
        /// <summary>
        /// 满足条件
        /// </summary>
        public decimal MeetCount { get; set; }
        /// <summary>
        /// 促销折扣
        /// </summary>
        public decimal Discount { get; set; }
        /// <summary>
        /// 赠品
        /// </summary>
        public string Goodsname { get; set; } = "";
        /// <summary>
        /// 赠品数量
        /// </summary>
        public decimal GiftQuantity { get; set; }
        /// <summary>
        /// 赠品价格
        /// </summary>
        public decimal GiftPrice { get; set; }
        /// <summary>
        /// 促销类型
        /// </summary>
        public string FaType { get; set; } = "";
        /// <summary>
        /// 服务器时间
        /// </summary>
        public string WebTime { get; set; } = "";
        /// <summary>
        /// 限购数量
        /// </summary>
        public string XgAmount { get; set; } = "";

    }
}