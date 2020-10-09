using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.Models
{
    public class IndexFloor
    {
        /// <summary>
        /// 企业id
        /// </summary>
        public string Entid {get;set;} = "";
        /// <summary>
        /// 楼层Id
        /// </summary>
        public string FloorId { get; set; } = "";
        /// <summary>
        /// 楼层类型
        /// </summary>
        public string FloorType { get; set; } = "";
        /// <summary>
        /// 排序
        /// </summary>
        public string Sort { get; set; } = "";
        /// <summary>
        /// 楼层名称
        /// </summary>
        public string FloorTitle { get; set;} = "";
        /// <summary>
        /// 楼层连接
        /// </summary>
        public string Link_Url { get; set; } = "";
        /// <summary>
        /// 楼层图片
        /// </summary>
        public string Floor_Img { get; set; } = "";
        /// <summary>
        /// 楼层详情
        /// </summary>
        public List<ImgInfo> FloorDetail { get; set; }
        /// <summary>
        /// 首页品牌推荐专属分类
        /// </summary>
        public List<CategoryFloor> FloorCattgory { get; set; }
        /// <summary>
        /// 楼层名称
        /// </summary>
        public string TypeName { get; set; } = "";
        /// <summary>
        /// 楼层类型
        /// </summary>
        public string Type { get; set; } = "";
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