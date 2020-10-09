using Sk_B2BAPI.App_Code;
using Sk_B2BAPI.Tool;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace Sk_B2BAPI.LeXinSDK
{
    /// <summary>
    /// 乐信API
    /// </summary>
    public class LeXinApi
    {
        #region 发送短信
        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="leXinData">提交给短信API的参数</param>
        /// <param name="receipt">是否推送短信回执 true/false</param>
        /// <param name="timeOut">超时时间</param>
        /// <returns>成功时返回调用结果，其他抛异常</returns>
        public LeXinData Send(LeXinData leXinData,bool receipt=false, int timeOut = 6)
        {
            string url = "http://sdk.lx198.com/sdk/send";
            //检测必填参数
            if (leXinData.IsSet("aimcodes"))//手机号码(多个手机号码之间用英文半角“,”隔开,单次最多支持5000个号码)
            {
                throw new LeXinException("提交发送短信API接口中，缺少必填参数手机号码【aimcodes】！");
            }
            else if (leXinData.IsSet("content")) //内容(内容长度请参照乐信(http://sdk.lx198.com)发送短信页面提示)，注意：在内容的最后需要加上在乐信(http://sdk.lx198.com)设置并通过审核的签名。如：公司定于1月25日召开今年的企业年会，请务必安排好时间。【动力思维】
            {
                throw new LeXinException("提交发送短信API接口中，缺少必填参数内容【content】！");
            }
            else if (receipt)
            {
                if (leXinData.IsSet("msgId"))//提交短信包的唯一id(15位以内数字)，推送短信回执时，会推送此值，用此值和手机号码来匹配短信的状态，如需要接受回执则必须提交此参数,单次提交只需要提交一个即可
                {
                    throw new LeXinException("提交发送短信API接口中，缺少必填短信包唯一id【msgId】！");
                }
            }

            if (leXinData.IsSet("accName"))//用户名(乐信登录账号)
            {
                leXinData.SetValue("accName", LeXinConfig.GetLeXinConfig().GetAccName());
            }
            if (leXinData.IsSet("accPwd"))//密码(乐信登录密码32位MD5加密后转大写，如123456加密完以后为：E10ADC3949BA59ABBE56E057F20F883E)
            {
                leXinData.SetValue("accPwd", LeXinConfig.GetLeXinConfig().GetAccPassword());
            }
            if (leXinData.IsSet("schTime")) //定时时间(格式为: 如为空则为即时短信, 如需定时时间格式为“yyyy - mm - dd hh24: mi:ss”。如果是GET提交，需要 转码)
            {
                leXinData.SetValue("schTime", "");
            }
            if (leXinData.IsSet("dataType"))//返回的数据(类型支持:json/xml/string 三种形式 不传默认string)
            {
                leXinData.SetValue("dataType", LeXinConfig.GetLeXinConfig().GetDataType());
            }

            leXinData.SetValue("content",leXinData.AppendSign(leXinData.GetValue("content").ToString()));
            string xml = leXinData.ToXml();

            string response = Post(xml, url, timeOut);//调用HTTP通信接口以提交数据到API

            //将xml格式的结果转换为对象以返回
            LeXinData result = new LeXinData();
            result.FromXml(response);
            return result;

        }
        #endregion

        #region Post请求
        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="xml">发送数据</param>
        /// <param name="url">发送路径</param>
        /// <param name="timeout">超时时间</param>
        /// <returns></returns>
        private static string Post(string xml, string url, int timeout)
        {
            System.GC.Collect();//垃圾回收，回收没有正常关闭的http连接
            string result = "";//返回结果

            HttpWebRequest request = null;
            HttpWebResponse response = null;
            Stream reqStream = null;
            try
            {
                //设置请求属性
                request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.Timeout = timeout * 1000;
                //设置POST的数据类型和长度
                request.ContentType = "text/xml";
                byte[] data = System.Text.Encoding.UTF8.GetBytes(xml);
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
                throw new LeXinException(e.ToString());
            }
            catch (Exception e)
            {
                Log.Error("HttpService", e.ToString());
                throw new LeXinException(e.ToString());
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