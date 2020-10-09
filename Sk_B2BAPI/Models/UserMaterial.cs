using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.Models
{
    /// <summary>
    /// 用户注册信息（包含资料）
    /// </summary>
    public class UserMaterial
    {
        /// <summary>
        /// 资质序号
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 资料名称
        /// </summary>
        public string MaterialName { get; set; } = "";
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; } = "";
        /// <summary>
        /// 资料Url
        /// </summary>
        public string MaterialUrl { get; set; } = "";
    }
}