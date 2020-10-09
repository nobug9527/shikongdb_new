using Sk_B2BAPI.App_Code;
using Sk_B2BAPI.Tool;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace Sk_B2BAPI.TianTaPay
{
    public class TianTaApi
    {
        #region 扫码支付接口
        /// <summary>
        /// 扫码支付接口
        /// </summary>
        /// <param name="tianTaData">提交的参数</param>
        /// <param name="timeOut">超时时间</param>
        /// <returns></returns>
        public static TianTaData ScanPay(TianTaData tianTaData,int timeOut = 6)
        {
            string url = "http://pay.tiantapay.com/scanpay.php/Gateway/scan";
            
            //检测必填参数
            if (!tianTaData.IsSet("money"))//订单金额
            {
                throw new TianTaException("提交扫码支付API接口中，缺少必填参数订单金额【money】！");
            }
            else if (!tianTaData.IsSet("body")) //商品名称
            {
                throw new TianTaException("提交扫码支付API接口中，缺少必填参数商品名称【body】！");
            }
            else if (!tianTaData.IsSet("orderNo")) //订单号
            {
                throw new TianTaException("提交扫码支付API接口中，缺少必填参数订单号【orderNo】！");
            }
            else if (!tianTaData.IsSet("type"))//支付渠道
            {
                throw new TianTaException("提交扫码支付API接口中，缺少必填参数支付渠道【type】！");
            }

            if (!tianTaData.IsSet("user_token"))//商户号
            {
                tianTaData.SetValue("user_token", TianTaConfig.GetConfig().GetMchID());
            }
            if (!tianTaData.IsSet("notifyUrl"))//异步通知url
            {
                if (tianTaData.GetValue("type").ToString()== "WECHAT")
                {
                    tianTaData.SetValue("notifyUrl", TianTaConfig.GetConfig().GetNotifyUrlWeChat());
                }
                else
                {
                    tianTaData.SetValue("notifyUrl", TianTaConfig.GetConfig().GetNotifyUrlAlipay());
                }
                
            }
            if (!tianTaData.IsSet("operatorId"))//店铺编号
            {
                tianTaData.SetValue("operatorId", TianTaConfig.GetConfig().GetOperatorId());
            }
            if (!tianTaData.IsSet("version"))//版本号
            {
                tianTaData.SetValue("version", TianTaConfig.GetConfig().GetVersion());
            }
            tianTaData.SetValue("sign", tianTaData.MakeSign());//签名  
            string param = tianTaData.ToArray();
            //LogQueue.Write(LogType.Debug, "甜塔扫码支付String", param);
            string response = Post(param, url, timeOut);//调用HTTP通信接口以提交数据到API
            TianTaData taData = new TianTaData();
            taData.FromJson(response);
            return taData;
        }
        #endregion

        #region 支付状态查询接口
        /// <summary>
        /// 支付状态查询接口
        /// </summary>
        /// <param name="tianTaData">提交的参数</param>
        /// <param name="timeOut">超时时间</param>
        /// <returns></returns>
        public static TianTaData Query(TianTaData tianTaData, int timeOut = 6)
        {
            string url = "http://pay.tiantapay.com/scanpay.php/Gateway/query";
            //检测必填参数
            if (!tianTaData.IsSet("pay_no") && !tianTaData.IsSet("trade_no"))
            {
                throw new TianTaException("订单查询接口中，商户订单号【pay_no】、第三方订单号【trade_no】至少填一个！");
            }
            else if (!tianTaData.IsSet("type"))//支付渠道
            {
                throw new TianTaException("提交扫码支付API接口中，缺少必填参数支付渠道【type】！");
            }

            if (!tianTaData.IsSet("user_token"))//商户号
            {
                tianTaData.SetValue("user_token", TianTaConfig.GetConfig().GetMchID());
            }
            if (!tianTaData.IsSet("version"))//版本号
            {
                tianTaData.SetValue("version", TianTaConfig.GetConfig().GetVersion());
            }
            //LogQueue.Write(LogType.Debug, "参数：", $"第三方订单号:{tianTaData.GetValue("trade_no")},商户订单号:{tianTaData.GetValue("pay_no")}");
            tianTaData.SetValue("sign", tianTaData.MakeSign());//签名
            //LogQueue.Write(LogType.Debug, "签名：", tianTaData.GetValue("sign").ToString());
            string param = tianTaData.ToArray();
            //LogQueue.Write(LogType.Debug, "Post：", param);
            string response = Post(param, url, timeOut);//调用HTTP通信接口以提交数据到API
            TianTaData taData = new TianTaData();
            taData.FromJson(response);

            return taData;
        }
        #endregion

        #region 微信JSAPI支付
        /// <summary>
        /// 微信JSAPI支付
        /// </summary>
        /// <param name="tianTaData">提交的参数</param>
        /// <param name="timeOut">超时时间</param>
        /// <returns></returns>
        public static TianTaData JSAPI(TianTaData tianTaData,int timeOut = 6)
        {
            string url = "http://pay.tiantapay.com/scanpay.php/Gateway/wxjspay";
            //检测必填参数
            if (!tianTaData.IsSet("money"))//订单金额
            {
                throw new TianTaException("提交扫码支付API接口中，缺少必填参数订单金额【money】！");
            }
            else if (!tianTaData.IsSet("body")) //商品名称
            {
                throw new TianTaException("提交扫码支付API接口中，缺少必填参数商品名称【body】！");
            }
            else if (!tianTaData.IsSet("orderNo")) //订单号
            {
                throw new TianTaException("提交扫码支付API接口中，缺少必填参数订单号【orderNo】！");
            }
            else if (!tianTaData.IsSet("type"))//支付渠道
            {
                throw new TianTaException("提交扫码支付API接口中，缺少必填参数支付渠道【type】！");
            }
            else if (!tianTaData.IsSet("openid"))//微信公众号Openid
            {
                throw new TianTaException("提交扫码支付API接口中，缺少必填参数微信公众号OpenId【type】！");
            }

            if (!tianTaData.IsSet("user_token"))//商户号
            {
                tianTaData.SetValue("user_token", TianTaConfig.GetConfig().GetMchID());
            }
            if (!tianTaData.IsSet("notifyUrl"))//异步通知url
            {
                tianTaData.SetValue("notifyUrl", TianTaConfig.GetConfig().GetNotifyUrlWeChat());
            }
            if (!tianTaData.IsSet("operatorId"))//店铺编号
            {
                tianTaData.SetValue("operatorId", TianTaConfig.GetConfig().GetOperatorId());
            }
            if (!tianTaData.IsSet("version"))//版本号
            {
                tianTaData.SetValue("version", TianTaConfig.GetConfig().GetVersion());
            }

            tianTaData.SetValue("sign", tianTaData.MakeSign());//签名  
            string param = tianTaData.ToArray();
            //LogQueue.Write(LogType.Debug, "甜塔扫码支付String", param);
            string response = Post(param, url, timeOut);//调用HTTP通信接口以提交数据到API
            TianTaData taData = new TianTaData();
            taData.FromJson(response);
            return taData;
        }
        #endregion

        #region 退款接口
        /// <summary>
        /// 退款接口
        /// </summary>
        /// <param name="tianTaData">提交的参数</param>
        /// <param name="timeOut">超时时间</param>
        /// <returns></returns>
        public static TianTaData Refund(TianTaData tianTaData,int timeOut = 6)
        {
            string url = "http://pay.tiantapay.com/scanpay.php/Gateway/refund";
            //检测必填参数
            if (!tianTaData.IsSet("pay_no") && !tianTaData.IsSet("trade_no"))
            {
                throw new TianTaException("订单查询接口中，商户订单号【pay_no】、第三方订单号【trade_no】至少填一个！");
            }
            else if (!tianTaData.IsSet("type"))//支付渠道
            {
                throw new TianTaException("提交扫码支付API接口中，缺少必填参数支付渠道【type】！");
            }
            else if (!tianTaData.IsSet("money"))//退款金额
            {
                throw new TianTaException("提交扫码支付API接口中，缺少必填参数退款金额【money】！");
            }
            else if (!tianTaData.IsSet("refund_no"))//商户退款单号
            {
                throw new TianTaException("提交扫码支付API接口中，缺少必填参数退款单号【refund_no】！");
            }

            if (!tianTaData.IsSet("user_token"))//商户号
            {
                tianTaData.SetValue("user_token", TianTaConfig.GetConfig().GetMchID());
            }
            if (!tianTaData.IsSet("version"))//版本号
            {
                tianTaData.SetValue("version", TianTaConfig.GetConfig().GetVersion());
            }

            tianTaData.SetValue("sign", tianTaData.MakeSign());//签名
            string param = tianTaData.ToArray();
            string response = Post(param, url, timeOut);//调用HTTP通信接口以提交数据到API
            TianTaData taData = new TianTaData();
            taData.FromJson(response);

            return taData;
        }
        #endregion

        #region 退款查询接口
        /// <summary>
        /// 退款查询接口
        /// </summary>
        /// <param name="tianTaData">提交的参数</param>
        /// <param name="timeOut">超时时间</param>
        /// <returns></returns>
        public static TianTaData RefundQuery(TianTaData tianTaData,int timeOut = 6)
        {
            string url = "http://pay.tiantapay.com/scanpay.php/Gateway/refund_query";
            //检测必填参数
            if (!tianTaData.IsSet("pay_no") && !tianTaData.IsSet("trade_no"))
            {
                throw new TianTaException("订单查询接口中，商户订单号【pay_no】、第三方订单号【trade_no】至少填一个！");
            }
            else if (!tianTaData.IsSet("type"))//支付渠道
            {
                throw new TianTaException("提交扫码支付API接口中，缺少必填参数支付渠道【type】！");
            }

            if (!tianTaData.IsSet("user_token"))//商户号
            {
                tianTaData.SetValue("user_token", TianTaConfig.GetConfig().GetMchID());
            }
            if (!tianTaData.IsSet("version"))//版本号
            {
                tianTaData.SetValue("version", TianTaConfig.GetConfig().GetVersion());
            }

            tianTaData.SetValue("sign", tianTaData.MakeSign());//签名
            string param = tianTaData.ToArray();
            string response = Post(param, url, timeOut);//调用HTTP通信接口以提交数据到API
            TianTaData taData = new TianTaData();
            taData.FromJson(response);

            return taData;
        }
        #endregion

        #region App支付
        /// <summary>
        /// App支付
        /// </summary>
        /// <param name="tianTaData">提交的参数</param>
        /// <param name="timeOut">超时时间</param>
        /// <returns></returns>
        public static TianTaData AppPay(TianTaData tianTaData, int timeOut = 6)
        {
            string url = "http://pay.tiantapay.com/scanpay.php/Gateway/apppay";

            //检测必填参数
            if (!tianTaData.IsSet("money"))//订单金额
            {
                throw new TianTaException("提交扫码支付API接口中，缺少必填参数订单金额【money】！");
            }
            else if (!tianTaData.IsSet("body")) //商品名称
            {
                throw new TianTaException("提交扫码支付API接口中，缺少必填参数商品名称【body】！");
            }
            else if (!tianTaData.IsSet("orderNo")) //订单号
            {
                throw new TianTaException("提交扫码支付API接口中，缺少必填参数订单号【orderNo】！");
            }
            else if (!tianTaData.IsSet("type"))//支付渠道
            {
                throw new TianTaException("提交扫码支付API接口中，缺少必填参数支付渠道【type】！");
            }

            if (!tianTaData.IsSet("appid"))//AppId
            {
                tianTaData.SetValue("appid", TianTaConfig.GetConfig().GetAppId());
            }
            if (!tianTaData.IsSet("user_token"))//商户号
            {
                tianTaData.SetValue("user_token", TianTaConfig.GetConfig().GetMchID());
            }
            if (!tianTaData.IsSet("notifyUrl"))//异步通知url
            {
                if (tianTaData.GetValue("type").ToString() == "WECHAT")
                {
                    tianTaData.SetValue("notifyUrl", TianTaConfig.GetConfig().GetNotifyUrlWeChat());
                }
                else
                {
                    tianTaData.SetValue("notifyUrl", TianTaConfig.GetConfig().GetNotifyUrlAlipay());
                }

            }
            if (!tianTaData.IsSet("operatorId"))//店铺编号
            {
                tianTaData.SetValue("operatorId", TianTaConfig.GetConfig().GetOperatorId());
            }
            if (!tianTaData.IsSet("version"))//版本号
            {
                tianTaData.SetValue("version", TianTaConfig.GetConfig().GetVersion());
            }
            tianTaData.SetValue("sign", tianTaData.MakeSign());//签名  
            string param = tianTaData.ToArray();
            //LogQueue.Write(LogType.Debug, "甜塔扫码支付String", param);
            string response = Post(param, url, timeOut);//调用HTTP通信接口以提交数据到API
            TianTaData taData = new TianTaData();
            taData.FromJson(response);
            return taData;
        }
        #endregion

        #region Post请求
        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="param">发送数据</param>
        /// <param name="url">发送路径</param>
        /// <param name="timeout">超时时间</param>
        /// <returns></returns>
        private static string Post(string param, string url,int timeout)
        {
            System.GC.Collect();//垃圾回收，回收没有正常关闭的http连接
            string result = "";//返回结果

            HttpWebRequest request = null;
            HttpWebResponse response = null;
            Stream reqStream = null;
            try
            {
                //设置最大连接数
                ServicePointManager.DefaultConnectionLimit = 200;
                //设置请求属性
                request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.Timeout = timeout * 1000;
                //设置POST的数据类型和长度
                request.ContentType = "application/x-www-form-urlencoded";// "application/json";
                byte[] data = System.Text.Encoding.UTF8.GetBytes(param);
                request.ContentLength = data.Length;
                //往服务器写入数据
                reqStream = request.GetRequestStream();
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();
                //获取服务端返回
                response = (HttpWebResponse)request.GetResponse();
                //获取服务端返回数据
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                result = sr.ReadToEnd().Trim();
                sr.Close();
            }
            catch (System.Threading.ThreadAbortException e)
            {
                Log.Error("HttpService", "Thread - caught ThreadAbortException - resetting.");
                Log.Error("Exception message: {0}", e.Message);
                System.Threading.Thread.ResetAbort();
            }
            catch (WebException e)
            {
                Log.Error("HttpService", e.ToString());
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    Log.Error("HttpService", "StatusCode : " + ((HttpWebResponse)e.Response).StatusCode);
                    Log.Error("HttpService", "StatusDescription : " + ((HttpWebResponse)e.Response).StatusDescription);
                }
                throw new TianTaException(e.ToString());
            }
            catch (Exception e)
            {
                Log.Error("HttpService", e.ToString());
                throw new TianTaException(e.ToString());
            }
            finally
            {
                //关闭连接和流
                if (response != null)
                {
                    response.Close();
                }
                if (request != null)
                {
                    request.Abort();
                }
            }
            return result;
        }
        #endregion
    
    }
}