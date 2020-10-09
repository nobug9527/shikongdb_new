using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.Models
{
    /// <summary>
    /// 积分商品
    /// </summary>
    public class IntegralGood
    {
        /// <summary>
        /// 积分商品Id
        /// </summary>
        public string GoodsId { get; set; } = "";
        /// <summary>
        /// 积分商品编号
        /// </summary>
        public string GoodsCode { get; set; } = "";
        /// <summary>
        /// 积分商品名称
        /// </summary>
        public string GoodsName { get; set; } = "";
        /// <summary>
        /// 积分商品
        /// </summary>
        public string DrugSpec { get; set; } = "";
        /// <summary>
        /// 积分商品库存
        /// </summary>
        public decimal Quantity { get; set; }
        /// <summary>
        /// 积分商品价格
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 积分商品积分
        /// </summary>
        public int Integral { get; set; }
        /// <summary>
        /// 积分商品厂家
        /// </summary>
        public string DrugFactory { get; set; } = "";
        /// <summary>
        /// 积分商品图片
        /// </summary>
        public string ImgUrl { get; set; } = "";
        /// <summary>
        /// 积分商品所属机构
        /// </summary>
        public string EntId { get; set; } = "";
        /// <summary>
        /// 积分商品排序
        /// </summary>
        public int Sort { get; set; }
    }
}