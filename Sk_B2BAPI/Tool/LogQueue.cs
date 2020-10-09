using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Web;

namespace Sk_B2BAPI.Tool
{
    public class LogQueue
    {
        private class Log
        {
            internal LogType Type { get; set; }
            internal string Key { get; set; }
            internal string Value { get; set; }

            internal Log() { }
            internal Log(LogType type, string key, string value)
            {
                this.Type = type;
                this.Key = key;
                this.Value = value;
            }
        }
        private static ConcurrentQueue<Log> logQueue = new ConcurrentQueue<Log>();
        private static Timer timer = null;
        public static void Start()
        {
            timer = new Timer(d => {
                Log log;
                logQueue.TryDequeue(out log);
                if (log == null)
                {
                    return;
                }
                LogHelper.WriteMsg(log.Type, log.Key, log.Value);
            });
            timer.Change(0, 500);
        }
        public static void Stop()
        {
            if (timer!=null)
            {
                timer.Dispose();
                timer = null;
            }
        }

        public static void Write(LogType type, string title, string msg)
        {
            logQueue.Enqueue(new Log(type, title, msg));
        }
    }

    public enum LogType
    {
        [Description("调试")]
        Debug = 0,
        [Description("详情")]
        Info = 1,
        [Description("错误")]
        Error = 2
    }

    /// <summary>
    /// 枚举的扩展辅助操作方法
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// 扩展方法，获得枚举的Description
        /// </summary>
        /// <param name="value">枚举值</param>
        /// <param name="nameInstead">当枚举值没有定义DescriptionAttribute，是否使用枚举名代替，默认是使用</param>
        /// <returns>枚举的Description</returns>
        public static string GetDescription(this Enum value, Boolean nameInstead = true)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            if (name == null)
            {
                return null;
            }

            FieldInfo field = type.GetField(name);
            DescriptionAttribute attribute = System.Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;

            if (attribute == null && nameInstead == true)
            {
                return name;
            }
            return attribute.Description;
        }
    }

    public class LogHelper
    {
        private static object obj = new object();
        private static object obj1 = new object();

        /// <summary>
        /// 记录信息
        /// </summary>
        /// <param name="type">信息类型</param>
        /// <param name="title">模块</param>
        /// <param name="msg">信息</param>
        public static void WriteMsg(LogType type, string title, string msg)
        {
            var path = AppDomain.CurrentDomain.BaseDirectory + "/Log";
            DeleteOldFiles(path);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            path = Path.Combine(path, DateTime.Now.ToString("yyyy-MM-dd") + ".txt");

            lock (obj)
            {
                var fs = new FileStream(path, System.IO.FileMode.Append);
                var sw = new StreamWriter(fs);

                sw.WriteLine("=================" + type.GetDescription() + " ====================");
                sw.WriteLine("时间：" + DateTime.Now);
                sw.WriteLine("模块:" + title);
                sw.WriteLine("Message:" + msg);
                sw.Close();
                fs.Close();
            }
        }

        public static void DeleteOldFiles(string path)
        {
            try
            {
                DirectoryInfo info = new DirectoryInfo(path);
                lock (obj1)
                {
                    //获取文件夹下所有的文件
                    foreach (FileInfo feInfo in info.GetFiles())
                    {
                        //判断文件日期是否小于今天，是则删除
                        if (feInfo.CreationTime < DateTime.Today.AddDays(-3))
                            feInfo.Delete();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

    }
}