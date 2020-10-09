using Sk_B2BAPI.App_Code;
using Sk_B2BAPI.DAL;
using Sk_B2BAPI.Models;
using Sk_B2BAPI.Tool;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using static Sk_B2BAPI.Controllers.UsersController;

namespace Sk_B2BAPI.Controllers
{
    public class TemplateController : Controller
    {

        #region 商城模板信息
        /// <summary>
        /// 获取商城模板信息
        /// </summary>
        /// <param name="entId">企业Id</param>
        /// <param name="htmlType">html页面类型</param>
        /// <returns></returns>
        public JsonResult HtmlTemplate(string entId)
        {
            try
            {
                if (string.IsNullOrEmpty(entId))
                {
                    entId = BaseConfiguration.EntId;
                }

                List<Template> list = new List<Template>();
                Template t = new Template();
                ////获取页面配置信息
                TemplateDal dal_t = new TemplateDal();
                t.List_Sz = dal_t.GetSystemBase(entId);
                //获取药品分类
                ImgInfoDal dal_i = new ImgInfoDal();
                t.List_Fl = dal_i.GetCategory(entId, 7);
                //底部资讯
                ArticleDal dal_a = new ArticleDal();
                t.List_Zx= dal_a.GetArticle(entId, 11, "Mt");
                ///商品轮播图片
                t.List_Lb = dal_i.GetImgInfo(10,"LB",entId,"PC","");
                list.Add(t);


                return Json(new { success = true, list = list });

            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Template/HtmlTemplate", ex.Message.ToString());
                return Json(new { success = false, message = "模板信息加载失败！" });
            }
        }
        #endregion

        #region 商城主题色
        /// <summary>
        /// 商城主题色
        /// </summary>
        /// <param name="entId">机构</param>
        /// <returns>成功返回true,color/失败返回false</returns>
        public ActionResult SubjectColor(string entId) 
        {
            try
            {
                if (string.IsNullOrEmpty(entId))
                {
                    entId = BaseConfiguration.EntId;
                }
                TemplateDal templateDal = new TemplateDal();
                string color = templateDal.SubjectColor(entId);
                return Json(new {success=true,message= "商城主题加载成功", color=color });
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Template/SubjectColor", ex.Message.ToString());
                return Json(new { success = false, message = "商城主题加载失败" });
            }
        }
        #endregion

