using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.Models
{
    /// <summary>
    /// 优惠券
    /// </summary>
    public class Coupon
    {   
        /// <summary>
        /// 优惠券名称
        /// </summary>
        public string CouponName { get; set; } = "";
        /// <summary>
        /// 优惠券编码
        /// </summary>
        public int CouponCode { get; set; }
        /// <summary>
        /// 优惠券剩余数量
        /// </summary>
        public int CouponsNumber { get; set; }

        /// <summary>
        /// 优惠券总量
        /// </summary>
        public int yhqzl { get; set; }

        /// <summary>
        /// 已领优惠券数量
        /// </summary>
        public int ylsl { get; set; }
        
        /// <summary>
        /// 开始时间
        /// </summary>
        public string StartingTime { get; set; } = "";
        /// <summary>
        /// 结束时间
        /// </summary>
        public string EndTime { get; set; } = "";
        /// <summary>
        /// 渠道类型
        /// </summary>
        public int TypeCoding { get; set; }
        /// <summary>
        /// 优惠券状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 领取类型
        /// </summary>
        public int ReceivingType { get; set; }
        /// <summary>
        /// 范围规则
        /// </summary>
        public int SceneCoding { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public int IsDel { get; set; }
        /// <summary>
        /// 赠品主键
        /// </summary>
        public string ProductCode { get; set; } = "";
        /// <summary>
        /// 满额
        /// </summary>
        public decimal AllAmout { get; set; }
        /// <summary>
        /// 满件
        /// </summary>
        public string Num { get; set; }
        /// <summary>
        /// 类型编号
        /// </summary>
        public int Types { get; set; }
        /// <summary>
        /// 产品ID
        /// </summary>
        public string SceneId { get; set; } = "";
        /// <summary>
        /// 满额
        /// </summary>
        public decimal FullAmount { get; set; }
        /// <summary>
        /// 减额
        /// </summary>
        public decimal Deduction { get; set; }
        /// <summary>
        /// 最大金额（限制折扣） 0为不限制
        /// </summary>
        public decimal MaximumAmount { get; set; }
        /// <summary>
        /// 折扣(限制折扣) 0为不限制
        /// </summary>
        public decimal Discount { get; set; }
        /// <summary>
        /// 件数（满件类型生效）
        /// </summary>
        public string Number { get; set; } = "";
        /// <summary>
        /// 类型名称
        /// </summary>
        public string TypeName { get; set; } = "";
        /// <summary>
        /// 赠品名称
        /// </summary>
        public string GoodsName { get; set; } = "";
        /// <summary>
        /// 范围类型
        /// </summary>
        public string SceneName { get; set; } = "";
        /// <summary>
        /// 使用对象
        /// </summary>
        public string Area { get; set; } = "";
    }
}