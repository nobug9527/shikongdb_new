using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.Models
{
    public class ValetOrderModel<T>
    {
        public string Success { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// 页容
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount { get; set; }
        /// <summary>
        /// 总条目数
        /// </summary>
        public int RecordCount { get; set; }
        /// <summary>
        /// 数据集
        /// </summary>
        public List<T> Data { get; set; }
    }
    public class ValetOrder_MemberList
    {
        /// <summary>
        /// 会员Id
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 登陆账号
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 用户姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 客户Id
        /// </summary>
        public string BusinessId { get; set; }
        /// <summary>
        /// 客户编号
        /// </summary>
        public string BusinessCode { get; set; }
        /// <summary>
        /// 客户名称
        /// </summary>
        public string BusinessName { get; set; }
        /// <summary>
        /// 购物车商品条目
        /// </summary>
        public string CountNumber { get; set; }
        /// <summary>
        /// 购物车商品总数
        /// </summary>
        public string TotalNumber { get; set; }


    }
}