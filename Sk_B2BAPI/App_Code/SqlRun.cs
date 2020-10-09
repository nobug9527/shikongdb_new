using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.App_Code
{
    public class SqlRun
    {
        public string connectionString = "";
        private static object obj = new object();
        public bool IsOpen()
        {
            //var connection = new SqlConnection(connectionString);
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    if (connection.State == ConnectionState.Open)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (SqlException)
                {
                    return false;
                }
            }
        }
        ///数据库连接字符
        public static string sqlstr = ConfigurationManager.ConnectionStrings["localconnection"].ConnectionString;
        /// <summary>
        /// 构造方法DBAccess
        /// </summary>
        /// <param name="connStr">连接字符串</param>
        public SqlRun(string connStr)
        {
            try
            {
                if (connectionString == "")
                {
                    connectionString = connStr;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        //=========下为数据库操作部分 添加、删除、修改（SQL语句或procedure）    ===============
        /// <summary>
        /// SQL语句用于增、删、改
        /// </summary>
        /// <param name="strSql">SQL语句</param>
        /// <returns></returns>
        public bool ExecuteSql(string strSql)
        {
            bool yesornot = false;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand comm= new SqlCommand(strSql, connection))
                {
                    try
                    {
                        connection.Open();
                        int n = comm.ExecuteNonQuery();
                        if (n > 0)yesornot = true;
                    }
                    catch
                    {
                        yesornot = false;
                    }
                }
            }
            return yesornot;
        }
        /// <summary>
        /// 执行sql语句，返回受影响行数
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public int ExecuteSql(string strSql, params SqlParameter[] commandParameters)
        {

            using (SqlCommand cmd = new SqlCommand())
            {
                var connection = new SqlConnection(connectionString);
                try
                {
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    cmd.Connection = connection;
                    cmd.CommandText = strSql;
                    cmd.CommandTimeout = 60;
                    cmd.CommandType = CommandType.Text;

                    if (commandParameters != null)
                    {
                        foreach (SqlParameter parm in commandParameters)
                            cmd.Parameters.Add(parm);
                    }
                    int val = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    return val;
                }
                catch (System.Data.SqlClient.SqlException E)
                {
                    throw new Exception(E.Message, E);
                }
                finally
                {
                    //cmd.Dispose();
                    connection.Dispose();
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }
        }
        /// <summary>
        /// 执行sql语句，返回DataTable
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataSet</returns>
        public DataTable RunSqlDataTable(string cmdText, params SqlParameter[] commandParameters)
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                var connection = new SqlConnection(connectionString);
                try
                {
                    using (SqlDataAdapter sqlDA = new SqlDataAdapter())
                    {
                        PrepareCommand(cmd, connection, null, CommandType.Text, cmdText, commandParameters);
                        sqlDA.SelectCommand = cmd;
                        //SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                        DataTable dt = new DataTable();
                        sqlDA.Fill(dt);
                        return dt;
                    }
                }
                catch (System.Data.SqlClient.SqlException E)
                {
                    throw new Exception(E.Message, E);
                }
                finally
                {
                    cmd.Dispose();
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }
        }

        /// <summary>
        /// 执行sql语句，返回受影响的行数
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public int RunSqlNumber(string cmdText, params SqlParameter[] commandParameters)
        {
            using (SqlCommand cmd=new SqlCommand())
            {
                var connection = new SqlConnection(connectionString);
                try
                {
                    PrepareCommand(cmd, connection, null, CommandType.Text, cmdText, commandParameters);
                    int val = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    return val;
                }
                catch (System.Data.SqlClient.SqlException E)
                {
                    throw new Exception(E.Message, E);
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }
        }
        #region ===========获取SQL字段第一行第一字段的数值，请不要用select===========
        /// <summary>
        /// 执行SQL语句，返回记录的条数【注意是记录数】；
        /// 获取SQL字段第一行第一字段的数值，请不要用select
        /// </summary>
        /// <param name="SQLstring">SQL语句</param>
        /// <returns>object</returns>
        public object RunTSqlScalar(string SQLstring)
        {
            var connection = new SqlConnection(connectionString);
            using (SqlCommand cmd = new SqlCommand(SQLstring, connection))
            {
                try
                {

                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    object val = cmd.ExecuteScalar();

                    return val;
                }
                catch (System.Data.SqlClient.SqlException E)
                {
                    throw new Exception(E.Message, E);
                }
                finally
                {
                    cmd.Dispose();
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }


        }
        #endregion
        #region ===========执行存储过程返回第一条记录第一列的object===========
        /// <summary>
        /// 执行一条返回第一条记录第一列的object
        /// 使用参数数组提供参数
        /// </summary>
        /// <param name="cmdText">存储过程的名字</param>
        /// <param name="commandParameters">以数组形式提供SqlCommand命令中用到的参数列表</param>
        /// <returns>返回一个object类型的数据，可以通过 Convert.To{Type}方法转换类型</returns>
        public object ExecuteScalar(string cmdText, params SqlParameter[] commandParameters)
        {
            var connection = new SqlConnection(connectionString);
            using (SqlCommand cmd = new SqlCommand())
            {
                try
                {
                    PrepareCommand(cmd, connection, null, CommandType.StoredProcedure, cmdText, commandParameters);
                    object val = cmd.ExecuteScalar();
                    cmd.Parameters.Clear();
                    return val;
                }
                catch (System.Data.SqlClient.SqlException E)
                {
                    throw new Exception(E.Message, E);
                }
                finally
                {
                    cmd.Dispose();
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }
        }
        #endregion
        /// <summary>
        /// 执行存储过程，返回受影响行数
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string cmdText, params SqlParameter[] commandParameters)
        {

            using (SqlCommand cmd = new SqlCommand())
            {
                var connection = new SqlConnection(connectionString);
                try
                {
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    cmd.Connection = connection;
                    cmd.CommandText = cmdText;
                    cmd.CommandTimeout = 60;
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (commandParameters != null)
                    {
                        foreach (SqlParameter parm in commandParameters)
                            cmd.Parameters.Add(parm);
                    }
                    int val = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    return val;
                }
                catch (System.Data.SqlClient.SqlException E)
                {
                    throw new Exception(E.Message, E);
                }
                finally
                {
                    cmd.Dispose();
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }
        }
        /// <summary>
        /// 使用事务执行存储过程
        /// </summary>
        /// <param name="cmdText">存储过程名字的数组</param>
        /// <param name="param">参数的数组序列</param>
        /// <returns></returns>
        public bool ExecuteNonQuery(string[] cmdText, List<SqlParameter[]> param)
        {

            using (SqlCommand cmd = new SqlCommand())
            {
                var connection = new SqlConnection(connectionString);
                bool b = true;
                try
                {
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    SqlTransaction sqlTransaction = connection.BeginTransaction();
                    cmd.Connection = connection;
                    cmd.Transaction = sqlTransaction;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        if (cmdText.Length != param.Count)
                        {
                            Exception e = new Exception("命令条数与参数条目不匹配");
                            throw e;
                        }
                        for (int i = 0; i < cmdText.Length; i++)
                        {
                            cmd.CommandText = cmdText[i];
                            cmd.CommandTimeout = 60;
                            if (param[i] != null)
                            {
                                cmd.Parameters.Clear();
                                for (int j = 0; j < param[i].Length; j++)
                                {
                                    cmd.Parameters.Add(param[i][j]);
                                }
                            }
                            int nn = cmd.ExecuteNonQuery();
                            if (nn <= 0)
                                b = b & false;
                        }
                        sqlTransaction.Commit();
                    }
                    catch (System.Data.SqlClient.SqlException E)
                    {
                        sqlTransaction.Rollback();
                        throw new Exception(E.Message, E);
                    }
                    if (!b)
                        sqlTransaction.Rollback();
                }
                catch (System.Data.SqlClient.SqlException E)
                {
                    throw new Exception(E.Message, E);
                }
                finally
                {
                    cmd.Dispose();
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }

                return b;
            }
        }
        #region ===========执行存储过程返回DataTable/DataSet===========
        /// <summary>
        /// 执行存储过程返回DataTable
        /// </summary>
        /// <param name="cmdText">存储过程名</param>
        /// <param name="commandParameters">存储过程参数</param>
        /// <returns>DataTable</returns>
        public DataTable RunProcedureDR(string cmdText, params SqlParameter[] commandParameters)
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                var connection = new SqlConnection(connectionString);
                try
                {
                    using (SqlDataAdapter sqlDA = new SqlDataAdapter())
                    {
                        PrepareCommand(cmd, connection, null, CommandType.StoredProcedure, cmdText, commandParameters);
                        sqlDA.SelectCommand = cmd;
                        //SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                        DataTable dt = new DataTable();
                        sqlDA.Fill(dt);
                        return dt;
                    }
                }
                catch (System.Data.SqlClient.SqlException E)
                {
                    throw new Exception(E.Message, E);
                }
                finally
                {
                    cmd.Dispose();
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }
        }
        /// <summary>
        /// 分页用返回DataSet
        /// </summary>
        /// <param name="cmdText">存储过程名称</param>
        /// <param name="commandParameters">参数数组</param>
        /// <returns></returns>
        public DataSet RunProDataSet(string cmdText, params SqlParameter[] commandParameters)
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                var connection = new SqlConnection(connectionString);
                try
                {
                    using (SqlDataAdapter sqlDA = new SqlDataAdapter())
                    {
                        DataSet ds = new DataSet();
                        PrepareCommand(cmd, connection, null, CommandType.StoredProcedure, cmdText, commandParameters);
                        sqlDA.SelectCommand = cmd;
                        //SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                        sqlDA.Fill(ds);
                        return ds;
                    }
                }
                catch (System.Data.SqlClient.SqlException E)
                {
                    throw new Exception(E.Message, E);
                }
                finally
                {
                    cmd.Dispose();
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }
        }
        #endregion
        #region=======执行sql语句返回DataTable==========================
        /// <summary>
        /// 执行sql语句获取datatable
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable RtDataTable(string sql)
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                var connection = new SqlConnection(connectionString);
                try
                {
                    using (SqlDataAdapter sqlDA = new SqlDataAdapter())
                    {
                        cmd.Connection = connection;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = sql;
                        cmd.CommandTimeout = 60;
                        sqlDA.SelectCommand = cmd;
                        DataTable dt_cm = new DataTable();
                        sqlDA.Fill(dt_cm);
                        return dt_cm;
                    }
                }
                catch (System.Data.SqlClient.SqlException E)
                {
                    throw new Exception(E.Message, E);
                }
                finally
                {
                    cmd.Dispose();
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }
        }
        /// <summary>
        /// 执行sql语句获取dataset
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public DataSet RtDataSet(string sql)
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                var connection = new SqlConnection(connectionString);
                try
                {
                    using (SqlDataAdapter sqlDA = new SqlDataAdapter())
                    {
                        cmd.Connection = connection;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = sql;
                        cmd.CommandTimeout = 60;
                        sqlDA.SelectCommand = cmd;
                        DataSet ds_cm = new DataSet();
                        sqlDA.Fill(ds_cm);
                        return ds_cm;
                    }
                }
                catch (System.Data.SqlClient.SqlException E)
                {
                    throw new Exception(E.Message, E);
                }
                finally
                {
                    cmd.Dispose();
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }
        }
        #endregion
        #region ===========为执行命令准备参数===========
        /// <summary>
        /// 为执行命令准备参数
        /// </summary>
        /// <param name="cmd">SqlCommand 命令</param>
        /// <param name="connection">已经存在的数据库连接</param>
        /// <param name="trans">数据库事物处理</param>
        /// <param name="cmdType">SqlCommand命令类型 (存储过程， T-SQL语句， 等等。)</param>
        /// <param name="cmdText">Command text，T-SQL语句 例如 Select * from Products</param>
        /// <param name="cmdParms">返回带参数的命令</param>
        private void PrepareCommand(SqlCommand cmd, SqlConnection connection, SqlTransaction trans, CommandType cmdType, string cmdText, SqlParameter[] cmdParms)
        {

            //判断数据库连接状态
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            cmd.Connection = connection;
            cmd.CommandText = cmdText;
            cmd.CommandTimeout = 60;

            //判断是否需要事务处理
            if (trans != null)
                cmd.Transaction = trans;

            cmd.CommandType = cmdType;

            if (cmdParms != null)
            {
                foreach (SqlParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }
        #endregion

        #region 获取中文表头
        /// <summary>
        /// 中文表头
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public DataTable GetChinaName(DataTable dt)
        {
            string cmdText = "select fdname,chnname from dt_fldlist where fdname in(";
            if (dt == null || dt.Columns.Count == 0)
            {
                return null;
            }
            foreach (DataColumn dc in dt.Columns)
            {
                cmdText += "'" + dc.ColumnName.Trim() + "',";
            }
            cmdText = cmdText.Substring(0, cmdText.Length - 1);
            cmdText += ")";
            using (SqlCommand cmd = new SqlCommand())
            {
                var connection = new SqlConnection(SqlRun.sqlstr);
                try
                {
                    using (SqlDataAdapter sqlDA = new SqlDataAdapter())
                    {
                        cmd.Connection = connection;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = cmdText;
                        cmd.CommandTimeout = 60;
                        sqlDA.SelectCommand = cmd;
                        DataTable dt_new = new DataTable();
                        sqlDA.Fill(dt_new);
                        return dt_new;
                    }
                }
                catch (System.Data.SqlClient.SqlException E)
                {
                    throw new Exception(E.Message, E);
                }
                finally
                {
                    cmd.Dispose();
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }
        }
        public object GetObjValue(string sqltxt)
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                var connection = new SqlConnection(SqlRun.sqlstr);
                try
                {
                    using (SqlDataAdapter sqlDA = new SqlDataAdapter())
                    {
                        if (connection.State != ConnectionState.Open)
                        {
                            connection.Open();
                        }

                        cmd.Connection = connection;


                        cmd.Connection = connection;

                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = sqltxt;
                        cmd.CommandTimeout = 60;
                        object b = cmd.ExecuteScalar();
                        return b;
                    }
                }
                catch (System.Data.SqlClient.SqlException E)
                {
                    throw new Exception(E.Message, E);
                }
                finally
                {
                    cmd.Dispose();
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }
        }
        #endregion

        #region 检测SQL语句的正确性，但不执行SQL语句
        /// <summary>
        /// 检测SQL语句的正确性，但不执行SQL语句
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public bool ValidateSQL(string sql,ref string msg)
        {
            bool bResult;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    if (conn.State != ConnectionState.Open)
                        conn.Open();
                    cmd.Connection = conn;
                    cmd.CommandText = "SET PARSEONLY ON";
                    cmd.ExecuteNonQuery();
                    try
                    {
                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();
                        bResult = true;
                    }
                    catch (Exception ex)
                    {
                        msg = ex.Message;
                        bResult = false;
                    }
                    finally
                    {
                        cmd.CommandText = "SET PARSEONLY OFF";
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            return bResult;
        }
        #endregion
    }
}