using System;
using System.Configuration;
using System.IO;
using System.Net.Mail;
using System.Text;

namespace Sk_B2BAPI.FileHelper
{
    /// <summary>
    ///发送邮件类
    /// </summary>
    public static class MailService
    {
        /// <summary>
        /// 下载局域网文件
        /// </summary>
        /// <param name="path">文件路径，如：\\192.168.10.1\app\app\123.zip</param>
        /// <param name="username">计算机名称</param>
        /// <param name="password">计算机密码</param>
        public static bool RequestWindowsShared(string path, string username, string password)
        {
            //文件总大小
            int allBytesCount = 0;
            //每次传输大小
            int byteTemp = 1024;
            //当前位置
            int bytePosition = 0;
            //剩下大小
            int remain = 0;
            System.Net.FileWebRequest request = null;
            System.Net.FileWebResponse response = null;
            System.IO.Stream stream = null;
            System.IO.FileStream fileStream = null;
            try
            {
                Uri uri = new Uri(path);
                request = (System.Net.FileWebRequest)System.Net.FileWebRequest.Create(uri);
                System.Net.ICredentials ic = new System.Net.NetworkCredential(username, password);
                request.Credentials = ic;
                response = (System.Net.FileWebResponse)request.GetResponse();
                stream = response.GetResponseStream();
                byte[] bytes = new byte[stream.Length];
                stream.Read(bytes, 0, bytes.Length);
                string filename = System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + System.IO.Path.GetFileName(path);
                fileStream = new FileStream(filename, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.ReadWrite);
                allBytesCount = bytes.Length;
                remain = allBytesCount;
                while (remain > 0)
                {
                    fileStream.Write(bytes, bytePosition, byteTemp);
                    remain = remain - byteTemp;
                    bytePosition = bytePosition + byteTemp;
                    fileStream.Flush();
                    if (remain < byteTemp)
                        byteTemp = remain;
                }
                Console.WriteLine("下载成功!");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                fileStream.Close();
                fileStream.Dispose();
                stream.Close();
                stream.Dispose();
            }
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="path">共享目录路径+文件名称</param>
        /// <param name="local">本地路径</param>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        public static bool ResponseWindowsShared(string path, string local, string username, string password)
        {
            //文件总大小
            int allBytesCount = 0;
            //每次传输大小
            int byteTemp = 1024;
            //当前位置
            int bytePosition = 0;
            //剩下大小
            int remain = 0;
            System.Net.FileWebRequest request = null;
            System.IO.Stream stream = null;
            try
            {
                //时间戳 
                string strBoundary = "----------" + DateTime.Now.Ticks.ToString("x");
                Uri uri = new Uri(path);
                byte[] bytes = System.IO.File.ReadAllBytes(local);
                request = (System.Net.FileWebRequest)System.Net.FileWebRequest.Create(uri);
                request.Method = "POST";
                //设置获得响应的超时时间（300秒） 
                request.Timeout = 300000;
                request.ContentType = "multipart/form-data; boundary=" + strBoundary;
                request.ContentLength = bytes.Length;
                System.Net.ICredentials ic = new System.Net.NetworkCredential(username, password);
                request.Credentials = ic;
                stream = request.GetRequestStream();
                allBytesCount = bytes.Length;
                remain = allBytesCount;
                while (remain > 0)
                {
                    stream.Write(bytes, bytePosition, byteTemp);
                    remain = remain - byteTemp;
                    bytePosition = bytePosition + byteTemp;
                    stream.Flush();
                    if (remain < byteTemp)
                        byteTemp = remain;
                }
                Console.WriteLine("上传成功!");
                return true;
               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                stream.Close();
                stream.Dispose();
            }
        }
    }

    public  class FileLib
    {
        #region 属性
        private string fileName = "";
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        #endregion

        #region 文件上传

