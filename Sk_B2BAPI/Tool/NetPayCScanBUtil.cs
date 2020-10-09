using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Sk_B2BAPI.Tool
{
    /// <summary>
    /// 银联云闪付工具类
    /// </summary>
    public class NetPayCScanBUtil
    {
        #region SHA256加密
        public static string MakeSignSHA256(Dictionary<string, object> requestParams, string md5Key)
        {
            string preString = BulidSignString(requestParams);
            string inputStr = (preString + md5Key);
            //如果str有中文，不同Encoding的sha是不同的！！
            byte[] SHA256Data = Encoding.UTF8.GetBytes(inputStr);

            SHA256Managed Sha256 = new SHA256Managed();
            byte[] by = Sha256.ComputeHash(SHA256Data);
            string result= BitConverter.ToString(by).Replace("-", "").ToUpper();
            return result;
        }
        #endregion

        #region MD5加密
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="encypStr"></param>
        /// <param name="charset"></param>
        /// <returns></returns>
        public static string MakeSignMd5(Dictionary<string, object> requestParams, string md5Key, string charset = "UTF-8")
        {
            string preString = BulidSignString(requestParams);
            string text = preString + md5Key;
            string sign;
            MD5CryptoServiceProvider m5 = new MD5CryptoServiceProvider();

            //创建md5对象
            byte[] inputBye;
            byte[] outputBye;

            //使用GB2312编码方式把字符串转化为字节数组．
            try
            {
                inputBye = Encoding.GetEncoding(charset).GetBytes(text);
            }
            catch
            {
                inputBye = Encoding.GetEncoding("GB2312").GetBytes(text);
            }
            outputBye = m5.ComputeHash(inputBye);

            sign = System.BitConverter.ToString(outputBye);
            sign = sign.Replace("-", "").ToUpper();
            return sign;
        }
        #endregion

        /// <summary>
        /// 数据签名验证
        /// </summary>
        /// <param name="responseData"></param>
        /// <param name="md5Key"></param>
        /// <param name="charset"></param>
        /// <returns></returns>
        public static bool CheckSign(Dictionary<string, object> responseData, string md5Key, string charset = "UTF-8")
        {
            try
            {
                string responseSign = (responseData["sign"]).ToString();

                if (string.IsNullOrEmpty(responseSign))
                    return false;

                responseData.Remove("sign");

                if (string.Equals(responseSign, MakeSignSHA256(responseData, md5Key)))
                    return true;
            }
            catch (Exception)
            {
                return false;
            }
            return false;
        }

        /// <summary>
        /// 生成get请求下单数据
        /// </summary>
        /// <param name="requestParams"></param>
        /// <param name="apiUrl"></param>
        /// <param name="md5Key"></param>
        /// <returns>返回下单数据</returns>
        public static string MakeOrderRequest(Dictionary<string, object> requestParams, string apiUrl, string md5Key)
        {
            requestParams.Add("sign", MakeSignSHA256(requestParams, md5Key));

            return apiUrl + "?" + BuildUrlParametersStr(requestParams);
        }

        /// <summary>
        /// 生成待签名数据
        /// </summary>
        /// <param name="requestParams">下单请求数据</param>
        /// <returns>排序好的待签名数据</returns> 
        public static string BulidSignString(Dictionary<string, object> requestParams)
        {
            if (requestParams == null || requestParams.Count == 0)
                return "";
            List<string> list = new List<string>(requestParams.Keys);
            list.Sort(StringComparer.Ordinal);

            StringBuilder sb = new StringBuilder();
            foreach (string key in list)
            {
                string value = (requestParams[key]).ToString().Replace("\r","").Replace("\n","");
                if (key == "appPayRequest")
                    value = value.Replace(" ", "");
                if (!string.IsNullOrEmpty(value))
                {
                    sb.Append(key).Append("=").Append(value).Append("&");
                }
            }
            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }

            return sb.ToString();
        }

        /// <summary>
        /// 将请求数据进行URL编码
        /// </summary>
        /// <param name="requestParams"></param>
        /// <returns>UrlEncode后的数据</returns>
        public static string BuildUrlParametersStr(Dictionary<string, object> requestParams)
        {
            if (requestParams == null || requestParams.Count == 0)
                return "";

            StringBuilder sb = new StringBuilder();
            foreach (var requestData in requestParams)
            {
                string value = (string)requestData.Value;
                if (!string.IsNullOrEmpty(value))
                {
                    try
                    {
                        value = System.Web.HttpUtility.UrlEncode(value);
                    }
                    catch (Exception ex)
                    {
                        //LogError(ex);
                        return "#ERROR: BuildUrlParametersStr Error!" + ex.Message;
                    }
                }
                if (!string.IsNullOrEmpty(value))
                {
                    sb.Append(requestData.Key).Append("=").Append(value).Append("&");
                }
            }
            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }

            return sb.ToString();
        }

        /// <summary>
        /// 发送HTTP POST请求
        /// </summary>
        /// <param name="requestJsonString"></param>
        /// <param name="apiUrl"></param>
        /// <returns>请求返回的数据</returns>
        public static string CreateHttpPostRequest(string requestJsonString, string apiUrl)
        {
            byte[] strReqStrByte = Encoding.UTF8.GetBytes(requestJsonString);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiUrl);
            request.Method = "POST";
            request.Timeout = 60000;
            request.ContentType = "application/json";
            request.ContentLength = strReqStrByte.Length;

            Stream requestStream = request.GetRequestStream();
            requestStream.Write(strReqStrByte, 0, strReqStrByte.Length);
            requestStream.Close();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            return reader.ReadToEnd();
        }
        /// <summary>
        /// 发送HTTP Get请求
        /// </summary>
        /// <param name="apiUrl"></param>
        /// <returns></returns>
        public static string CreateHttpGetRequest(string apiUrl)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiUrl);
            request.Method = "GET";
            request.Timeout = 60000;

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            return reader.ReadToEnd();
        }

        /// <summary>
        /// 账单查询返回数据为两层JsonString，示例:
        /// billDate=2018-10-09&billPayment={
        ///  "payTime": "2018-10-09 15:50:57",
        ///  "totalAmount": 1,
        /// }&msgSrc=WWW.TEST.COM
        /// 
        /// 嵌套的那一层数据即billPaymen直接使用时会有换行符，这会导致验签失败，需要处理下
        /// </summary>
        /// <param name="multiJsonString"></param>
        /// <returns></returns>
        public static Dictionary<string, object> ParseMultiJsonObject(string multiJsonString)
        {
            if (string.IsNullOrWhiteSpace(multiJsonString))
            {
                return null;
            }

            Dictionary<string, object> resultParams = new Dictionary<string, object>();

            try
            {
                Dictionary<string, object> jsonParams = JsonConvert.DeserializeObject<Dictionary<string, object>>(multiJsonString);

                foreach (var item in jsonParams)
                {
                    if (item.Value.GetType() == typeof(Newtonsoft.Json.Linq.JObject))
                    {
                        resultParams.Add(item.Key, JsonConvert.SerializeObject(item.Value));
                    }
                    else
                    {
                        resultParams.Add(item.Key, item.Value.ToString());
                    }
                }
            }
            catch (Exception)
            { }
            return resultParams;
        }
    }
}