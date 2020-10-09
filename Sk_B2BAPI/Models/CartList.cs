using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.Models
{

    public class CartEenDoc
    {
        /// <summary>
        /// 机构Id
        /// </summary>
        public string EntId { get; set; }
        /// <summary>
        /// 机构名称
        /// </summary>
        public string EntName { get; set; }
        /// <summary>
        /// 购物车列表
        /// </summary>
        public CartList cartlist { get; set; }
    }

    public class CartList
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
        /// 订单总金额
        /// </summary>
        public decimal Order_Amount{ get; set; }
        /// <summary>
        /// 订单应付金额
        /// </summary>
        public decimal Real_Amount { get; set; }
        /// <summary>
        /// 优惠金额
        /// </summary>
        public decimal Discount_Amount { get; set; }
        /// <summary>
        /// 商品条目数
        /// </summary>
        public int Num { get; set; }
        /// <summary>
        /// 购物车商品列表
        /// </summary>
        public List<CartGoods> GoodsInfo { get; set; }
        /// <summary>
        /// 购物车普通商品列表
        /// </summary>
        public List<CartGoods> CommonList { get; set; }
        /// <summary>
        /// 购物车单品促销商品列表
        /// </summary>
        public List<CartGoods> SingleList { get; set; }
        /// <summary>
        /// 购物车组合促销商品列表
        /// </summary>
        public List<CartGoods> GroupList { get; set; }
        /// <summary>
        /// 购物车品牌促销商品列表
        /// </summary>
        public List<CartGoods> BrandList { get; set; }
        /// <summary>
        /// 购物车分类促销商品列表
        /// </summary>
        public List<CartGoods> ClassifyList { get; set; }
        /// <summary>
        /// 购物车全场促销商品列表
        /// </summary>
        public List<CartGoods> AllList { get; set; }
    }
}