using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.Models
{
    public class SystemBase
    {
        /// <summary>
        /// 网站标题
        /// </summary>
        public string Title { get; set; } = "";
        /// <summary>
        /// 网站站点
        /// </summary>
        public string Web_Ip { get; set; } = "";
        /// <summary>
        /// 公司名称
        /// </summary>
        public string Company { get; set; } = "";
        /// <summary>
        /// 投诉电话
        /// </summary>
        public string Complaints { get; set; } = "";
        /// <summary>
        /// 交易证
        /// </summary>
        public string Xxjyz { get; set; } = "";
        /// <summary>
        /// 信息服务证
        /// </summary>
        public string Xxfwz { get; set; } = "";
        /// <summary>
        /// ICP编号
        /// </summary>
        public string ICP { get; set; } = "";
        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; } = "";
        /// <summary>
        /// logo图片
        /// </summary>
        public string Img_Logo { get; set; } = "";
        /// <summary>
        /// app下载图
        /// </summary>
        public string Img_App { get; set; } = "";
        /// <summary>
        /// 左侧图
        /// </summary>
        public string Img_Left { get; set; } = "";
        /// <summary>
        /// 右侧图
        /// </summary>
        public string Img_Right { get; set; } = "";
        /// <summary>
        /// 在线客服图
        /// </summary>
        public string Img_Service { get; set; } = "";
        /// <summary>
        /// 在线客户连接
        /// </summary>
        public string Link_Service { get; set; } = "";


    }
}