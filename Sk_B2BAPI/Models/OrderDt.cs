using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.Models
{
    public class OrderDt
    {
        /// <summary>
        /// 明细ID
        /// </summary>
        public int Id {get;set;}
        /// <summary>
        /// 订单Id
        /// </summary>
        public string Order_Id { get; set; } = "";
        /// <summary>
        /// 订单编号
        /// </summary>
        public string Order_No { get; set; } = "";
        /// <summary>
        /// 商品Id
        /// </summary>
        public string Article_Id { get; set;} = "";
        /// <summary>
        /// 商品名称
        /// </summary>
        public string Goods_Title { get; set;} = "";
        /// <summary>
        /// 商品价格
        /// </summary>
        public decimal Goods_Price { get; set;}
        /// <summary>
        /// 实付价格
        /// </summary>
        public decimal Real_Price { get; set; }
        /// <summary>
        /// 订单数量
        /// </summary>
        public decimal Quantity { get; set; }
        /// <summary>
        /// 实发数量
        /// </summary>
        public decimal NetNumber { get; set; }
        /// <summary>
        /// 退货数量
        /// </summary>
        public decimal ReturnNumber { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal TaxAmount { get; set; }
        /// <summary>
        /// 促销标识
        /// </summary>
        public string cxbs { get; set; } = "";
        /// <summary>
        /// 商品编号
        /// </summary>
        public string GoodsCode { get; set; } = "";
        /// <summary>
        /// 上产厂家
        /// </summary>
        public string Drug_Factory { get; set; } = "";
        /// <summary>
        /// 商品规格
        /// </summary>
        public string Drug_Spec { get; set; } = "";
        /// <summary>
        /// 包装单位
        /// </summary>
        public string Package_Unit { get; set; } = "";
        /// <summary>
        /// 中包装
        /// </summary>
        public decimal Min_Package { get; set; }
        /// <summary>
        /// 效期
        /// </summary>
        public string Valdate { get; set; } = "";
        /// <summary>
        /// 图片
        /// </summary>
        public string ImgUrl { get; set; } = "";
        /// <summary>
        /// 促销标识
        /// </summary>
        public string Tag { get; set; } = "";
        /// <summary>
        /// 是否评论
        /// </summary>
        public string IsCriticism { get; set; } = "";
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 明细状态中文
        /// </summary>
        public string StatusName { get; set; }

        /// <summary>
        /// 批号
        /// </summary>
        public string pihao { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public decimal shl { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        public decimal hshj { get; set; }
        /// <summary>
        /// 总金额
        /// </summary>
        public decimal hsje { get; set; }
    }
}