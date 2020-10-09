using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.Models
{
    /// <summary>
    /// 大宗商品阶梯价格 CommodityLadderPrice
    /// </summary>
    public class CLPrice
    {
        /// <summary>
        /// 机构编号
        /// </summary>
        public string Entid { get; set; } = "";
        /// <summary>
        /// 文章ID
        /// </summary>
        public int Article_Id { get; set; }
        /// <summary>
        /// 价格级别
        /// </summary>
        public string PriceLevel { get; set; } = "";
        /// <summary>
        /// 价格
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 最后修改时间
        /// </summary>
        public string LastModifyTime { get; set; } = "";
    }
}