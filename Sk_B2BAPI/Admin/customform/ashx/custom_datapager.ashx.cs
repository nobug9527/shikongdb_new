using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Sk_B2BAPI.Tool;
using Newtonsoft.Json;
using System.Web.Helpers;
using System.Dynamic;
using System.Web.SessionState;

namespace Sk_B2BAPI.Admin.customform
{
    /// <summary>
    /// custom_datapager 的摘要说明
    /// </summary>
    public class custom_datapager : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            //判断登陆
            Models.Admin.Dt_User user = App_Code.RoleFuns.IsLoginAdmin(context.Session["user"]);
            if (user == null)
            {
                var tempResult = new { IsSuccess = false, Message = "您未登录，或登录超时，请登录后操作" };
                context.Response.Write(JsonConvert.SerializeObject(tempResult));
                return;
            }
            // 各个参数
            int id=Convert.ToInt32(context.Request.QueryString["id"]);
            int pageIndex = Convert.ToInt32(context.Request.Form["PageIndex"]);
            int pageSize= Convert.ToInt32(context.Request.Form["PageSize"]);
            string where = context.Request.Form["Where"];

            // 获取自定义表单数据库内容
            var model = DAL.CustomFormDal.GetCustomFormDAL.GetSingleCustomForm(id,false);

            // 获取查询字段,替换SQLwhere条件值
            if (!string.IsNullOrWhiteSpace(where))
            {
                var fields = JsonConvert.DeserializeObject<List<entity.Fields>>(model.Fields);
                var whereDic = JsonConvert.DeserializeObject<IDictionary<string, string>>(where);
                foreach (var f in fields)
                {
                    if (f.isShow)
                        model.SQL = model.SQL.Replace(f.field, whereDic.FirstOrDefault(k => k.Key == f.field).Value);
                    else
                        model.SQL = model.SQL.Replace(f.field, f.defaultValue);
                }
            }
            // 获取数据源
            int recordcount = 0, pagecount = 0;
            DataSet dataset = DAL.CustomFormDal.GetCustomFormDAL.GetCustomSQLData(model.SQL, pageIndex, pageSize, ref recordcount, ref pagecount);
            pagecount = pagecount == 0 ? 1 : pagecount;
            // 组装数据td
            DataTable data = dataset.Tables[1];
            var datalist = new { data = data, total = recordcount , pagecount= pagecount, IsSuccess = true, Message = "获取成功" };
            string result = JsonConvert.SerializeObject(datalist);
            // 输出
            context.Response.Write(result);
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