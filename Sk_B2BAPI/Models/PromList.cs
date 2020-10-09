using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.Models
{
    public class PromList
    {
        /// <summary>
        /// 企业Id
        /// </summary>
        public string EntId { get; set; } = "";
        /// <summary>
        /// 机构名称
        /// </summary>
        public string EntName { get; set; } = "";
        /// <summary>
        /// 方案编号
        /// </summary>
        public string Fabh { get; set; } = "";
        /// <summary>
        /// 方案名称
        /// </summary>
        public string FaTitle { get; set; } = "";
        /// <summary>
        /// 方案标识
        /// </summary>
        public string Fabs { get; set; } = "";
        /// <summary>
        /// 开始日期
        /// </summary>
        public string StartDate { get; set; } = "";
        /// <summary>
        /// 结束日期
        /// </summary>
        public string EndDate { get; set; } = "";
        /// <summary>
        /// 总库存
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 占用库存
        /// </summary>
        public decimal YAmount { get; set; }
        /// <summary>
        /// 客户可销数量
        /// </summary>
        public decimal KhAmount { get; set; }
        /// <summary>
        /// 方案描述
        /// </summary>
        public string Describe { get; set; } = "";

        /// <summary>
        /// 商品ID
        /// </summary>
        public string Article_Id { get; set; } = "";
        /// <summary>
        /// 商品名称
        /// </summary>
        public string Sub_Title { get; set; } = "";
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
        public decimal Min_Package { get; set; }
        /// <summary>
        /// 大包装
        /// </summary>
        public decimal Big_Package { get; set; }
        /// <summary>
        /// 库存
        /// </summary>
        public decimal Stock_Quantity { get; set; }
        /// <summary>
        /// 商品价格
        /// </summary>
        public string Price { get; set; } = "";
        /// <summary>
        /// 建议价格
        /// </summary>
        public decimal ProposalPrice { get; set; } = 0;
        /// <summary>
        /// 商品图片地址
        /// </summary>
        public string Img_Url { get; set; } = "";
        /// <summary>
        /// 现价
        /// </summary>
        public string CPrice { get; set; } = "";
        /// <summary>
        /// 数量
        /// </summary>
        public decimal Quantity { get; set; }
        /// <summary>
        /// 是否限销
        /// </summary>
        public string Limit { get; set; } = "";
        /// <summary>
        /// 限销内容
        /// </summary>
        public string GoodsLimit { get; set; } = "";
    }
}