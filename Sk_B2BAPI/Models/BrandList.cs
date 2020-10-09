using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.Models
{
    public class BrandList
    {
        /// <summary>
        /// 企业Id
        /// </summary>
        public string EntId { get; set;} = "";
        /// <summary>
        /// 方案编号
        /// </summary>
        public string BillNo { get; set; } = "";
        /// <summary>
        /// 方案名称
        /// </summary>
        public string Name { get; set; } = "";
        /// <summary>
        /// 备注
        /// </summary>
        public string BeiZhu { get; set; } = "";
        /// <summary>
        /// 排序Id
        /// </summary>
        public int Sort_Id { get; set; }
        /// <summary>
        /// 品牌图片
        /// </summary>
        public string ImgUrl { get; set; } = "";
        /// <summary>
        /// 品牌详情图片
        /// </summary>
        public string ImgUrlDt { get; set; } = "";
        /// <summary>
        /// 促销编号
        /// </summary>
        public string Fabh { get; set; } = "";
        /// <summary>
        /// 促销标识
        /// </summary>
        public string Cxbs { get; set; } = "";
        /// <summary>
        /// 商品列表
        /// </summary>
        public List<GoodsInfo> GoodsList { get; set; }

    }
}