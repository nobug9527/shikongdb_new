using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.Models
{
    /// <summary>
    /// 商品首营图片Model
    /// </summary>
    public class ProductFirstCampPictures
    {
        /// <summary>
        /// 商品编号
        /// </summary>
        public string GoodsNo { get; set; }
        /// <summary>
        /// 图片名称
        /// </summary>
        public string PicName { get; set; } = "";
        /// <summary>
        /// 图片路径
        /// </summary>
        public string ImgPath { get; set; } = "";
        /// <summary>
        /// 分组关键字
        /// </summary>
        public string KeyWord { get; set; } = "";
    }
}