using Sk_B2BAPI.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.DAL
{
    public class UserGroupDal
    {
        #region 小组实体类
        public class Group{

        }
        public class funbool {
            public bool fiag { get; set; }
            public string msg { get; set; }

            public funbool(bool fiag, string msg) {
                this.fiag = fiag;
                this.msg = msg;
            }
        }
        #endregion

        SqlRun sql;

        #region 创建小队
        public funbool AddUserGroup(string groupname,string telphone,string usergroupid) {
            sql=new SqlRun(SqlRun.sqlstr);
            DataTable dt=sql.RunProcedureDR("PC_Group", new SqlParameter[]{
                                                new SqlParameter("@type","AddUserGroup"),
                                                new SqlParameter("@GroupName",groupname),
                                                new SqlParameter("@TelPhone",telphone),
                                                new SqlParameter("@UserGroupId",usergroupid)});
            if (dt.Rows.Count > 0) {
                if (dt.Rows[0]["flag"].ToString() == "1")
                {
                    return new funbool(true, dt.Rows[0]["msg"].ToString());
                }
                else {
                    return new funbool(false, dt.Rows[0]["msg"].ToString());
                }
            }
            else {
                return new funbool(false,"数据库未查出成功提示!");
            }
        }
        #endregion

        #region 编辑小组
        public funbool UpdateUserGroup(string groupname, string groupid)
        {
            sql = new SqlRun(SqlRun.sqlstr);
            DataTable dt = sql.RunProcedureDR("PC_Group", new SqlParameter[]{
                                                new SqlParameter("@type","UpdateUserGroup"),
                                                new SqlParameter("@GroupName",groupname),
                                                new SqlParameter("@GroupId",groupid)}
                                            );
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["flag"].ToString() == "1")
                {
                    return new funbool(true, dt.Rows[0]["msg"].ToString());
                }
                else
                {
                    return new funbool(false, dt.Rows[0]["msg"].ToString());
                }
            }
            else
            {
                return new funbool(false, "数据库未查出成功提示!");
            }
        }
        #endregion

        #region 解散小组
        public funbool DelUserGroup(string usergroupid, string groupid)
        {
            sql = new SqlRun(SqlRun.sqlstr);
            DataTable dt = sql.RunProcedureDR("PC_Group", new SqlParameter[]{
                                                new SqlParameter("@type","DelUserGroup"),
                                                new SqlParameter("@UserGroupId",usergroupid),
                                                new SqlParameter("@GroupId",groupid)}
                                            );
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["flag"].ToString() == "1")
                {
                    return new funbool(true, dt.Rows[0]["msg"].ToString());
                }
                else
                {
                    return new funbool(false, dt.Rows[0]["msg"].ToString());
                }
            }
            else
            {
                return new funbool(false, "数据库未查出成功提示!");
            }
        }
        #endregion

        #region 邀请组队
        public funbool InvitationUserGroup(string usergroupid, string groupid,string telphone)
        {
            sql = new SqlRun(SqlRun.sqlstr);
            DataTable dt = sql.RunProcedureDR("PC_Group", new SqlParameter[]{
                                                new SqlParameter("@type","InvitationUserGroup"),
                                                new SqlParameter("@UserGroupId",usergroupid),
                                                new SqlParameter("@TelPhone",telphone),
                                                new SqlParameter("@GroupId",groupid)}
                                            );
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["flag"].ToString() == "1")
                {
                    return new funbool(true, dt.Rows[0]["msg"].ToString());
                }
                else
                {
                    return new funbool(false, dt.Rows[0]["msg"].ToString());
                }
            }
            else
            {
                return new funbool(false, "数据库未查出成功提示!");
            }
        }
        #endregion

        #region 同意组队
        public funbool ConsentUserGroup(string usergroupid, string groupid, string telphone)
        {
            sql = new SqlRun(SqlRun.sqlstr);
            DataTable dt = sql.RunProcedureDR("PC_Group", new SqlParameter[]{
                                                new SqlParameter("@type","ConsentUserGroup"),
                                                new SqlParameter("@UserGroupId",usergroupid),
                                                new SqlParameter("@GroupId",groupid)}
                                            );
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["flag"].ToString() == "1")
                {
                    return new funbool(true, dt.Rows[0]["msg"].ToString());
                }
                else
                {
                    return new funbool(false, dt.Rows[0]["msg"].ToString());
                }
            }
            else
            {
                return new funbool(false, "数据库未查出成功提示!");
            }
        }
        #endregion

        #region 拒绝组队
        public funbool RefuseUserGroup(string usergroupid, string groupid, string telphone)
        {
            sql = new SqlRun(SqlRun.sqlstr);
            DataTable dt = sql.RunProcedureDR("PC_Group", new SqlParameter[]{
                                                new SqlParameter("@type","RefuseUserGroup"),
                                                new SqlParameter("@UserGroupId",usergroupid),
                                                new SqlParameter("@GroupId",groupid)}
                                            );
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["flag"].ToString() == "1")
                {
                    return new funbool(true, dt.Rows[0]["msg"].ToString());
                }
                else
                {
                    return new funbool(false, dt.Rows[0]["msg"].ToString());
                }
            }
            else
            {
                return new funbool(false, "数据库未查出成功提示!");
            }
        }
        #endregion

        #region 开放小组登录

        #endregion

        #region 开放小组流水共享

        #endregion

        #region 获取小组信息及成员

        #endregion

        #region 踢出小组成员

        #endregion
    }
}