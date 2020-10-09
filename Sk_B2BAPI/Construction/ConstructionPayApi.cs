using Sk_B2BAPI.App_Code;
using Sk_B2BAPI.Tool;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;

namespace Sk_B2BAPI.Construction
{
    /// <summary>
    /// 建设银行个人网银支付API
    /// </summary>
    public class ConstructionPayApi
    {
        #region 建设银行个人网银二维码接口
        /// <summary>
        /// 建设银行个人网银二维码接口
        /// </summary>
        /// <param name="construnctionData">提交的参数</param>
        /// <param name="timeOut">超时时间</param>
        /// <returns></returns>
        public static ConstructionData RQCode(ConstructionData construnctionData, int timeOut = 6)
        {
            //第三方商户提交给网银的网关地址：注意用post方式提交参数
            string url = "https://ibsbjstar.ccb.com.cn/CCBIS/ccbMain?CCB_IBSVersion=V6";
            //判断必传参数
            if (!construnctionData.IsSet("ORDERID"))//订单号
            {
                throw new ConstructionException("提交二维码接口中，缺少必填参数订单号【ORDERID】！");
            }
            if (!construnctionData.IsSet("PAYMENT"))//付款金额
            {
                throw new ConstructionException("提交二维码接口中，缺少必填参数付款金额【PAYMENT】！");
            }

            if (!construnctionData.IsSet("MERCHANTID"))//商户代码
            {
                construnctionData.SetValue("MERCHANTID", ConstructionConfig.Initialize().GetMerchantId());
            }
            if (!construnctionData.IsSet("POSID"))//商户柜台代码
            {
                construnctionData.SetValue("POSID", ConstructionConfig.Initialize().GetPosId());
            }
            if (!construnctionData.IsSet("BRANCHID"))//分行代码
            {
                construnctionData.SetValue("BRANCHID", ConstructionConfig.Initialize().GetBranchId());
            }
            if (!construnctionData.IsSet("CURCODE"))//币种
            {
                construnctionData.SetValue("CURCODE", "01");
            }
            if (!construnctionData.IsSet("TXCODE"))//交易码
            {
                construnctionData.SetValue("TXCODE", "530550");
            }
            if (!construnctionData.IsSet("RETURNTYPE"))//返回类型
            {
                construnctionData.SetValue("RETURNTYPE", "3");
            }
            construnctionData.SetValue("MAC", construnctionData.MACCheck());//MAC校验域

            string param = construnctionData.Joint();
            string response = Post(param, url, timeOut);//调用HTTP通信接口以提交数据到API
            ConstructionData data = new ConstructionData();
            data.Dictionary(response);
            return data;
        }
        #endregion

        #region 发送https的Post请求
        /// <summary>
        /// 发送https的Post请求
        /// </summary>
        /// <param name="url">请求路径</param>
        /// <param name="timeOut">超时时间</param>
        /// <returns>返回结果</returns>
        public static ConstructionData HttpsPost(string url, int timeOut = 6)
        {
            string response = Post(url, timeOut);
            ConstructionData data = new ConstructionData();
            data.Dictionary(response);
            return data;
        }
        #endregion

        #region http Post请求
        /// <summary>
        /// http Post请求
        /// </summary>
        /// <param name="param">发送数据</param>
        /// <param name="url">发送Url</param>
        /// <param name="timeOut">超时时间 </param>
        /// <returns></returns>
        private static string Post(string param, string url, int timeOut = 6)
        {
            System.GC.Collect();//垃圾回收，回收没有正常关闭的http连接
            string result = "";//返回结果
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            try
            {
                //设置最大连接数
                ServicePointManager.DefaultConnectionLimit = 200;
                //设置请求属性
                request = (HttpWebRequest)WebRequest.Create(url);
                request.Method="POST";
                request.Timeout = timeOut*1000;
                //设置数据类型及长度
                request.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
                byte[] data = System.Text.Encoding.UTF8.GetBytes(param);
                request.ContentLength = data.Length;
                //往请求服务器写入数据
                Stream reqStream = request.GetRequestStream();
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();
                //获取请求服务器返回
                response = (HttpWebResponse)request.GetResponse();
                //获取请求服务器返回数据
                StreamReader stream = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                result = stream.ReadToEnd().Trim();
                stream.Close();
            }
            catch (System.Threading.ThreadAbortException e)
            {
                LogQueue.Write(LogType.Error, "ConstructionPayApi/http Post", $"http服务线程异常:{e.Message}");
                System.Threading.Thread.ResetAbort();
            }
            catch (WebException e)
            {
                LogQueue.Write(LogType.Error, "ConstructionPayApi/http Post", $"http服务异常:{e.Message}");
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    LogQueue.Write(LogType.Error, "ConstructionPayApi/http Post", $"http服务线程异常,StatusCode:{((HttpWebResponse)e.Response).StatusCode}");
                    LogQueue.Write(LogType.Error, "ConstructionPayApi/http Post", $"http服务线程异常,StatusDescription:{((HttpWebResponse)e.Response).StatusDescription}");
                }
                throw new ConstructionException(e.ToString());
            }
            catch (Exception e)
            {
                LogQueue.Write(LogType.Error, "ConstructionPayApi/http Post", $"异常:{e.Message}");
                throw new ConstructionException(e.ToString());
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

        #region https Post请求
        /// <summary>
        /// https Post请求
        /// </summary>
        /// <param name="url">https请求路径</param>
        /// <param name="timeOut">超时时间</param>
        /// <returns></returns>
        public static string Post(string url, int timeOut = 6)
        {
            System.GC.Collect();//垃圾回收，回收没有正常关闭的http连接
            string result = "";//返回结果
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            try
            {
                //设置最大连接数
                ServicePointManager.DefaultConnectionLimit = 200;
                //设置https验证方式
                if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback =
                            new RemoteCertificateValidationCallback(CheckValidationResult);
                }
                //设置请求属性
                request = WebRequest.Create(url) as HttpWebRequest;
                request.Method = "POST";
                request.Timeout = timeOut * 1000;
                //设置数据类型
                request.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
                
                //获取请求服务器返回
                response = request.GetResponse() as HttpWebResponse;
                //获取请求服务器返回数据
                StreamReader stream = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                result = stream.ReadToEnd().Trim();
                stream.Close();
            }
            catch (System.Threading.ThreadAbortException e)
            {
                LogQueue.Write(LogType.Error, "ConstructionPayApi/https Post", $"http服务线程异常:{e.Message}");
                System.Threading.Thread.ResetAbort();
            }
            catch (WebException e)
            {
                LogQueue.Write(LogType.Error, "ConstructionPayApi/https Post", $"http服务异常:{e.Message}");
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    LogQueue.Write(LogType.Error, "ConstructionPayApi/https Post", $"http服务线程异常,StatusCode:{((HttpWebResponse)e.Response).StatusCode}");
                    LogQueue.Write(LogType.Error, "ConstructionPayApi/https Post", $"http服务线程异常,StatusDescription:{((HttpWebResponse)e.Response).StatusDescription}");
                }
                throw new ConstructionException(e.ToString());
            }
            catch (Exception e)
            {
                LogQueue.Write(LogType.Error, "ConstructionPayApi/https Post", $"异常:{e.Message}");
                throw new ConstructionException(e.ToString());
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

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            //直接确认，否则打不开    
            return true;
        }
        #endregion
    }
}