using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;

namespace Sk_B2BAPI.Tool
{
    public class StringHelper
    {

        private static readonly Regex RegNumber = new Regex("^[0-9]+$");

        private static readonly Regex RegNumberSign = new Regex("^[+-]?[0-9]+$");

        private static readonly Regex RegDecimal = new Regex("^[0-9]+[.]?[0-9]+$");

        private static readonly Regex RegDecimalSign = new Regex("^[+-]?[0-9]+[.]?[0-9]+$"); //等价于^[+-]?\d+[.]?\d+$

        private static readonly Regex RegEmail = new Regex(@"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$");

        //w 英文字母或数字的字符串，和 [a-zA-Z0-9] 语法一样 

        private static readonly Regex RegChzn = new Regex("[\u4e00-\u9fa5]");

        #region 常规字符串操作

        /// <summary>
        /// 检查字符串是否为空
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>返回值</returns>
        public static bool IsEmpty(string str)
        {
            if (string.IsNullOrEmpty(str))
                return true;
            return false;
        }

        /// <summary>
        /// 检查字符串中是否包含非法字符
        /// </summary>
        /// <param name="s">单字符</param>
        /// <returns>返回值</returns>
        public static bool CheckValidity(string s)
        {
            string str = s;
            if (str.IndexOf("'", StringComparison.Ordinal) > 0 || str.IndexOf("&", StringComparison.Ordinal) > 0 || str.IndexOf("%", StringComparison.Ordinal) > 0 || str.IndexOf("+", StringComparison.Ordinal) > 0 ||
                str.IndexOf("\"", StringComparison.Ordinal) > 0 || str.IndexOf("=", StringComparison.Ordinal) > 0 || str.IndexOf("!", StringComparison.Ordinal) > 0)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 把价格精确至小数点两位
        /// </summary>
        /// <param name="dPrice">价格</param>
        /// <returns>返回值</returns>
        public static string TransformPrice(double dPrice)
        {
            double d = dPrice;
            var myNfi = new NumberFormatInfo { NumberNegativePattern = 2 };
            string s = d.ToString("N", myNfi);
            return s;
        }

        /// <summary> 
        /// 检测含有中文字符串的实际长度 
        /// </summary> 
        /// <param name="str">字符串</param> 
        /// <returns>返回值</returns>
        public static int GetLength(string str)
        {
            var n = new ASCIIEncoding();
            byte[] b = n.GetBytes(str);
            int l = 0; // l 为字符串之实际长度 
            for (int i = 0; i <= b.Length - 1; i++)
            {
                if (b[i] == 63) //判断是否为汉字或全脚符号 
                {
                    l++;
                }
                l++;
            }
            return l;
        }

        /// <summary>
        /// 截取长度,num是英文字母的总数，一个中文算两个英文
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="iNum">英文字母的总数</param>
        /// <param name="bAddDot">是否计算标点</param>
        /// <returns>返回值</returns>
        public static string GetLetter(string str, int iNum, bool bAddDot)
        {
            if (str == null || iNum <= 0) return "";

            if (str.Length < iNum && str.Length * 2 < iNum)
            {
                return str;
            }

            string sContent = str;
            int iTmp = iNum;

            char[] arrC = str.ToCharArray(0, sContent.Length >= iTmp ? iTmp : sContent.Length);

            int i = 0;
            int iLength = 0;
            foreach (char ch in arrC)
            {
                iLength++;

                int k = ch;
                if (k > 127 || k < 0)
                {
                    i += 2;
                }
                else
                {
                    i++;
                }

                if (i > iTmp)
                {
                    iLength--;
                    break;
                }
                if (i == iTmp)
                {
                    break;
                }
            }

            if (iLength < str.Length && bAddDot)
                sContent = sContent.Substring(0, iLength - 3) + "...";
            else
                sContent = sContent.Substring(0, iLength);

            return sContent;
        }

        /// <summary>
        /// 获取日期字符串
        /// </summary>
        /// <param name="dt">日期时间</param>
        /// <returns>返回值</returns>
        public static string GetDateString(DateTime dt)
        {
            return dt.Year.ToString() + dt.Month.ToString().PadLeft(2, '0') + dt.Day.ToString().PadLeft(2, '0');
        }

        /// <summary>
        /// 根据指定字符，截取相应字符串
        /// </summary>
        /// <param name="sOrg">源字符串</param>
        /// <param name="sLast">指定字符串</param>
        /// <returns>返回值</returns>
        public static string GetStrByLast(string sOrg, string sLast)
        {
            int iLast = sOrg.LastIndexOf(sLast, StringComparison.Ordinal);
            if (iLast > 0)
                return sOrg.Substring(iLast + 1);
            return sOrg;
        }

        /// <summary>
        /// 根据指定字符，截取相应字符串
        /// </summary>
        /// <param name="sOrg">源字符串</param>
        /// <param name="sLast">指定字符串</param>
        /// <returns>返回值</returns>
        public static string GetPreStrByLast(string sOrg, string sLast)
        {
            int iLast = sOrg.LastIndexOf(sLast, StringComparison.Ordinal);
            if (iLast > 0)
                return sOrg.Substring(0, iLast);
            return sOrg;
        }

        /// <summary>
        /// 根据指定字符，截取相应字符串
        /// </summary>
        /// <param name="sOrg">源字符串</param>
        /// <param name="sEnd">终止字符串</param>
        /// <returns>返回值</returns>
        public static string RemoveEndWith(string sOrg, string sEnd)
        {
            if (sOrg.EndsWith(sEnd))
                sOrg = sOrg.Remove(sOrg.IndexOf(sEnd, StringComparison.Ordinal), sEnd.Length);
            return sOrg;
        }
        /// <summary>
        /// 字符串转整型数组
        /// </summary>
        /// <param name="strs">源字符串</param>
        /// <param name="f">分隔符</param>
        /// <returns></returns>
        public static int[] StrsToInts(string strs, char f)
        {
            try
            {
                if (string.IsNullOrEmpty(strs)) return new int[0];
                string[] li = strs.Split(f);
                List<string> list = li.Where(c => c != "").ToList();
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    try
                    {
                        int testid = int.Parse(list[i]);
                    }
                    catch
                    {
                        list.RemoveAt(i);
                    }
                }
                int[] result = new int[list.Count];
                for (int i = 0; i < list.Count; i++)
                {
                    result[i] = int.Parse(list[i]);
                }
                return result;
            }
            catch
            {
                return new int[0];
            }
        }



