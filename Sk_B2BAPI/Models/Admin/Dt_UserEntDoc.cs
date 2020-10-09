using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.Models.Admin
{
    /// <summary>
    /// 用户机构绑定关系
    /// </summary>
    [SugarTable("Dt_UserEntDoc")]
    public class Dt_UserEntDoc
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int id { get; set; } = 0;
        /// <summary>
        /// 用户Id
        /// </summary>
        public string userId { get; set; } = "";
        /// <summary>
        /// 机构Id
        /// </summary>
        public string entId { get; set; } = "";
        /// <summary>
        /// 状态
        /// </summary>
        public int status { get; set; } = 0;
        /// <summary>
        /// 添加时间
        /// </summary>
        public string addTime { get; set; } = "";
        /// <summary>
        /// 价格级别
        /// </summary>
        public string PriceLevel { get; set; } = "";
    }
}