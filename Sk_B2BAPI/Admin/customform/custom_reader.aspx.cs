using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Sk_B2BAPI.Tool;
using Newtonsoft.Json;
using SqlSugar;
using System.Data;
using Sk_B2BAPI.App_Code;

namespace Sk_B2BAPI.Admin.customform
{
    public partial class custom_reader : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                WriteContent();
            }
        }

        #region 获取模板
        private static volatile string _tempPlate;
        private static object lockhelper = new object();
        /// <summary>
        /// 模板内容
        /// </summary>
        private string TempPlate
        {
            get
            {
                if (string.IsNullOrEmpty(_tempPlate))
                {
                    lock (lockhelper)
                    {
                        if (string.IsNullOrEmpty(_tempPlate))
                        {
                            _tempPlate = FileUtil.Read(Server.MapPath("custom_template.html"), Encoding.UTF8);
                        }
                    }
                }
                return _tempPlate;
            }
        }
        #endregion

        #region 写页面内容
        private void WriteContent()
        {
            // 获得id和页码
            int id = Convert.ToInt32(Request.QueryString["id"]);
            // 获取自定义表单内容
            var model = DAL.CustomFormDal.GetCustomFormDAL.GetSingleCustomForm(id,true);
            // 获得自定义查询字段
            List<entity.Fields> fields = JsonConvert.DeserializeObject<List<entity.Fields>>(model.Fields);
            // 组装搜索框内容
            StringBuilder tempfields = new StringBuilder("");
            foreach (var f in fields)
            {
                if (f.isShow)
                {
                    if (f.type == "text")
                    {
                        tempfields.Append(f.name + "：<input type=\"text\" class=\"input-text sou\" name=\"" + f.field + "\" value=\"" + f.defaultValue + "\" style=\"width:150px;margin-right:10px;\">");
                    }
                    else if (f.type == "int")
                    {
                        tempfields.Append(f.name + "：<span class=\"select-box\" style=\"width:150px;margin-right:10px;\"><select name=\"" + f.field + "\" class=\"select sou\" size=\"1\">");
                        foreach (var s in f.selectItem)
                        {
                            tempfields.Append("<option value=\"" + s.value + "\"" + (f.defaultValue == s.value ? " selected" : "") + ">" + s.text + "</ option >");
                        }
                        tempfields.Append("</select></span>");
                    }
                    else if (f.type == "date")
                    {
                        tempfields.Append(f.name + "：<input type=\"text\" name=\"" + f.field + "\" value=\"" + f.defaultValue + "\" class=\"input-text sou Wdate\" onfocus=\"WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss',readOnly:true,isShowClear:false})\" style=\"width:165px;margin-right:10px;\">");
                    }
                }
            }
            // 获得数据
            int recordcount=0,pagecount = 0;
            DataSet dataset = DAL.CustomFormDal.GetCustomFormDAL.GetCustomSQLData(model.SQL, 1, 10, ref recordcount,ref pagecount);
            DataTable chnnameDt = GetColumnName.GetChinaName(dataset.Tables[0]);
            pagecount = pagecount == 0 ? 1 : pagecount;
            // 组装表头
            StringBuilder tempth = new StringBuilder("");
            for (int i = 0; i < dataset.Tables[0].Columns.Count; i++)
            {
                string b = string.Empty;
                for (int j = 0; j < chnnameDt.Rows.Count; j++)
                {
                    if (dataset.Tables[0].Columns[i].ColumnName.Equals(chnnameDt.Rows[j]["fdname"].ToString()))
                    {
                        b = chnnameDt.Rows[j]["chnname"].ToString();
                        continue;
                    }
                }
                if(!string.IsNullOrEmpty(b))
                    tempth.Append("<th>"+ b+ "</th>");
                else
                    tempth.Append("<th>" + dataset.Tables[0].Columns[i].ColumnName + "</th>");
            }
            // 组装表格
            StringBuilder temptd = new StringBuilder("");
            for (int i = 0; i < dataset.Tables[1].Rows.Count; i++)
            {
                temptd.Append("<tr class=\"text-c\">");
                temptd.Append("<td align=\"center\">" + (i + 1) + "</td>");
                for (int j = 0; j < dataset.Tables[1].Columns.Count; j++)
                {
                    temptd.Append("<td>" + dataset.Tables[1].Rows[i][j] + "</td>");
                }
                temptd.Append("</tr>");
            }
            // 组装JS表格程序
            StringBuilder tempjsdt = new StringBuilder("");
            for (int i = 0; i < dataset.Tables[0].Columns.Count; i++)
            {
                if (i == 0)
                {
                    tempjsdt.Append("\"<td>\"+obj[i][\"" + dataset.Tables[0].Columns[i].ColumnName + "\"]+\"</td>\";");
                    continue;
                }
                tempjsdt.Append("html+=\"<td>\"+obj[i][\"" + dataset.Tables[0].Columns[i].ColumnName + "\"]+\"</td>\";");
            }
            // 获取模板
            StringBuilder tempplate = new StringBuilder(this.TempPlate);
            // 替换模板标签
            tempplate = tempplate.Replace("{#custom_name}", model.Name)
                .Replace("{#custom_fields}", tempfields.ToString())
                .Replace("{#custom_th}", tempth.ToString())
                .Replace("{#custom_td}", temptd.ToString())
                .Replace("{#custom_jstd}", tempjsdt.ToString())
                .Replace("{#custom_recordcount}", recordcount.ToString())
                .Replace("{#custom_pagecount}", pagecount.ToString())
                .Replace("{#custom_data_url}", "ashx/custom_datapager.ashx?id=" + id);
            // 输出
            Response.Write(tempplate);
        }
        #endregion
    }
}