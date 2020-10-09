using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.Models
{
    /// <summary>
    /// 配送方式
    /// </summary>
    public class Dispatch
    {
        /// <summary>
        /// 配送方式编号
        /// </summary>
        public string DispatchId { get; set; } = "";
        /// <summary>
        /// 配送方式
        /// </summary>
        public string DispatchName { get; set; } = "";
        /// <summary>
        /// 是否启用
        /// </summary>
        public string Enable { get; set; } = "";
        /// <summary>
        /// 操作人
        /// </summary>
        public string UserId { get; set; } = "";
        /// <summary>
        /// 机构
        /// </summary>
        public string Entid { get; set; } = "";
    }
}