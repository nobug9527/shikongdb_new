using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.Models
{
    /// <summary>
    /// 接口返回值模型
    /// </summary>
    public class ResponseModel<T>
    {
        /// <summary>
        /// 返回值编码
        /// </summary>
        public EReturnCode Code { set; get; }

        /// <summary>
        /// 返回值说明
        /// </summary>
        public string Message { get; set; } = "";
        /// <summary>
        /// 分页用，每页个数
        /// </summary>
        public int PageSize { set; get; }
        /// <summary>
        /// 分页用，当前页码
        /// </summary>
        public int PageIndex { set; get; }
        /// <summary>
        /// 记录总数
        /// </summary>
        public int TotalCount { set; get; }
        /// <summary>
        /// 返回的数据
        /// </summary>
        public T Source { set; get; }
    }
}