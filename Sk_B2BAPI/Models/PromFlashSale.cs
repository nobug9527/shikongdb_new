using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.Models
{
    public class PromFlashSale
    {
        /// <summary>
        /// 促销title
        /// </summary>
        public string Title { get; set; } = "";
        /// <summary>
        /// 促销开始日期
        /// </summary>
        public string StartDate { get; set; } = "";
        /// <summary>
        /// 促销结束日期
        /// </summary>
        public string EndDat { get; set; } = "";
        /// <summary>
        /// 是否开始
        /// </summary>
        public int Start { get; set; }
        /// <summary>
        /// 促销商品
        /// </summary>
        public List<PromList> list { get; set; } 
    }
}