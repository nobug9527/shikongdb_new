using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.Models
{
    public class Template
    {
        /// <summary>
        /// 系统设置
        /// </summary>
        public List<SystemBase> List_Sz { get; set; }
        /// <summary>
        /// 药品分类
        /// </summary>
        public List<Category> List_Fl { get; set; }
        /// <summary>
        /// 底部资讯
        /// </summary>
        public List<Article> List_Zx { get; set; }
        /// <summary>
        /// 轮播图
        /// </summary>
        public List<ImgInfo> List_Lb { get; set; }
    }
}