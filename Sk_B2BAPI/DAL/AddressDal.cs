using Sk_B2BAPI.App_Code;
using Sk_B2BAPI.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Sk_B2BAPI.DAL
{
    public class AddressDal
    {
        #region 用户地址
        /// <summary>
        /// 获取用户地址
        /// </summary>
        /// <param name="entId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<Address> GetUserAddress(string entId,string userId)
        {
            DataTable dt = new DataTable();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id,user_id,accept_name,province,city,prefecture,address,telphone,is_default from dt_user_address (nolock) where entId=@entid and user_Id=@userId order by is_default desc");
            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@entid",entId),
                new SqlParameter("@userId",userId)
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            dt = sql.RunSqlDataTable(strSql.ToString(), param);
            List<Address> list = new List<Address>();
            if (dt.Rows.Count > 0)
            {
                list = SetUserAddress(dt);
            }
            return list;
        }
        /// <summary>
        /// 填充Address
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public List<Address> SetUserAddress(DataTable dt)
        {
            List<Address> list = new List<Address>();
            foreach (DataRow dr in dt.Rows)
            {
                Address a = new Address();
                a.Id = dr["id"].ToString();
                a.UserId = dr["user_id"].ToString();
                a.Accept_Name = dr["accept_name"].ToString();
                a.Province = dr["province"].ToString();
                a.City = dr["city"].ToString();
                a.Prefecture = dr["prefecture"].ToString();
                a.UserAddress = dr["address"].ToString();
                a.Telphone = dr["telphone"].ToString();
                a.IsDefault = int.Parse(dr["is_default"].ToString());
                list.Add(a);
            }
            return list;
        }
        #endregion

        #region 编辑地址
        /// <summary>
        /// 修改或添加用户地址
        /// </summary>
        /// <param name="entId">企业Id</param>
        /// <param name="userId">用户Id</param>
        /// <param name="id">地址Id（修改id/新建0）</param>
        /// <param name="acceptName">收货人</param>
        /// <param name="address">收货地址</param>
        /// <param name="telphone">收货人电话</param>
        /// <returns></returns>
        public bool UpdateUserAddress(string entId, string userId, int id, string acceptName, string province, string city, string prefecture, string address, string telphone, int isDefault)
        {
            DataTable dt = new DataTable();
            StringBuilder strSql = new StringBuilder();
            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@type","UpdateAddress"),
                new SqlParameter("@entid",entId),
                new SqlParameter("@userId",userId),
                new SqlParameter("@addressId",id),
                new SqlParameter("@accept_Name",acceptName),
                new SqlParameter("@province",province),
                new SqlParameter("@city",city),
                new SqlParameter("@prefecture",prefecture),
                new SqlParameter("@address",address),
                new SqlParameter("@telphone",telphone),
                new SqlParameter("@isDefault",isDefault)
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            int n = sql.ExecuteNonQuery("Proc_UserInfo", param);
            if (n > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region 删除地址
        /// <summary>
        /// 删除地址
        /// </summary>
        /// <param name="userId">用户</param>
        /// <param name="entId">机构</param>
        /// <param name="id">用户地址ID</param>
        /// <param name="msg">返回操作信息</param>
        /// <returns>成功返回true/失败返回false</returns>
        public bool DeleteAddress(string userId, string entId, int id,out string msg)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                SqlParameter[] param = new SqlParameter[]{
                    new SqlParameter("@type","DeleteAddress"),
                    new SqlParameter("@entid",entId),
                    new SqlParameter("@userId",userId),
                    new SqlParameter("@addressId",id)
                };
                SqlRun sql = new SqlRun(SqlRun.sqlstr);
                int n = sql.ExecuteNonQuery("Proc_UserInfo", param);
                if (n > 0)
                {
                    msg = "操作成功";
                    return true;
                }
                else
                {
                    msg = "操作失败";
                    return false;
                }
            }
            catch (System.Exception ex)
            {
                msg = ex.Message.ToString();
                return false;
            }
        }
        #endregion
    }
}