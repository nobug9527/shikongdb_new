using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Net;
using System.Text;
using System.IO; 

namespace DTcms.Web.admin.FactoryClerk
{
    public partial class ceshi : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            //源码是替换掉模板中的特征字符 
            string mbPath = Server.MapPath("IntegralWater.aspx");
            Encoding code = Encoding.GetEncoding("gb2312");
            StreamReader sr = null;
            StreamWriter sw = null;
            string str = null;
            //读取 
            try
            {
                sr = new StreamReader(mbPath, code);
                str = sr.ReadToEnd();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sr.Close();
            }
            //根据时间自动重命名，扩展名也可以自行修改 
            string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".htm";
            str = str.Replace("$title$", txtTitle.Text);//替换Title 
            str = str.Replace("$content$", txtContent.Text);//替换content 
            //生成静态文件 
            try
            {
                sw = new StreamWriter(Server.MapPath("html/") + fileName, false, code);
                sw.Write(str);
                sw.Flush();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sw.Close();
                Response.Write("恭喜<a href=html/" + fileName + " target=_blank>" + fileName + "</a>已经生成，保存在html文件夹下！");
            }
        }
        protected void Button2_Click(object sender, EventArgs e)
        {
            Encoding code = Encoding.GetEncoding("utf-8");
            StreamReader sr = null;
            StreamWriter sw = null;
            string str = null;
            //读取远程路径 
            WebRequest temp = WebRequest.Create(txtUrl.Text.Trim());
            WebResponse myTemp = temp.GetResponse();
            sr = new StreamReader(myTemp.GetResponseStream(), code);
            //读取 
            try
            {
                sr = new StreamReader(myTemp.GetResponseStream(), code);
                str = sr.ReadToEnd();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sr.Close();
            }
            string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".htm";
            //写入 
            try
            {
                sw = new StreamWriter(Server.MapPath("htm/") + fileName, false, code);
                sw.Write(str);
                sw.Flush();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sw.Close();
                Response.Write("恭喜<a href=htm/" + fileName + " target=_blank>" + fileName + "</a>已经生成，保存在htm文件夹下！");
            }
        } 
    }
}