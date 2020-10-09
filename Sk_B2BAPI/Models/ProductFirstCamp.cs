using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.Models
{
    /// <summary>
    /// 首营商品数据
    /// </summary>
    public class ProductFirstCamp
    {
        /// <summary>
        /// 商品编号
        /// </summary>
        public string GoodsNo { get; set; } = "";
        /// <summary>
        /// 商品名称
        /// </summary>
        public string GoodsName { get; set; } = "";
        /// <summary>
        /// 规格
        /// </summary>
        public string Specifications { get; set; } = "";
        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; } = "";
        /// <summary>
        /// 剂型
        /// </summary>
        public string DosageForms { get; set; } = "";
        /// <summary>
        /// 厂家
        /// </summary>
        public string Manufacturer { get; set; } = "";
        /// <summary>
        /// 图片数量
        /// </summary>
        public int Pictures { get; set; } = 0;
    }
}