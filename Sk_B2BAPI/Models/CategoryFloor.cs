using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.Models
{
    public class CategoryFloor
    {
        /// <summary>
        /// 分类名
        /// </summary>
        public string Category { get; set; } = "";
        /// <summary>
        /// 分类商品
        /// </summary>
        public List<ImgInfo> Imgs { get; set; }
    }
}