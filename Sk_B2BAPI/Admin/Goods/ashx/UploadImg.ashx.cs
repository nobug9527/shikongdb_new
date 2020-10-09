using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using Sk_B2BAPI.App_Code;
using System.Data.SqlClient;
using System.Data;

namespace Sk_B2BAPI.Admin.Goods.ashx
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
            }
            catch (Exception e)
            {
                string msg = e.Message.ToString().Trim().Replace("\"", "\\\"").Replace("\r", "").Replace("\n", "");
                returnJson =  JsonMethod.GetError(4, msg);;
            }
            context.Response.Write(returnJson);
        }
        /// <summary>
        /// 上传图片
        /// </summary>
        protected string UploadImage(HttpContext context)
        {
            string filePath = string.Empty;
            string fileNewName = string.Empty;
            HttpPostedFile _upfile = context.Request.Files["fulFile"];
            //HttpFileCollection files = context.Request.Files["fulFile"];
            if (_upfile == null)
            {
                return JsonMethod.GetError(1, "请选择要上传的文件");//请选择要上传的文件  
            }
            else
            {
                string fileName = _upfile.FileName;/*获取文件名： C:\Documents and Settings\Administrator\桌面\123.jpg*/
                string suffix = fileName.Substring(fileName.LastIndexOf(".") + 1).ToLower();/*获取后缀名并转为小写： jpg*/
                int bytes = _upfile.ContentLength;//获取文件的字节大小  
                if (suffix != "jpg" && suffix != "png")
                {
                    return JsonMethod.GetError(1, "上传文件格式错误"); //只能上传JPG格式图片  
                }
                else if (bytes > 1024 * 1024 * 5)
                {
                    return JsonMethod.GetError(1, "图片不能大于5M"); //图片不能大于1M 
                }
                String path = context.Server.MapPath("/UploadFile/Images/");
                string year = DateTime.Now.Year.ToString();
                string month = DateTime.Now.Month.ToString();
                string date = DateTime.Now.ToFileTimeUtc().ToString();
                string newfileName = date + suffix;
                string newPath = path + "/" + year + "/" + month;
                _upfile.SaveAs(newPath + "/" + newfileName);//保存图片  
                return JsonMethod.GetError(0, "上传成功");
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