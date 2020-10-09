using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.Models
{
    public class PromDt
    {
        /// <summary>
        /// 方案编号
        /// </summary>
        public string Fabh { get; set; } = "";
        /// <summary>
        /// 方案明细序号
        /// </summary>
        public int Fa_Sn { get; set; }
        /// <summary>
        /// 商品ID
        /// </summary>
        public string Article_Id { get; set; } = "";
        /// <summary>
        /// 商品名称
        /// </summary>
        public string Sub_Title { get; set;} = "";
        /// <summary>
        /// 生产厂家
        /// </summary>
        public string Drug_Factory { get; set; } = "";
        /// <summary>
        /// 商品规格
        /// </summary>
        public string Drug_Spec { get; set; } = "";
        /// <summary>
        /// 中包装
        /// </summary>
        public decimal Min_Package { get; set; } = 0;
        /// <summary>
        /// 大包装
        /// </summary>
        public decimal Big_Package { get; set; } = 0;
        /// <summary>
        /// 库存
        /// </summary>
        public decimal Stock_Quantity { get; set; } = 0;
        /// <summary>
        /// 商品价格
        /// </summary>
        public string Price { get; set; } = "";
        /// <summary>
        /// 促销价格
        /// </summary>
        public string PromPrice { get; set; } = "";
        /// <summary>
        /// 满足条件
        /// </summary>
        public decimal MeetCount { get; set; } = 0;
        /// <summary>
        /// 折扣
        /// </summary>
        public decimal Discount { get; set; } = 0;
        /// <summary>
        /// 赠品Id
        /// </summary>
        public string GiftId { get; set; } = "";
        /// <summary>
        /// 购买数量
        /// </summary>
        public decimal Quantity { get; set; } = 0;
        /// <summary>
        /// 客户限购数量
        /// </summary>
        public decimal MaxQuantity { get; set; } = 0;
        /// <summary>
        /// 已售数量
        /// </summary>
        public decimal SaleQuantity { get; set; } = 0;
        /// <summary>
        /// 商品图片地址
        /// </summary>
        public string Img_Url { get; set; } = "";
        /// <summary>
        /// 包装单位
        /// </summary>
        public string Package_Unit { get; set; } = "";
        /// <summary>
        /// 是否限销
        /// </summary>
        public string Limit { get; set; } = "N";
        /// <summary>
        /// 限销内容
        /// </summary>
        public string GoodsLimit { get; set; } = "";
    }
}