using Sk_B2BAPI.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace Sk_B2BAPI.LeXinSDK
{
    public class LeXinData
    {
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
        /// 追加签名，详见签名生成算法
        /// </summary>
        /// <returns>追加签名后内容</returns>
        public string AppendSign(string content)
        {
            return MakeSign(content);
        }

        /// <summary>
        /// 签名生成算法
        /// </summary>
        /// <param name="signType">签名方式</param>
        /// <returns>签名, sign字段不参加签名</returns>
        public string MakeSign(string content)
        {
            return content+="【"+LeXinConfig.GetLeXinConfig().GetSign()+"】";
        }

        /// <summary>
        /// 将Dictionary转成xml
        /// </summary>
        /// <returns>转换得到的xml串</returns>
        public string ToXml()
        {
            //数据为空时不能转化为xml格式
            if (0 == valuePairs.Count)
            {
                Log.Error(this.GetType().ToString(), "LeXinData数据为空!");
                throw new LeXinException("LeXinData数据为空!");
            }

            string xml = "<xml>";
            foreach (KeyValuePair<string, object> pair in valuePairs)
            {
                //字段值不能为null，会影响后续流程
                if (pair.Value == null)
                {
                    Log.Error(this.GetType().ToString(), "LeXinData内部含有值为null的字段!");
                    throw new LeXinException("LeXinData内部含有值为null的字段!");
                }

                if (pair.Value.GetType() == typeof(int))
                {
                    xml += "<" + pair.Key + ">" + pair.Value + "</" + pair.Key + ">";
                }
                else if (pair.Value.GetType() == typeof(string))
                {
                    xml += "<" + pair.Key + ">" + "<![CDATA[" + pair.Value + "]]></" + pair.Key + ">";
                }
                else//除了string和int类型不能含有其他数据类型
                {
                    Log.Error(this.GetType().ToString(), "LeXinData字段数据类型错误!");
                    throw new LeXinException("LeXinData字段数据类型错误!");
                }
            }
            xml += "</xml>";
            return xml;
        }

        /// <summary>
        /// 将xml转为WxPayData对象并返回对象内部的数据
        /// </summary>
        /// <param name="xml">需要转换的xml</param>
        /// <param name="verify">是否进行签名校验（true/false）</param>
        /// <returns></returns>
        public SortedDictionary<string, object> FromXml(string xml, bool verify = true)
        {
            if (string.IsNullOrEmpty(xml))
            {
                Log.Error(this.GetType().ToString(), "将空的xml串转换为LeXinData不合法!");
                throw new LeXinException("将空的xml串转换为WxPayData不合法!");
            }


            NoteSafeXmlDocument xmlDoc = new NoteSafeXmlDocument();
            xmlDoc.LoadXml(xml);
            XmlNode xmlNode = xmlDoc.FirstChild;//获取到根节点<xml>
            XmlNodeList nodes = xmlNode.ChildNodes;
            foreach (XmlNode xn in nodes)
            {
                XmlElement xe = (XmlElement)xn;
                valuePairs[xe.Name] = xe.InnerText;//获取xml的键值对到WxPayData内部的数据中
            }

            return valuePairs;
        }
    }
}