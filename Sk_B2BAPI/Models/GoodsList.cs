using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.Models
{
    public class GoodsList
    {
        /// <summary>
        ///总条目数
        /// </summary>
        public int RountCount { get; set; }
        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount { get; set; }
        /// <summary>
        /// 当前页数
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// 每页显示条目数
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 商品列表
        /// </summary>
        public List<GoodsInfo> GoodsInfo{ get; set; }
        /// <summary>
        /// 商品分类
        /// </summary>
        public List<Category> GoodsCategory { get; set; }

    }
}