        #region 判断变量是否为空
        /// <summary>
        /// 判断变量是否为空 有空为false
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNull(params string[] str)
        {
            return str.All(s => !string.IsNullOrWhiteSpace(s));
        }

        #endregion

        #endregion  常规字符串操作

        #region 数字字符串检查

        /// <summary>
        /// int有效性
        /// </summary>
        /// <param name="val">字符串</param>
        /// <returns>返回值</returns>
        public static bool IsValidInt(string val)
        {
            return Regex.IsMatch(val, @"^[1-9]\d*\.?[0]*$");
        }

        /// <summary>
        /// 是否数字字符串
        /// </summary>
        /// <param name="inputData">输入字符串</param>
        /// <returns>返回值</returns>
        public static bool IsNumber(string inputData)
        {
            Match m = RegNumber.Match(inputData);
            return m.Success;
        }

        /// <summary>
        /// 是否数字字符串 可带正负号
        /// </summary>
        /// <param name="inputData">输入字符串</param>
        /// <returns>返回值</returns>
        public static bool IsNumberSign(string inputData)
        {
            Match m = RegNumberSign.Match(inputData);
            return m.Success;
        }

        /// <summary>
        /// 是否是浮点数
        /// </summary>
        /// <param name="inputData">输入字符串</param>
        /// <returns>返回值</returns>
        public static bool IsDecimal(string inputData)
        {
            Match m = RegDecimal.Match(inputData);
            return m.Success;
        }

        /// <summary>
        /// 是否是浮点数 可带正负号
        /// </summary>
        /// <param name="inputData">输入字符串</param>
        /// <returns>返回值</returns>
        public static bool IsDecimalSign(string inputData)
        {
            Match m = RegDecimalSign.Match(inputData);
            return m.Success;
        }

