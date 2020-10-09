using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.Models
{
    public class CartGoods
    {
        public string Id { get; set; } = "";
        /// <summary>
        /// 企业Id
        /// </summary>
        public string EntId { get; set; } = "";
        /// <summary>
        /// 商品Id
        /// </summary>
        public string Article_Id { get; set;} = "";
        /// <summary>
        /// 商品名称
        /// </summary>
        public string Sub_Title { get; set; } = "";
        /// <summary>
        /// 中包装显示用
        /// </summary>
        public decimal Zbz { get; set; }
        /// <summary>
        /// 中包装控制用
        /// </summary>
        public decimal Min_Package { get; set; }
        /// <summary>
        /// 大包装
        /// </summary>
        public decimal Big_Package { get; set; }
        /// <summary>
        /// 商品规格
        /// </summary>
        public string Drug_Spec { get; set; } = "";
        /// <summary>
        /// 生产厂家
        /// </summary>
        public string Drug_Factory { get; set; } = "";
        /// <summary>
        /// 显示库存
        /// </summary>
        public string Inventory { get; set; } = "";
        /// <summary>
        /// 库存
        /// </summary>
        public decimal Stock_Quantity { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 真实价格/原价
        /// </summary>
        public decimal RealPrice { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public decimal Quantity { get; set; }
        /// <summary>
        /// 商品金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 是否拆零
        /// </summary>
        public string Scattered { get; set; } = "";
        /// <summary>
        /// 商品图片
        /// </summary>
        public string Img_Url { get; set; } = "";
        /// <summary>
        /// 活动编号
        /// </summary>
        public string Fabh { get; set; } = "";
        /// <summary>
        /// 方案标识
        /// </summary>
        public string Fabs { get; set; } = "";
        /// <summary>
        /// 商品类型
        /// </summary>
        public string GoodsType { get; set; } = "";
        /// <summary>
        /// 商品折扣
        /// </summary>
        public decimal Discount { get; set; }
        /// <summary>
        /// 商品减额
        /// </summary>
        public decimal Derate { get; set; }
        /// <summary>
        /// 促销范围
        /// </summary>
        public string PromScenario { get; set; } = "";
        /// <summary>
        /// 倍数
        /// </summary>
        public decimal Multiple { get; set; }
    }
}