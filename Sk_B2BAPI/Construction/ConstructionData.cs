using Newtonsoft.Json;
using Sk_B2BAPI.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Sk_B2BAPI.Construction
{
    /// <summary>
    /// 建设银行个人网银支付数据类
    /// </summary>
    public class ConstructionData
    {
        public ConstructionData()
        {

        }

        //采用排序的Dictionary的好处是方便对数据包进行签名
        private Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
        /// <summary>
        /// 设置某个字段的值
        /// </summary>
        /// <param name="key">字段名</param>
        /// <param name="value">值</param>
        public void SetValue(string key,object value)
        {
            keyValuePairs[key] = value;
        }

        /// <summary>
        /// 根据字段名获取某个字段的值
        /// </summary>
        /// <param name="key">字段名</param>
        /// <returns>值</returns>
        public object GetValue(string key)
        {
            keyValuePairs.TryGetValue(key, out object o);
            return o;
        }

        /// <summary>
        /// 判断某个字段是否已设置
        /// </summary>
        /// <param name="key">字段名</param>
        /// <returns>若字段key已被设置，则返回true，否则返回false</returns>
        public bool IsSet(string key)
        {
            keyValuePairs.TryGetValue(key, out object o);
            if (null != o)
                return true;
            else
                return false;
        }

        /// <summary>
        /// MAC校验
        /// MAC字段参与摘要运算的字符串及其顺序：
        /// 注意：PUB字段为对应柜台的公钥后30位
        /// MERCHANTID = 123456789 & POSID = 000000000 & BRANCHID = 110000000 & ORDERID = 19991101234 & PAYMENT = 0.01 & CURCODE = 01 & TXCODE = 530550 & REMARK1 = &REMARK2 = &RETURNTYPE = 3 & TIMEOUT = &PUB = 30819d300d06092a864886f70d0108
        /// </summary>
        /// <returns>返回加密字符串</returns>
        public string MACCheck()
        {
            try
            {
                //获取对应柜台的公钥后30位
                string pub = ConstructionConfig.Initialize().GetPub();
                string REMARK1, REMARK2, TIMEOUT, RETURNTYPE;
                //判断是否存在REMARK1
                if (keyValuePairs.Keys.Contains("REMARK1"))
                {
                    REMARK1 = keyValuePairs["REMARK1"].ToString();
                }
                else
                {
                    REMARK1 = "";
                }
                //判断是否存在REMARK2
                if (keyValuePairs.Keys.Contains("REMARK2"))
                {
                    REMARK2 = keyValuePairs["REMARK2"].ToString();
                }
                else
                {
                    REMARK2 = "";
                }
                //判断是否存在TIMEOUT
                if (keyValuePairs.Keys.Contains("TIMEOUT"))
                {
                    TIMEOUT = keyValuePairs["TIMEOUT"].ToString();
                }
                else
                {
                    TIMEOUT = "";
                }
                //判断是否存在RETURNTYPE
                if (keyValuePairs.Keys.Contains("RETURNTYPE"))
                {
                    RETURNTYPE = keyValuePairs["RETURNTYPE"].ToString();
                }
                else
                {
                    RETURNTYPE = "3";
                }
                //拼接加密字符串
                string str = "MERCHANTID=" + keyValuePairs["MERCHANTID"].ToString() + "&POSID=" + keyValuePairs["POSID"].ToString() + "&BRANCHID=" + keyValuePairs["BRANCHID"] + "&ORDERID=" + keyValuePairs["ORDERID"] + "&PAYMENT=" + keyValuePairs["PAYMENT"] + "&CURCODE=" + keyValuePairs["CURCODE"].ToString() + "&TXCODE=" + keyValuePairs["TXCODE"] + "&REMARK1=" + REMARK1 + "&REMARK2=" + REMARK2 + "&RETURNTYPE=" + RETURNTYPE + "&TIMEOUT=" + TIMEOUT + "&PUB=" + pub + "";
                var md5 = MD5.Create();
                var bs = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
                var sb = new StringBuilder();
                foreach (byte b in bs)
                {
                    sb.Append(b.ToString("x2"));
                }
                //所有字符转为大写
                return sb.ToString().ToUpper();
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "ConstrunctionData/MACCheck", ex.Message.ToString());
                throw new Exception(ex.Message.ToString());
            }
        }

        /// <summary>
        /// SortedDictionary格式转为拼接字符串
        /// </summary>
        /// <returns>&拼接后的字符串</returns>
        public string Joint()
        {
            string buff = "";
            foreach (KeyValuePair<string, object> pair in keyValuePairs)
            {
                if (pair.Value == null)
                {
                    LogQueue.Write(LogType.Error, "ConstrunctionData/Joint", "ConstrunctionData内部含有值为null的字段!");
                    throw new ConstructionException("ConstrunctionData内部含有值为null的字段!");
                }
                buff += pair.Key + "=" + pair.Value + "&";
            }
            buff = buff.Trim('&');
            return buff;
        }

        /// <summary>
        /// json字符串转为SortedDictionary格式
        /// </summary>
        /// <param name="json">json字符串</param>
        /// <param name="verify">是否验签 true/false</param>
        /// <returns>SortedDictionary格式</returns>
        public Dictionary<string, object> Dictionary(string json,bool verify=true)
        {
            if (string.IsNullOrEmpty(json))
            {
                LogQueue.Write(LogType.Error, "ConstrunctionData/Dictionary", "ConstrunctionData内部含有值为null的字段!");
                throw new ConstructionException("将空的json串转换为ConstrunctionData不合法!");
            }
            keyValuePairs = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            if (verify)
            {
                try
                {
                    if (!keyValuePairs.Keys.Contains<string>("SIGN"))
                    {
                        return keyValuePairs;
                    }
                    //验签
                    CheckSing();
                }
                catch (Exception ex)
                {
                    throw new ConstructionException(ex.Message);
                }
            }
            return keyValuePairs;
        }

        /// <summary>
        /// 验签
        /// </summary>
        /// <returns></returns>
        private bool CheckSing()
        {
            if (!IsSet("SIGN"))
            {
                LogQueue.Write(LogType.Error, "", "ConstructionData签名不存在");
                throw new ConstructionException("ConstructionData签名不存在");
            }
            else if (string.IsNullOrEmpty(GetValue("SIGN").ToString()))
            {
                LogQueue.Write(LogType.Error, "", "ConstructionData签名存在,但不合法");
                throw new ConstructionException("ConstructionData签名存在,但不合法");
            }
            string returnSing = GetValue("SIGN").ToString();
            //本地生成签名
            string local=MakeSign();
            //比对签名
            if (returnSing==local)
            {
                return true;
            }

            LogQueue.Write(LogType.Error, "", "ConstructionData签名验证错误");
            throw new ConstructionException("ConstructionData签名验证错误");
        }

        /// <summary>
        /// 建行返回结果加密
        /// </summary>
        /// <returns></returns>
        public string MakeSign()
        {
            string str = Joint();
            var md5 = MD5.Create();
            var bs = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
            var sb = new StringBuilder();
            foreach (byte b in bs)
            {
                sb.Append(b.ToString("x2"));
            }
            //所有字符转为大写
            return sb.ToString().ToUpper();
        }
    }
}