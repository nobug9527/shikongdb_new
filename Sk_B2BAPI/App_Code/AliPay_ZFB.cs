using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Sk_B2BAPI.App_Code
{
    public class AliPay_ZFB
    {
        /// <summary>
        /// 与ASP兼容的MD5加密算法
        /// </summary>
        public static string GetMD5(string s, string _input_charset)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] t = md5.ComputeHash(Encoding.GetEncoding(_input_charset).GetBytes(s));
            StringBuilder sb = new StringBuilder(32);
            for (int i = 0; i < t.Length; i++)
            {
                sb.Append(t[i].ToString("x").PadLeft(2, '0'));
            }
            return sb.ToString();
        }
        public static  string GetMD5(string s)
        {
            /// <summary>
            /// 与ASP兼容的MD5加密算法
            /// </summary>
 
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] t = md5.ComputeHash(Encoding.GetEncoding("utf-8").GetBytes(s));
            StringBuilder sb = new StringBuilder(32);
            for (int i = 0; i < t.Length; i++)
            {
                sb.Append(t[i].ToString("x").PadLeft(2, '0'));
            }
            return sb.ToString();
        }
        /// <summary>
        /// 冒泡排序法
        /// </summary>
        public static string[] BubbleSort(string[] r)
        {
            int i, j; //交换标志 
            string temp;

            bool exchange;

            for (i = 0; i < r.Length; i++) //最多做R.Length-1趟排序 
            {
                exchange = false; //本趟排序开始前，交换标志应为假

                for (j = r.Length - 2; j >= i; j--)
                {
                    if (System.String.CompareOrdinal(r[j + 1], r[j]) < 0)　//交换条件
                    {
                        temp = r[j + 1];
                        r[j + 1] = r[j];
                        r[j] = temp;

                        exchange = true; //发生了交换，故将交换标志置为真 
                    }
                }

                if (!exchange) //本趟排序未发生交换，提前终止算法 
                {
                    break;
                }

            }
            return r;
        }
        public string CreatUrl(string gateway, string service, string partner, string sign_type, string out_trade_no, string subject, string body, string payment_type, string total_fee, string show_url, string seller_email, string key, string return_url, string _input_charset, string notify_url, string extra_common_param = "")
        {
            return CreatUrl(gateway, service, partner, sign_type, out_trade_no, subject, body, payment_type, total_fee, show_url, seller_email, key, return_url, _input_charset, notify_url, "", extra_common_param);
        }
        public string CreatUrl(string gateway, string service, string partner, string sign_type, string out_trade_no, string subject, string body, string payment_type, string total_fee, string show_url, string seller_email, string key, string return_url, string _input_charset, string notify_url, string token, string extra_common_param = "")
        {
            int i;
            string[] Oristr;
            //构造数组；
            if (!string.IsNullOrEmpty(token))
            {
                if (extra_common_param == "COD2MOTOPAY")
                {
                    Oristr = new[]{ 
                    "service="+service, 
                    "partner=" + partner, 
                    "subject=" + subject, 
                    "body=" + body, 
                    "out_trade_no=" + out_trade_no, 
                    "total_fee=" + total_fee, 
                    "show_url=" + show_url,  
                    "payment_type=" + payment_type, 
                    "seller_email=" + seller_email, 
                    "notify_url=" + notify_url,
                    "_input_charset="+_input_charset,          
                    "return_url=" + return_url,
                    "token="+token,
                    "extra_common_param="+extra_common_param
                    };
                }
                else
                {
                    Oristr = new[]{ 
                    "service="+service, 
                    "partner=" + partner, 
                    "subject=" + subject, 
                    "body=" + body, 
                    "out_trade_no=" + out_trade_no, 
                    "total_fee=" +total_fee, 
                    "show_url=" + show_url,  
                    "payment_type=" + payment_type, 
                    "seller_email=" + seller_email, 
                    "notify_url=" + notify_url,
                    "_input_charset="+_input_charset,          
                    "return_url=" + return_url,
                    "token="+token
                    };
                }

            }
            else
            {
                if (extra_common_param == "COD2MOTOPAY")
                {
                    Oristr = new[]{ 
                    "service="+service, 
                    "partner=" + partner, 
                    "subject=" + subject, 
                    "body=" + body, 
                    "out_trade_no=" + out_trade_no, 
                    "total_fee="+total_fee, 
                    "show_url=" + show_url,  
                    "payment_type=" + payment_type, 
                    "seller_email=" + seller_email, 
                    "notify_url=" + notify_url,
                    "_input_charset="+_input_charset,          
                    "return_url=" + return_url,
                    "extra_common_param="+extra_common_param
                    };
                }
                else
                {
                    Oristr = new[]{ 
                    "service="+service, 
                    "partner=" + partner, 
                    "subject=" + subject, 
                    "body=" + body, 
                    "out_trade_no=" + out_trade_no, 
                    "total_fee=" + total_fee, 
                    "show_url=" + show_url,  
                    "payment_type=" + payment_type, 
                    "seller_email=" + seller_email, 
                    "notify_url=" + notify_url,
                    "_input_charset="+_input_charset,          
                    "return_url=" + return_url
                    };
                }
            }
            //进行排序；
            string[] Sortedstr = BubbleSort(Oristr);


            //构造待md5摘要字符串 ；

            StringBuilder prestr = new StringBuilder();

            for (i = 0; i < Sortedstr.Length; i++)
            {
                if (i == Sortedstr.Length - 1)
                {
                    prestr.Append(Sortedstr[i]);
                }
                else
                {
                    prestr.Append(Sortedstr[i] + "&");
                }
            }

            prestr.Append(key);

            //生成Md5摘要；
            string sign = GetMD5(prestr.ToString(), _input_charset);

            //构造支付Url；
            char[] delimiterChars = { '=' };
            StringBuilder parameter = new StringBuilder();
            parameter.Append(gateway);
            for (i = 0; i < Sortedstr.Length; i++)
            {

                parameter.Append(Sortedstr[i].Split(delimiterChars)[0] + "=" + HttpUtility.UrlEncode(Sortedstr[i].Split(delimiterChars)[1]) + "&");
            }

            parameter.Append("sign=" + sign + "&sign_type=" + sign_type);


            //返回支付Url；
            return parameter.ToString();
        }
        public string CreatUrl_MissPayments(string gateway, string service, string partner, string sign_type, string out_trade_no, string key, string _input_charset)
        {
            /// <summary>
            /// created by sunzhizhi 2006.5.21,sunzhizhi@msn.com。
            /// </summary>
            int i;

            //构造数组；
            string[] Oristr ={ 
                "service="+service, 
                "partner=" + partner,
                "out_trade_no=" + out_trade_no,     
                "_input_charset="+_input_charset          
 
                };

            //进行排序；
            string[] Sortedstr = BubbleSort(Oristr);


            //构造待md5摘要字符串 ；

            StringBuilder prestr = new StringBuilder();

            for (i = 0; i < Sortedstr.Length; i++)
            {
                if (i == Sortedstr.Length - 1)
                {
                    prestr.Append(Sortedstr[i]);

                }
                else
                {

                    prestr.Append(Sortedstr[i] + "&");
                }

            }

            prestr.Append(key);

            //生成Md5摘要；
            string sign = GetMD5(prestr.ToString(), _input_charset);

            //构造支付Url；
            char[] delimiterChars = { '=' };
            StringBuilder parameter = new StringBuilder();
            parameter.Append(gateway);
            for (i = 0; i < Sortedstr.Length; i++)
            {

                parameter.Append(Sortedstr[i].Split(delimiterChars)[0] + "=" + HttpUtility.UrlEncode(Sortedstr[i].Split(delimiterChars)[1]) + "&");
            }

            parameter.Append("sign=" + sign + "&sign_type=" + sign_type);


            //返回支付Url；
            return parameter.ToString();
        }
        public static string GetTranSign(string TranData)
        {
            if (string.IsNullOrWhiteSpace(TranData))
                return null;
            string sign = GetMD5(TranData + "alskdjfaow;fjel;asdjf", "utf-8");
            return sign;
        }
        //获取远程服务器ATN结果
        public static String Get_Http(String a_strUrl, int timeout)
        {
            string strResult;
            try
            {
                HttpWebRequest myReq = (HttpWebRequest)HttpWebRequest.Create(a_strUrl);
                myReq.Timeout = timeout;
                HttpWebResponse HttpWResp = (HttpWebResponse)myReq.GetResponse();
                Stream myStream = HttpWResp.GetResponseStream();
                StreamReader sr = new StreamReader(myStream, Encoding.Default);
                StringBuilder strBuilder = new StringBuilder();
                while (-1 != sr.Peek())
                {
                    strBuilder.Append(sr.ReadLine());
                }

                strResult = strBuilder.ToString();
            }
            catch (Exception exp)
            {

                strResult = "错误：" + exp.Message;
            }
            return strResult;
        }
    }
}