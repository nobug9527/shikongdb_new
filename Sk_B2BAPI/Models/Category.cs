using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.Models
{
    public class Category
    {
        /// <summary>
        /// 企业id
        /// </summary>
        public string Entid { get; set; } = "";
        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public int Channel_Id { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set;} = "";
        /// <summary>
        /// 编号
        /// </summary>
        public string Call_Index { get; set;} = "";
        /// <summary>
        /// 上级ID
        /// </summary>
        public int Parent_Id { get; set; }
        /// <summary>
        /// 分类编号
        /// </summary>
        public string Class_List { get; set; } = "";
        /// <summary>
        /// 排序编号
        /// </summary>
        public int Sort_Id { get; set; }
        /// <summary>
        /// 连接路径
        /// </summary>
        public string Link_Url { get; set; } = "";
        /// <summary>
        /// 图片路径
        /// </summary>
        public string Img_Url { get; set; } = "";
        /// <summary>
        /// 类容
        /// </summary>
        public string Content { get; set; } = "";
        /// <summary>
        /// 二级分类
        /// </summary>
        public List<Category> Category_Scd { get; set; }

    }
}