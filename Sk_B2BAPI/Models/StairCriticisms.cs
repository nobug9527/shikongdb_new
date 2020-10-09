using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.Models
{
    /// <summary>
    /// 一级评论
    /// </summary>
    public class StairCriticisms: Criticisms
    {
        /// <summary>
        /// 回复条数
        /// </summary>
        public int Totality { get; set; }
        /// <summary>
        /// 下级评论
        /// </summary>
        public List<Criticisms> Subordinate { get; set; }
    }
}