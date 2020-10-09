using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace Sk_B2BAPI.Controllers
{
    public class CustomFormController : Controller
    {
        // GET: CustomForm
        /// <summary>
        /// 前端自定义表单接口，带分页
        /// </summary>
        /// <param name="id"></param>
        /// <param name="where"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult GetDataPager(int id,string where,int pageIndex,int pageSize)
        {
            // 各个参数
            //int id = Convert.ToInt32(context.Request.QueryString["id"]);
            //int pageIndex = Convert.ToInt32(context.Request.Form["PageIndex"]);
            //int pageSize = Convert.ToInt32(context.Request.Form["PageSize"]);
            //string where = context.Request.Form["Where"];

            // 获取自定义表单数据库内容
            var model = DAL.CustomFormDal.GetCustomFormDAL.GetSingleCustomForm(id, false);

            // 获取查询字段,替换SQLwhere条件值
            if (!string.IsNullOrWhiteSpace(where))
            {
                var fields = JsonConvert.DeserializeObject<List<Admin.customform.entity.Fields>>(model.Fields);
                var whereDic = JsonConvert.DeserializeObject<IDictionary<string, string>>(where);
                foreach (var f in fields)
                {
                    model.SQL = model.SQL.Replace(f.field, whereDic.FirstOrDefault(k => k.Key == f.field).Value);
                }
            }
            // 获取数据源
            int recordcount = 0, pagecount = 0;
            DataSet dataset = DAL.CustomFormDal.GetCustomFormDAL.GetCustomSQLData(model.SQL, pageIndex, pageSize, ref recordcount, ref pagecount);
            pagecount = pagecount == 0 ? 1 : pagecount;
            // 组装数据td
            DataTable data = dataset.Tables[1];
            var datalist = new { data = data, recordcount = recordcount, pagecount = pagecount, IsSuccess = true, Message = "获取成功" };
            string result = JsonConvert.SerializeObject(datalist);
            // 输出
            return Json(result,JsonRequestBehavior.AllowGet);
        }
    }
}