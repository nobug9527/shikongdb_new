using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sk_B2BAPI.Tool;
using Newtonsoft.Json;
using Sk_B2BAPI.DAL;
using Sk_B2BAPI.App_Code;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Data;
using Sk_B2BAPI.Models.Admin;
using System.Web.SessionState;

namespace Sk_B2BAPI.Admin.customform
{
    /// <summary>
    /// custom_handle 的摘要说明
    /// </summary>
    public class custom_handle : IHttpHandler, IRequiresSessionState
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

            string type = context.Request.Form["type"].Trim();
            if (type == "add")
            {
                Add(context);
            }
            else if (type == "list")
            {
                List(context);
            }
            else if (type == "det")
            {
                Det(context);
            }
            else if (type == "upt")
            {
                Upt(context);
            }
            else if (type == "single")
            {
                Single(context);
            }
        }

        #region 添加
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="context"></param>
        private void Add(HttpContext context)
        {
            entity.ResponseResult result = new entity.ResponseResult();
            try
            {
                Models.CustomForm model = new Models.CustomForm();
                model.Name = context.Request.Form["Name"];
                List<entity.Fields> fields = JsonConvert.DeserializeObject<List<entity.Fields>>(context.Request.Form["Fields"]);
                model.Fields = JsonConvert.SerializeObject(fields);
                model.SQL = context.Request.Form["SQL"];
                model.ModuleID = context.Request.Form["ModuleID"];
                if (string.IsNullOrWhiteSpace(model.Name) || string.IsNullOrWhiteSpace(model.SQL) || fields.Count == 0)
                {
                    result.IsSuccess = false;
                    result.Message = "自定义内容，全部都为必填项。";
                }
                else
                {
                    if (model.SQL.ToLower().Trim().StartsWith("select") == false)
                    {
                        result.IsSuccess = false;
                        result.Message = "SQL语句只能为select语句。";
                    }
                    else
                    {
                        string validSqlMsg = "";
                        bool isValidSql = ValidateSQL(model.SQL, fields,ref validSqlMsg);
                        if (!isValidSql)
                        {
                            result.IsSuccess = false;
                            result.Message = validSqlMsg;
                        }
                        else
                        {
                            int flag = CustomFormDal.GetCustomFormDAL.InsertCustomForm(model);
                            if (flag > 0)
                            {
                                result.IsSuccess = true;
                                result.Message = "添加成功。";
                            }
                            else
                            {
                                result.IsSuccess = false;
                                result.Message = "添加失败，请稍后重试。";
                            }
                        }
                    }
                }
            }
            catch(Exception)
            {
                result.IsSuccess = false;
                result.Message = "添加失败，请稍后重试。";
            }
            context.Response.Write(JsonConvert.SerializeObject(result));
        }
        #endregion

        #region 获取管理列表
        /// <summary>
        /// 获取管理列表
        /// </summary>
        /// <param name="context"></param>
        private void List(HttpContext context)
        {
            int pageIndex=Convert.ToInt32(context.Request.Form["PageIndex"]);
            int pageSize = Convert.ToInt32(context.Request.Form["PageSize"]);
            string souStr = context.Request.Form["SouStr"];
            int recordCount = 0;
            DataTable dt = CustomFormDal.GetCustomFormDAL.GetCustomList(pageIndex, pageSize, souStr, ref recordCount);
            var data = new { total = recordCount, data = dt ,IsSuccess = true, Message = "获取成功" };
            string result = JsonConvert.SerializeObject(data);
            context.Response.Write(result);
        }
        #endregion

        #region 批量删除
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="context"></param>
        private void Det(HttpContext context)
        {
            entity.ResponseResult result = new entity.ResponseResult();
            int[] ids = JsonConvert.DeserializeObject<int[]>(context.Request.Form["ids"]);
            bool flag = DAL.CustomFormDal.GetCustomFormDAL.BatchDelete(ids);
            if (flag)
            {
                result.IsSuccess = true;
                result.Message = "删除成功";
            }
            else
            {
                result.IsSuccess = false;
                result.Message = "删除失败，请稍后重试";
            }
            context.Response.Write(JsonConvert.SerializeObject(result));
        }
        #endregion

        #region 修改
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="context"></param>
        private void Upt(HttpContext context)
        {
            entity.ResponseResult result = new entity.ResponseResult();
            try
            {
                Models.CustomForm model = new Models.CustomForm();
                model.ID = Convert.ToInt32(context.Request.Form["ID"]);
                model.Name = context.Request.Form["Name"];
                List<entity.Fields> fields = JsonConvert.DeserializeObject<List<entity.Fields>>(context.Request.Form["Fields"]);
                model.Fields = JsonConvert.SerializeObject(fields);
                model.SQL = context.Request.Form["SQL"];
                model.ModuleID = context.Request.Form["ModuleID"];
                if (string.IsNullOrWhiteSpace(model.Name) || fields.Count == 0 || string.IsNullOrWhiteSpace(model.SQL) || model.ID==0)
                {
                    result.IsSuccess = false;
                    result.Message = "自定义内容，全部都为必填项。";
                }
                else
                {
                    if (model.SQL.ToLower().Trim().StartsWith("select") == false)
                    {
                        result.IsSuccess = false;
                        result.Message = "SQL语句只能为select语句。";
                    }
                    else
                    {
                        string validSqlMsg = "";
                        bool isValidSql = ValidateSQL(model.SQL, fields, ref validSqlMsg);
                        if (!isValidSql)
                        {
                            result.IsSuccess = false;
                            result.Message = validSqlMsg;
                        }
                        else
                        {
                            int flag = CustomFormDal.GetCustomFormDAL.UpdateCustomForm(model);
                            if (flag > 0)
                            {
                                result.IsSuccess = true;
                                result.Message = "修改成功。";
                            }
                            else
                            {
                                result.IsSuccess = false;
                                result.Message = "修改失败，请稍后重试。";
                            }
                        }
                    }
                }
            }
            catch
            {
                result.IsSuccess = false;
                result.Message = "修改失败，请稍后重试。";
            }
            context.Response.Write(JsonConvert.SerializeObject(result));
        }
        #endregion

        #region 获取单个
        private void Single(HttpContext context)
        {
            int id = Convert.ToInt32(context.Request.Form["id"]);
            var model = DAL.CustomFormDal.GetCustomFormDAL.GetSingleCustomForm(id,false);
            var data = new { data = model, IsSuccess = true , Message ="获取成功"};
            string result = JsonConvert.SerializeObject(data);
            context.Response.Write(result);
        }
        #endregion

        #region 根据默认参数值 ，验证输入的SQL语句正确性
        /// <summary>
        /// 根据默认值 ，验证输入的SQL语句正确性
        /// </summary>
        /// <returns></returns>
        private bool ValidateSQL(string sql, List<entity.Fields> fields,ref string validSqlMsg)
        {
            SqlRun sqlHelper = new SqlRun(SqlRun.sqlstr);
            foreach (var f in fields)
            {
                if (f.type == "date" && string.IsNullOrWhiteSpace(f.defaultValue))
                    f.defaultValue = DateTime.Now.Date.ToString("yyyy-MM-dd HH:mm:ss");
                if (f.type == "int" && string.IsNullOrWhiteSpace(f.defaultValue))
                    f.defaultValue = "1";
                sql = sql.Replace(f.field, f.defaultValue);
            }
            return sqlHelper.ValidateSQL(sql, ref validSqlMsg);
        }
        #endregion
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}