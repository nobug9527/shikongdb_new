using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.Models
{
    public class UserInfo
    {
        /// <summary>
        /// 企业id
        /// </summary>
        public string EntId { get; set; } = "";
        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId { get; set; } = "";
        /// <summary>
        /// 登陆账号
        /// </summary>
        public string UserName{ get; set; } = "";
        /// <summary>
        /// 密码
        /// </summary>
        public string PassWord { get; set; } = "";
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; } = "";
        /// <summary>
        /// 性别
        /// </summary>
        public string Sex { get; set; } = "";
        /// <summary>
        /// 生日
        /// </summary>
        public string Birthday { get; set; } = "";
        /// <summary>
        /// email
        /// </summary>
        public string Email { get; set; } = "";
        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; } = "";
        /// <summary>
        /// 联系方式
        /// </summary>
        public string Telphone { get; set; } = "";
        /// <summary>
        /// 余额
        /// </summary>
        public string Balance { get; set; } = "";
        /// <summary>
        /// 积分
        /// </summary>
        public int Point { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 客户Id
        /// </summary>
        public string BusinessId { get; set; } = "";
        /// <summary>
        /// 经营范围
        /// </summary>
        public string BusinessScope { get; set; } = "";
        /// <summary>
        /// 客户名称
        /// </summary>
        public string BusinessName { get; set; } = "";
        /// <summary>
        /// 省
        /// </summary>
        public string Province { get; set; } = "";
        /// <summary>
        /// 市
        /// </summary>
        public string City { get; set; } = "";
        /// <summary>
        /// 经度
        /// </summary>
        public string Longitude { get; set; } = "";
        /// <summary>
        /// 纬度
        /// </summary>
        public string Latitude { get; set; } = "";
        /// <summary>
        /// 客户分类
        /// </summary>
        public string KhType { get; set; } = "";
        /// <summary>
        /// 客户类型（中文）
        /// </summary>
        public string ClientType { get; set; } = "";
        /// <summary>
        /// 客户类型Id
        /// </summary>
        public string CustomerTypeId { get; set; } = "";
        /// <summary>
        /// 客户价格级别
        /// </summary>
        public string Pricelevel { get; set; } = "";
        /// <summary>
        /// 图片信息
        /// </summary>
        public string Img_Url {get; set; } = "";
        /// <summary>
        /// 可用优惠券数量
        /// </summary>
        public int CouponCount { get; set; }
        /// <summary>
        /// 客户限销
        /// </summary>
        public string ClientLimit { get; set; } = "";
        /// <summary>
        /// 是否存在证书过期
        /// </summary>
        public bool StaleDated { get; set; } = false;
        /// <summary>
        /// 证书过期个数
        /// </summary>
        public int OverdueNumber { get; set; } = 0;
        /// <summary>
        /// 证书过期信息
        /// </summary>
        public string StaleMsg { get; set; } = "";
        #region _弃用
        ///// <summary>
        ///// 医疗器械经营许可证
        ///// </summary>
        //public string YLQXJYXKZXQ { get; set; } = "";
        ///// <summary>
        ///// 药品生产许可证效期
        ///// </summary>
        //public string YPSCXKZXQ { get; set; } = "";
        ///// <summary>
        ///// 医疗器械生产许可证效期
        ///// </summary>
        //public string YLQXSCXKZXQ { get; set; } = "";
        ///// <summary>
        ///// 医疗机构执业许可证效期
        ///// </summary>
        //public string YLJGZYXKZXQ { get; set; } = "";
        ///// <summary>
        ///// 年度报告效期
        ///// </summary>
        //public string NDBGXQ { get; set; } = "";
        ///// <summary>
        ///// 质量保证协议效期
        ///// </summary>
        //public string ZLBZXYXQ { get; set; } = "";
        ///// <summary>
        ///// 母婴保健许可证
        ///// </summary>
        //public string MYBJXKZXQ { get; set; } = "";
        ///// <summary>
        ///// 母婴保健技术许可证
        ///// </summary>
        //public string MYBJJSXKZXQ { get; set; } = "";
        ///// <summary>
        ///// gsp证书效期
        ///// </summary>
        //public string GSPZSYXQ { get; set; } = "";
        ///// <summary>
        ///// 药品经营许可证效期
        ///// </summary>
        //public string YPJYXKZXQ { get; set; } = "";
        ///// <summary>
        ///// 营业执照效期
        ///// </summary>
        //public string YYZZXQ { get; set; } = "";
        ///// <summary>
        ///// 食品流通许可证效期
        ///// </summary>
        //public string SPLTXKZXQ { get; set; } = "";
        ///// <summary>
        ///// 委托书
        ///// </summary>
        //public string WTSXQ { get; set; } = "";
        ///// <summary>
        ///// 卫生许可证
        ///// </summary>
        //public string WSXKZXQ { get; set; } = "";
        ///// <summary>
        ///// 公示有效期
        ///// </summary>
        //public string GSYXQ { get; set; } = "";
        ///// <summary>
        ///// 许可证有效期
        ///// </summary>
        //public string XKZXQ { get; set; } = "";
        #endregion
        /// <summary>
        /// 证书列表
        /// </summary>
        public List<Certificate> Certificates { get; set; } = null;
    }
}