        #region 主题接口
        /// <summary>
        /// 主题接口
        /// </summary>
        /// <returns></returns>
        public JsonResult GetTemplateList(string entId, string userId) {
            try {
                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { success = false, message = "用户未登录，请先登录" });
                }
                SqlRun sql = new SqlRun(SqlRun.sqlstr);
                DataTable dt = sql.RunProcedureDR("Pc_TemplateRelation", new SqlParameter[]
                {
                      new SqlParameter("@type","CX_GetTemplateList"),
                      new SqlParameter("@entId",entId)
                 });
                return Json(JsonMethod.DataTableToJson("1",dt));
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Template/GetTemplateList", ex.Message.ToString());
                return Json(new { success = false, message = "商城主题加载失败" });
            }
        }
        #endregion

        #region 富文本接口
        public string ActivityTheme(string promotionCode, string templateCode,string lx= "PC") {
            AppMsg<Object> jsonResult = new AppMsg<Object>();
            string html = "";
            try
            {
                SqlRun sql = new SqlRun(SqlRun.sqlstr);
                DataSet dt = sql.RunProDataSet("Pc_TemplateRelation", new SqlParameter[]
                {
                      new SqlParameter("@type","CX_GetTemplateRelation"),
                      new SqlParameter("@PromotionCode",promotionCode),
                      new SqlParameter("@TemplateCode",templateCode)
                 });
                if (dt.Tables.Count == 2 && dt.Tables[0].Rows.Count > 0 && dt.Tables[1].Rows.Count > 0)
                {
                    html = dt.Tables[0].Rows[0]["TemplateText"].ToString();
                    DataRowCollection datalist = dt.Tables[1].Rows;
                    string boxTop = GetRegValue(html, "<!--截取TopStatr-->", "<!--截取TopEnd-->");
                    string tboxTop = boxTop;
                    if (boxTop!="") {
                        string Topleft = GetRegValue(boxTop, "<!--leftTopStatr-->", "<!--leftTopEnd-->");
                        string tleft = Topleft;
                        if (datalist.Count == 1)
                        {
                            tleft = tleft.Replace("@left@article_id", datalist[0]["article_id"].ToString());
                            tleft = tleft.Replace("@left@pdoduct", datalist[0]["sub_title"].ToString());
                            tleft = tleft.Replace("@left@old", datalist[0]["price"].ToString());
                            tleft = tleft.Replace("@left@new", datalist[0]["price"].ToString());
                            tleft = tleft.Replace("@left@image", datalist[0]["img_url"].ToString());
                            tleft = tleft.Replace("@left@gg", datalist[0]["drug_spec"].ToString());
                            tboxTop.Replace(Topleft,tleft);
                            html.Replace(boxTop, tboxTop);
                            datalist.Remove(datalist[0]);
                        }
                        else if(datalist.Count >= 2)
                        {
                            var leftobj = datalist[0];
                            var rightobj = datalist[1];
                            tleft = tleft.Replace("@left@article_id", leftobj["article_id"].ToString());
                            tleft = tleft.Replace("@left@pdoduct", leftobj["sub_title"].ToString());
                            tleft = tleft.Replace("@left@old", leftobj["price"].ToString());
                            tleft = tleft.Replace("@left@new", leftobj["price"].ToString());
                            tleft = tleft.Replace("@left@image", leftobj["img_url"].ToString());
                            tleft = tleft.Replace("@left@gg", leftobj["drug_spec"].ToString());
                            //分割线 第二块
                            tboxTop = tboxTop.Replace("@right@article_id", rightobj["article_id"].ToString());
                            tboxTop = tboxTop.Replace("@right@pdoduct", rightobj["sub_title"].ToString());
                            tboxTop = tboxTop.Replace("@right@old", rightobj["price"].ToString());
                            tboxTop = tboxTop.Replace("@right@new", rightobj["price"].ToString());
                            tboxTop = tboxTop.Replace("@right@image", rightobj["img_url"].ToString());
                            tboxTop = tboxTop.Replace("@right@gg", rightobj["drug_spec"].ToString());
                            tboxTop= tboxTop.Replace(Topleft, tleft);
                            html=html.Replace(boxTop, tboxTop);
                            datalist.Remove(leftobj);
                            datalist.Remove(rightobj);
                        }
                    }
                    string boxAll = GetRegValue(html, "<!--截取Statr-->", "<!--截取end-->");
                    string leftAll = GetRegValue(boxAll, "<!--leftStatr-->", "<!--leftEnd-->");
                    string left = leftAll;
                    string box = boxAll;
                    string texthtml = "";
                    if (datalist.Count == 1)
                    {
                        //left = left.Replace("@left@huodong", datalist[0]["fabh"].ToString());
                        left = left.Replace("@left@article_id", datalist[0]["article_id"].ToString());
                        left = left.Replace("@left@pdoduct", datalist[0]["sub_title"].ToString());
                        left = left.Replace("@left@old", datalist[0]["price"].ToString());
                        left = left.Replace("@left@new", datalist[0]["price"].ToString());
                        left = left.Replace("@left@image", datalist[0]["img_url"].ToString());
                        left = left.Replace("@left@gg", datalist[0]["drug_spec"].ToString());
                        texthtml += left;
                        left = leftAll;
                    }
                    else if (datalist.Count % 2 != 0)
                    {
                        for (var i = 0; i < datalist.Count; i++)
                        {
                            //box = box.Replace("@left@huodong", datalist[i]["fabh"].ToString());
                            box = box.Replace("@left@article_id", datalist[i]["article_id"].ToString());
                            box = box.Replace("@left@pdoduct", datalist[i]["sub_title"].ToString());
                            box = box.Replace("@left@old", datalist[i]["price"].ToString());
                            box = box.Replace("@left@new", datalist[i]["price"].ToString());
                            box = box.Replace("@left@image", datalist[i]["img_url"].ToString());
                            box = box.Replace("@left@gg", datalist[i]["drug_spec"].ToString());
                            i++;
                            //box = box.Replace("@right@huodong", datalist[i]["fabh"].ToString());
                            box = box.Replace("@right@article_id", datalist[i]["article_id"].ToString());
                            box = box.Replace("@right@pdoduct", datalist[i]["sub_title"].ToString());
                            box = box.Replace("@right@old", datalist[i]["price"].ToString());
                            box = box.Replace("@right@new", datalist[i]["price"].ToString());
                            box = box.Replace("@right@image", datalist[i]["img_url"].ToString());
                            box = box.Replace("@right@gg", datalist[i]["drug_spec"].ToString());
                            texthtml += box;
                            box = boxAll;
                            if ((i + 1) == (datalist.Count - 1))
                            {
                                i++;
                                //left = left.Replace("@left@huodong", datalist[i]["fabh"].ToString());
                                left = left.Replace("@left@article_id", datalist[i]["article_id"].ToString());
                                left = left.Replace("@left@pdoduct", datalist[i]["sub_title"].ToString());
                                left = left.Replace("@left@old", datalist[i]["price"].ToString());
                                left = left.Replace("@left@new", datalist[i]["price"].ToString());
                                left = left.Replace("@left@image", datalist[i]["img_url"].ToString());
                                left = left.Replace("@left@gg", datalist[i]["drug_spec"].ToString());
                                texthtml += left;
                                left = leftAll;

                            }
                        }

                    }
                    else
                    {
                        for (var i = 0; i < datalist.Count; i++)
                        {
                            //box = box.Replace("@left@huodong", datalist[i]["fabh"].ToString());
                            box = box.Replace("@left@article_id", datalist[i]["article_id"].ToString());
                            box = box.Replace("@left@pdoduct", datalist[i]["sub_title"].ToString());
                            box = box.Replace("@left@old", datalist[i]["price"].ToString());
                            box = box.Replace("@left@new", datalist[i]["price"].ToString());
                            box = box.Replace("@left@image", datalist[i]["img_url"].ToString());
                            box = box.Replace("@left@gg", datalist[i]["drug_spec"].ToString());
                            i++;
                            //box = box.Replace("@right@huodong", datalist[i]["fabh"].ToString());
                            box = box.Replace("@right@article_id", datalist[i]["article_id"].ToString());
                            box = box.Replace("@right@pdoduct", datalist[i]["sub_title"].ToString());
                            box = box.Replace("@right@old", datalist[i]["price"].ToString());
                            box = box.Replace("@right@new", datalist[i]["price"].ToString());
                            box = box.Replace("@right@image", datalist[i]["img_url"].ToString());
                            box = box.Replace("@right@gg", datalist[i]["drug_spec"].ToString());
                            texthtml += box;
                            box = boxAll;
                        }
                    }
                    html = "<body>" + html.Replace(boxAll, texthtml) + "</body>";
                }
                else
                {
                    throw new Exception("没有主题页");
                }
            }
            catch (Exception ex)
            {
                html = ex.Message;

            }
           return html;
        }
        /// <summary>  
        /// 正则获得字符串中开始和结束字符串之间的值  
        /// </summary>  
        /// <param name="str">字符串</param>  
        /// <param name="s">开始</param>  
        /// <param name="e">结束</param>  
        /// <returns></returns>  
        public List<string> GetRegValueArr(string str, string s, string e)
        {
            Match m;
            List<string> list = new List<string>();
            Regex rg = new Regex("(?<=(" + s + "))[.\\s\\S]*?(?=(" + e + "))", RegexOptions.Multiline | RegexOptions.Singleline);
            for (m = rg.Match(str); m.Success; m = m.NextMatch())
            {
                list.Add(m.Groups[0].Value);
            }
            return list;
        }
        /// <summary>  
        /// 正则获得字符串中开始和结束字符串之间的值  
        /// </summary>  
        /// <param name="str">字符串</param>  
        /// <param name="s">开始</param>  
        /// <param name="e">结束</param>  
        /// <returns></returns>   
        public string GetRegValue(string str, string s, string e)
        {
            Regex rg = new Regex("(?<=(" + s + "))[.\\s\\S]*?(?=(" + e + "))", RegexOptions.Multiline | RegexOptions.Singleline);
            return rg.Match(str).Value;
        }
        #endregion
    }
}
