using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;
using System.Collections.Concurrent;
using System.Threading;
using Sk_B2BAPI.Models.Admin;
using Sk_B2BAPI.App_Code;

namespace Sk_B2BAPI.App_Code
{
    /// <summary>
    /// 规则封装
    /// </summary>
    public static class RoleFuns
    {
        static SqlRun sql = new SqlRun(SqlRun.sqlstr);
        
        #region 登录验证
        /// <summary>
        /// 登录验证
        /// </summary>
        public static Dt_User IsLoginAdmin(Object entity)
        {
            try
            {
                if (entity == null)
                {
                    return null;
                }
                Dt_User user = DateTableTool.DataTalbeToEntity<Dt_User>((DataTable)entity);
                RoleFuns.UpAdminLoginLog(user.username, DateTime.Now.AddMinutes(30));
                return user;
            }
            catch (Exception e)
            {
                Log.Error("检测登录状态，错误信息", e.Message);
                return null;
            }
        }
        #endregion

        #region 权限认证
        /// <summary>
        /// 后台权限验证
        /// </summary>
        public static bool IsLoginPowerView(Dt_User user, string absolutePath)
        {
            try
            {
                if (user.role_type == 2 || absolutePath == "通用")
                {
                    return true;
                }
                if (user.role_type == 1 && user.role_id <= 0)
                {
                    return false;
                }
                var role = GetRole(user.role_id);
                if (role == null)
                {
                    return false;
                }
                int[] partnoArr = DateTableTool.DataTableToList<Dt_RoleMethod>(sql.RtDataTable("select * from Dt_RoleMethod where RoleId = {" + role.Id + "}")).Select(m => m.MethodId).ToArray();
                var menu = DateTableTool.DataTalbeToEntity<Dt_Method>(sql.RtDataTable("select top 1 * from Dt_Method where Power='" + absolutePath + "'"));
                if (menu != null)
                {
                    return false;
                }
                return true;
            }
            catch (Exception e)
            {
                Log.Error("检测权限状态，错误信息", e.Message);
                return false;
            }
        }
        #endregion

