using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.Models
{
    public class Article
    {
        /// <summary>
        /// 文章Id
        /// </summary>
        public int Id { get; set;}
        /// <summary>
        /// 页面标识
        /// </summary>
        public string Call_Index { get; set; } = "";
        /// <summary>
        /// 文章标题
        /// </summary>
        public string Title { get; set; } = "";
        /// <summary>
        /// 文章摘要
        /// </summary>
        public string Digest { get; set; } = "";
        /// <summary>
        /// 商品Id
        /// </summary>
        public string Parent_Id { get; set; } = "";
        /// <summary>
        /// 分类标题
        /// </summary>
        public string Category { get; set; } = "";
        /// <summary>
        /// 分类标识
        /// </summary>
        public string ClassList { get; set; } = "";
        /// <summary>
        /// 级别
        /// </summary>
        public string Class_layer { get; set; } = "";
        /// <summary>
        /// 类容
        /// </summary>
        public string Content { get; set; } = "";
        /// <summary>
        /// 二级分类
        /// </summary>
        public List<Article> ArticleList{ get; set;}
        /// <summary>
        /// 发布时间
        /// </summary>
        public string Add_Time { get; set; } = "";
    }
}