        #endregion

        #region 中文检测

        /// <summary>
        /// 检测是否有中文字符
        /// </summary>
        /// <param name="inputData">输入字符串</param>
        /// <returns>返回值</returns>
        public static bool IsHasChzn(string inputData)
        {
            Match m = RegChzn.Match(inputData);
            return m.Success;
        }

        /// <summary> 
        /// 检测含有中文字符串的实际长度 
        /// </summary>
        /// <param name="inputData">输入字符串</param>
        /// <returns>返回值</returns>
        public static int GetCHZNLength(string inputData)
        {
            var n = new ASCIIEncoding();
            byte[] bytes = n.GetBytes(inputData);

            int length = 0; // l 为字符串之实际长度 
            for (int i = 0; i <= bytes.Length - 1; i++)
            {
                if (bytes[i] == 63) //判断是否为汉字或全脚符号 
                {
                    length++;
                }
                length++;
            }
            return length;
        }

        #endregion

        #region 常用格式

        /// <summary>
        /// 验证身份证是否合法  15 和  18位两种
        /// </summary>
        /// <param name="idCard">要验证的身份证</param>
        /// <returns>返回值</returns>
        public static bool IsIdCard(string idCard)
        {
            if (string.IsNullOrEmpty(idCard))
            {
                return false;
            }

            if (idCard.Length == 15)
            {
                return Regex.IsMatch(idCard, @"^[1-9]\d{7}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{3}$");
            }
            if (idCard.Length == 18)
            {
                return Regex.IsMatch(idCard,
                                     @"^[1-9]\d{5}[1-9]\d{3}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])((\d{4})|\d{3}[A-Z])$",
                                     RegexOptions.IgnoreCase);
            }
            return false;
        }

        /// <summary>
        /// 是否是邮件地址
        /// </summary>
        /// <param name="inputData">输入字符串</param>
        /// <returns>返回值</returns>
        public static bool IsEmail(string inputData)
        {
            Match m = RegEmail.Match(inputData);
            return m.Success;
        }

        /// <summary>
        /// 邮编有效性
        /// </summary>
        /// <param name="zip">输入字符串</param>
        /// <returns>返回值</returns>
        public static bool IsValidZip(string zip)
        {
            var rx = new Regex(@"^\d{6}$", RegexOptions.None);
            Match m = rx.Match(zip);
            return m.Success;
        }

        /// <summary>
        /// 固定电话有效性
        /// </summary>
        /// <param name="phone">输入字符串</param>
        /// <returns>返回值</returns>
        public static bool IsValidPhone(string phone)
        {
            var rx = new Regex(@"^(\(\d{3,4}\)|\d{3,4}-)?\d{7,8}$", RegexOptions.None);
            Match m = rx.Match(phone);
            return m.Success;
        }

        /// <summary>
        /// 手机有效性
        /// </summary>
        /// <param name="mobile">输入字符串</param>
        /// <returns>返回值</returns>
        public static bool IsValidMobile(string mobile)
        {
            var rx = new Regex(@"(13|15|17|18)\d{9}$", RegexOptions.None);
            Match m = rx.Match(mobile);
            return m.Success;
        }

        /// <summary>
        /// 电话有效性（固话和手机 ）
        /// </summary>
        /// <param name="number">输入字符串</param>
        /// <returns>返回值</returns>
        public static bool IsValidPhoneAndMobile(string number)
        {
            var rx = new Regex(@"^(\(\d{3,4}\)|\d{3,4}-)?\d{7,8}$|^(13|15)\d{9}$", RegexOptions.None);
            Match m = rx.Match(number);
            return m.Success;
        }

        /// <summary>
        /// Url有效性
        /// </summary>
        /// <param name="url">输入字符串</param>
        /// <returns>返回值</returns>
        public static bool IsValidUrl(string url)
        {
            return Regex.IsMatch(url,
                                 @"^(http|https|ftp)\://[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&%\$#\=~])*[^\.\,\)\(\s]$");
        }

