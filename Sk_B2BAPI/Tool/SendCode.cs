using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Core.Http;
using Aliyun.Acs.Core.Profile;

namespace Sk_B2BAPI.Tool
{
    /// <summary>
    /// 阿里短信
    /// </summary>
    public class SendCode
    {
        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="accessKeyId">访问密钥，标识用户</param>
        /// <param name="accessSecret">访问密钥，验证用户</param>
        /// <param name="phoneNumber">接受短信的手机号码</param>
        /// <param name="signName">短信签名名称</param>
        /// <param name="TemplateCode">短信模板ID</param>
        /// <param name="code">短信模板参数</param>
        /// <returns></returns>
        public static string SendSms(string accessKeyId,string accessSecret,string phoneNumber,string signName,string TemplateCode,string code)
        {
            var msg = "";
            //注意刚刚下载的AccessKey的excel中的accessKeyId和accessSecret填入
            IClientProfile profile = DefaultProfile.GetProfile("default", "<"+accessKeyId+">", "<"+accessSecret+">");
            DefaultAcsClient client = new DefaultAcsClient(profile);
            CommonRequest request = new CommonRequest();
            request.Method = MethodType.POST;
            request.Domain = "dysmsapi.aliyuncs.com";
            request.Version = "2017-05-25";
            request.Action = "SendSms";
            // request.Protocol = ProtocolType.HTTP;
            request.AddQueryParameters("PhoneNumbers", phoneNumber);
            request.AddQueryParameters("SignName", signName);
            request.AddQueryParameters("TemplateCode", TemplateCode);
            request.AddQueryParameters("TemplateParam", "{\"code\":\"" + code + "\"}");
            // request.Protocol = ProtocolType.HTTP;

            try
            {
                CommonResponse response = client.GetCommonResponse(request);
                msg = System.Text.Encoding.Default.GetString(response.HttpResponse.Content);
            }
            catch (ServerException e)
            {
                msg = e.ErrorMessage;
            }
            catch (ClientException e)
            {
                msg = e.ErrorMessage;
            }
            return msg;
        }
    }
}