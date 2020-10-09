using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.Models
{
    public class EnumType
    {
    }

    /// <summary>
    /// 促销规则对应的促销内容的类型
    /// </summary>
    public enum EPromotionContentType
    {
        /// <summary>
        /// 单品
        /// </summary>
        SingleGoods,
        /// <summary>
        /// 品牌
        /// </summary>
        Brand,
        /// <summary>
        /// 客户分组
        /// </summary>
        CustomsGroup
    }

    /// <summary>
    /// 促销规则的优惠方式
    /// </summary>
    public enum EPromotionRule
    {
        /// <summary>
        /// 打折
        /// </summary>
        Discount,
        /// <summary>
        /// 满减
        /// </summary>
        FJ,
        /// <summary>
        /// 满赠
        /// </summary>
        FZ,
        /// <summary>
        /// 抢购
        /// </summary>
        QG,
        /// <summary>
        /// 近效期优惠
        /// </summary>
        XQ
    }

    /// <summary>
    /// 促销的优惠规则
    /// </summary>
    public enum EPromotionType
    {
        /// <summary>
        /// 满足一定的数量
        /// </summary>
        SumQuality,
        /// <summary>
        /// 满足一定的金额
        /// </summary>
        SumMoney,
        /// <summary>
        /// 满足累计的数量的当前数
        /// </summary>
        ArriveNum

    }

    /// <summary>
    /// 接口返回值的状态码
    /// </summary>
    public enum EReturnCode
    {
        Success = 00,
        ParamError = 05,
        SystemError = 99
    }

    /// <summary>
    /// 优惠券类型
    /// </summary>
    public enum EPrizeType
    {
        /// <summary>
        /// 虚拟物品-优惠券
        /// </summary>
        优惠券,
        /// <summary>
        /// 实体物品
        /// </summary>
        实体物品,
        /// <summary>
        /// 虚拟物品-会员积分
        /// </summary>
        会员积分,
        /// <summary>
        /// 空奖，未中奖
        /// </summary>
        空奖
    }
}