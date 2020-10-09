using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace DTcms.Web.admin.IntegralGoods.ashx
{
    /// <summary>
    /// PictureUpload 的摘要说明
    /// </summary>
    public class PictureUpload : IHttpHandler
    {

        public int return_code;
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string Json = "";
            Json = Upload(context);
            context.Response.Write(Json);
        }
        public string Upload(HttpContext context)
        {
            string Json = "";
            HttpPostedFile _upfile = context.Request.Files["File"];
            if (_upfile.ContentLength < 500000)
            {
                if (string.IsNullOrEmpty(_upfile.FileName))
                {
                    return_code = 1;
                    string error = "请上传图片";
                    Json = DTcms.Common.GetJson.GetErrorJson(return_code, 0, error);
                }
                string fileFullname = _upfile.FileName;
                string dataName = DateTime.Now.ToString("yyyyMMddhhmmss");
                string fileName = fileFullname.Substring(0, fileFullname.LastIndexOf("."));
                string type = fileFullname.Substring(fileFullname.LastIndexOf(".") + 1);
                if (type == "bmp" || type == "jpg" || type == "gif" || type == "JPG" || type == "BMP" || type == "GIF"|| type == "PNG")
                {
                    string path = HttpContext.Current.Server.MapPath("~/photo");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    string imgUrl = path + "/" + dataName + "." + type;
                    _upfile.SaveAs(imgUrl);
                    var ip = context.Request.Url.Host;
                    var port = context.Request.Url.Port;
                    var ImgUrl = "http://" + ip + ":" + port + "/photo/" + dataName + "." + type;
                    HttpCookie cookie = new HttpCookie("photo");
                    return_code = 0;
                    string success = "上传成功！";
                    Json = DTcms.Common.GetJson.GetPirtueJson(return_code, success, ImgUrl);
                }
                else
                {
                    return_code = 1;
                    string error = "支持格式：|jpg|gif|bmp|";
                    Json = DTcms.Common.GetJson.GetErrorJson(return_code, 0, error);
                }
            }
            else
            {
                return_code = 1;
                string error = "你的图片已经超过500K的大小！";
                Json = DTcms.Common.GetJson.GetErrorJson(return_code, 0, error);
            }
            return Json;
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