        #region 获取Method对象
        /// <summary>
        /// 获取Method对象
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        public static Dt_Method GetMethod(long? id)
        {
            if (id != null)
            {
                return DateTableTool.DataTalbeToEntity<Dt_Method>(sql.RtDataTable("select * from Dt_Method where Id=" + id));
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region 获取Method导航
        /// <summary>
        /// 获取Method父级导航
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        public static string GetMethodNavTitle(long? id)
        {
            if (id != null)
            {
                var method = GetMethod(id);
                if (method != null)
                {
                    if (method.NavTitle != null)
                    {
                        return method.NavTitle + " > ";
                    }
                    else
                    {
                        return "";
                    }
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }
        #endregion
        
        #region 获取Role对象
        /// <summary>
        /// 获取Role对象
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        public static Dt_Role GetRole(int id)
        {
            return DateTableTool.DataTalbeToEntity<Dt_Role>(sql.RtDataTable("select * from Dt_Role where Id={" + id + "}"));
        }
        #endregion

        #region 获取操作日志
        /// <summary>
        /// 记录管理员操作日志
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        public static int SetAdminLog(string username, string type, List<String> rolelist)
        {
            string logmessage = "";
            switch (type)
            {
                case "GoodsList":
                    //if (!rolelist.Contains(type))
                    //{
                    //    return 0;
                    //}
                    logmessage = "访问了商品列表";
                    break;
                case "GetOrderList":
                    logmessage = "访问了订单列表";
                    break;
                case "GetUserInfo":
                    logmessage = "访问了会员列表";
                    break;
                case "GetImgList":
                    logmessage = "访问了图片列表";
                    break;
                case "GetArticle":
                    logmessage = "访问了资讯列表";
                    break;
                case "GetAdminList":
                    logmessage = "访问了管理员列表";
                    break;
                case "PC_GetConfiguration":
                    logmessage = "访问了系统设置";
                    break;
                case "PC_GetImgType":
                    logmessage = "访问了楼层设置";
                    break;
                case "Pc_GetBusinessCode":
                    logmessage = "访问了证书过期管理";
                    break;
                case "PC_Register":
                    logmessage = "访问了注册审核管理";
                    break;
                case "PC_CustomerTypeBFList":
                    logmessage = "访问了用资质管理";
                    break;
                case "Pc_EntidManageList":
                    logmessage = "访问了机构列表";
                    break;
                case "PC_RechargeGoods":
                    logmessage = "访问了充值活动列表";
                    break;
                case "PC_RechargeOrderList":
                    logmessage = "访问了充值流水列表";
                    break;
                case "GetPromPlanDt":
                    logmessage = "访问了促销方案列表";
                    break;
                case "GetGoodsCategory_yj":
                    logmessage = "访问了分类管理";
                    break;
                case "GiftList":
                    logmessage = "访问了礼品列表";
                    break;
                case "Pc_GovernGoodList":
                    logmessage = "访问了控销商品列表";
                    break;
                case "Pc_GetCoupon":
                    logmessage = "访问了优惠券列表";
                    break;
                case "Pc_GetXFCoupon":
                    logmessage = "访问了优惠券下发管理";
                    break;
                case "PC_UserMoneyList":
                    logmessage = "访问了消费流水列表";
                    break;
                case "Returngoods":
                    logmessage = "访问了退单审核管理";
                    break;
                case "PC_GetCriticismsList":
                    logmessage = "访问了评论审核管理";
                    break;
                case "Query_Brand":
                    logmessage = "访问了品牌专区管理";
                    break;
                case "Pc_GetArrondiProduct":
                    logmessage = "访问了药店门诊管理";
                    break;
                case "Pc_GetAdminLogList":
                    logmessage = "访问了管理员操作日志列表";
                    break;
                case "Pc_GetUserIPList":
                    logmessage = "访问了用户IP捕捉列表";
                    break;
                case "GoodsSearch":
                    logmessage = "访问了搜索频率分析";
                    break;
                case "Pc_CartProductSearch":
                    logmessage = "访问了购物车商品统计分析";
                    break;
                case "Pc_SaveOrderStatistical":
                    logmessage = "访问了下单时间统计分析";
                    break;
                case "Pc_CollectionStatistical":
                    logmessage = "访问了收藏统计分析";
                    break;
                case "Pc_ProductSaveOrder":
                    logmessage = "访问了下单频率统计分析";
                    break;
                case "Pc_UserOrderPrice":
                    logmessage = "访问了客单价统计分析";
                    break;
                case "Pc_OrderSource":
                    logmessage = "访问了订单来源统计分析";
                    break;
                case "Pc_OrderPayType":
                    logmessage = "访问了支付类型统计分析";
                    break;
                case "GetGoodsCategory_help":
                    logmessage = "访问了帮助中心列表";
                    break;
                case "Pc_SetReturnMoney":
                    logmessage = "执行了退款操作";
                    break;
                case "Pc_AuditSalesman":
                    logmessage = "执行了审核注册业务员";
                    break;
                case "Pc_DetAuditSalesman":
                    logmessage = "执行了删除待审核注册业务员";
                    break;
                case "GoodsRecommendList":
                    logmessage = "访问了推荐商品列表";
                    break;
                case "GoodsRecommendType":
                    logmessage = "访问了推荐商品类型数据";
                    break;
                default:
                    logmessage = "访问了"+ type.ToString();
                    break;
            }
            AdminCaoZuoLog.Write(username, logmessage, type);
            return 1;
        }


        #endregion

        #region 获取登录日志
        /// <summary>
        /// 记录管理员登录日志
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="user_name"></param>
        /// <param name="login_ip"></param>
        /// <param name="end_host"></param>
        public static void AddAdminLoginLog(string user_name, string login_ip, string login_prot)
        {
            Action act = () =>
            {
                sql.ExecuteNonQuery("Pc_Log", new SqlParameter[]{
                    new SqlParameter("type","Pc_AddAdminLoginLog"),
                    new SqlParameter("@username", user_name),
                    new SqlParameter("@login_time", DateTime.Now),
                    new SqlParameter("@login_ip", login_ip),
                    new SqlParameter("@end_time",  DateTime.Now.AddMinutes(30)),
                    new SqlParameter("@login_prot",login_prot)
            });
            };
            act.BeginInvoke(d => { }, null);
        }
        /// <summary>
        /// 记录管理员登录日志-修改
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="user_name"></param>
        /// <param name="login_ip"></param>
        /// <param name="end_host"></param>
        public static void UpAdminLoginLog(string user_name, DateTime date)
        {
            Action act = () =>
            {
                sql.ExecuteNonQuery("Pc_Log", new SqlParameter[]{
                    new SqlParameter("type","Pc_AddAdminLoginLog"),
                    new SqlParameter("@username", user_name),
                    new SqlParameter("@end_time", date)
                 });
            };
            act.BeginInvoke(d => { }, null);
        }
        #endregion
    }

    public static class DateTableTool
    {
        /// <summary>
        /// 将DataTable类型的数据转换成List<T>集合 T实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static List<T> DataTableToList<T>(DataTable dataTable)
        {
            List<T> list = new List<T>();
            Type targetType = typeof(T);
            PropertyInfo[] allPropertyArray = targetType.GetProperties();
            foreach (DataRow rowElement in dataTable.Rows)
            {
                T element = Activator.CreateInstance<T>();
                foreach (DataColumn columnElement in dataTable.Columns)
                {
                    foreach (PropertyInfo property in allPropertyArray)
                    {
                        if (property.Name.Equals(columnElement.ColumnName))
                        {
                            if (rowElement[columnElement.ColumnName] == DBNull.Value)
                            {
                                property.SetValue(element, null, null);
                            }
                            else
                            {
                                property.SetValue(element, rowElement
                                [columnElement.ColumnName], null);
                            }
                        }
                    }
                }
                list.Add(element);
            }
            return list;
        }
        /// <summary>
        /// 将DataTable的第一行转换为实体T 表转实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static T DataTalbeToEntity<T>(DataTable dataTable)
        {
            T element = Activator.CreateInstance<T>();
            Type targetType = typeof(T);
            PropertyInfo[] allPropertyArray = targetType.GetProperties();
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                DataRow rowElement = dataTable.Rows[0];
                foreach (DataColumn columnElement in dataTable.Columns)
                {
                    foreach (PropertyInfo property in allPropertyArray)
                    {
                        if (property.Name.Equals(columnElement.ColumnName))
                        {
                            if (rowElement[columnElement.ColumnName] == DBNull.Value)
                            {
                                property.SetValue(element, null, null);
                            }
                            else
                            {
                                property.SetValue(element, rowElement
                                [columnElement.ColumnName], null);
                            }
                        }
                    }
                }
            }
            else
            {
                return default(T);//返回null
            }
            return element;
        }
    }
}
public class AdminCaoZuoLog
{
    private static ConcurrentQueue<AdminLog> queue = new ConcurrentQueue<AdminLog>();
    private static Timer ti = null;
    public static void Start()
    {
        ti = new Timer((d) =>
        {
            AdminLog alog;
            queue.TryDequeue(out alog);
            if (alog == null) { return; }
            AdminLogHelper.WriteAdminLog(alog.username, alog.logmessage, alog.type);
        });
        ti.Change(0, 500);
    }
    public static void Stop()
    {
        if (ti != null)
        {
            ti.Dispose();
            ti = null;
        }
    }
    class AdminLog
    {
        public string username { get; set; }
        public string logmessage { get; set; }
        public string type { get; set; }
        public AdminLog(string username, string logmessage, string type)
        {
            this.username = username;
            this.logmessage = logmessage;
            this.type = type;
        }
    }
    public static void Write(string username, string logmessage, string type)
    {
        queue.Enqueue(new AdminLog(username, logmessage, type));
    }
}

public class AdminLogHelper
{
    static SqlRun sql = new SqlRun(SqlRun.sqlstr);
    private static object obj = new object();

    /// <summary>
    /// 记录错误信息
    /// </summary>
    /// <param name="errorTitle">发生错误的模块</param>
    /// <param name="se">异常信息模型</param>
    public static void WriteAdminLog(string username, string logmessage, string type)
    {
        lock (obj)
        {
            logmessage = "管理员【" + username + "】于时间:" + DateTime.Now + "进行了：" + logmessage;
            sql.ExecuteNonQuery("Pc_Log", new SqlParameter[] {
                    new SqlParameter("@type","Pc_AddAdminLog"),
                    new SqlParameter("@username",username),
                    new SqlParameter("@logmessage",logmessage),
                    new SqlParameter("@datenow",DateTime.Now),
                    new SqlParameter("@recalltype",type)
            });
        }
    }
}