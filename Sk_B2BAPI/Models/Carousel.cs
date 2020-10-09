using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.Models
{
    /// <summary>
    /// 轮播图
    /// </summary>
    public class Carousel
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string CarouselName { get; set; } = "";
        /// <summary>
        /// 图片路径
        /// </summary>
        public string ImgPath { get; set; } = "";
        /// <summary>
        /// 跳转路径
        /// </summary>
        public string SkipLink { get; set; } = "";
        /// <summary>
        /// 序号-跳转模块
        /// </summary>
        public string SerialNumber { get; set; } = "";
        /// <summary>
        /// 跳转商品
        /// </summary>
        public string ArticleID { get; set; } = "";
        /// <summary>
        /// APP跳转链接
        /// </summary>
        public string AndroidLinkUrl { get; set; } = "";
        /// <summary>
        /// APP跳转类型
        /// </summary>
        public int AndroidLinkType { get; set; }
    }
}