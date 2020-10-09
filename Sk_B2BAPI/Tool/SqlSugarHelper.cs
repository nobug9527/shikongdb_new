using SqlSugar;
using System.Configuration;
using System.Web.Mvc;

namespace Sk_B2BAPI.Tool
{
    public class SqlSugarHelper : Controller
    {
        public SqlSugarHelper()
        {
            Db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString,
                DbType = DbType.SqlServer,
                IsAutoCloseConnection = true
            });
        }
        public SqlSugarClient Db;
    }

    public class SqlSugarDb
    {
        public SqlSugarDb()
        {
            Db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString,
                DbType = DbType.SqlServer,
                IsAutoCloseConnection = true
            });
        }
        public SqlSugarClient Db;
    }
}