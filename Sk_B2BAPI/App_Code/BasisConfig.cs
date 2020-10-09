using Sk_B2BAPI.Tool;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;

namespace Sk_B2BAPI.App_Code
{
    public class BasisConfig
    {
        /// <summary>
        /// 获取配置文件(config)key值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetConfigString(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
        /// <summary>
        /// 修改配置文件Key值
        /// </summary>
        /// <param name="ConnenctionString"></param>
        /// <param name="strKey"></param>
        private void SaveConfig(string ConnenctionString, string strKey)
        {
            XmlDocument doc = new XmlDocument();
            //获得配置文件的全路径  
            string strFileName = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
            // string  strFileName= AppDomain.CurrentDomain.BaseDirectory + "\\exe.config";  
            doc.Load(strFileName);
            //找出名称为“add”的所有元素  
            XmlNodeList nodes = doc.GetElementsByTagName("add");
            for (int i = 0; i < nodes.Count; i++)
            {
                //获得将当前元素的key属性  
                XmlAttribute att = nodes[i].Attributes["key"];
                //根据元素的第一个属性来判断当前的元素是不是目标元素  
                if (att.Value == strKey)
                {
                    //对目标元素中的第二个属性赋值  
                    att = nodes[i].Attributes["value"];
                    att.Value = ConnenctionString;
                    break;
                }
            }
            //保存上面的修改  
            doc.Save(strFileName);
            System.Configuration.ConfigurationManager.RefreshSection("appSettings");
        }
        /// <summary>
        /// object型转换为decimal型
        /// </summary>
        /// <param name="expression">要转换的字符串</param>
        /// <param name="defvalue">缺省值</param>
        /// <param name="type">节点</param>
        /// <returns>转换后的decimal类型结果</returns>
        public static decimal ObjToDecimal(object expression, NodeType type= NodeType.Null, decimal defvalue = 0.00m)
        {
            string place = "0";
            if (type != NodeType.Null)
            {
                //place =convert.toint32(xmloperation.readxml("base", type.getdescription()));
                place = XmlOperation.ReadXml("base", type.GetDescription());
            }
            if (expression != null)
                return StrToDecimal(expression.ToString(), defvalue, place);

            return defvalue;
        }
        /// <summary>
        /// Object型转换为decimal型
        /// </summary>
        /// <param name="expression">要转换的字符串</param>
        /// <param name="place">位数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的decimal类型结果</returns>
        public static decimal ObjToDecimal(object expression, string place = "0", decimal defValue = 0.00M)
        {
            if (expression != null)
                return StrToDecimal(expression.ToString(), defValue, place);
            return defValue;
        }
        /// <summary>
        /// string型转换为decimal型
        /// </summary>
        /// <param name="expression">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的decimal类型结果</returns>
        public static decimal StrToDecimal(string expression, decimal defValue,string plcae)
        {
            if ((expression == null) || (expression.Length > 18))
                return defValue;

            decimal intValue = defValue;
            if (expression != null)
            {
                bool IsDecimal = Regex.IsMatch(expression, @"^([-]|[0-9])[0-9]*(\.\w*)?$");
                if (IsDecimal)
                {
                    decimal.TryParse(expression, out intValue);
                    //intValue=Math.Round(intValue, plcae);//四舍五入
                    intValue = Convert.ToDecimal(intValue.ToString("f"+ plcae));//直接截取
                }
            }
            return intValue;
        }
        /// <summary>
        /// 将对象转换为Int类型
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static long ObjToLong(object obj)
        {
            if (isNumber(obj))
            {
                return long.Parse(obj.ToString());
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// 判断对象是否可以转成int型
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static bool isNumber(object o)
        {
            long tmpInt;
            if (o == null)
            {
                return false;
            }
            if (o.ToString().Trim().Length == 0)
            {
                return false;
            }
            if (!long.TryParse(o.ToString(), out tmpInt))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #region========修改webconfig的值==================
        #region 修改config文件
        /// <summary>  
        /// 修改config文件(AppSetting节点)  
        /// </summary>  
        /// <param name="key">键</param>  
        /// <param name="value">要修改成的值</param>  
        public static void UpdateAppSetting(string key, string value)
        {
            XmlDocument doc = new XmlDocument();
            //获得配置文件的全路径   
            string strFileName = AppDomain.CurrentDomain.BaseDirectory.ToString() + "Web.config";
            doc.Load(strFileName);
            //找出名称为“add”的所有元素   
            XmlNodeList nodes = doc.GetElementsByTagName("add");
            for (int i = 0; i < nodes.Count; i++)
            {
                //获得将当前元素的key属性   
                XmlAttribute _key = nodes[i].Attributes["key"];
                //根据元素的第一个属性来判断当前的元素是不是目标元素   
                if (_key != null)
                {
                    if (_key.Value == key)
                    {
                        //对目标元素中的第二个属性赋值   
                        _key = nodes[i].Attributes["value"];

                        _key.Value = value;
                        break;
                    }
                }
            }
            //保存上面的修改   
            doc.Save(strFileName);
        }

        /// <summary>  
        /// 修改config文件(ConnectionString节点)  
        /// </summary>  
        /// <param name="name">键</param>  
        /// <param name="value">要修改成的值</param>  
        public static void UpdateConnectionString(string name, string value)
        {
            XmlDocument doc = new XmlDocument();
            //获得配置文件的全路径   
            string strFileName = AppDomain.CurrentDomain.BaseDirectory.ToString() + "Web.config";
            doc.Load(strFileName);
            //找出名称为“add”的所有元素   
            XmlNodeList nodes = doc.GetElementsByTagName("add");
            for (int i = 0; i < nodes.Count; i++)
            {
                //获得将当前元素的key属性   
                XmlAttribute _name = nodes[i].Attributes["name"];
                //根据元素的第一个属性来判断当前的元素是不是目标元素   
                if (_name != null)
                {
                    if (_name.Value == name)
                    {
                        //对目标元素中的第二个属性赋值   
                        _name = nodes[i].Attributes["connectionString"];

                        _name.Value = value;
                        break;
                    }
                }
            }
            //保存上面的修改   
            doc.Save(strFileName);
        }
        #endregion
        #endregion

        #region 计算日期差（天数）
        /// <summary>
        /// 计算日期差（天数）
        /// </summary>
        /// <param name="dateStart">开始日期</param>
        /// <param name="dateEnd">结束日期</param>
        /// <returns></returns>
        public static int DateDiff(DateTime dateStart, DateTime dateEnd)
        {
            DateTime start = Convert.ToDateTime(dateStart.ToShortDateString());
            DateTime end = Convert.ToDateTime(dateEnd.ToShortDateString());
            TimeSpan sp = end.Subtract(start);
            return sp.Days;
        }
        #endregion
    }
}