        /// <summary>
               /// 上传文件（自动分割）
               /// </summary>
               /// <param name="filePath">待上传的文件全路径名称(@"E:/FTP/ftproot/20070228DQCK.zip")</param>
               /// <param name="hostURL">服务器的地址</param>
               /// <param name="byteCount">分割的字节大小</param>        
               /// <param name="userID">主机用户ID</param>
               /// <param name="cruuent">当前字节指针</param>
               /// <returns>成功返回"";失败则返回错误信息</returns>
        public string UpLoadFile(string filePath, string hostURL, int byteCount, string userID, long cruuent)
        {
            string tmpURL = hostURL;
            byteCount = byteCount * 1024;
            //http://localhost:8080/fism/app?service=fileupload&beanId=com.cfcc.fism.service.upload.CollFileSaveServiceImpl&action=upload&filename=AI1215900000020051130411.zip&userid=test&npos=333
            //action=length

            System.Net.WebClient WebClientObj = new System.Net.WebClient();
            FileStream fStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            BinaryReader bReader = new BinaryReader(fStream);
            long length = fStream.Length;
            string sMsg = "版式上传成功";
            string fileName = filePath.Substring(filePath.LastIndexOf('/') + 1);
            try
            {

                #region 续传处理
                byte[] data;
                if (cruuent > 0)
                {
                    fStream.Seek(cruuent, SeekOrigin.Current);
                }
                #endregion 

                #region 分割文件上传
                for (; cruuent <= length; cruuent = cruuent + byteCount)
                {
                    if (cruuent + byteCount > length)
                    {
                        data = new byte[Convert.ToInt64((length - cruuent))];
                        bReader.Read(data, 0, Convert.ToInt32((length - cruuent)));
                    }
                    else
                    {
                        data = new byte[byteCount];
                        bReader.Read(data, 0, byteCount);
                    }

                    try
                    {
                        LogHelper.WriteErrorMsg("提示",data.ToString());
                        //***
                        hostURL = tmpURL + "&action=upload" + "&filename=" + fileName + "&userid=" + userID + "&npos=" + cruuent.ToString();
                        byte[] byRemoteInfo = WebClientObj.UploadData(hostURL, "POST", data);
                        string sRemoteInfo = System.Text.Encoding.Default.GetString(byRemoteInfo);

                        //  获取返回信息
                        if (sRemoteInfo.Trim() != "")
                        {
                            sMsg = sRemoteInfo;
                            break;

                        }
                    }
                    catch (Exception ex)
                    {
                        sMsg = ex.ToString();
                        break;
                    }
                    #endregion

                }
            }
            catch (Exception ex)
            {
                sMsg = sMsg + ex.ToString();
            }
            try
            {
                bReader.Close();
                fStream.Close();
            }
            catch (Exception exMsg)
            {
                sMsg = exMsg.ToString();
            }

            GC.Collect();
            return sMsg;
        }
        #endregion

        #region 获取文件大小
        /// <summary>
               /// 获取远程服务器文件字节大小
               /// </summary>
               /// <param name="filePath">待上传的文件全路径名称</param>
               /// <param name="hostURL">服务器的地址</param>
               /// <param name="userID">主机用户ID</param>
               /// <returns>远程文件大小</returns>
        public long GetRemoteFileLength(string filePath, string hostURL, string userID)
        {
            long length = 0;
            System.Net.WebClient WebClientObj = new System.Net.WebClient();

            string fileName = filePath.Substring(filePath.LastIndexOf('/') + 1);

            hostURL = hostURL + "&action=length" + "&filename=" + fileName + "&userid=" + userID + "&npos=0";

            byte[] data = new byte[0];
            byte[] byRemoteInfo = WebClientObj.UploadData(hostURL, "POST", data);
            string sRemoteInfo = System.Text.Encoding.Default.GetString(byRemoteInfo);//主系统没有作异常处理
            try
            {
                length = Convert.ToInt64(sRemoteInfo);
            }
            catch (Exception exx)
            {
                LogHelper.WriteErrorMsg("FileLib类GetRemoteFileLength()中length = Convert.ToInt64(sRemoteInfo)语句异常：", exx.Message);
                //我们强制处理异常
                length = 0;
            }
            GC.Collect();

            return length;

        }

        /// <summary>
               /// 获得本地文件字节大小
               /// </summary>
               /// <param name="filePath">本地文件全路径</param>
               /// <returns>本地文件字节大小</returns>
        public long GetLocalFileLength(string filePath)
        {
            long length = 0;
            try
            {
                string fileName = filePath.Substring(filePath.LastIndexOf('/') + 1);
                FileStream s = new FileStream(filePath, FileMode.Open);
                length = s.Length;
                s.Close();
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorMsg("FileLib类中获取本地文件大小异常：" ,ex.Message);
            }
            return length;

        }
        #endregion

        #region 文件下载
        public bool DownLoadFile(string localPath, string hostURL, int byteCount, string userID, long cruuent)
        {

            bool result = true;


            string tmpURL = hostURL;

            byteCount = byteCount * 1024;
            hostURL = tmpURL + "&npos=" + cruuent.ToString();

            System.IO.FileStream fs;
            fs = new FileStream(localPath, FileMode.OpenOrCreate);
            if (cruuent > 0)
            {
                //偏移指针
                fs.Seek(cruuent, System.IO.SeekOrigin.Current);
            }


            System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(hostURL);
            if (cruuent > 0)
            {
                request.AddRange(Convert.ToInt32(cruuent));    //设置Range值
            }

            try
            {
                //向服务器请求，获得服务器回应数据流
                System.IO.Stream ns = request.GetResponse().GetResponseStream();

                byte[] nbytes = new byte[byteCount];
                int nReadSize = 0;
                nReadSize = ns.Read(nbytes, 0, byteCount);

                while (nReadSize > 0)
                {
                    fs.Write(nbytes, 0, nReadSize);
                    nReadSize = ns.Read(nbytes, 0, byteCount);

                }
                fs.Close();
                ns.Close();
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorMsg("下载" + localPath + "的时候失败！" + "原因是:" , ex.Message);
                fs.Close();
                result = false;
            }


            return result;

        }
        #endregion
    }

}
