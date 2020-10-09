using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.Models
{
    public class ImgInfo
    {
        /// <summary>
        /// 企业ID
        /// </summary>
        public string Entid { get; set; } = "";
        /// <summary>
        /// 图片id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 图片排序
        /// </summary>
        public int SortId { get; set; }
        /// <summary>
        /// 标题/楼层名称
        /// </summary>
        public string Title { get; set; } = "";
        /// <summary>
        /// 图片路径
        /// </summary>
        public string ImgUrl { get; set; } = "";
        /// <summary>
        /// 左侧图
        /// </summary>
        public string LeftPic { get; set; } = "";
        /// <summary>
        /// 图片连接地址
        /// </summary>
        public string LinkUrl { get; set; } = "";
        /// <summary>
        /// 品牌图
        /// </summary>
        public string BrandImgUrl { get; set; } = "";
        /// <summary>
        /// 图片类型
        /// </summary>
        public string ImgType { get; set; } = "";
        /// <summary>
        /// 商品ID
        /// </summary>
        public string ArticleId { get; set; } = "";
        /// <summary>
        /// 颜色编码
        /// </summary>
        public string Color { get; set; } = "";
        /// <summary>
        /// 备注
        /// </summary>
        public string BeiZhu { get; set; } = "";
        /// <summary>
        /// 商品名称
        /// </summary>
        public string SubTitle { get; set; } = "";
        /// <summary>
        /// 商品库存
        /// </summary>
        public string Quantity { get; set; } = "";
        /// <summary>
        /// 商品规格
        /// </summary>
        public string DrugSpec { get; set; } = "";
        /// <summary>
        /// 商品单位
        /// </summary>
        public string PackageUnit { get; set; } = "";
        /// <summary>
        /// 生产厂家
        /// </summary>
        public string DrugFactory { get; set; } = "";
        /// <summary>
        /// 商品价格
        /// </summary>
        public string Price { get; set; } = "";
        /// <summary>
        /// 积分
        /// </summary>
        public string Integral { get; set; } = "";
        /// <summary>
        /// 是否品牌
        /// </summary>
        public string IsBrand { get; set; } = "";
        /// <summary>
        /// 品牌编号
        /// </summary>
        public string BrandCode { get; set; } = "";
        /// <summary>
        /// 活动编号
        /// </summary>
        public string Fabh { get; set; } = "";
        /// <summary>
        /// 促销标识
        /// </summary>
        public string Cxbs { get; set; } = "";
        /// <summary>
        /// APP跳转链接
        /// </summary>
        public string AndroidLinkUrl { get; set; } = "";
        /// <summary>
        /// APP跳转类型
        /// </summary>
        public int AndroidLinkType { get; set; }
        /// <summary>
        /// 楼层图片类型
        /// </summary>
        public string TypeName { get; set; } = "";
    }
}