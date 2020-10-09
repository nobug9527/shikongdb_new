using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Xml;
using LitJson;
using Newtonsoft.Json;
using Sk_B2BAPI.App_Code;
using Sk_B2BAPI.Tool;
using Sk_B2BAPI.WxPayAPI;

namespace Sk_B2BAPI.TianTaPay
{
    public class TianTaData
    {
        public const string SIGN_TYPE_MD5 = "MD5";
        public const string SIGN_TYPE_HMAC_SHA256 = "HMAC-SHA256";
        public TianTaData()
        {

        }
        //采用排序的Dictionary的好处是方便对数据包进行签名，不用再签名之前再做一次排序
        private SortedDictionary<string, object> valuePairs = new SortedDictionary<string, object>();
        /// <summary>
        ///  设置某个字段的值
        /// </summary>
        /// <param name="key">字段名</param>
        /// <param name="value">字段值</param>
        public void SetValue(string key, object value)
        {
            valuePairs[key] = value;
        }
        
        /// <summary>
        /// 根据字段名获取某个字段的值
        /// </summary>
        /// <param name="key">字段名</param>
        /// <returns>key对应的字段值</returns>
        public object GetValue(string key)
        {
            object o = null;
            valuePairs.TryGetValue(key, out o);
            return o;
        }
        
        /// <summary>
        /// 判断某个字段是否已设置
        /// </summary>
        /// <param name="key">字段名</param>
        /// <returns>若字段key已被设置，则返回true，否则返回false</returns>
        public bool IsSet(string key)
        {
            object o = null;
            valuePairs.TryGetValue(key, out o);
            if (null != o)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Dictionary格式转化成url参数格式
        /// </summary>
        /// <returns>url格式串, 该串不包含sign字段值</returns>
        public string ToUrl()
        {
            string buff = "";
            foreach (KeyValuePair<string, object> pair in valuePairs)
            {
                if (pair.Value == null)
                {
                    Log.Error(this.GetType().ToString(), "TianTaData内部含有值为null的字段!");
                    throw new TianTaException("TianTaData内部含有值为null的字段!");
                }

                if (pair.Key != "sign" && pair.Value.ToString() != "")
                {
                    buff += pair.Key + "=" + pair.Value + "&";
                }
            }
            buff = buff.Trim('&');
            return buff;
        }

        public SortedDictionary<string, object> FromUrl(string url)
        {
            //LogQueue.Write(LogType.Debug, "FromUrl", "进入");
            if (string.IsNullOrEmpty(url))
            {
                Log.Error(this.GetType().ToString(), "将空的url串转换为TianTaData不合法!");
                throw new TianTaException("将空的url串转换为TianTaData不合法!");
            }
            string[] keyvalues = url.Split('&');
            foreach (string item in keyvalues)
            {
                string[] vs = item.Split('=');
                valuePairs[vs[0]] = vs[1];
                //LogQueue.Write(LogType.Debug, vs[0], vs[1]);
            }
            return valuePairs;
        }

        /// <summary>
        /// Dictionary格式化成Json
        /// </summary>
        /// <returns>json串数据</returns>
        public string ToJson()
        {
            //string jsonStr = JsonMapper.ToJson(valuePairs);
            string jsonStr = JsonConvert.SerializeObject(valuePairs);
            return jsonStr;
        }

        /// <summary>
        /// Dictionary格式化成&拼接
        /// </summary>
        /// <returns>字符</returns>
        public string ToArray()
        {
            string buff = "";
            foreach (KeyValuePair<string, object> pair in valuePairs)
            {
                if (pair.Value == null)
                {
                    Log.Error(this.GetType().ToString(), "TianTaData内部含有值为null的字段!");
                    throw new TianTaException("TianTaData内部含有值为null的字段!");
                }
                buff += pair.Key + "=" + pair.Value + "&";
            }
            buff = buff.Trim('&');
            return buff;
        }

        /// <summary>
        /// Json格式化成Dictionary
        /// </summary>
        /// <param name="json">Json字符串</param>
        /// <returns>Dictionary对象</returns>
        public SortedDictionary<string, object> FromJson(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                Log.Error(this.GetType().ToString(), "将空的json串转换为TianTaData不合法!");
                throw new TianTaException("将空的json串转换为TianTaData不合法!");
            }
            return valuePairs = JsonConvert.DeserializeObject<SortedDictionary<string,object>>(json);
        }

        /// <summary>
        /// 生成签名，详见签名生成算法
        /// </summary>
        /// <returns>签名, sign字段不参加签名</returns>
        public string MakeSign()
        {
            return MakeSign(SIGN_TYPE_MD5);
        }

        /// <summary>
        /// 生成签名
        /// </summary>
        /// <param name="key">支付密钥</param>
        /// <returns></returns>
        public string MakeSignByKey(string key)
        {
            return MakeSing(key,SIGN_TYPE_MD5);
        }

        /// <summary>
        /// 生成时间戳，标准北京时间，时区为东八区，自1970年1月1日 0点0分0秒以来的秒数
        /// </summary>
        /// <returns>时间戳</returns>
        public static string GenerateTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        /// <summary>
        /// 签名生成算法
        /// </summary>
        /// <param name="signType">签名方式</param>
        /// <returns>签名, sign字段不参加签名</returns>
        public string MakeSign(string signType)
        {
            //转url格式
            string str = ToUrl();
            //在string后加入API KEY
            str +=TianTaConfig.GetConfig().GetKey();
            //LogQueue.Write(LogType.Debug, "签名前参数：", str);
            if (signType == SIGN_TYPE_MD5)
            {
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
            else if (signType == SIGN_TYPE_HMAC_SHA256)
            {
                return CalcHMACSHA256Hash(str, TianTaConfig.GetConfig().GetKey());
            }
            else
            {
                throw new TianTaException("sign_type 不合法");
            }
        }
        /// <summary>
        /// 签名生成算法
        /// </summary>
        /// <param name="key">支付密钥</param>
        /// <param name="signType">签名方式</param>
        /// <returns></returns>
        public string MakeSing(string key,string signType)
        {
            //转url格式
            string str = ToUrl();
            //LogQueue.Write(LogType.Debug, "url：", str);
            //在string后加入API KEY
            str +="&key="+key;
            //LogQueue.Write(LogType.Debug, "urlkey：", str);
            if (signType == SIGN_TYPE_MD5)
            {
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
            else if (signType == SIGN_TYPE_HMAC_SHA256)
            {
                return CalcHMACSHA256Hash(str, TianTaConfig.GetConfig().GetKey());
            }
            else
            {
                throw new TianTaException("sign_type 不合法");
            }
        }
        /// <summary>
        /// HMAC-SHA256加密
        /// </summary>
        /// <param name="plaintext">需加密的字串</param>
        /// <param name="salt">密钥</param>
        /// <returns></returns>
        private string CalcHMACSHA256Hash(string plaintext, string salt)
        {
            string result = "";
            var enc = Encoding.Default;
            byte[]
            baText2BeHashed = enc.GetBytes(plaintext),
            baSalt = enc.GetBytes(salt);
            System.Security.Cryptography.HMACSHA256 hasher = new HMACSHA256(baSalt);
            byte[] baHashedText = hasher.ComputeHash(baText2BeHashed);
            result = string.Join("", baHashedText.ToList().Select(b => b.ToString("x2")).ToArray());
            return result;
        }
    }
}