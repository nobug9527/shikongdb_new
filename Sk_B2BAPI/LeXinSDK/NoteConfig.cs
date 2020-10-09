using DTcms.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Sk_B2BAPI.Tool;

namespace Sk_B2BAPI.LeXinSDK
{
    public class NoteConfig:ILeXinConfig
    {
        public NoteConfig()
        {

        }
        /// <summary>
        /// 用户名(乐信登录账号)
        /// </summary>
        /// <returns>乐信登录账号</returns>
        public string GetAccName()
        {
            return BaseConfiguration.AccName;
        }
        /// <summary>
        /// 密码(乐信登录密码32位MD5加密后转大写)
        /// </summary>
        /// <returns>乐信登录密码32位MD5加密后转大写</returns>
        public string GetAccPassword()
        {
            return BaseConfiguration.AccPassword;
        }
        /// <summary>
        /// 签名(乐信签名)
        /// 在乐信(http://sdk.lx198.com)设置并通过审核的签名
        /// </summary>
        /// <returns></returns>
        public string GetSign()
        {
            return BaseConfiguration.Sign;
        }
        /// <summary>
        /// 返回数据类型(类型支持:json/xml/string),默认xml
        /// </summary>
        /// <returns>json</returns>
        public string GetDataType()
        {
            return "xml";
        }

        /// <summary>
        /// MD5字符串加密
        /// </summary>
        /// <param name="txt"></param>
        /// <returns>加密后字符串</returns>
        public static string GenerateMD5(string txt)
        {
            using (MD5 mi = MD5.Create())
            {
                byte[] buffer = Encoding.Default.GetBytes(txt);
                //开始加密
                byte[] newBuffer = mi.ComputeHash(buffer);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < newBuffer.Length; i++)
                {
                    sb.Append(newBuffer[i].ToString("x2"));
                }
                return sb.ToString().ToUpper();
            }
        }
    }
}