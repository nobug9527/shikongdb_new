using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;


/// <summary>
/// 各种操作的日志记录
/// </summary>
//public class Log
//{
//    private static ConcurrentQueue<KeyValueModel> queue = new ConcurrentQueue<KeyValueModel>();
//    private static Timer ti = null;
//    public static void Start()
//    {
//        ti = new Timer((d) =>
//          {
//              KeyValueModel txt;
//              queue.TryDequeue(out txt);
//              if (txt == null) { return; }
//              LogHelper.WriteErrorMsg(txt.Key, txt.Value);
//          });
//        ti.Change(0, 500);
//    }
//    private class KeyValueModel
//    {
//        internal string Key { set; get; }
//        internal string Value { set; get; }

//        internal KeyValueModel() { }
//        internal KeyValueModel(string key, string value)
//        {
//            Key = key;
//            Value = value;
//        }
//    }
//    public static void Write(string title, string msg)
//    {
//        queue.Enqueue(new KeyValueModel(title, msg));
//    }
//}
public class LogHelper
{
    private static object obj = new object();

    /// <summary>
    /// 记录错误信息
    /// </summary>
    /// <param name="errorTitle">发生错误的模块</param>
    /// <param name="se">异常信息模型</param>
    public static void WriteErrorMsg(string errorTitle, string msg)
    {
        var path = AppDomain.CurrentDomain.BaseDirectory + "/Log"; //Path.Combine(Application.StartupPath, "Log");
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        path = Path.Combine(path, DateTime.Now.ToString("yyyy-MM-dd") + ".txt");

        lock (obj)
        {
            var fs = new FileStream(path, System.IO.FileMode.Append);
            var sw = new StreamWriter(fs);

            sw.WriteLine("=====================================");
            sw.WriteLine("时间：" + DateTime.Now);
            sw.WriteLine("模块:" + errorTitle);
            sw.WriteLine("Message:" + msg);
            sw.Close();
            fs.Close();
        }
    }
}

