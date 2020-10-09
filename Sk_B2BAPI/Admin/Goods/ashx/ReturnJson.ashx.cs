using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sk_B2BAPI.App_Code;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using Sk_B2BAPI.Models.Admin;
using System.Collections.Generic;
using System.Web.Helpers;

namespace Sk_B2BAPI.Admin.Goods.ashx
{
    /// <summary>
    /// ReturnJson1 的摘要说明
    /// </summary>
    public class ReturnJson1 : IHttpHandler, IRequiresSessionState
    {
        [ValidateInput(false)]
        [HttpPost]
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string returnJson = "";
            try
            {
                Dt_User user = RoleFuns.IsLoginAdmin(context.Session["user"]);
                if (user != null)//登录检测，权限检测 context.Session["user"]
                {
                    string json = context.Request.Params["json"].Trim();//请求参数(json类型)
                    string type = context.Request.Params["type"].Trim();//请求类型
                    string proc = context.Request.Params["proc"].Trim();//存储过程名称
                    JObject obj = (JObject)JsonConvert.DeserializeObject(json);
                    if (obj["type"] != null)
                    {
                        List<string> rolestr = context.Session["role"]!=null? (List<string>)context.Session["role"]:null;
                        RoleFuns.SetAdminLog(user.username, obj["type"].ToString(), rolestr);
                        //if (/*rolestr == null || RoleFuns.SetAdminLog(user.username, obj["type"].ToString(), rolestr) == 0*/false)
                        //{
                        //    returnJson = JsonMethod.GetError(4, "抱歉您没有权限进入！");
                        //}
                        //else {
                            //执行查询返回列表
                            switch (type)
                            {
                                case "ReturnList":
                                    returnJson = GetReturnJson(json, proc, user.userId, user.entId);
                                    break;
                                case "ReturnNumber":
                                    returnJson = GetReturnJsonInt(json, proc, user.userId, user.entId);
                                    break;
                                case "LoadCheckBox":
                                    returnJson = LoadCheckBox(json, proc, user.userId, user.entId, context);
                                    break;
                                case "ReturnDataSet":
                                    returnJson = ReturnDataSetJson(json, proc, user.userId, user.entId);
                                    break;
                                //case "UpdateGoodsInfo":
                                //    returnJson = UpdateGoodsInfo(json, proc, user.userId, user.entId);
                                //    break;
                                case "GoodsCategory":
                                    returnJson = GetGoodsCategory(json, proc, user.userId, user.entId);
                                    break;
                                case "GoodsRecommendList":
                                    returnJson = GetGoodRecommendList(json);
                                    break;
                                case "GoodsRecommendType":
                                    returnJson = GetGoodsRecommendType();
                                    break;
                                case "AddGoodsRecommend":
                                    returnJson = AddGoodsRecommend(json);
                                    break;
                                case "SingleGoodsRecommend":
                                    returnJson = SingleGoodsRecommend(json);
                                    break;
                                case "StatusGoodsRecommend":
                                    returnJson = StatusGoodsRecommend(json);
                                    break;
                                case "DeleteGoodsRecommend":
                                    returnJson = DeleteGoodsRecommend(json);
                                    break;
                                case "UpdateGoodsRecommend":
                                    returnJson = UpdateGoodsRecommend(json);
                                    break;
                                default:
                                    returnJson = JsonMethod.GetError(4, "参数错误！");
                                    break;
                            }
                        //}
                    }
                  
                }
                else
                {
                    returnJson = JsonMethod.GetError(2, "登陆超时,请重新登陆！");
                }
            }
            catch (Exception e)
            {
                string msg = e.Message.ToString().Trim().Replace("\"", "\\\"").Replace("\r", "").Replace("\n", "");
                returnJson = JsonMethod.GetError(4, msg);
            }
            context.Response.Write(returnJson);
        }
        /// <summary>
        /// 返回DataTable
        /// </summary>
        /// <param name="json"></param>
        /// <param name="proc"></param>
        /// <param name="userId"></param>
        /// <param name="entid"></param>
        /// <returns></returns>
        protected string GetReturnJson(string json, string proc, string userId, string entId)
        {
            SqlParameter[] param = (JsonMethod.ListParameter(json, userId, entId)).ToArray();//动态解析json参数
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataSet ds = sql.RunProDataSet(proc, param);
            string r_json;
            if (ds.Tables.Count >= 3)
            {
                if (ds.Tables[1].Rows.Count > 0)
                {
                    int recordCount = Convert.ToInt32(ds.Tables[2].Rows[0]["recordCount"].ToString().Trim());
                    int pageCount = Convert.ToInt32(ds.Tables[2].Rows[0]["pageCount"].ToString().Trim());
                    DataTable table = sql.GetChinaName(ds.Tables[1]);
                    r_json = JsonMethod.GetDataTable(0, recordCount, pageCount, table, ds.Tables[1]);
                }
                else
                {
                    string error = "无数据";
                    r_json = JsonMethod.GetError(1, error);
                }
            }
            else
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    int recordCount = 0;
                    int pageCount = 0;
                    r_json = JsonMethod.GetDataTable(0, recordCount, pageCount, ds.Tables[0]);
                }
                else
                {
                    string error = "无数据";
                    r_json = JsonMethod.GetError(1, error);
                }
            }
            return r_json;
        }
        /// <summary>
        /// 更新数据目录
        /// </summary>
        /// <param name="json"></param>
        /// <param name="proc"></param>
        /// <returns></returns>
        protected string GetReturnJsonInt(string json, string proc, string userId, string entId)
        {
            SqlParameter[] param = (JsonMethod.ListParameter(json, userId, entId)).ToArray();//动态解析json参数
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            int n = sql.ExecuteNonQuery(proc, param);
            string r_json;
            string msg;
            if (n > 0)
            {
                msg = "更新成功";
                r_json = JsonMethod.GetError(0, msg);
            }
            else
            {
                msg = "更新失败！";
                r_json = JsonMethod.GetError(1, msg);
            }
            return r_json;
        }
        /// <summary>
        /// 加载商品选项分类/属性/状态
        /// </summary>
        /// <returns></returns>
        protected string LoadCheckBox(string json, string proc, string userId, string entId,HttpContext context)
        {
            string checkType = context.Request.QueryString["CheckType"];
            SqlParameter[] param = (JsonMethod.ListParameter(json, userId, entId)).ToArray();//动态解析json参数
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataSet ds = sql.RunProDataSet(proc, param);
            ///加载商品分类
            string categoryList = "";
            DataTable dtc = ds.Tables[0];
            if (dtc.Rows.Count > 0)
            {
                DataRow[] drc_1 = dtc.Select("class_layer=1", "sort_id asc");
                foreach (DataRow dr_c1 in drc_1)
                {
                    categoryList += "<option value='" + dr_c1["id"] + "'>" + dr_c1["title"] + "</option>";
                    DataRow[] drc_2 = dtc.Select("class_layer=2 and parent_id=" + dr_c1["id"] + " ", "sort_id asc");
                    foreach (DataRow dr_c2 in drc_2)
                    {
                        categoryList += "<option value='" + dr_c2["id"] + "'>&nbsp;&nbsp;├" + dr_c2["title"] + "</option>";
                    }
                }
            }
            ///加载商品属性
            string attributeList = "";
            int no = 0;
            DataTable dta = ds.Tables[1];
            if (dta.Rows.Count > 0)
            {
                foreach (DataRow dr_a in dta.Rows)
                {
                    if (checkType == "CheckBox")
                    {
                        attributeList += "<div class='radio-box'><input name='isRecommend' type='checkbox' id='tjCheck_" + no + "' value='" + dr_a["id"] + "' ><label for='status-"+no+"'>" + dr_a["title"] + "</label></div>";
                    }
                    else
                    {
                        attributeList += "<option value='" + dr_a["id"] + "'>" + dr_a["title"] + "</option>";
                    }
                    no++;
                }
            }
            return "{\"flag\":\"0\",\"categoryList\":\"" + categoryList + "\",\"attributeList\":\"" + attributeList + "\"}";
        }
        /// <summary>
        /// 获取多个表返回Json
        /// </summary>
        /// <param name="json"></param>
        /// <param name="proc"></param>
        /// <param name="userId"></param>
        /// <param name="entid"></param>
        /// <returns></returns>
        protected string ReturnDataSetJson(string json, string proc, string userId, string entId)
        {
            SqlParameter[] param = (JsonMethod.ListParameter(json, userId, entId)).ToArray();//动态解析json参数
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataSet ds = sql.RunProDataSet(proc, param);
            string r_json;
            if (ds.Tables.Count > 0)
            {
                r_json = JsonMethod.DataSetToJson("0", ds);
            }
            else
            {
                string error = "无数据";
                r_json = JsonMethod.GetError(1, error);
            }
            return r_json;
        }
        [ValidateInput(false)]
        protected string UpdateGoodsInfo(string json, string proc, string userId, string entId)
        {
            JObject obj = (JObject)JsonConvert.DeserializeObject(json);
            StringBuilder strSql = new StringBuilder();
            //修改商品主表
            strSql.Append("update dt_article set category_id='"+obj["category_id"].ToString().Trim()+"',");
            strSql.Append("title='"+obj["sub_title"].ToString().Trim()+"',img_url='"+obj["img_url"].ToString().Trim()+"',");
            strSql.Append("left_pic='"+obj["left_pic"].ToString().Trim()+"',content='"+obj["content"].ToString().Trim()+"',");
            strSql.Append("brand_img_url='" + obj["brand_img_url"].ToString().Trim() + "',sort_id='" + obj["sort_id"].ToString().Trim() + "' ");
            strSql.Append(" where id='" + obj["article_id"].ToString().Trim() + "' and entid='" + entId + "' ;");
            //修改属性表
            strSql.Append("update dt_article_attribute set sub_title='"+obj["sub_title"].ToString().Trim()+"',");
            strSql.Append("generic='"+obj["generic"].ToString().Trim()+"',drug_factory='"+obj["drug_factory"].ToString().Trim()+"',");
            strSql.Append("approval_number='"+obj["approval_number"].ToString().Trim()+"',drug_spec='"+obj["drug_spec"].ToString().Trim()+"',");
            strSql.Append("big_package='"+obj["big_package"].ToString().Trim()+"',min_package='"+obj["min_package"].ToString().Trim()+"',");
            strSql.Append("package_unit='"+obj["package_unit"].ToString().Trim()+"',Storage_conditions='"+obj["Storage_conditions"].ToString().Trim()+"',");
            strSql.Append("category='"+obj["category"].ToString().Trim()+"',mnemonic_code='"+obj["mnemonic_code"].ToString().Trim()+"',");
            strSql.Append("dosage_form='" + obj["dosage_form"].ToString().Trim() + "',rate='" + obj["rate"].ToString().Trim() + "',brandId='" + obj["BrandCode"].ToString().Trim() + "',");
            strSql.Append("price='"+obj["price"].ToString().Trim()+"',min_package_astrict='"+obj["min_package_astrict"].ToString().Trim()+"'");
            strSql.Append(" where article_id='" + obj["article_id"] + "' and entid='" + entId + "'");
            //status = " + obj["status"].ToString().Trim() + "
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            bool flag = sql.ExecuteSql(strSql.ToString());
            if (flag)
            {
                SqlParameter[] prmt = new SqlParameter[] { 
                    new SqlParameter("@type","GoodsRecommend"),
                    new SqlParameter("@recommendList",obj["recommendList"].ToString()),
                    new SqlParameter("@article_id",obj["article_id"].ToString()),
                    new SqlParameter("@BrandCode",obj["BrandCode"].ToString()),
                    new SqlParameter("@entId",entId),
                };
                int n = sql.ExecuteNonQuery("Proc_Admin_GoodsList", prmt);
                return JsonMethod.GetError(0, "操作成功");
            }
            else
            {
                return JsonMethod.GetError(1, "操作失败");
            }
        }
        /// <summary>
        /// 获取商品分类列表
        /// </summary>
        /// <param name="json"></param>
        /// <param name="proc"></param>
        /// <param name="userId"></param>
        /// <param name="entId"></param>
        /// <returns></returns>
        protected string GetGoodsCategory(string json, string proc, string userId, string entId)
        {
            SqlParameter[] param = (JsonMethod.ListParameter(json, userId, entId)).ToArray();//动态解析json参数
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataTable dt = sql.RunProcedureDR(proc, param);
            string r_json;
            if (dt.Rows.Count > 0)
            {
                r_json = JsonMethod.GoodsCategory(dt);
            }
            else
            {
                string error = "无数据";
                r_json = JsonMethod.GetError(1, error);
            }
            return r_json;
        }
        /// <summary>
        /// 获取购物车推荐的商品列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        private string GetGoodRecommendList(string json)
        {
            dynamic souObj = JsonConvert.DeserializeObject<dynamic>(json);
            int type = Convert.ToInt32(souObj.code);
            int status = Convert.ToInt32(souObj.status);
            string soustr = souObj.soustr;
            int pageindex = Convert.ToInt32(souObj.pageindex);
            int pagesize = Convert.ToInt32(souObj.pagesize);
            int recordcount = 0;
            int pagecount = 0;
            DAL.GoodsInfoDal goodsInfoDal = new DAL.GoodsInfoDal();
            DataTable dataTable = goodsInfoDal.GetGoodsRecommendList(pageindex, pagesize, soustr.Trim().ToUpper(), type, status, ref recordcount, ref pagecount).Tables[1];
            string result=JsonConvert.SerializeObject(new { data = dataTable, recordcount = recordcount, pagecount = pagecount });
            return result;
        }
        /// <summary>
        /// 获取推荐商品类型集合
        /// </summary>
        /// <returns></returns>
        private string GetGoodsRecommendType()
        {
            DAL.GoodsInfoDal goodsInfoDal = new DAL.GoodsInfoDal();
            DataTable dataTable = goodsInfoDal.GetGoodsRecommendType();
            string result = JsonConvert.SerializeObject(new { data = dataTable });
            return result;
        }
        /// <summary>
        /// 添加推荐商品
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        private string AddGoodsRecommend(string json)
        {
            dynamic obj = JsonConvert.DeserializeObject<dynamic>(json);
            int type = Convert.ToInt32(obj.code);
            int status = Convert.ToInt32(obj.status);
            int articleid = Convert.ToInt32(obj.articleid);
            int sort = Convert.ToInt32(obj.sort);
            DAL.GoodsInfoDal goodsInfoDal = new DAL.GoodsInfoDal();
            bool isExist = goodsInfoDal.ExistGoodsRecommend(articleid, type);
            if (isExist) {
                return JsonMethod.GetError(3, "此推荐商品已存在");// "{\"d\":false,\"msg\":\"此推荐商品已存在\"}";
            }
            bool result = goodsInfoDal.AddGoodsRecommend(articleid, type, status, sort);
            return result ? JsonMethod.GetError(1,"添加成功") : JsonMethod.GetError(0,"添加失败，请稍后重试");// "{\"d\":true,\"msg\":\"添加成功\"}" : "{\"d\":false,\"msg\":\"系统错误，请稍后重试\"}";
        }
        /// <summary>
        /// 获取单个推荐商品
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        private string SingleGoodsRecommend(string json)
        {
            dynamic obj = JsonConvert.DeserializeObject<dynamic>(json);
            int type= Convert.ToInt32(obj.code);
            int articleid = Convert.ToInt32(obj.articleid);
            DAL.GoodsInfoDal goodsInfoDal = new DAL.GoodsInfoDal();
            DataTable dt = goodsInfoDal.SingleGoodsRecommend(articleid, type);
            string result = JsonConvert.SerializeObject(dt);
            return result;
        }
        /// <summary>
        /// 上下架推荐商品
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        private string StatusGoodsRecommend(string json)
        {
            dynamic obj = JsonConvert.DeserializeObject<dynamic>(json);
            int type = Convert.ToInt32(obj.code);
            int status = Convert.ToInt32(obj.status);
            int articleid = Convert.ToInt32(obj.articleid);
            DAL.GoodsInfoDal goodsInfoDal = new DAL.GoodsInfoDal();
            bool result = goodsInfoDal.StatusGoodsRecommend(articleid, type, status);
            return result ? JsonMethod.GetError(1, "操作成功") : JsonMethod.GetError(0, "操作失败，请稍后重试");
        }
        /// <summary>
        /// 删除推荐商品
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        private string DeleteGoodsRecommend(string json)
        {
            dynamic obj = JsonConvert.DeserializeObject<dynamic>(json);
            int type = Convert.ToInt32(obj.code);
            int articleid = Convert.ToInt32(obj.articleid);
            DAL.GoodsInfoDal goodsInfoDal = new DAL.GoodsInfoDal();
            bool result = goodsInfoDal.DeleteGoodsRecommend(articleid, type);
            return result ? JsonMethod.GetError(1, "删除成功") : JsonMethod.GetError(0, "删除失败，请稍后重试");
        }
        /// <summary>
        /// 修改推荐商品
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        private string UpdateGoodsRecommend(string json)
        {
            dynamic obj = JsonConvert.DeserializeObject<dynamic>(json);
            int articleid = Convert.ToInt32(obj.articleid);
            int old_articleid = Convert.ToInt32(obj.old_articleid);
            int type = Convert.ToInt32(obj.code);
            int old_type = Convert.ToInt32(obj.old_type);
            int status = Convert.ToInt32(obj.status);
            int sort = Convert.ToInt32(obj.sort);
            DAL.GoodsInfoDal goodsInfoDal = new DAL.GoodsInfoDal();
            if (articleid != old_articleid)
            {
                bool isExist = goodsInfoDal.ExistGoodsRecommend(articleid, type);
                if (isExist)
                {
                    return JsonMethod.GetError(3, "此推荐商品已存在");
                }
            }
            bool result = goodsInfoDal.UpdateGoodsRecommend(articleid,old_articleid,type,old_type,status,sort);
            return result ? JsonMethod.GetError(1, "修改成功") : JsonMethod.GetError(0, "修改失败，请稍后重试");
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