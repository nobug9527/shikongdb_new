using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Web;

namespace Sk_B2BAPI.Tool
{
    /// <summary>
    /// 提供操作文件的常用方法（无文件上传方法）
    /// </summary>
    public class FileUtil
    {
        /// <summary>
        /// 读取文件（默认编码）
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        public static string Read(string path)
        {
            return Read(GetMapPath(path), Encoding.Default);
        }
        /// <summary>
        /// 读取文件（指定编码）
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="encode">编码方式</param>
        /// <returns></returns>
        public static string Read(string path, Encoding encode)
        {
            FileStream fs = null;
            for (int i = 0; i < 5; i++)
            {
                try
                {
                    fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
                    break;
                }
                catch(Exception)
                {
                    System.Threading.Thread.Sleep(50);
                }
            }
            if (fs == null) return "";
            StreamReader sr = null;
            try
            {
                sr = new StreamReader(fs, encode);
                return sr.ReadToEnd();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sr.Close();
                fs.Close();
            }
        }
        /// <summary>
        /// 返回文件行的数组
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        public static string[] ReadLines(string path)
        {
            return ReadLines(GetMapPath(path), Encoding.Default);
        }
        /// <summary>
        /// 返回文件行的数组
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="encode">编码方式</param>
        /// <returns></returns>
        public static string[] ReadLines(string path, Encoding encode)
        {
            try
            {
                return File.ReadAllLines(path, encode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 向文件写入内容（默认编码）
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="content">内容</param>
        /// <returns></returns>
        public static bool Write(string path, string content)
        {
            return Write(path, content, Encoding.Default);
        }
        /// <summary>
        /// 向文件写入内容（指定编码）
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="content">内容</param>
        /// <param name="encode">编码</param>
        /// <returns></returns>
        public static bool Write(string path, string content, Encoding encode)
        {
            FileStream fs = null;
            for (int i = 0; i < 5; i++)
            {
                try
                {
                    fs = new FileStream(GetMapPath(path), FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
                    break;
                }
                catch
                {
                    System.Threading.Thread.Sleep(50);
                }
            }
            if (fs == null) return false;
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(fs, encode);
                sw.Write(content);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sw.Close();
                fs.Close();
            }
        }
        /// <summary>
        /// 向文件追加内容（默认编码）
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="content">内容</param>
        /// <returns></returns>
        public static bool Append(string path, string content)
        {
            return Append(path, content, Encoding.Default);
        }
        /// <summary>
        /// 向文件追加内容（指定编码）
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="content">内容</param>
        /// <param name="encode">编码</param>
        /// <returns></returns>
        public static bool Append(string path, string content, Encoding encode)
        {
            FileStream fs = null;
            for (int i = 0; i < 5; i++)
            {
                try
                {
                    fs = new FileStream(GetMapPath(path), FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                    break;
                }
                catch
                {
                    System.Threading.Thread.Sleep(50);
                }
            }
            if (fs == null) return false;
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(fs, encode);
                sw.Write(content);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sw.Close();
                fs.Close();
            }
        }
        /// <summary>
        /// 删除指定文件
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns></returns>
        public static bool Delete(string path)
        {
            try
            {
                File.Delete(GetMapPath(path));
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 获取指定目录下所有文件
        /// </summary>
        /// <param name="dir">目录</param>
        /// <returns></returns>
        public static string[] GetFiles(string dir)
        {
            try
            {
                return Directory.GetFiles(GetMapPath(dir));
            }
            catch (DirectoryNotFoundException ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 获取目录中所有文件路径
        /// </summary>
        /// <param name="dir">目录</param>
        /// <param name="patt">匹配正则(如：*.(txt|log))</param>
        /// <returns></returns>
        public static string[] GetFiles(string dir, string patt)
        {
            string pattern = "(?i)^" + patt.Replace(".", @"\.") + "$";

            Regex reg = new Regex(pattern);

            List<string> list = new List<string>();

            try
            {
                string[] files = Directory.GetFiles(dir);

                foreach (string file in files)
                {
                    if (reg.IsMatch(file))
                        list.Add(file);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list.ToArray();
        }

        /// <summary>
        /// 根据文件路径，或文件夹路径，创建文件夹
        /// </summary>
        /// <param name="dir"></param>
        public static void CreateDir(string dir)
        {
            if (dir.Contains("."))
            {
                dir = dir.Substring(0, dir.LastIndexOf("\\"));
            }
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }
        /// <summary>
        /// 读文件，可以按txt原本格式读出
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ReadFormatTxt(string path)
        {
            ///从指定的目录以打开或者创建的形式读取日志文件
            FileStream fs = new FileStream(GetMapPath(path), FileMode.Open, FileAccess.Read);
            ///定义输出字符串
            StringBuilder txt = new StringBuilder();
            txt.Length = 0;
            ///为上面创建的文件流创建读取数据流
            StreamReader sr = new StreamReader(fs, System.Text.Encoding.GetEncoding("gb2312"));
            ///设置当前流的起始位置为文件流的起始点
            sr.BaseStream.Seek(0, SeekOrigin.Begin);
            ///读取文件
            while (sr.Peek() > -1)
            {
                ///取文件的一行内容并换行
                txt.Append(sr.ReadLine() + "\n<br />");
            }
            //txt.Append(sr.ReadToEnd());
            fs.Close();
            sr.Close();
            return txt.ToString();
        }
        /// <summary>
        /// 获取MapPath
        /// </summary>
        /// <param name="strPath"></param>
        /// <returns></returns>
        public static string GetMapPath(string strPath)
        {
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Server.MapPath(strPath);
            }
            else //非web程序引用
            {
                strPath = strPath.Replace("/", "\\");
                if (strPath.StartsWith("\\"))
                {
                    strPath = strPath.Substring(strPath.IndexOf('\\', 1)).TrimStart('\\');
                }
                return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, strPath);
            }
        }
    }
}
