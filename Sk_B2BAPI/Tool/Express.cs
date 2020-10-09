using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
namespace Sk_B2BAPI.Tool
{
    public class Express
    {
        private static readonly string DefaultUserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受     
        }

        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="parameters">参数</param>
        /// <param name="charset">编码格式</param>
        /// <returns></returns>
        public static HttpWebResponse CreatePostHttpResponse(string url, IDictionary<string, string> parameters, Encoding charset)
        {
            HttpWebRequest request = null;
            //HTTPSQ请求  
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            request = WebRequest.Create(url) as HttpWebRequest;
            request.ProtocolVersion = HttpVersion.Version10;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.UserAgent = DefaultUserAgent;
            //如果需要POST数据     
            if (!(parameters == null || parameters.Count == 0))
            {
                StringBuilder buffer = new StringBuilder();
                int i = 0;
                foreach (string key in parameters.Keys)
                {
                    if (i > 0)
                    {
                        buffer.AppendFormat("&{0}={1}", key, parameters[key]);
                    }
                    else
                    {
                        buffer.AppendFormat("{0}={1}", key, parameters[key]);
                    }
                    i++;
                }
                byte[] data = charset.GetBytes(buffer.ToString());
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            return request.GetResponse() as HttpWebResponse;
        }

        /// <summary>
        /// 返回物流信息
        /// </summary>
        /// <param name="com">快递名称</param>
        /// <param name="num">快递单号</param>
        /// <returns></returns>
        public static string ReturnKDDate(string com, string num)
        {
            string url = "http://poll.kuaidi100.com/poll/query.do";
            Encoding encoding = Encoding.GetEncoding("utf-8");

            //参数
            string str = num;
            string param = "{\"com\":\"" + com + "\",\"num\":\"" + str + "\",\"from\":\"\",\"to\":\"\"}";
            string customer = BaseConfiguration.ExpressCustomer;
            string key = BaseConfiguration.ExpressKey;
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] InBytes = Encoding.GetEncoding("UTF-8").GetBytes(param + key + customer);
            byte[] OutBytes = md5.ComputeHash(InBytes);
            string OutString = "";
            for (int i = 0; i < OutBytes.Length; i++)
            {
                OutString += OutBytes[i].ToString("x2");
            }
            string sign = OutString.ToUpper();
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("param", param);
            parameters.Add("customer", customer);
            parameters.Add("sign", sign);
            HttpWebResponse response = CreatePostHttpResponse(url, parameters, encoding);
            //打印返回值  
            Stream stream = response.GetResponseStream();   //获取响应的字符串流  
            StreamReader sr = new StreamReader(stream); //创建一个stream读取流  
            string html = sr.ReadToEnd();   //从头读到尾，放到字符串html  
                                            //Console.WriteLine(html);
            return html;
        }

    }
}