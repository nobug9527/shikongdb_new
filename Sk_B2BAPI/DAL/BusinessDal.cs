using Sk_B2BAPI.App_Code;
using Sk_B2BAPI.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Sk_B2BAPI.DAL
{
    public class BusinessDal
    {
        /// <summary>
        /// 公司列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="entId"></param>
        /// <param name="isjh"></param>
        /// <param name="isxs"></param>
        /// <param name="strWhere"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="recordCount"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        public List<Business> BusnessList(string userId,string entId, string isjh, string isxs, string strWhere, int pageIndex, int pageSize, out int recordCount, out int pageCount)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@type","BusinessList"),
                new SqlParameter("@userId",userId),
                new SqlParameter("@entId",entId),
                new SqlParameter("@strWhere",strWhere),
                new SqlParameter("@isjh",isjh),
                new SqlParameter("@isxs",isxs),
                new SqlParameter("@pageIndex",pageIndex),
                new SqlParameter("@pageSize",pageSize)
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataSet set;
            
            set = sql.RunProDataSet("Proc_OperationBusiness", parameters);
            List<Business> list = new List<Business>();
            recordCount = 0; pageCount = 0;
            if (set.Tables.Count >= 3)
            {
                pageCount = int.Parse(set.Tables[2].Rows[0]["pageCount"].ToString());
                recordCount = int.Parse(set.Tables[2].Rows[0]["recordCount"].ToString());
                if (set.Tables[1].Rows.Count>0)
                {
                    list = FillList(set.Tables[1], userId);
                }
            }
            return list;
        }
        /// <summary>
        /// 代课下单客户
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="entId"></param>
        /// <param name="isjh"></param>
        /// <param name="isxs"></param>
        /// <param name="strWhere"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="recordCount"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        public List<Business> ClientList(string userId, string entId, string isjh, string isxs, string strWhere, int pageIndex, int pageSize, out int recordCount, out int pageCount)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@type","ClientList"),
                new SqlParameter("@userId",userId),
                new SqlParameter("@entId",entId),
                new SqlParameter("@strWhere",strWhere),
                new SqlParameter("@isjh",isjh),
                new SqlParameter("@isxs",isxs),
                new SqlParameter("@pageIndex",pageIndex),
                new SqlParameter("@pageSize",pageSize)
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataSet set;

            set = sql.RunProDataSet("Proc_OperationBusiness", parameters);
            List<Business> list = new List<Business>();
            recordCount = 0; pageCount = 0;
            if (set.Tables.Count >= 3)
            {
                pageCount = int.Parse(set.Tables[2].Rows[0]["pageCount"].ToString());
                recordCount = int.Parse(set.Tables[2].Rows[0]["recordCount"].ToString());
                if (set.Tables[1].Rows.Count>0)
                {
                    list = FillList(set.Tables[1]);
                }
            }
            return list;
        }
        /// <summary>
        /// 代客下单购物车有商品客户
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="entId"></param>
        /// <param name="isjh"></param>
        /// <param name="isxs"></param>
        /// <param name="strWhere"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="recordCount"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        public List<Business> CartClientList(string userId, string entId, string isjh, string isxs, string strWhere, int pageIndex, int pageSize, out int recordCount, out int pageCount)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@type","CartBusinessList"),
                new SqlParameter("@userId",userId),
                new SqlParameter("@entId",entId),
                new SqlParameter("@strWhere",strWhere),
                new SqlParameter("@isjh",isjh),
                new SqlParameter("@isxs",isxs),
                new SqlParameter("@pageIndex",pageIndex),
                new SqlParameter("@pageSize",pageSize)
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataSet set;

            set = sql.RunProDataSet("Proc_OperationBusiness", parameters);
            List<Business> list = new List<Business>();
            recordCount = 0; pageCount = 0;
            if (set.Tables.Count >= 3)
            {
                pageCount = int.Parse(set.Tables[2].Rows[0]["pageCount"].ToString());
                recordCount = int.Parse(set.Tables[2].Rows[0]["recordCount"].ToString());
                if (set.Tables[1].Rows.Count>0)
                {
                    list = FillList(set.Tables[1]);
                }
            }
            return list;
        }
        /// <summary>
        /// 代课下单客户数据填充
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public List<Business> FillList(DataTable table)
        {
            List<Business> list = new List<Business>();
            foreach (DataRow item in table.Rows)
            {
                Business business = new Business()
                {
                    UserId=item["userid"].ToString().Trim(),
                    EntId=item["entid"].ToString().Trim(),
                    BusinessName = item["businessname"].ToString().Trim(),
                    BusinessId = item["businessid"].ToString().Trim(),
                    BusinessCode=item["businesscode"].ToString().Trim()
                };
                list.Add(business);
            }
            return list;
        }
        /// <summary>
        /// 填充数据
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public List<Business> FillList(DataTable table,string userId)
        {
            List<Business> businesses = new List<Business>();
            //用户注册时获取单位
            if (userId=="")
            {
                foreach (DataRow item in table.Rows)
                {
                    Business business = new Business()
                    {
                        BusinessName = item["businessname"].ToString().Trim(),
                        BusinessId = item["businessid"].ToString().Trim()
                    };
                    businesses.Add(business);
                }
            }
            else//代客下单时获取单位
            {
                foreach (DataRow item in table.Rows)
                {
                    Business business = new Business()
                    {
                        BusinessName = item["businessname"].ToString().Trim(),
                        BusinessId = item["businessid"].ToString().Trim(),
                        Address = item["address"].ToString().Trim(),
                        AddTime = item["add_time"].ToString().Trim(),
                        Beactive = item["beactive"].ToString().Trim(),
                        BusinessCode = item["businesscode"].ToString().Trim(),
                        BusinessCont = item["businesscont"].ToString().Trim(),
                        BusinessLicense = item["yyzzyxq"].ToString().Trim(),
                        City = item["city"].ToString().Trim(),
                        ClientType = item["clienttype"].ToString().Trim(),
                        County = item["county"].ToString().Trim(),
                        EntId = item["entid"].ToString().Trim(),
                        GSP = item["gspzsyxq"].ToString().Trim(),
                        LastModifyTime = item["lastmodifytime"].ToString().Trim(),
                        Licence = item["xkzyxq"].ToString().Trim(),
                        Logogram = item["logogram"].ToString().Trim(),
                        PriceLevel = item["shortname"].ToString().Trim(),
                        Principal = item["wtr"].ToString().Trim(),
                        Province = item["province"].ToString().Trim(),
                        Proxy = item["wtsyxq"].ToString().Trim(),
                        Sell = item["isxs"].ToString().Trim(),
                        Stock = item["isjh"].ToString().Trim(),
                        Telphone = item["telephone"].ToString().Trim()
                    };
                    businesses.Add(business);
                }
            }

            return businesses;
        }

        /// <summary>
        /// 客户类型
        /// </summary>
        /// <returns></returns>
        public List<ClintType> ClientType()
        {
            SqlParameter[] sqls = new SqlParameter[]
            {
                new SqlParameter("@type","ClientType")
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataTable table = sql.RunProcedureDR("Proc_OperationUser", sqls);
            List<ClintType> types = new List<ClintType>();
            if (table.Rows.Count>0)
            {
                return types = FillClientType(table);
            }
            else
            {
                return types = null;
            }
        }

        /// <summary>
        /// 客户类型填充数据
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public List<ClintType> FillClientType(DataTable table)
        {
            List<ClintType> types = new List<ClintType>();
            foreach (DataRow row in table.Rows)
            {
                ClintType type = new ClintType()
                {
                    Id=int.Parse(row["ID"].ToString()),
                    TypeId=row["TypeID"].ToString(),
                    TypeName=row["name"].ToString(),
                    ClientType=row["clienttype"].ToString()
                };
                types.Add(type);
            }
            return types;
        }

        /// <summary>
        /// 资料类型
        /// </summary>
        /// <returns></returns>
        public List<UserMaterial> MaterialType(string customerType)
        {
            SqlParameter[] sqls = new SqlParameter[]
            {
                new SqlParameter("@type","MaterialType"),
                new SqlParameter("@customerType",customerType)
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataTable table = sql.RunProcedureDR("Proc_OperationUser", sqls);
            List<UserMaterial> types = new List<UserMaterial>();
            if (table.Rows.Count > 0)
            {
                return types = FillMaterial(table);
            }
            else
            {
                return types = null;
            }
        }

        /// <summary>
        /// 资料信息填充
        /// </summary>
        /// <param name="set"></param>
        /// <returns></returns>
        public List<UserMaterial> FillMaterial(DataTable table)
        {
            List<UserMaterial> infos = new List<UserMaterial>();
            foreach (DataRow item in table.Rows)
            {
                UserMaterial material = new UserMaterial()
                {
                    Id=int.Parse(item["id"].ToString()),
                    MaterialName = item["materialName"].ToString(),
                    Remark=item["remark"].ToString()
                };
                infos.Add(material);
            }
            return infos;
        }
    }
}