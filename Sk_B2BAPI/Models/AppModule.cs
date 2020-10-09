using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.Models
{
    /// <summary>
    /// App模块
    /// </summary>
    public class AppModule
    {
        /// <summary>
        /// 模块名称
        /// </summary>
        public string ModuleName { get; set; } = "";
        /// <summary>
        /// 图标路径
        /// </summary>
        public string ImgPath { get; set; } = "";
        /// <summary>
        /// 序号-跳转模块
        /// </summary>
        public string SerialNumber { get; set; } = "";
        /// <summary>
        /// 跳转路径
        /// </summary>
        public string SkipLink { get; set; } = "";
        /// <summary>
        /// 积分
        /// </summary>
        public int Point { get; set; }
        /// <summary>
        /// 余额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 模块类型
        /// </summary>
        public string Type { get; set; } = "";
    }
}