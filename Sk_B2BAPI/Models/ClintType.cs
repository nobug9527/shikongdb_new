using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.Models
{
    /// <summary>
    /// 客户类型
    /// </summary>
    public class ClintType
    {
        /// <summary>
        /// 客户类型序号
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 客户类型编号
        /// </summary>
        public string TypeId { get; set; } = "";
        /// <summary>
        /// 客户类型名称
        /// </summary>
        public string TypeName { get; set; } = "";
        /// <summary>
        /// 客户类型
        /// </summary>
        public string ClientType { get; set; } = "";
    }
}