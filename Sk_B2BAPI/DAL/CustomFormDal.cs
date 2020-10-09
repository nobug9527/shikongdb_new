using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sk_B2BAPI.Tool;
using Sk_B2BAPI.Admin.customform.entity;
using Sk_B2BAPI.App_Code;
using System.Data.SqlClient;
using System.Data;
using Aop.Api.Domain;
using NPOI.HSSF.Model;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;

namespace Sk_B2BAPI.DAL
{
    public class CustomFormDal
    {
        public readonly static CustomFormDal GetCustomFormDAL = new CustomFormDal();
        /// <summary>
        /// 插入自定义表单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int InsertCustomForm(Models.CustomForm model)
        {
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            SqlParameter[] sqlparams = new SqlParameter[] {
                new SqlParameter("@name",model.Name),
                new SqlParameter("@fields",model.Fields),
                new SqlParameter("@sql",model.SQL),
                new SqlParameter("@moduleid",model.ModuleID)
            };
            int result = sql.ExecuteNonQuery("Proc_AddCustomForm", sqlparams);
            return result;
        }
        /// <summary>
        /// 修改自定义表单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateCustomForm(Models.CustomForm model)
        {
            SqlRun sqlhelper = new SqlRun(SqlRun.sqlstr);
            string sql = "update dt_custom_form set [Name]=@name,[Fields]=@fields,[SQL]=@sql,ModuleID=@moduleid where [ID]=@id";
            SqlParameter[] sqlparams = new SqlParameter[] {
                new SqlParameter("@name",model.Name),
                new SqlParameter("@fields",model.Fields),
                new SqlParameter("@sql",model.SQL),
                new SqlParameter("@moduleid",model.ModuleID),
                new SqlParameter("@id",model.ID)
            };
            int result = sqlhelper.ExecuteSql(sql, sqlparams);
            return result;
        }
        /// <summary>
        /// 根据ID获取单条自定义表单
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isDefaultValue">是否设置搜索字段的默认值</param>
        /// <returns></returns>
        public Models.CustomForm GetSingleCustomForm(int id,bool isDefaultValue)
        {
            SqlRun sqlhelper = new SqlRun(SqlRun.sqlstr);
            string sql = "select * from dt_custom_form where [ID]=" + id;
            DataTable dt = sqlhelper.RtDataTable(sql);
            Models.CustomForm model = new Models.CustomForm();
            if (dt.Rows.Count > 0)
            {
                model.ID = Convert.ToInt32(dt.Rows[0]["ID"]);
                model.Name = dt.Rows[0]["Name"].ToString();
                model.SQL = dt.Rows[0]["SQL"].ToString();
                model.Fields = dt.Rows[0]["Fields"].ToString();
                List<Fields> fieldsList = JsonConvert.DeserializeObject<List<Fields>>(model.Fields);
                // 使用默认值读数据
                if (isDefaultValue)
                {
                    foreach (var f in fieldsList)
                    {
                        if (f.type == "date" && string.IsNullOrWhiteSpace(f.defaultValue))
                            f.defaultValue = DateTime.Now.Date.ToString("yyyy-MM-dd HH:mm:ss");
                        model.SQL = model.SQL.Replace(f.field, f.defaultValue);
                    }
                }
            }
            return model;
        }
        /// <summary>
        /// 获取自定义数据源，根据自定义SQL语句,带分页
        /// </summary>
        /// <param name="sql">数据源SQL语句</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">单页数据量</param>
        /// <param name="total">总数据量</param>
        /// <returns></returns>
        public DataSet GetCustomSQLData(string sql,int pageIndex,int pageSize,ref int recordCount,ref int pageCount)
        {
            SqlRun sqlhelper = new SqlRun(SqlRun.sqlstr);
            SqlParameter[] sqlParams = new SqlParameter[] {
                new SqlParameter("@sql",sql),
                new SqlParameter("@page",pageIndex),
                new SqlParameter("@pagesize",pageSize),
                new SqlParameter("@pageCount",0),//总页数
                new SqlParameter("@recordCount",0),//总记录数
                new SqlParameter("@SearchTime",0)//执行时间 毫秒
            };
            sqlParams[3].Direction = ParameterDirection.Output;
            sqlParams[4].Direction = ParameterDirection.Output;
            sqlParams[5].Direction = ParameterDirection.Output;
            DataSet dataSet = sqlhelper.RunProDataSet("Paging", sqlParams);
            recordCount = Convert.ToInt32(sqlParams[4].Value);
            pageCount= Convert.ToInt32(sqlParams[3].Value);
            for (int i = 0; i < dataSet.Tables.Count; i++)
            {
                dataSet.Tables[i].Columns.Remove("ROWSTAT");
            }        
            return dataSet;
        }
        /// <summary>
        /// 获取自定义表单列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="souStr"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public DataTable GetCustomList(int pageIndex, int pageSize, string souStr,ref int recordCount)
        {
            SqlRun sqlhelper = new SqlRun(SqlRun.sqlstr);
            SqlParameter[] sqlParams = new SqlParameter[] {
                new SqlParameter("@SouStr",souStr),
                new SqlParameter("@PageIndex",pageIndex),
                new SqlParameter("@PageSize",pageSize),
                new SqlParameter("@PageCount",0),
                new SqlParameter("@RecordCount",0),
                new SqlParameter("@SearchTime",0)
            };
            sqlParams[3].Direction = ParameterDirection.Output;
            sqlParams[4].Direction = ParameterDirection.Output;
            sqlParams[5].Direction = ParameterDirection.Output;
            DataSet dataSet = sqlhelper.RunProDataSet("Proc_CustomForm_List", sqlParams);
            recordCount = Convert.ToInt32(sqlParams[4].Value);
            return dataSet.Tables[1];
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool BatchDelete(int[] ids)
        {
            if (ids.Length == 0)
                return false;
            SqlRun sqlhelper = new SqlRun(SqlRun.sqlstr);
            int flag = 0;
            for (int i=0;i<ids.Length;i++)
            {
                SqlParameter[] sqlParams = new SqlParameter[] { new SqlParameter("@id", ids[i]) };
                flag+=sqlhelper.ExecuteNonQuery("Proc_DeleteCustomForm", sqlParams);
            }
            if (flag > 0)
                return true;
            else
                return false;
        }
    }
}