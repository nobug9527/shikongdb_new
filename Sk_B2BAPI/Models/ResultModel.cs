using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.Models
{
    public class ResultModel<T>
    {
        /// <summary>
        /// 请求状态说明
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// 返回值说明
        /// </summary>
        public string Message { get; set; } = "";
        /// <summary>
        /// 返回的数据
        /// </summary>
        public T Source { set; get; }
    }
    public class ApliayResult<T>:ResultModel<T>
    {
        public string orderNo { get; set; } = "";
        public decimal fee { get; set; } = 0;
        public string initiationTime { get; set; } = "";
    }
}