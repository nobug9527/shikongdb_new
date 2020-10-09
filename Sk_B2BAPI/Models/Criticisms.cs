using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.Models
{
    /// <summary>
    /// 评论
    /// </summary>
    public class Criticisms
    {
        /// <summary>
        /// 评论Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; } = "";

        /// <summary>
        /// 头像
        /// </summary>
        public string ImgUrl { get; set; } = "";

        /// <summary>
        /// 发布时间
        /// </summary>
        public string AddTime { get; set; } = "";

        /// <summary>
        /// 评论内容
        /// </summary>
        public string Content { get; set; } = "";

        /// <summary>
        /// 星级评价
        /// </summary>
        public decimal? StarLevel { get; set; }

        /// <summary>
        /// 点赞数
        /// </summary>
        public int LikeSum { get; set; }
        /// <summary>
        /// 是否点赞
        /// </summary>
        public string IsLike { get; set; } = "";
        /// <summary>
        /// 级别
        /// </summary>
        public int Rank { get; set; }
    }
}