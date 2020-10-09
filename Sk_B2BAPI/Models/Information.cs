using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.Models
{
    public class Information
    {
        /// <summary>
        /// 轮播图
        /// </summary>
        public List<Carousel> Carousel { get; set; }
        /// <summary>
        /// App模块
        /// </summary>
        public List<AppModule> AppModul { get; set; }
    }
}