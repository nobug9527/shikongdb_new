using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.Models
{
    public class GoodsInfo
    {
        /// <summary>
        /// 机构名称
        /// </summary>
        public string EntName { get; set; } = "";
        /// <summary>
        /// 商品Id
        /// </summary>
        public string Article_Id { get; set; } = "";
        /// <summary>
        /// 商品编号
        /// </summary>
        public string GoodsCode { get; set; } = "";
        /// <summary>
        /// 促销标识
        /// </summary>
        public string Cxbs { get; set;} = "";
        /// <summary>
        /// 促销编号
        /// </summary>
        public string Fabh { get; set;} = "";
        /// <summary>
        /// 促销描述
        /// </summary>
        public string Describe { get; set; } = "";
        /// <summary>
        /// 商品名称
        /// </summary>
        public string Sub_Title { get; set; } = "";
        /// <summary>
        /// 商品规格
        /// </summary>
        public string Drug_Spec { get; set; } = "";
        /// <summary>
        /// 包装单位
        /// </summary>
        public string Package_Unit{ get; set; } = "";
        /// <summary>
        /// 商品产地
        /// </summary>
        public string GoodsOrigin { get; set; } = "";
        /// <summary>
        /// 生产厂家
        /// </summary>
        public string Drug_Factory{get; set; } = "";
        /// <summary>
        /// 大包装
        /// </summary>
        public decimal? Big_Package { get; set; } = 0;
        /// <summary>
        /// 中包装控制
        /// </summary>
        public decimal? Min_Package { get; set; } = 0;
        /// <summary>
        /// 商品分类
        /// </summary>
        public string Category { get; set; } = "";
        /// <summary>
        /// 库存(真实库存)
        /// </summary>
        public decimal? Stock_Quantity { get; set;} = 0;
        /// <summary>
        /// 显示库存
        /// </summary>
        public string Inventory { get; set; } = "";
        /// <summary>
        /// 商品价格
        /// </summary>
        public string Price { get; set; } = "";
        /// <summary>
        /// 建议价格
        /// </summary>
        public decimal ProposalPrice { get; set; } = 0;
        /// <summary>
        /// 效期
        /// </summary>
        public string Valdate { get; set; } = "";
        /// <summary>
        /// 老效期
        /// </summary>
        public string OldValdate { get; set; } = "";
        /// <summary>
        /// 剂型
        /// </summary>
        public string DosageForm { get; set; } = "";
        /// <summary>
        /// 是否拆零
        /// </summary>
        public string Scattered { get; set; } = "";
        /// <summary>
        /// 商品图片
        /// </summary>
        public string ImgUrl { get; set; } = "";
        /// <summary>
        /// 详情图片
        /// </summary>
        public List<ImgInfo> ImgList { get; set; }
        /// <summary>
        /// 促销信息
        /// </summary>
        public List<Promotion> Promotion { get; set; }
        /// <summary>
        /// 商品说明书
        /// </summary>
        public string Content { get; set; } = "";
        /// <summary>
        /// 收藏Id
        /// </summary>
        public string CollectId { get; set; } = "";
        /// <summary>
        /// 有效期至
        /// </summary>
        public string ApprovalNumber { get; set; } = "";
        /// <summary>
        /// 是否限销
        /// </summary>
        public string Limit { get; set; } = "";
        /// <summary>
        /// 限销内容
        /// </summary>
        public string GoodsLimit { get; set; } = "";
        /// <summary>
        /// 评论数
        /// </summary>
        public int? PassSum { get; set; } = 0;
        /// <summary>
        /// 好评率
        /// </summary>
        public decimal? RaveReviews { get; set; } = 0;
        /// <summary>
        /// 中包装
        /// </summary>
        public decimal? Zbz { get; set; } = 1;
        /// <summary>
        /// 批号
        /// </summary>
        public string BatchNumber { get; set; }
        /// <summary>
        /// 摘要
        /// </summary>
        public string Abstract { get; set; }

    }
}