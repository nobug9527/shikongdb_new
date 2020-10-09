using Sk_B2BAPI.WxPayAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sk_B2BAPI.App_Code;
using Sk_B2BAPI.Models;
using Sk_B2BAPI.DAL;
using System.Text;
using Sk_B2BAPI.Models.Admin;
using Sk_B2BAPI.Tool;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;

namespace Sk_B2BAPI.Controllers
{
    public class RoleController : Controller
    {
       
        SqlRun sql;

        public RoleController() {
            sql = new SqlRun(SqlRun.sqlstr);
        }
        #region 权限路径
        [HttpPost]
        public ActionResult PowerList(string status,string source,string strWhere, int pageIndex,int pageSize)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Id,Title,NavTitle,Power,isnull(FatherId,0) as FatherId,Sort,AddTime,UpdateTime,IcoAddress,Source,Status  from Dt_Method   where  1=1");
            if (status != "99")
            {
                strSql.Append(" and  status=" + status);
            }
            if (source != "99")
            {
                strSql.Append(" and  (source='" + source + "'or source='ALL')");
            }
            StringBuilder strSqlYj = new StringBuilder();
            strSqlYj.Append(strSql.ToString());
            if (strWhere != "")
            {
                strSqlYj.Append(" and  (Title like '%"+ strWhere + "%' or  NavTitle like '%" + strWhere + "%' or  id like '%" + strWhere + "%' )");
            }
            var usermlist = DateTableTool.DataTableToList<Dt_Method>(sql.RtDataTable(strSqlYj.ToString()+" and  isnull(FatherId,0)<>0"));
            if (strWhere != "" && usermlist.Count > 0)
            {
                strSql.Append(" and id in ( ");
                usermlist.ForEach(p =>
                {
                    strSql.Append("'" + p.FatherId + "',");
                });
                strSql.Remove(strSql.Length - 1, 1);
                strSql.Append(")");
            }
            else
            {
                strSql.Append(" and  (Title like '%" + strWhere + "%' or  NavTitle like '%" + strWhere + "%' or  id like '%" + strWhere + "%' )");
            }
            var list = DateTableTool.DataTableToList<Dt_Method>(sql.RtDataTable(strSql.ToString() + "and isnull(FatherId,0)=0")).OrderBy(m => m.Sort);
            var mlist = new List<Dt_Method>();
            foreach (var l in list)
            {
                mlist.Add(l);
                if (usermlist.Count(m => m.FatherId == l.Id) > 0)
                {
                    var flist = usermlist.Where(m => m.FatherId == l.Id).ToList().OrderBy(m => m.Sort);
                    foreach (var f in flist)
                    {
                        mlist.Add(f);
                        if (usermlist.Count(m => m.FatherId == f.Id) > 0)
                        {
                            var slist = usermlist.Where(m => m.FatherId == f.Id).ToList().OrderBy(m => m.Sort);
                            foreach (var s in slist)
                            {
                                mlist.Add(s);
                                if (usermlist.Count(m => m.FatherId == s.Id) > 0)
                                {
                                    var tlist = usermlist.Where(m => m.FatherId == s.Id).ToList().OrderBy(m => m.Sort);
                                    foreach (var t in tlist)
                                    {
                                        mlist.Add(t);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            int page = 1;
            if (pageIndex > 0)
            {
                page = pageIndex;
            }
            int total = 0;
            if (mlist.Count % pageSize > 0)
            {
               total = mlist.Count / pageSize + 1;
            }
            else
            {
                total = mlist.Count / pageSize;
            }
            //ViewBag.orderList = mlist.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return Json(new JsonMsg() { Code = true, Msg = "获取成功", Obj = mlist.Skip((page - 1) * pageSize).Take(pageSize).ToList(), Total = total, Page = page });
        }
        #endregion

        #region 添加权限路径[页面]
        
        public JsonResult PowerAdd()
        {
            //RoleFuns.IsLoginPowerView();
            string source = Request.Params["source"];
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from Dt_Method where isnull(FatherId,0)=0 and status=1 ");
            if (source != "ALL")
            {
                strSql.Append(" and source='"+ source + "'");
            }
            var list = DateTableTool.DataTableToList<Dt_Method>(sql.RtDataTable(strSql.ToString())).OrderBy(m => m.Sort);
            //var mlist = new List<Dt_Method>();
            //var usermlist = DateTableTool.DataTableToList<Dt_Method>(sql.RtDataTable($"select * from Dt_Method"));
            //foreach (var l in list)
            //{
            //    mlist.Add(l);
            //    if (usermlist.Count(m => m.FatherId == l.Id) > 0)
            //    {
            //        var flist = usermlist.Where(m => m.FatherId == l.Id).ToList().OrderBy(m => m.Sort);
            //        foreach (var f in flist)
            //        {
            //            mlist.Add(f);
            //            if (usermlist.Count(m => m.FatherId == f.Id) > 0)
            //            {
            //                var slist = usermlist.Where(m => m.FatherId == f.Id).ToList().OrderBy(m => m.Sort);
            //                foreach (var s in slist)
            //                {
            //                    mlist.Add(s);
            //                }
            //            }
            //        }
            //    }
            //}
            return Json(new JsonMsg() { Code = true, Msg = "获取成功", Obj = list });
            
        }
        #endregion

        #region 添加权限路径[方法]
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult PowerAdd_Action()
        {
            try
            {
                if (StringHelper.IsNull(Request.Params["power"], Request.Params["action"], Request.Params["father"], Request.Params["sort"]))
                {
                    string title = Request.Params["power"];
                    string action = Request.Params["action"];
                    int father = int.Parse(Request.Params["father"]);
                    int sort = int.Parse(Request.Params["sort"]);
                    string source = Request.Params["source"];
                    Encoding gb2312 = Encoding.GetEncoding("gb2312");
                    Encoding utf8 = Encoding.UTF8;
                    int status = 0;
                    //首先用utf-8进行解码
                    string ico = HttpUtility.UrlDecode(Request.Params["Idico"], utf8);
                    var date = DateTime.Now;
                    string fatherId = father == 0 ? "Null" : father.ToString();
                    sql.RunTSqlScalar($"INSERT INTO [dbo].[Dt_Method](Title,NavTitle,POWER,FatherId,Sort,AddTime,UpdateTime,IcoAddress,Source,status) VALUES ('{title}','{ RoleFuns.GetMethodNavTitle(father) + title}','{action}',{fatherId},{sort},'{date}','{date}','{ico}','{source}','{status}')");

                    return Json(new JsonMsg() { Code = true, Msg = "权限路径添加成功" });
                }
                else
                {
                    return Json(new JsonMsg() { Code = false, Msg = "参数错误" });
                }
            }
            catch (Exception ex)
            {
                return Json(new JsonMsg() { Code = false, Msg = ex.Message });
            }

        }
        #endregion
        
        #region 编辑权限路径[方法]
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult PowerEdit_Action()
        {
            if (StringHelper.IsNull(Request.Params["id"], Request.Params["power"], Request.Params["action"], Request.Params["father"], Request.Params["sort"]) && int.Parse(Request.Params["id"]) >= 0 && int.Parse(Request.Params["sort"]) >= 0)
            {
                int id = int.Parse(Request.Params["id"]);
                var method = DateTableTool.DataTalbeToEntity<Dt_Method>(sql.RtDataTable($"select * from Dt_Method where Id = {id}"));
                if (method != null)
                {
                    string title = Request.Params["power"];
                    string action = Request.Params["action"];
                    int fatherId = int.Parse(Request.Params["father"]);
                    int sort = int.Parse(Request.Params["sort"]);
                    string Idico =Request.Params["Idico"];
                    string source = Request.Params["source"];
                    Encoding gb2312 = Encoding.GetEncoding("gb2312");
                    Encoding utf8 = Encoding.UTF8;
                    //首先用utf-8进行解码
                    string key = HttpUtility.UrlDecode(Idico, utf8);
                    if (sql.RunSqlNumber($"UPDATE [dbo].[Dt_Method]SET [Title] = '{title}',[NavTitle] = '{RoleFuns.GetMethodNavTitle(fatherId) + title}',[Power] = '{action}',[FatherId] = {fatherId},[Sort] = {sort},[IcoAddress] = '{key}',source='{source}'  WHERE Id={method.Id}") > 0)
                    {
                        return Json(new JsonMsg() { Code = true, Msg = "权限路径更新成功" });
                    }
                    else
                    {
                        return Json(new JsonMsg() { Code = false, Msg = "系统繁忙，请稍后再试" });
                    }
                }
                else
                {
                    return Json(new JsonMsg() { Code = false, Msg = "权限路径不存在" });
                }

            }
            else
            {
                return Json(new JsonMsg() { Code = false, Msg = "参数错误" });
            }
        }
        #endregion

        #region 删除权限路径[方法]
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult PowerDel()
        {
            if (StringHelper.IsNull(Request.Params["ids"]))
            {
                string ids = Request.Params["ids"];
                int[] delids = StringHelper.StrsToInts(ids, ',');

                if (delids.Length > 0)
                {
                    foreach (var delid in delids)
                    {
                        sql.RunTSqlScalar($"DELETE FROM [dbo].[Dt_Method] where Id={delid} ");
                    }
                    return Json(new JsonMsg() { Code = true, Msg = "权限路径删除成功" });
                }
                else
                {
                    return Json(new JsonMsg() { Code = false, Msg = "参数错误" });
                }
            }
            else
            {
                return Json(new JsonMsg() { Code = false, Msg = "参数错误" });
            }
        }

        /// <summary>
        /// 修改状态
        /// </summary>
        /// <returns></returns>
        public JsonResult UpdateMethod(int id,int status,string fatherId)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                if (status == 2)
                {
                    strSql.Append("delete from dbo.Dt_RoleMethod where MethodId=" + id + ";");
                    strSql.Append("delete from Dt_Method where id=" + id + ";");
                    if (fatherId == "" || fatherId == "0")
                    {
                        strSql.Append("delete from dbo.Dt_RoleMethod where MethodId in(select  id  from Dt_Method where fatherId=" + id + ");");
                        strSql.Append("delete from Dt_Method where fatherId=" + id + ";");
                    }
                }
                else
                {
                    strSql.Append("update  Dt_Method set status=" + status + " where id=" + id + ";");
                    if (fatherId == "" || fatherId == "0")
                    {
                        strSql.Append("update  Dt_Method set status=" + status + " where fatherId=" + id + ";");
                    }
                }
                sql.ExecuteSql(strSql.ToString());
                return Json(new JsonMsg() { Code = true, Msg = "操作成功" });
            }
            catch (Exception ex)
            {
                return Json(new JsonMsg() { Code = false, Msg = ex.Message });
            }
        }
        #endregion

        //********************************-------  系统图标  ---------**********************************//

        //#region 图标管理

        //public JsonResult IcoList()
        //{
        //    var mlist = DateTableTool.DataTableToList<Dt_Ico>(sql.RtDataTable($"select * from Dt_Ico"));
        //    int page = 1;
        //    if (StringHelper.IsNull(Request.Params["p"]) && int.Parse(Request.Params["p"]) > 0)
        //    {
        //        page = int.Parse(Request.Params["p"]);
        //    }
        //    int pageSize = ConfigHelper.GetConfigInt("PageSize");
        //    if (pageSize == 0)
        //    {
        //        pageSize = int.Parse(Request.Params["s"]);
        //    }
        //    int total = 0;
        //    if (mlist.Count % pageSize > 0)
        //    {
        //        total = mlist.Count / pageSize + 1;
        //    }
        //    else
        //    {
        //        total = mlist.Count / pageSize;
        //    }
        //    return Json(new JsonMsg() { Code = true, Msg = "获取成功", Obj = mlist.Skip((page - 1) * pageSize).Take(pageSize).ToList(), Total = total, Page = page });
        //}
        //#endregion

        //#region 添加系统图标[方法]

        //public JsonResult IcoAdd_Action()
        //{
        //    if (true)//msg.Code)
        //    {
        //        if (StringHelper.IsNull(Request.Params["roleName"], Request.Params["sort"], Request.Params["ids"]) && int.Parse(Request.Params["sort"]) >= 0)
        //        {
        //            string roleName = Request.Params["roleName"];
        //            if (DateTableTool.DataTableToList<Dt_Role>(sql.RtDataTable($"select * from Dt_Role where RoleName='{roleName}'")).Count() > 0)
        //            {
        //                return Json(new JsonMsg() { Code = false, Msg = "角色名不能重复" });
        //            }
        //            string describe = Request.Params["describe"];
        //            int sort = int.Parse(Request.Params["sort"]);
        //            string ids = Request.Params["ids"].TrimEnd(',');
        //            int[] idInts = StringHelper.StrsToInts(ids, ',');
        //            var date = DateTime.Now;

        //            //需要返回id
        //            sql.RunTSqlScalar($"INSERT INTO [dbo].[Dt_Role] VALUES('{roleName}','{describe}',{sort},'{date}','{date}')");
        //            int id = DateTableTool.DataTalbeToEntity<Dt_Role>(sql.RtDataTable($"select * from Dt_Role where RoleName='{roleName}'")).Id;
        //            if (id > 0)
        //            {
        //                foreach (var i in idInts)
        //                {
        //                    sql.RunTSqlScalar($"INSERT INTO [dbo].[Dt_RoleMethod] VALUES ({id},{i},'{date}','{date}')");
        //                }
        //                return Json(new JsonMsg() { Code = true, Msg = "角色添加成功" });
        //            }
        //            else
        //            {
        //                return Json(new JsonMsg() { Code = false, Msg = "系统繁忙，请稍后再试" });
        //            }
        //        }
        //        else
        //        {
        //            return Json(new JsonMsg() { Code = false, Msg = "参数错误" });
        //        }
        //    }
        //    else
        //    {
        //        return Json(new JsonMsg() { Code = false, Msg = "没有权限操作" });
        //    }
        //}
        //#endregion


        //********************************-------  角色管理  ---------**********************************//

        #region 角色管理
        /// <summary>
        /// 后台角色管理
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <param name="status">状态</param>
        /// <param name="Source">类型</param>
        /// <returns></returns>
        public JsonResult RoleList(string strWhere,int status,string Source)
        {
            try
            {
                Dt_User user = RoleFuns.IsLoginAdmin(Session["user"]);
                StringBuilder strSql = new StringBuilder();
                strSql.Append($"select Id,RoleName,Describe,Sort,AddTime,UpdateTime,Source,status,entId,FatherId from Dt_Role where 1=1 ");
                if (status != 99)
                {
                    strSql.Append(" and  status=" + status);
                }
                if (user.entId != "superintendent")
                {
                    strSql.Append(" and  entid='" + user.entId + "'");
                }
                if (Source != "99")
                {
                    strSql.Append(" and  Source='" + Source + "'");
                }
                if (strWhere != "")
                {
                    strSql.Append(" and  (RoleName  like '%" + Source + "%' or  Describe like '%" + Source + "%')");
                }
                strSql.Append(" order  by Sort asc");
                var mlist = DateTableTool.DataTableToList<Dt_Role>(sql.RtDataTable(strSql.ToString()));

                int page = 1;
                if (StringHelper.IsNull(Request.Params["pageIndex"]) && int.Parse(Request.Params["pageIndex"]) > 0)
                {
                    page = int.Parse(Request.Params["pageIndex"]);
                }
                int pageSize = ConfigHelper.GetConfigInt("PageSize");
                if (pageSize == 0)
                {
                    pageSize = int.Parse(Request.Params["pageSize"]);
                }
                int total = 0;
                if (mlist.Count % pageSize > 0)
                {
                    total = mlist.Count / pageSize + 1;
                }
                else
                {
                    total = mlist.Count / pageSize;
                }
                return Json(new JsonMsg() { Code = true, Msg = "获取成功", Obj = mlist.Skip((page - 1) * pageSize).Take(pageSize).ToList(), Total = total, Page = page });
            }
            catch (Exception ex)
            {
                return Json(new JsonMsg() { Code = true, Msg=ex.Message });
            }
        }
        /// <summary>
        /// 角色状态修改和删除
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public JsonResult UpdateRole(string id,int status)
        {
            try
            {
                StringBuilder sqlStr = new StringBuilder();
                if (status == 2)
                {
                    sqlStr.Append(" delete from Dt_RoleMethod where  RoleId =" + id + ";");
                    sqlStr.Append(" delete from Dt_Role where id=" + id);
                }
                else
                {
                    sqlStr.Append("update  Dt_Role set status=" + status + " where id=" + id);
                }
                sql.ExecuteSql(sqlStr.ToString());
                return Json(new JsonMsg() { Code = true, Msg = "操作成功" });
            }
            catch (Exception ex)
            {
                return Json(new JsonMsg() { Code = false, Msg = ex.Message });
            }
        }

        #endregion

        #region 添加角色[页面]
        /// <summary>
        /// 角色维护获取功能列表
        /// </summary>
        /// <param name="Source">类型</param>
        /// <param name="keyId">角色Id</param>
        /// <returns></returns>
        public JsonResult RoleAdd(string Source,string keyId)
        {
            try
            {
                if (keyId==null || keyId == "")
                {
                    keyId = "0";
                }
                Dt_User user = RoleFuns.IsLoginAdmin(Session["user"]);
                StringBuilder mkId = new StringBuilder();
                if (user.entId != "superintendent")
                {
                    ///获取上级管理员权限
                    string sqlMk = $"select c.MethodId from dt_users a(nolock) join Dt_RoleMethod c(nolock) on a.role_id=c.RoleId where  a.userId='{user.userId}'";
                    DataTable dtMk = sql.RtDataTable(sqlMk);
                    if (dtMk != null && dtMk.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtMk.Rows)
                        {
                            mkId.Append(dr["MethodId"] + ",");
                        }
                        mkId.Remove(mkId.Length - 1, 1);
                    }
                }
                StringBuilder strSql = new StringBuilder();
                strSql.Append("SELECT  a.Id,a.RoleName,a.Describe,case Source when 'YWT' then 'APP' ELSE 'PC' end as RoleTyle,Source,b.MethodId,a.Sort  FROM Dt_Role a join Dt_RoleMethod b on a.Id=b.RoleId where RoleId='" + keyId + "' and RoleId<>0");
                var roleList = DateTableTool.DataTableToList<Dt_Role>(sql.RtDataTable(strSql.ToString()));
                if (roleList != null && roleList.Count > 0)
                {
                    Source = roleList[0].Source;
                }
                strSql.Remove(0, strSql.Length);
                strSql.Append($"select  Id,Title,NavTitle,power,FatherId,Sort,case Source when 'YWT' then 'APP' ELSE 'PC' end as RoleTyle,Source,Status  from Dt_Method where status=1 ");
                if (Source != "ALL")
                {
                    strSql.Append(" and Source='" + Source + "'");
                }
                if (mkId.ToString() != "")
                {
                    strSql.Append($" and Id  in ({mkId.ToString()})");
                }
                var usermlist = DateTableTool.DataTableToList<Dt_Method>(sql.RtDataTable(strSql.ToString()));
                strSql.Append(" and FatherId is null ");

                var list = DateTableTool.DataTableToList<Dt_Method>(sql.RtDataTable(strSql.ToString())).OrderBy(m => m.Sort);

                var mlist = new List<Dt_Method>();
                foreach (var l in list)
                {
                    mlist.Add(l);
                    if (usermlist.Count(m => m.FatherId == l.Id) > 0)
                    {
                        var flist = usermlist.Where(m => m.FatherId == l.Id).ToList().OrderBy(m => m.Sort);
                        foreach (var f in flist)
                        {
                            mlist.Add(f);
                            if (usermlist.Count(m => m.FatherId == f.Id) > 0)
                            {
                                var slist = usermlist.Where(m => m.FatherId == f.Id).ToList().OrderBy(m => m.Sort);
                                foreach (var s in slist)
                                {
                                    mlist.Add(s);
                                    if (usermlist.Count(m => m.FatherId == s.Id) > 0)
                                    {
                                        var tlist = usermlist.Where(m => m.FatherId == s.Id).ToList().OrderBy(m => m.Sort);
                                        foreach (var t in tlist)
                                        {
                                            mlist.Add(t);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                var newlist = mlist.Select(m => new MenuTree()
                {
                    Id = m.Id,
                    Title = m.Title,
                    FatherId = m.FatherId,
                    Source=m.RoleTyle
                });
                // ViewBag.Menu = JsonHelper.JsonSerializer(newlist);
                //return View();
                //return Json(new JsonMsg() { Code = true, Msg = "获取成功", Obj = newlist});
                return Json(new { Code = true, Msg = "获取成功", Obj = newlist, RoleList= roleList });
            }
            catch (Exception ex)
            {
                return Json(new JsonMsg() { Code = true, Msg =ex.Message });
            }
        }
        #endregion

        #region 添加角色[方法]
        [HttpPost]
        public JsonResult RoleAdd_Action(string entId)
        {
            try
            {
                string source = Request.Params["source"];
                string keyId= Request.Params["keyId"];
                if (keyId == null || keyId == "")
                {
                    keyId = "0";
                }
                if (StringHelper.IsNull(Request.Params["roleName"], Request.Params["sort"], Request.Params["ids"]) && int.Parse(Request.Params["sort"]) >= 0)
                {
                    string roleName = Request.Params["roleName"];
                    
                    
                    string describe = Request.Params["describe"];
                    int sort = int.Parse(Request.Params["sort"]);
                    int status = 0;
                    string ids = Request.Params["ids"].TrimEnd(',');

                    int[] idInts = StringHelper.StrsToInts(ids, ',');
                    var date = DateTime.Now;
                    if (DateTableTool.DataTableToList<Dt_Role>(sql.RtDataTable($"select * from Dt_Role where id='{keyId}'")).Count() > 0)
                    {
                        if (sql.ExecuteSql($"update Dt_Role set RoleName='{roleName}',Describe='{describe}',Sort='{sort}',UpdateTime='{date}',Source='{source}',entId='{entId}'  where id='{keyId}'"))
                        {
                            //删除原有绑定关系
                            StringBuilder sqlStr = new StringBuilder();
                            sqlStr.Append($"delete from Dt_RoleMethod where RoleId='{keyId}'");
                            foreach (var i in idInts)
                            {
                                sqlStr.Append($"INSERT INTO [dbo].[Dt_RoleMethod](RoleId,MethodId,AddTime,UpdateTime) VALUES ('{keyId}',{i},'{date}','{date}')");
                            }
                            if (sql.ExecuteSql(sqlStr.ToString()))
                            {
                                return Json(new JsonMsg() { Code = true, Msg = "修改成功" });
                            }
                            else
                            {
                                return Json(new JsonMsg() { Code = false, Msg = "修改失败" });
                            }
                        }
                        else
                        {
                            return Json(new JsonMsg() { Code = false, Msg = "修改失败" });
                        }
                    }
                    else
                    {
                        if (DateTableTool.DataTableToList<Dt_Role>(sql.RtDataTable($"select * from Dt_Role where RoleName='{roleName}' and source='{source}'")).Count() > 0)
                        {
                            return Json(new JsonMsg() { Code = false, Msg = "角色名不能重复" });
                        }
                        //需要返回id
                        sql.RunTSqlScalar($"INSERT INTO [dbo].[Dt_Role](RoleName,Describe,Sort,AddTime,UpdateTime,Source,Status,entId) VALUES('{roleName}','{describe}',{sort},'{date}','{date}','{source}','{status}','{entId}')");
                        int id = DateTableTool.DataTalbeToEntity<Dt_Role>(sql.RtDataTable($"select * from Dt_Role where RoleName='{roleName}' and source='{source}'")).Id;
                        if (id > 0)
                        {
                            foreach (var i in idInts)
                            {
                                sql.RunTSqlScalar($"INSERT INTO [dbo].[Dt_RoleMethod](RoleId,MethodId,AddTime,UpdateTime) VALUES ('{id}',{i},'{date}','{date}')");
                            }
                            return Json(new JsonMsg() { Code = true, Msg = "角色添加成功" });
                        }
                        else
                        {
                            return Json(new JsonMsg() { Code = false, Msg = "系统繁忙，请稍后再试" });
                        }
                    }
                }
                else
                {
                    return Json(new JsonMsg() { Code = false, Msg = "参数错误" });
                }
            }
            catch (Exception ex)
            {
                return Json(new JsonMsg() { Code = false, Msg =ex.Message });
            }
        }
        #endregion

        //#region 编辑角色[页面]

        //public ActionResult RoleEdit()
        //{
        //    RoleFuns.IsLoginPowerView();
        //    var list = DateTableTool.DataTableToList<Dt_Method>(sql.RtDataTable($"select * from Dt_Method where FatherId is null")).OrderBy(m => m.Sort);
        //    var mlist = new List<Dt_Method>();
        //    foreach (var l in list)
        //    {
        //        mlist.Add(l);
        //        if (_methodRepository.Entities.Count(m => m.FatherId == l.Id) > 0)
        //        {
        //            var flist = _methodRepository.Entities.Where(m => m.FatherId == l.Id).ToList().OrderBy(m => m.Sort);
        //            foreach (var f in flist)
        //            {
        //                mlist.Add(f);
        //                if (_methodRepository.Entities.Count(m => m.FatherId == f.Id) > 0)
        //                {
        //                    var slist = _methodRepository.Entities.Where(m => m.FatherId == f.Id).ToList().OrderBy(m => m.Sort);
        //                    foreach (var s in slist)
        //                    {
        //                        mlist.Add(s);
        //                        if (_methodRepository.Entities.Count(m => m.FatherId == s.Id) > 0)
        //                        {
        //                            var tlist = _methodRepository.Entities.Where(m => m.FatherId == s.Id).ToList().OrderBy(m => m.Sort);
        //                            foreach (var t in tlist)
        //                            {
        //                                mlist.Add(t);
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    var newlist = mlist.Select(m => new MenuTree()
        //    {
        //        Id = m.Id,
        //        Title = m.Title,
        //        FatherId = m.FatherId
        //    });
        //    ViewBag.Menu = JsonHelper.JsonSerializer(newlist);

        //    if (StringHelper.IsNull(Request.Params["id"]))
        //    {
        //        int id = int.Parse(Request.Params["id"]);
        //        var role = _roleRepository.Entities.FirstOrDefault(m => m.Id == id);
        //        if (role != null)
        //        {
        //            ViewBag.role = role;
        //        }
        //        else
        //        {
        //            Response.Redirect("/ZhiLin/Error/Notfound");
        //        }
        //    }
        //    else
        //    {
        //        Response.Redirect("/ZhiLin/Error/Notfound");
        //    }

        //    return View();
        //}
        //#endregion

        //#region 编辑角色[方法]

        //public JsonResult RoleEdit_Action()
        //{
        //    JsonMsg msg = RoleFuns.IsLoginPowerAction();
        //    if (msg.Code)
        //    {
        //        if (StringHelper.IsNull(Request.Params["id"], Request.Params["roleName"], Request.Params["sort"], Request.Params["ids"]) && int.Parse(Request.Params["sort"]) >= 0 && int.Parse(Request.Params["id"]) > 0)
        //        {
        //            int id = int.Parse(Request.Params["id"]);
        //            string roleName = Request.Params["roleName"];
        //            string describe = Request.Params["describe"];
        //            int sort = int.Parse(Request.Params["sort"]);
        //            string ids = Request.Params["ids"].TrimEnd(',');
        //            int[] idInts = StringHelper.StrsToInts(ids, ',');
        //            var role = _roleRepository.Entities.FirstOrDefault(m => m.Id == id);
        //            if (role != null)
        //            {
        //                role.RoleName = roleName;
        //                role.Describe = describe;
        //                role.Sort = sort;
        //                _roleRepository.Update(role);
        //                _roleMethodRepository.Delete(m => m.RoleId == id);
        //                foreach (var i in idInts)
        //                {
        //                    _roleMethodRepository.Insert(new Dt_RoleMethod() { RoleId = id, MethodId = i });
        //                }
        //                return Json(new JsonMsg() { Code = true, Msg = "角色更新成功" });
        //            }
        //            else
        //            {
        //                return Json(new JsonMsg() { Code = false, Msg = "角色不存在" });
        //            }
        //        }
        //        else
        //        {
        //            return Json(new JsonMsg() { Code = false, Msg = "参数错误" });
        //        }
        //    }
        //    else
        //    {
        //        return Json(msg);
        //    }
        //}
        //#endregion

        #region 角色权限是否选中[方法]

        public JsonResult RoleMethod_Checked()
        {
            if (StringHelper.IsNull(Request.Params["rid"], Request.Params["mid"]))
            {
                int rid = int.Parse(Request.Params["rid"]);
                int mid = int.Parse(Request.Params["mid"]);
                if (DateTableTool.DataTableToList<Dt_Method>(sql.RtDataTable($"select * from Dt_RoleMethod where RoleId = {rid} and MethodId = {mid}")).Count() == 1)
                {
                    return Json(new JsonMsg() { Code = true, Msg = "存在" });
                }
                else
                {
                    return Json(new JsonMsg() { Code = false, Msg = "不存在" });
                }
            }
            else
            {
                return Json(new JsonMsg() { Code = false, Msg = "参数错误" });
            }
        }
        #endregion

        #region 删除角色[方法]
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult RoleDel()
        {
            if (StringHelper.IsNull(Request.Params["ids"]))
            {
                string ids = Request.Params["ids"];
                int[] delids = StringHelper.StrsToInts(ids, ',');

                if (delids.Length > 0)
                {
                    foreach (var delid in delids)
                    {
                        if (delid != 1)
                        {
                            sql.RtDataTable($"delete table  Dt_ReloMethod where RoleId = {delid}");
                            sql.RtDataTable($"delete table  Dt_Relo where RoleId = {delid}");
                        }
                    }
                    return Json(new JsonMsg() { Code = true, Msg = "角色删除成功<br/>注意：系统管理员角色系统设定为不可删除" });
                }
                else
                {
                    return Json(new JsonMsg() { Code = false, Msg = "参数错误" });
                }
            }
            else
            {
                return Json(new JsonMsg() { Code = false, Msg = "参数错误" });
            }
        }
        #endregion

       //********************************-------  账号管理  ---------**********************************//

        #region 账号管理

        //public ActionResult AdminList()
        //{
        //    RoleFuns.IsLoginPowerView();
        //    var mlist = DateTableTool.DataTableToList<dt_users>(sql.RtDataTable($"select * from dt_users"));

        //    int page = 1;
        //    if (StringHelper.IsNull(Request.Params["p"]) && int.Parse(Request.Params["p"]) > 0)
        //    {
        //        page = int.Parse(Request.Params["p"]);
        //    }
        //    int pageSize = ConfigHelper.GetConfigInt("PageSize");
        //    if (mlist.Count % pageSize > 0)
        //    {
        //        ViewBag.total = mlist.Count / pageSize + 1;
        //    }
        //    else
        //    {
        //        ViewBag.total = mlist.Count / pageSize;
        //    }
        //    ViewBag.orderList = mlist.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        //    return View();
        //}
        #endregion

        #region 添加账号[页面]
        [HttpPost]
        public JsonResult AdminAdd()
        {
            Dt_User user = RoleFuns.IsLoginAdmin(Session["user"]);
            //RoleFuns.IsLoginPowerView();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  Id,RoleName,Describe,Sort,AddTime,UpdateTime,Source,status,entId,FatherId from dt_Role");
            if (user.entId != "superintendent")
            {
                strSql.Append($" where entid='{user.entId}'");
            }
            var roles = DateTableTool.DataTableToList<Dt_Role>(sql.RtDataTable(strSql.ToString())); 
            ViewBag.roles = roles;
            return Json(new JsonMsg() { Code = true, Msg = "获取成功", Obj = roles });
        }
        #endregion

        #region 添加机构[页面]
        [HttpPost]
        public JsonResult EntIdAdd()
        {
            Dt_User user = RoleFuns.IsLoginAdmin(Session["user"]);
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT entid,entname FROM [dt_entdoc] where  1=1 ");
            if (user.entId != "superintendent")
            {
                strSql.Append($" and entid='{user.entId}'");
            }
            var roles = DateTableTool.DataTableToList<Dt_Entid>(sql.RtDataTable(strSql.ToString()));
            ViewBag.roles = roles;
            return Json(new JsonMsg() { Code = true, Msg = "获取成功", Obj = roles });
        }
        private class Dt_Entid {
           public string entid { get; set; }
           public string entname { get; set; }
        }
        /// <summary>
        /// 修改管理员密码
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="xpwd"></param>
        /// <param name="ypwd"></param>
        /// <returns></returns>
        public JsonResult UpdatePwd(string xpwd,string ypwd)
        {
            try
            {
                Dt_User user = RoleFuns.IsLoginAdmin(Session["user"]);
                string sqlStr = $"select userid,password from dt_users  where userid='{user.userId}' and role_type=2";
                DataTable dt = sql.RtDataTable(sqlStr);
                if (dt != null && dt.Rows.Count > 0)
                {
                    string dpwd = dt.Rows[0]["password"].ToString();
                    if (dpwd != Encryption.GetMD5_16(ypwd))
                    {
                        return Json(new { Code = false, Msg = "原始密码错误" });
                    }
                    string pwd = Encryption.GetMD5_16(xpwd);
                    string sqlU = $"update dt_users set password='{pwd}' where userId='{user.userId}'";
                    bool flag = sql.ExecuteSql(sqlU);
                    if (flag)
                    {
                        return Json(new JsonMsg() { Code = flag, Msg = "修改成功" });
                    }
                    else
                    {
                        return Json(new JsonMsg() { Code = flag, Msg = "修改失败" });

                    }
                    
                }
                else
                {
                    return Json(new JsonMsg() { Code = false, Msg = "修改失败，该账号不存在" });
                }
            }
            catch (Exception ex)
            {
                return Json(new JsonMsg() { Code = false, Msg = ex.Message });
            }
        }
        #endregion

        #region 添加账号[方法]
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult AdminAdd_Action(string username,string password,string Entid,int roles,string truename,string phone,int sex,string address,string khId)
        {
            try
            {
                //JsonMsg msg = RoleFuns.IsLoginPowerAction();
                if (true)//msg.Code)
                {
                    if (StringHelper.IsNull(username, password, roles.ToString(), truename, phone,Entid))
                    {
                        if (DateTableTool.DataTableToList<Dt_Role>(sql.RtDataTable($"select * from Dt_Role where Id='{roles}'")).Count() > 0)
                        {
                            if (sex == 1 || sex == 2)
                            {
                                if (true)//DateTableTool.DataTableToList<dt_users>(sql.RtDataTable($"select * from dt_users where UserName='{username}' and status!=0")).Count() == 0
                                {
                                    Dt_User user = RoleFuns.IsLoginAdmin(Session["user"]);
                                    var dt = sql.RunProcedureDR("Proc_Admin_MembersQuery", new System.Data.SqlClient.SqlParameter[] {
                                        new System.Data.SqlClient.SqlParameter("@type","Pc_AddUserAdmin"),
                                        new System.Data.SqlClient.SqlParameter("@name",truename),
                                        new System.Data.SqlClient.SqlParameter("@telphone",phone),
                                        new System.Data.SqlClient.SqlParameter("@password",Encryption.GetMD5_16(password)),
                                        new System.Data.SqlClient.SqlParameter("@username",username),
                                        new System.Data.SqlClient.SqlParameter("@sex",sex==1?"男":"女"),
                                        new System.Data.SqlClient.SqlParameter("@address",address),
                                        new System.Data.SqlClient.SqlParameter("@RoleId",roles),
                                        new System.Data.SqlClient.SqlParameter("@khId",khId),
                                        new System.Data.SqlClient.SqlParameter("@Entid",Entid),
                                        new System.Data.SqlClient.SqlParameter("@userId",user.userId),
                                    });
                                    if (dt.Rows.Count > 0 && dt.Rows[0]["flag"].ToString() == "1")
                                    {
                                        return Json(new JsonMsg() { Code = true, Msg = "账号添加/编辑成功" });
                                    }
                                    else
                                    {
                                        return Json(new JsonMsg() { Code = false, Msg = "系统繁忙，请稍后再试" });
                                    }
                                }
                                //else
                                //{
                                //    return Json(new JsonMsg() { Code = false, Msg = "用户名已存在" });
                                //}
                            }
                            else
                            {
                                return Json(new JsonMsg() { Code = false, Msg = "性别参数错误" });
                            }
                        }
                        else
                        {
                            return Json(new JsonMsg() { Code = false, Msg = "角色不存在" });
                        }
                    }
                    else
                    {
                        return Json(new JsonMsg() { Code = false, Msg = "参数错误" });
                    }
                    //return Json(new JsonMsg() { Code = false, Msg = "参数错误" });
                }
            }
            catch (Exception ex)
            {
                return Json(new JsonMsg() { Code = false, Msg = ex.Message });
            }
            //else
            //{
            //    return Json(new JsonMsg() { Code = false, Msg = "权限不足" });
            //    //return Json(msg);
            //}
        }
        #endregion

        //#region 编辑账号[页面]

        //public ActionResult AdminEdit()
        //{
        //    RoleFuns.IsLoginPowerView();
        //    var roles = _roleRepository.Entities.Where(m => m.Id > 0).OrderBy(m => m.Id).ToList();
        //    ViewBag.roles = roles;

        //    if (StringHelper.IsNull(Request.Params["id"]))
        //    {
        //        int id = int.Parse(Request.Params["id"]);
        //        var admin = _adminRepository.Entities.FirstOrDefault(m => m.Id == id);
        //        if (admin != null)
        //        {
        //            ViewBag.admin = admin;
        //        }
        //        else
        //        {
        //            Response.Redirect("/ZhiLin/Error/Notfound");
        //        }
        //    }
        //    else
        //    {
        //        Response.Redirect("/ZhiLin/Error/Notfound");
        //    }

        //    return View();
        //}
        //#endregion

        //#region 编辑账号[方法]

        //public JsonResult AdminEdit_Action()
        //{
        //    JsonMsg msg = RoleFuns.IsLoginPowerAction();
        //    if (msg.Code)
        //    {
        //        if (StringHelper.IsNull(Request.Params["id"], Request.Params["isAdministrator"], Request.Params["roles"], Request.Params["truename"], Request.Params["phone"], Request.Params["sex"]))
        //        {
        //            int id = int.Parse(Request.Params["id"]);
        //            //string username = Request.Params["username"].Trim();
        //            string password = Request.Params["password"].Trim();
        //            string confirmpassword = Request.Params["confirmpassword"].Trim();
        //            string safepassword = Request.Params["safepassword"].Trim();
        //            string confirmsafepassword = Request.Params["confirmsafepassword"].Trim();
        //            int isAdministrator = int.Parse(Request.Params["isAdministrator"]);
        //            int roles = int.Parse(Request.Params["roles"]);
        //            string truename = Request.Params["truename"].Trim();
        //            string phone = Request.Params["phone"].Trim();
        //            string email = Request.Params["email"].Trim();
        //            int sex = int.Parse(Request.Params["sex"]);
        //            string address = Request.Params["address"].Trim();
        //            if (password == confirmpassword)
        //            {
        //                if (safepassword == confirmsafepassword)
        //                {
        //                    if (isAdministrator == 1 || isAdministrator == 0)
        //                    {
        //                        if (.Count(m => m.Id == roles) > 0)
        //                        {
        //                            if (sex == 1 || sex == 2)
        //                            {
        //                                var admin = _adminRepository.Entities.FirstOrDefault(m => m.Id == id);
        //                                if (admin != null)
        //                                {
        //                                    //admin.UserName = username;
        //                                    if (StringHelper.IsNull(confirmpassword))
        //                                    {
        //                                        admin.PassWord = (confirmpassword + "ZhiLin").Encrypt();
        //                                    }
        //                                    if (StringHelper.IsNull(confirmsafepassword))
        //                                    {
        //                                        admin.SafePwd = (confirmsafepassword + "ZhiLin").Encrypt();
        //                                    }
        //                                    if (admin.UserName != "administrator")
        //                                    {
        //                                        admin.IsAdinistrator = true;
        //                                        if (isAdministrator == 0)
        //                                        {
        //                                            admin.IsAdinistrator = false;
        //                                        }
        //                                        admin.RoleId = roles;
        //                                    }
        //                                    admin.TrueName = truename;
        //                                    admin.Phone = phone;
        //                                    admin.Email = email;
        //                                    admin.Sex = (AdminSex)sex;
        //                                    admin.Address = address;
        //                                    if (_adminRepository.Update(admin) > 0)
        //                                    {
        //                                        return Json(new JsonMsg() { Code = true, Msg = "账号更新成功" });
        //                                    }
        //                                    else
        //                                    {
        //                                        return Json(new JsonMsg() { Code = false, Msg = "账信息号无更新" });
        //                                    }
        //                                }
        //                                else
        //                                {
        //                                    return Json(new JsonMsg() { Code = false, Msg = "账户不存在" });
        //                                }
        //                            }
        //                            else
        //                            {
        //                                return Json(new JsonMsg() { Code = false, Msg = "性别参数错误" });
        //                            }
        //                        }
        //                        else
        //                        {
        //                            return Json(new JsonMsg() { Code = false, Msg = "角色不存在" });
        //                        }
        //                    }
        //                    else
        //                    {
        //                        return Json(new JsonMsg() { Code = false, Msg = "账号类型参数错误" });
        //                    }
        //                }
        //                else
        //                {
        //                    return Json(new JsonMsg() { Code = false, Msg = "安全密码与确认安全密码不一致" });
        //                }
        //            }
        //            else
        //            {
        //                return Json(new JsonMsg() { Code = false, Msg = "登录密码与确认登录密码不一致" });
        //            }
        //        }
        //        else
        //        {
        //            return Json(new JsonMsg() { Code = false, Msg = "参数错误" });
        //        }
        //    }
        //    else
        //    {
        //        return Json(msg);
        //    }
        //}
        //#endregion

        ////#region 删除账号[方法]
        ////[HttpPost]
        ////[ValidateInput(false)]
        ////public JsonResult AdminDel()
        ////{
        ////    JsonMsg msg = RoleFuns.IsLoginPowerAction();
        ////    if (msg.Code)
        ////    {
        ////        if (StringHelper.IsNull(Request.Params["ids"]))
        ////        {
        ////            string ids = Request.Params["ids"];
        ////            int[] delids = StringHelper.StrsToInts(ids, ',');

        ////            if (delids.Length > 0)
        ////            {
        ////                int m = 0;
        ////                int i = 0;
        ////                foreach (var delid in delids)
        ////                {
        ////                    if (_adminRepository.Entities.FirstOrDefault(n => n.Id == delid)?.UserName != "administrator")
        ////                    {
        ////                        _adminRepository.Delete(delid);
        ////                        i++;
        ////                    }
        ////                    else
        ////                    {
        ////                        m++;
        ////                    }
        ////                }
        ////                if (m > 0 && i > 0)
        ////                {
        ////                    return Json(new JsonMsg() { Code = true, Msg = "账号删除成功<br/>注意：系统管理员账号系统设定为不可删除" });
        ////                }
        ////                else if (m > 0 && i == 0)
        ////                {
        ////                    return Json(new JsonMsg() { Code = true, Msg = "系统管理员账号系统设定为不可删除" });
        ////                }
        ////                else
        ////                {
        ////                    return Json(new JsonMsg() { Code = true, Msg = "账号删除成功" });
        ////                }
        ////            }
        ////            else
        ////            {
        ////                return Json(new JsonMsg() { Code = false, Msg = "参数错误" });
        ////            }
        ////        }
        ////        else
        ////        {
        ////            return Json(new JsonMsg() { Code = false, Msg = "参数错误" });
        ////        }
        ////    }
        ////    else
        ////    {
        ////        return Json(msg);
        ////    }
        ////}
        ////#endregion
    }

    #region Json返回对象
    /// <summary>
    /// Json返回对象
    /// </summary>
    public class JsonMsg
    {
        /// <summary>
        /// 响应结果
        /// </summary>
        public bool Code { get; set; }
        /// <summary>
        /// 响应反馈信息
        /// </summary>
        public string Msg { get; set; }
        /// <summary>
        /// 对象内容
        /// </summary>
        public object Obj { get; set; }
        /// <summary>
        /// 对象内容
        /// </summary>
        public int Page { get; set; }
        /// <summary>
        /// 对象内容
        /// </summary>
        public int Total { get; set; }
    }
    #endregion

    #region 权限菜单tree返回对象
    /// <summary>
    /// 权限菜单tree返回对象
    /// </summary>
    public class MenuTree
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 父级ID
        /// </summary>
        public int? FatherId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Source { get; set; }
    }
    #endregion
}
