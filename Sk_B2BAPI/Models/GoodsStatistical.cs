using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.Models
{
    public class GoodsStatistical
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 商品Id
        /// </summary>
        public string Article_Id { get; set; } = "";
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
        public string Package_Unit { get; set; } = "";
        /// <summary>
        /// 生产厂家
        /// </summary>
        public string Drug_Factory { get; set; } = "";
        /// <summary>
        /// 中包装
        /// </summary>
        public decimal Min_Package { get; set; } 
        /// <summary>
        /// 价格
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Num { get; set; }
        /// <summary>
        /// 商品图片
        /// </summary>
        public string ImgUrl { get; set; } = "";
        /// <summary>
        /// 商品库存
        /// </summary>
        public decimal Stock_Quantity { get; set; }

    }
    public class GoodsArrival {
        /// <summary>
        /// 主键Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 商品Id
        /// </summary>
        public string ProductId { get; set; } = "";
        /// <summary>
        /// 到货提醒状态
        /// </summary>
        public int ArrivalStatus { get; set; }
        /// <summary>
        /// 阅读状态
        /// </summary>
        public int ReadStatus { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public string NewTime { get; set; } = "";
        /// <summary>
        /// 添加时间
        /// </summary>
        public string AddTime { get; set; } = "";
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 用户主键
        /// </summary>
        public string UserId { get; set; } = "";
        /// <summary>
        /// 机构主键
        /// </summary>
        public string EntId { get; set; } = "";
        /// <summary>
        /// 到货提醒信息
        /// </summary>
        public string Remak { get; set; } = "";
    }
}