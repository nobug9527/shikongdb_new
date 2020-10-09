using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sk_B2BAPI.Tool;

namespace Sk_B2BAPI.AlipayPayAPI
{
    public class AlipayConfigHelper
    {
        //↓↓↓↓↓↓↓↓↓↓请在这里配置您的基本信息↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
        //支付宝网关地址
        public static string serviceUrl = BaseConfiguration.AlipayServiceUrl;

        //应用ID,以2088开头由16位纯数字组成的字符串
        public static string appId = BaseConfiguration.AlipayAppId;

        //开发者私钥，由开发者自己生成
        public static string privateKey = BaseConfiguration.AlipayPrivateKey;

        //支付宝的公钥，由支付宝生成
        public static string alipayPublicKey = BaseConfiguration.AlipayPublicKey;

        //服务器异步通知页面路径，需http://格式的完整路径，不能加?id=123这类自定义参数,必须外网可以正常访问
        public static string notify_url = "";

        //页面跳转同步通知页面路径，需http://格式的完整路径，不能加?id=123这类自定义参数，必须外网可以正常访问
        public static string return_url = "";

        //参数返回格式，只支持json
        public static string format = "json";

        // 调用的接口版本，固定为：1.0
        public static string version = "1.0";

        // 商户生成签名字符串所使用的签名算法类型，目前支持RSA2和RSA，推荐使用RSA2
        public static string signType = "RSA2";

        // 字符编码格式 目前支持utf-8
        public static string charset = "utf-8";

        // false 表示不从文件加载密钥
        public static bool keyFromFile = false;

        //↑↑↑↑↑↑↑↑↑↑请在这里配置您的基本信息↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑
    }
}