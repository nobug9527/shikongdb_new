using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace DTcms.Web.admin.IntegralGoods.ashx
{
    /// <summary>
    /// IntegralGoodsOperation 的摘要说明
    /// </summary>
    public class IntegralGoodsOperation : IHttpHandler
    {

        #region ================type判断执行操作================

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string Json = "";
            try
            {
                string type = context.Request.QueryString["type"].ToString().Trim();
                if (type == "Integral_Goods_Insert")
                {
                    Json = insert(context);
                }
                else if (type == "Integral_Goods_Update")
                {
                    Json = Update(context);
                }
                else if (type == "Integral_Goods_SelectByGoodsId")
                {
                    Json = Selectbygoodsid(context);
                }
                else if (type == "Integral_Goods_SelectAllByGoodsType")
                {
                    Json = Selectallbygoodstype(context);
                }
                else if (type == "Integral_Goods_SelectTop*ByGoodsType")
                {
                    Json = Selecttopbygoodstype(context);
                }
            }
            catch (Exception e)
            {
                int return_code = 2;
                string err = e.Message.ToString().Trim().Replace("\"", "\\\"").Replace("\r", "").Replace("\n", "");
                Json = DTcms.Common.GetJson.GetErrorJson(return_code, 0, err);
            }

            context.Response.Write(Json);
        }
        #endregion

        #region ==============新增积分商品数据===============

        public string insert(HttpContext context)
        {
            int rowsAffected = 0;
            string Json = "";
            string goodsName = context.Request.QueryString["GoodsName"].ToString().Trim();
            string goodsSpecifications = context.Request.QueryString["GoodsSpecifications"].ToString().Trim();
            string unit = context.Request.QueryString["Unit"].ToString().Trim();
            string classification = context.Request.QueryString["Classification"].ToString().Trim();
            string floor = context.Request.QueryString["Floor"].ToString().Trim();
            string hiddenUrl = context.Request.QueryString["HiddenUrl"].ToString().Trim();
            string vendel = context.Request.QueryString["Vendel"].ToString().Trim();
            string price = context.Request.QueryString["Price"].ToString().Trim();
            string Integral = context.Request.QueryString["Integral"].ToString().Trim();
            string Procedure = context.Request.QueryString["Procedure"].ToString().Trim();
            string type = context.Request.QueryString["type"].ToString().Trim();
            string Quantity = context.Request.QueryString["Quantity"].ToString().Trim();
            SqlParameter[] parameter = new SqlParameter[] 
           { 
           new SqlParameter("@TYPE",type),
           new SqlParameter("@goodsname",goodsName),
           new SqlParameter("@drug_spec",goodsSpecifications),
           new SqlParameter("@package_unit",unit),
           new SqlParameter("@goodstype",classification),
           new SqlParameter("@floottype",floor),
           new SqlParameter("@imageurl",hiddenUrl),
           new SqlParameter("@vendel",vendel),
           new SqlParameter("@price",price),
           new SqlParameter("@integral",Integral),
          new SqlParameter("@stock",Quantity)

           };
            int n = DTcms.DBUtility.DbHelperSQL.RunProcedure(Procedure, parameter, out rowsAffected);
            if (rowsAffected != 0)
            {
                Json = DTcms.Common.GetJson.GetErrorJson(0, 0, "操作成功！");
            }
            else
            {
                Json = DTcms.Common.GetJson.GetErrorJson(1, 0, "操作失败！");
            }
            return Json;
        }
        #endregion

        #region =============更新积分商品数据=============

        public string Update(HttpContext context)
        {
            int rowsAffected = 0;
            string Json = "";
            var goodsId = context.Request.QueryString["goodsid"].ToString().Trim();
            var goodsName = context.Request.QueryString["goodsname"].ToString().Trim();
            var drug_spec = context.Request.QueryString["drug_spec"].ToString().Trim();
            var package_unit = context.Request.QueryString["package_unit"].ToString().Trim();
            var FlootType = context.Request.QueryString["FlootType"].ToString().Trim();
            var GoodsType = context.Request.QueryString["GoodsType"].ToString().Trim();
            var Price = context.Request.QueryString["Price"].ToString().Trim();
            var img_url = context.Request.QueryString["img_url"].ToString().Trim();
            var Integral = context.Request.QueryString["Integral"].ToString().Trim();
            var factories_choosing = context.Request.QueryString["factories_choosing"].ToString().Trim();
            var type = context.Request.QueryString["type"].ToString().Trim();
            var Procedure = context.Request.QueryString["Procedure"].ToString().Trim();
            var Quantity = context.Request.QueryString["Quantity"].ToString().Trim();
            SqlParameter[] Parameter = new SqlParameter[]{
            new SqlParameter("@TYPE",type),
            new SqlParameter("@goodsname",goodsName),
            new SqlParameter("@drug_spec",drug_spec),
            new SqlParameter("@package_unit",package_unit),
            new SqlParameter("@floottype",FlootType),
            new SqlParameter("@goodstype",GoodsType),
            new SqlParameter("@price",Price),
            new SqlParameter("@imageurl",img_url),
            new SqlParameter("@integral",Integral),
            new SqlParameter("@vendel",factories_choosing),
            new SqlParameter("@goodsid",goodsId),
            new SqlParameter("@Quantity",Quantity)
            };
            int n = DTcms.DBUtility.DbHelperSQL.RunProcedure(Procedure, Parameter, out rowsAffected);
            if (rowsAffected != 0)
            {
                Json = DTcms.Common.GetJson.GetErrorJson(0, 0, "操作成功！");
            }
            else
            {
                Json = DTcms.Common.GetJson.GetErrorJson(1, 0, "操作失败！");
            }
            return Json;
        }
        #endregion

        #region ===========goodsid查询积分商品数据============

        public string Selectbygoodsid(HttpContext context)
        {
            string Json = "";
            int return_code = 0;
            string goodsid = context.Request.QueryString["goodsid"].ToString().Trim();
            string type = context.Request.QueryString["type"].ToString().Trim();
            string Procedure = context.Request.QueryString["Procedure"].ToString().Trim();
            SqlParameter[] Parameter = new SqlParameter[] { 
             new SqlParameter("@TYPE",type),
             new SqlParameter("@goodsid",goodsid)
            };
            DataSet ds = DTcms.DBUtility.DbHelperSQL.RunProcedure(Procedure, Parameter, "Single");

            if (ds.Tables["Single"].Rows.Count == 1)
            {
                return_code = 0;
                int recordCount = 0, pageCount = 0;
                Json = DTcms.Common.GetJson.GetDataJson(return_code, recordCount, pageCount, ds.Tables["Single"]);
            }
            else if (ds.Tables["Single"].Rows.Count < 1)
            {
                return_code = 1;
                string error = "无法获取数据";
                Json = DTcms.Common.GetJson.GetErrorJson(return_code, 0, error);
            }
            else if (ds.Tables["Single"].Rows.Count > 1)
            {
                return_code = 1;
                string error = "商品编号不唯一";
                Json = DTcms.Common.GetJson.GetErrorJson(return_code, 0, error);
            }
            return Json;
        }
        #endregion

        #region ==========goodstype查询所有积分商品数据============

        public string Selectallbygoodstype(HttpContext context)
        {
            string Json = "", error = "";
            int return_code = 0;
            var type = context.Request.QueryString["type"].ToString().Trim();
            var Procedure = context.Request.QueryString["procedure"].ToString().Trim();
            var goodstype = context.Request.QueryString["goodstype"].ToString().Trim();
            SqlParameter[] Parameter = new SqlParameter[] { 
            new SqlParameter("@TYPE",type),
            new SqlParameter("@goodstype",goodstype)};
            DataSet ds = DTcms.DBUtility.DbHelperSQL.RunProcedure(Procedure, Parameter, "All");
            if (ds.Tables["All"].Rows.Count > 0)
            {
                return_code = 0;
                Json = DTcms.Common.GetJson.GetDataJson(return_code, 0, 0, ds.Tables["All"]);
            }
            else if (ds.Tables["All"].Rows.Count <= 0)
            {
                return_code = 1;
                error = "查询数据为空";
                Json = DTcms.Common.GetJson.GetErrorJson(return_code, 0, error);
            }
            return Json;
        }
        #endregion

        #region ===========goodstype查询Top(n)个积分商品数据==========
        public string Selecttopbygoodstype(HttpContext context)
        {
            string Json = "", error = "";
            int return_code = 0;
            var type = context.Request.QueryString["type"].ToString().Trim();
            var Procedure = context.Request.QueryString["Procedure"].ToString().Trim();
            var sum = context.Request.QueryString["sum"].ToString().Trim();
            var goodstype = context.Request.QueryString["goodstype"].ToString().Trim();
            SqlParameter[] Parameter = new SqlParameter[] { 
            new SqlParameter("@TYPE",type),
            new SqlParameter("@sum",sum),
            new SqlParameter("@goodstype",goodstype)};
            DataSet ds = DTcms.DBUtility.DbHelperSQL.RunProcedure(Procedure, Parameter, "Top");
            if (ds.Tables["Top"].Rows.Count > 0)
            {
                return_code = 0;
                Json = DTcms.Common.GetJson.GetDataJson(return_code, 0, 0, ds.Tables["Top"]);
            }
            else if (ds.Tables["Top"].Rows.Count <= 0)
            {
                return_code = 1;
                error = "查询数据为空";
                Json = DTcms.Common.GetJson.GetErrorJson(return_code, 0, error);
            }
            return Json;
        }
        #endregion

        #region ===========goodsname查询积分商品==========
        public string Selectbygoodsname(HttpContext context)
        {
            string Json = "";
            int return_code;
            var type = context.Request.QueryString["type"].ToString().Trim();
            var Precdure = context.Request.QueryString["Precdure"].ToString().Trim();
            var goodsname = context.Request.QueryString["goodsname"].ToString().Trim();
            SqlParameter[] Parameter = new SqlParameter[] { 
            new SqlParameter("@TYPE",type),
            new SqlParameter("@goodsname",goodsname)};
            DataSet ds = DTcms.DBUtility.DbHelperSQL.RunProcedure(Precdure, Parameter, "Select");
            if (ds.Tables["Select"].Rows.Count > 0)
            {
                return_code = 0;
                Json = DTcms.Common.GetJson.GetDataJson(return_code, 0, 0, ds.Tables["Select"]);
            }
            else if (ds.Tables["Select"].Rows.Count <= 0)
            {
                return_code = 1;
                string error = "数据查询为空";
                Json = DTcms.Common.GetJson.GetErrorJson(return_code, 0, error);
            }
            return Json;
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