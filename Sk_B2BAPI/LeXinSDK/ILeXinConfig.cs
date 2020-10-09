using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.LeXinSDK
{
    public interface ILeXinConfig
    {
        /// <summary>
        /// 用户名(乐信登录账号)
        /// </summary>
        /// <returns>乐信登录账号</returns>
        string GetAccName();
        /// <summary>
        /// 密码(乐信登录密码32位MD5加密后转大写)
        /// </summary>
        /// <returns>乐信登录密码32位MD5加密后转大写</returns>
        string GetAccPassword();
        /// <summary>
        /// 签名(乐信签名)
        /// 在乐信(http://sdk.lx198.com)设置并通过审核的签名
        /// </summary>
        /// <returns>在乐信(http://sdk.lx198.com)设置并通过审核的签名</returns>
        string GetSign();
        /// <summary>
        /// 返回数据类型(类型支持:json/xml/string)
        /// </summary>
        /// <returns></returns>
        string GetDataType();
    }
}