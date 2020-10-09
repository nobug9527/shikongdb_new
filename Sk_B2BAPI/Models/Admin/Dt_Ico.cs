using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.Models.Admin
{
    public class Dt_Ico
    {

        /// <summary>
        /// 名称
        /// </summary>
        public long Id { get; set; }
        
        /// <summary>
        /// 名称
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Describe { get; set; }

        /// <summary>
        /// 是否为CSS图标
        /// </summary>
        public bool IsCssOrImages { get; set; }

        /// <summary>
        /// CSS图标样式
        /// </summary>
        public string CssName { get; set; }

        /// <summary>
        /// 图片路径
        /// </summary>
        public string Url { get; set; }
    }
}