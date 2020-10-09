using Sk_B2BAPI.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sk_B2BAPI.Tool;

namespace Sk_B2BAPI.Admin.ashx
{
    /// <summary>
    /// UploadImg 的摘要说明
    /// </summary>
    public class UploadImg : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string returnJson = "";
            try
            {
                string type = context.Request.QueryString["type"].Trim();//请求类型
                if (type == "UploadImage")
                {
                    returnJson = UploadImage(context);
                }
                else if(type== "UploadApk")
                {
                    returnJson = UploadApk(context);
                }
            }
            catch (Exception e)
            {
                string msg = e.Message.ToString().Trim().Replace("\"", "\\\"").Replace("\r", "").Replace("\n", "");
                returnJson = JsonMethod.GetError(4, msg); ;
            }
            context.Response.Write(returnJson);
        }
        /// <summary>
        /// 上传图片
        /// </summary>
        protected string UploadImage(HttpContext context)
        {
            try
            {
                string filePath = string.Empty;
                string fileNewName = string.Empty;
                string imgType = context.Request["imgType"].Trim();
                var file = context.Request.Files;
                if (file.Count <= 0)
                {
                    return JsonMethod.GetError(1, "请选择要上传的文件");//请选择要上传的文件  
                }
                var ufile = file[0];
                string fileName = ufile.FileName;/*获取文件名： C:\Documents and Settings\Administrator\桌面\123.jpg*/
                string suffix = fileName.Substring(fileName.LastIndexOf(".") + 1).ToLower();/*获取后缀名并转为小写： jpg*/
                int bytes = ufile.ContentLength;//获取文件的字节大小  
                if (suffix != "jpg" && suffix != "png")
                {
                    return JsonMethod.GetError(1, "上传文件格式错误");//只能上传JPG格式图片  
                }
                else if (bytes > 1024 * 1024 * 1)
                {
                    return JsonMethod.GetError(1, "图片不能大于1M"); //图片不能大于1M 
                }
                string path = context.Server.MapPath("/UploadFile/" + imgType + "/");
                string year = DateTime.Now.Year.ToString();
                string month = DateTime.Now.Month.ToString();
                string date = DateTime.Now.ToFileTimeUtc().ToString();
                string newfileName = date + "." + suffix;
                string newfile = year + "_" + month + "/" + newfileName;
                string newPath = path + "/" + year + "_" + month;
                if (false == System.IO.Directory.Exists(newPath))
                {
                    //创建文件夹
                    System.IO.Directory.CreateDirectory(newPath);
                }
                ufile.SaveAs(newPath + "/" + newfileName);//保存图片  
                ///保存图片路径
                string dataFile = "/UploadFile/" + imgType + "/" + newfile;
                //bool flag = UserInfoDal.UpdateUserInfo(entId, userId, telPhone, dataFile);

                ///获取网站ip
                string web_url = BaseConfiguration.SercerIp;
                return JsonMethod.GetError(0, dataFile);

            }
            catch (Exception ex)
            {
                return JsonMethod.GetError(1, ex.Message);
            }
        }

        /// <summary>
        /// 上传Apk
        /// </summary>
        protected string UploadApk(HttpContext context)
        {
            try
            {
                string filePath = string.Empty;
                string fileNewName = string.Empty;
                string apptype = context.Request["apptype"].Trim();
                var file = context.Request.Files;
                if (file.Count <= 0)
                {
                    return JsonMethod.GetError(1, "请选择要上传的文件");//请选择要上传的文件  
                }
                var ufile = file[0];
                string fileName = ufile.FileName;/*获取文件名： C:\Documents and Settings\Administrator\桌面\123.jpg*/
                string suffix = fileName.Substring(fileName.LastIndexOf(".") + 1).ToLower();/*获取后缀名并转为小写： jpg*/
                int bytes = ufile.ContentLength;//获取文件的字节大小  
                if (suffix != "apk")
                {
                    return JsonMethod.GetError(1, "上传文件格式错误,请上传安装包！");//只能上传JPG格式图片  
                }
                //else if (bytes > 1024 * 1024 * 1)
                //{
                //    return JsonMethod.GetError(1, "图片不能大于1M"); //图片不能大于1M 
                //}
                string path = context.Server.MapPath("/UploadFile/" + apptype + "/");
                //string year = DateTime.Now.Year.ToString();
                //string month = DateTime.Now.Month.ToString();
                string date = DateTime.Now.ToFileTimeUtc().ToString();
                string AppFileName = BasisConfig.GetConfigString("AppFileName").ToString();
                string newfileName = AppFileName+"." + suffix;

                //string newfile = year + "_" + month + "/" + newfileName;
               
                if (false == System.IO.Directory.Exists(path))
                {
                    //创建文件夹
                    System.IO.Directory.CreateDirectory(path);
                }
                ufile.SaveAs(path + "/" + newfileName);//保存安装包 
                ///保存安装包路径
                string dataFile = "/UploadFile/" + apptype + "/" + newfileName;
                //bool flag = UserInfoDal.UpdateUserInfo(entId, userId, telPhone, dataFile);

                ///获取网站ip
                string web_url = BaseConfiguration.SercerIp;
                return JsonMethod.GetError(0, dataFile);

            }
            catch (Exception ex)
            {
                return JsonMethod.GetError(1, ex.Message);
            }
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}