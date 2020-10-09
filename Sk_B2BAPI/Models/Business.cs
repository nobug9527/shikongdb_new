using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.Models
{
    /// <summary>
    /// 往来客户与供应商
    /// </summary>
    public class Business
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId { get; set; } = "";
        /// <summary>
        /// 机构
        /// </summary>
        public string EntId { get; set; } = "";
        /// <summary>
        /// 公司Id
        /// </summary>
        public string BusinessId { get; set; } = "";
        /// <summary>
        /// 公司Code
        /// </summary>
        public string BusinessCode { get; set; } = "";
        /// <summary>
        /// 公司名称
        /// </summary>
        public string BusinessName { get; set; } = "";
        /// <summary>
        /// 经营范围
        /// </summary>
        public string BusinessCont { get; set; } = "";
        /// <summary>
        /// 助记码
        /// </summary>
        public string Logogram { get; set; } = "";
        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; } = "";
        /// <summary>
        /// 联系方式
        /// </summary>
        public string Telphone { get; set; } = "";
        /// <summary>
        /// 是否进货isjh
        /// </summary>
        public string Stock { get; set; } = "";
        /// <summary>
        /// 是否销售isxs
        /// </summary>
        public string Sell { get; set; } = "";
        /// <summary>
        /// 是否活动
        /// </summary>
        public string Beactive { get; set; } = "";
        /// <summary>
        /// 价格级别shortname
        /// </summary>
        public string PriceLevel { get; set; } = "";
        /// <summary>
        /// 公司类型
        /// </summary>
        public string ClientType { get; set; } = "";
        /// <summary>
        /// 省
        /// </summary>
        public string Province { get; set; } = "";
        /// <summary>
        /// 市
        /// </summary>
        public string City { get; set; } = "";
        /// <summary>
        /// 区
        /// </summary>
        public string County { get; set; } = "";
        /// <summary>
        /// 许可证效期xkzyxq
        /// </summary>
        public string Licence { get; set; } = "";
        /// <summary>
        /// 营业执照效期yyzzyxq
        /// </summary>
        public string BusinessLicense { get; set; } = "";
        /// <summary>
        /// GSP证书gspzsyxq
        /// </summary>
        public string GSP { get; set; } = "";
        /// <summary>
        /// 委托人wtr
        /// </summary>
        public string Principal { get; set; } = "";
        /// <summary>
        /// 委托书效期wtsyxq
        /// </summary>
        public string Proxy { get; set; } = "";
        /// <summary>
        /// 添加时间
        /// </summary>
        public string AddTime { get; set; } = "";
        /// <summary>
        /// 最后更新时间
        /// </summary>
        public string LastModifyTime { get; set; } = "";
    }
}