        /// <summary>
        /// IP有效性
        /// </summary>
        /// <param name="ip">输入字符串</param>
        /// <returns>返回值</returns>
        public static bool IsValidIp(string ip)
        {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }

        /// <summary>
        /// domain 有效性
        /// </summary>
        /// <param name="host">域名</param>
        /// <returns>返回值</returns>
        public static bool IsValidDomain(string host)
        {
            var r = new Regex(@"^\d+$");
            if (host.IndexOf(".", StringComparison.Ordinal) == -1)
            {
                return false;
            }
            return !r.IsMatch(host.Replace(".", string.Empty));
        }

        /// <summary>
        /// 判断是否为base64字符串
        /// </summary>
        /// <param name="str">输入字符串</param>
        /// <returns>返回值</returns>
        public static bool IsBase64String(string str)
        {
            return Regex.IsMatch(str, @"[A-Za-z0-9\+\/\=]");
        }

        /// <summary>
        /// 验证字符串是否是GUID
        /// </summary>
        /// <param name="guid">字符串</param>
        /// <returns>返回值</returns>
        public static bool IsGuid(string guid)
        {
            if (string.IsNullOrEmpty(guid))
                return false;

            return Regex.IsMatch(guid, "[A-F0-9]{8}(-[A-F0-9]{4}){3}-[A-F0-9]{12}|[A-F0-9]{32}", RegexOptions.IgnoreCase);
        }

        #endregion

        #region 日期检查

        /// <summary>
        /// 判断输入的字符是否为日期
        /// </summary>
        /// <param name="strValue">输入字符串</param>
        /// <returns>返回值</returns>
        public static bool IsDate(string strValue)
        {
            return Regex.IsMatch(strValue,
                                 @"^((\d{2}(([02468][048])|([13579][26]))[\-\/\s]?((((0?[13578])|(1[02]))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(3[01])))|(((0?[469])|(11))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(30)))|(0?2[\-\/\s]?((0?[1-9])|([1-2][0-9])))))|(\d{2}(([02468][1235679])|([13579][01345789]))[\-\/\s]?((((0?[13578])|(1[02]))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(3[01])))|(((0?[469])|(11))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(30)))|(0?2[\-\/\s]?((0?[1-9])|(1[0-9])|(2[0-8]))))))");
        }

        /// <summary>
        /// 判断输入的字符是否为日期,如2004-07-12 14:25|||1900-01-01 00:00|||9999-12-31 23:59
        /// </summary>
        /// <param name="strValue">输入字符串</param>
        /// <returns>返回值</returns>
        public static bool IsDateHourMinute(string strValue)
        {
            return Regex.IsMatch(strValue,
                                 @"^(19[0-9]{2}|[2-9][0-9]{3})-((0(1|3|5|7|8)|10|12)-(0[1-9]|1[0-9]|2[0-9]|3[0-1])|(0(4|6|9)|11)-(0[1-9]|1[0-9]|2[0-9]|30)|(02)-(0[1-9]|1[0-9]|2[0-9]))\x20(0[0-9]|1[0-9]|2[0-3])(:[0-5][0-9]){1}$");
        }

        #endregion

        /// <summary>
        /// 替换字符串中的回车换行为空格
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ReplaceEnterToSpace(string str)
        {
            return Regex.Replace(str, @"\n", " ");
        }

        private void AccessAppSettings()
        {
            //获取Configuration对象
            Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //根据Key读取<add>元素的Value
            string name = config.AppSettings.Settings["name"].Value;
            //写入<add>元素的Value
            config.AppSettings.Settings["name"].Value = "fx163";
            //增加<add>元素
            config.AppSettings.Settings.Add("url", "http://www.fx163.net");
            //删除<add>元素
            config.AppSettings.Settings.Remove("name");
            //一定要记得保存，写不带参数的config.Save()也可以
            config.Save(ConfigurationSaveMode.Modified);
            //刷新，否则程序读取的还是之前的值（可能已装入内存）
            System.Configuration.ConfigurationManager.RefreshSection("appSettings");
        }
    }

}
