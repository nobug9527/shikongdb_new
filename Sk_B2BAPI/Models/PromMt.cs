using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.Models
{
    public class PromMt
    {
        /// <summary>
        /// 企业Id
        /// </summary>
        public string EntId { get; set;} = "";
        /// <summary>
        /// 方案编号
        /// </summary>
        public string Fabh { get; set; } = "";
        /// <summary>
        /// 方案名称
        /// </summary>
        public string FaTitle { get; set; } = "";
        /// <summary>
        /// 方案标识
        /// </summary>
        public string Fabs { get; set; } = "";
        /// <summary>
        /// 开始日期
        /// </summary>
        public string StartDate { get; set; } = "";
        /// <summary>
        /// 结束日期
        /// </summary>
        public string EndDate{ get; set; } = "";
        /// <summary>
        /// 总库存
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 占用库存
        /// </summary>
        public decimal YAmount { get; set; }
        /// <summary>
        /// 客户可销数量
        /// </summary>
        public decimal KhAmount { get; set; }
        /// <summary>
        /// 方案描述
        /// </summary>
        public string Describe { get; set; } = "";
        /// <summary>
        /// 促销方案详情
        /// </summary>
        public List<PromDt> PromDt { get; set; }

    }
}