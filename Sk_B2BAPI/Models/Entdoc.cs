using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.Models
{
    /// <summary>
    /// 机构
    /// </summary>
    public class Entdoc
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; } 
        /// <summary>
        /// 机构ID
        /// </summary>
        public string EntId { get; set; } = "";
        /// <summary>
        /// 机构名称
        /// </summary>
        public string EntName { get; set; } = "";
        /// <summary>
        /// 机构编码
        /// </summary>
        public string EntCode { get; set; } = "";
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
    }
}