using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.Models
{
    public class JsonStr
    {
        /// <summary>
        /// 信息码
        /// </summary>
        public ReturnCode Msg_code { get; set; }
        /// <summary>
        /// 信息内容
        /// </summary>
        public string Msg_info { get; set; }
        public string jsonresult { get; set; }

        public string GetJonStr()
        {
            long s = (long)Msg_code;
            jsonresult = "{\"Code\"" + ":" + "\"" + s + "\"" + "," +
                    "\"Result" + "\"" + ":" + Msg_info +
                    "}";
            return jsonresult;
        }
    }

    public enum ReturnCode : long
    {
        /// <summary>
        /// 参数错误
        /// </summary>
        paramater_error = 4001,//参数错误
        paramater_none = 4003,
        paramater_text_error = 4002,//参数格式错误
        /// <summary>
        /// 服务器异常
        /// </summary>
        server_error = 5001,//服务器异常
        /// <summary>
        /// 无结果
        /// </summary>
        null_result = 4000,//无结果
        /// <summary>
        /// 正常通过
        /// </summary>
        pass = 0//正常通过
    }
}