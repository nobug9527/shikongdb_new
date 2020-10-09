using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.AlipayPayAPI
{
    public class ResultCode
    {
        /// <summary>
        /// 接口调用成功
        /// </summary>
        public const string SUCCESS = "10000";

        public const string INRROCESS = "10003";
        /// <summary>
        /// 业务处理失败:	具体失败原因参见接口返回的错误码    
        /// </summary>
        public const string FAIL = "40004";
        /// <summary>
        /// 业务出现未知错误或者系统异常: 如果支付接口返回，需要调用查询接口确认订单状态或者发起撤销
        /// 服务不可用
        /// </summary>
        public const string ERROR = "20000";
        /// <summary>
        /// 非法的参数
        /// </summary>
        public const string LLLEGALITY = "40002";
        /// <summary>
        /// 授权权限不足
        /// </summary>
        public const string AUTHORIZATION = "20001";
        /// <summary>
        /// 缺少必选参数
        /// </summary>
        public const string PARAMETER = "40001";
        /// <summary>
        /// 权限不足
        /// </summary>
        public const string AUTHORITY = "40006";
    }
}