using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.Models
{
    public class Address
    {
        public string Id { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId { get; set; } = "";
        /// <summary>
        /// 联系人
        /// </summary>
        public string Accept_Name { get; set; } = "";
        /// <summary>
        /// 省份
        /// </summary>
        public string Province { get; set; } = "";
        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; } = "";
        /// <summary>
        /// 县/辖区
        /// </summary>
        public string Prefecture { get; set; } = "";
        /// <summary>
        /// 详细地址
        /// </summary>
        public string UserAddress { get; set; } = "";
        /// <summary>
        /// 联系方式
        /// </summary>
        public string Telphone { get; set; } = "";
        /// <summary>
        /// 默认地址
        /// </summary>
        public int IsDefault { get; set; }
    }
}