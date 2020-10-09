using System;
using System.Configuration;
using System.IO;
using System.Xml;


namespace Sk_B2BAPI.Tool
{
    public sealed class ConfigHelper
    {
        #region 自定义config
        private readonly string _filePath;
        /// <summary>
        /// 用户指定具体的配置文件路径
        /// </summary>
        /// <param name="configFilePath">配置文件路径（绝对路径）</param>
        public ConfigHelper(string configFilePath)
        {
            string webconfig = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configFilePath);

            if (File.Exists(webconfig))
            {
                _filePath = webconfig;
            }
            else
            {
                throw new ArgumentNullException($"没有找到Web.Config文件或者应用程序配置文件, 请指定配置文件");
            }
        }

        /// <summary>
        /// 设置程序的config文件
        /// </summary>
        /// <param name="keyName">键名</param>
        /// <param name="keyValue">键值</param>
        public void AppConfigSet(string keyName, string keyValue)
        {
            var document = new XmlDocument();
            document.Load(_filePath);

            XmlNodeList nodes = document.GetElementsByTagName("add");
            for (var i = 0; i < nodes.Count; i++)
            {
                //获得将当前元素的key属性
                var xmlAttributeCollection = nodes[i].Attributes;
                XmlAttribute attribute = xmlAttributeCollection?["key"];
                //根据元素的第一个属性来判断当前的元素是不是目标元素
                if (attribute != null && (attribute.Value == keyName))
                {
                    attribute = xmlAttributeCollection["value"];
                    //对目标元素中的第二个属性赋值
                    if (attribute != null)
                    {
                        attribute.Value = keyValue;
                        break;
                    }
                }
            }
            document.Save(_filePath);
        }

        /// <summary>
        /// 读取程序的config文件的键值。
        /// 如果键名不存在，返回空
        /// </summary>
        /// <param name="keyName">键名</param>
        /// <returns>键值</returns>
        public string AppConfigGet(string keyName)
        {
            string strReturn = string.Empty;
            try
            {
                var document = new XmlDocument();
                document.Load(_filePath);

                XmlNodeList nodes = document.GetElementsByTagName("add");
                for (var i = 0; i < nodes.Count; i++)
                {
                    //获得将当前元素的key属性
                    var xmlAttributeCollection = nodes[i].Attributes;
                    XmlAttribute attribute = xmlAttributeCollection?["key"];
                    //根据元素的第一个属性来判断当前的元素是不是目标元素
                    if (attribute != null && (attribute.Value == keyName))
                    {
                        attribute = xmlAttributeCollection["value"];
                        if (attribute != null)
                        {
                            strReturn = attribute.Value;
                            break;
                        }
                    }
                }
            }
            catch (Exception)
            {
                return "";
            }

            return strReturn;
        }

        /// <summary>
        /// 获取指定键名中的子项的值
        /// </summary>
        /// <param name="keyName">键名</param>
        /// <param name="subKeyName">以分号(;)为分隔符的子项名称</param>
        /// <returns>对应子项名称的值（即是=号后面的值）</returns>
        public string GetSubValue(string keyName, string subKeyName)
        {
            string connectionString = AppConfigGet(keyName).ToLower();
            string[] item = connectionString.Split(new[] { ';' });

            foreach (string t in item)
            {
                string itemValue = t.ToLower();
                if (itemValue.IndexOf(subKeyName.ToLower(), StringComparison.Ordinal) >= 0) //如果含有指定的关键字
                {
                    int startIndex = t.IndexOf("=", StringComparison.Ordinal); //等号开始的位置
                    return t.Substring(startIndex, 1); //获取等号后面的值即为Value
                }
            }
            return string.Empty;
        }
        #endregion

        #region 获取AppSettings中的配置字符串信息
        /// <summary>
        /// 得到AppSettings中的配置字符串信息
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        public static string GetConfigString(string key)
        {
            string cacheKey = "AppSettings-" + key;
            object objModel = CacheHelper.GetCache(cacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = ConfigurationManager.AppSettings[key];
                    if (objModel != null)
                    {
                        CacheHelper.SetCache(cacheKey, objModel, DateTime.Now.AddMinutes(180), TimeSpan.Zero);
                    }
                }
                catch
                {
                    return "";
                }
            }
            return objModel?.ToString();
        }
        #endregion

        #region 获取AppSettings中的配置Bool信息
        /// <summary>
        /// 得到AppSettings中的配置Bool信息
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        public static bool GetConfigBool(string key)
        {
            bool result = false;
            string cfgVal = GetConfigString(key);
            if (!string.IsNullOrEmpty(cfgVal))
            {
                try
                {
                    result = bool.Parse(cfgVal);
                }
                catch (FormatException)
                {
                    result = false;
                }
            }
            return result;
        }
        #endregion

        #region 获取AppSettings中的配置Decimal信息
        /// <summary>
        /// 得到AppSettings中的配置Decimal信息
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        public static decimal GetConfigDecimal(string key)
        {
            decimal result = 0;
            string cfgVal = GetConfigString(key);
            if (!string.IsNullOrEmpty(cfgVal))
            {
                try
                {
                    result = decimal.Parse(cfgVal);
                }
                catch (FormatException)
                {
                    result = 0;
                }
            }

            return result;
        }
        #endregion

        #region 获取AppSettings中的配置int信息
        /// <summary>
        /// 得到AppSettings中的配置int信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int GetConfigInt(string key)
        {
            int result = 0;
            string cfgVal = GetConfigString(key);
            if (!string.IsNullOrEmpty(cfgVal))
            {
                try
                {
                    result = int.Parse(cfgVal);
                }
                catch (FormatException)
                {
                    result = 0;
                }
            }

            return result;
        }
        #endregion

    }
}
