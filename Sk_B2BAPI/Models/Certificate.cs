using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.Models
{
    /// <summary>
    /// 证书
    /// </summary>
    public class Certificate
    {
        /// <summary>
        /// 证书中文名称
        /// </summary>
        public string CertificateName { get; set; }
        /// <summary>
        /// 字段名称
        /// </summary>
        public string FieldName { get; set; }
        /// <summary>
        /// 到期时间
        /// </summary>
        public string ExpiryDate { get; set; }
        /// <summary>
        /// 是否到期
        /// </summary>
        public bool TimeExpiration { get; set; }
    }
}