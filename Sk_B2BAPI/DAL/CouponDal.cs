using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sk_B2BAPI.Models;
using System.Data;
using System.Data.SqlClient;
using Sk_B2BAPI.App_Code;

namespace Sk_B2BAPI.DAL
{
    public class CouponDal
    {
        /// <summary>
        /// 获取用户优惠券
        /// </summary>
        /// <param name="entId">机构</param>
        /// <param name="userId">用户</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="pageSize">每页条目</param>
        /// <param name="enable">是否使用</param>
        /// <param name="limit">是否过期</param>
        /// <param name="recordCount">总条目</param>
        /// <param name="pageCount">总页数</param>
        /// <returns></returns>
        public List<Coupon> UserCouponList(string entId,string userId,string source, int pageIndex,int pageSize, int enable, out int recordCount, out int pageCount)
        {
            recordCount = 0;pageCount = 0;
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@type","UserCouponList"),
                new SqlParameter("@entId",entId),
                new SqlParameter("@userId",userId),
                new SqlParameter("@enable",enable),
                new SqlParameter("@pageIndex",pageIndex),
                new SqlParameter("@pageSize",pageSize)
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            var list = new List<Coupon>();
            var set = sql.RunProDataSet("Proc_OperationCoupon", parameters);
            if (set.Tables.Count>=3)
            {
                pageCount = int.Parse(set.Tables[2].Rows[0]["pageCount"].ToString());
                recordCount = int.Parse(set.Tables[2].Rows[0]["recordCount"].ToString());
                int code = 10006;
                switch (source)
                {
                    case "苹果端":
                        code = 10005;
                        break;
                    case "电脑端":
                        code = 10000;
                        break;
                    case "安卓端":
                        code = 10003;
                        break;

                }
                list = FillList(set.Tables[1]).Where(a => a.TypeCoding == 10006 | a.TypeCoding == code).ToList();
            }
            return list;
        }

        /// <summary>
        /// 个人中心优惠券数据填充
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public List<Coupon> FillList(DataTable table)
        {
            var list = new List<Coupon>();
            foreach (DataRow item in table.Rows)
            {
                var coupon = new Coupon()
                {
                    CouponName = item["couponName"].ToString().Trim(),
                    CouponCode=int.Parse( item["couponCode"].ToString()),
                    CouponsNumber=int.Parse( item["couponsNumber"].ToString()),
                    StartingTime = item["startingTime"].ToString().Trim(),
                    EndTime = item["endTime"].ToString().Trim(),
                    TypeCoding =int.Parse( item["typeCoding"].ToString()),
                    Status =int.Parse( item["status"].ToString()),
                    ReceivingType =int.Parse( item["receivingType"].ToString()),
                    SceneCoding =int.Parse( item["SceneCoding"].ToString()),
                    IsDel =int.Parse( item["IsDel"].ToString()),
                    ProductCode = item["ProductCode"].ToString().Trim(),
                    AllAmout = decimal.Parse(item["AllAmount"].ToString()),
                    Num = item["Num"].ToString().Trim(),
                    Types =int.Parse( item["types"].ToString()),
                    SceneId = item["SceneId"].ToString().Trim(),
                    FullAmount = decimal.Parse(item["fullAmount"].ToString()),
                    Deduction = decimal.Parse(item["deduction"].ToString()),
                    MaximumAmount = decimal.Parse(item["maximumAmount"].ToString()),
                    Discount =decimal.Parse(item["discount"].ToString()),
                    Number = item["number"].ToString().Trim(),
                    TypeName=item["Name"].ToString().Trim(),
                    GoodsName=item["goodsname"].ToString(),
                    Area=item["area"].ToString().Trim(),
                    SceneName=item["SceneName"].ToString().Trim()
                };
                list.Add(coupon);
            }
            return list;
        }
        /// <summary>
        /// 领券中心领取优惠券列表
        /// </summary>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页条目</param>
        /// <param name="pageCount">总页数</param>
        /// <param name="recordCount">总条数</param>
        /// <returns></returns>
        public List<CouponCentre> CouponList(int pageIndex, int pageSize,string userId,string entId,string source, out int pageCount,out int recordCount)
        {
            pageCount = 0; recordCount = 0;
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@type","CouponList"),
                new SqlParameter("@userId",userId),
                new SqlParameter("@entId",entId),
                new SqlParameter("@pageIndex",pageIndex),
                new SqlParameter("@pageSize",pageSize)
            };
            var list = new List<CouponCentre>();
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataSet set = sql.RunProDataSet("Proc_OperationCoupon", parameters);
            if (set.Tables.Count > 0)
            {
                pageCount = int.Parse(set.Tables[2].Rows[0]["pageCount"].ToString());
                recordCount = int.Parse(set.Tables[2].Rows[0]["recordCount"].ToString());
                int code = 10006;
                switch (source) {
                    case "苹果端":
                        code = 10005;
                        break;
                    case "电脑端":
                        code = 10000;
                        break;
                    case "安卓端":
                        code = 10003;
                        break;

                }
                list = CFillList(set.Tables[1]).Where(a => a.TypeCoding == 10006 | a.TypeCoding == code).ToList();
            }
            return list;
        }
        /// <summary>
        /// 领券中心数据填充
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public List<CouponCentre> CFillList(DataTable table)
        {
            var list = new List<CouponCentre>();
            foreach (DataRow item in table.Rows)
            {
                var CouponCentre = new CouponCentre()
                {
                    CouponName = item["couponName"].ToString().Trim(),
                    CouponCode = int.Parse(item["couponCode"].ToString()),
                    CouponsNumber = int.Parse(item["couponsNumber"].ToString()),
                    ylsl = int.Parse(item["ylsl"].ToString()),
                    StartingTime = item["startingTime"].ToString().Trim(),
                    EndTime = item["endTime"].ToString().Trim(),
                    TypeCoding = int.Parse(item["typeCoding"].ToString()),
                    Status = int.Parse(item["status"].ToString()),
                    ReceivingType = int.Parse(item["receivingType"].ToString()),
                    SceneCoding = int.Parse(item["SceneCoding"].ToString()),
                    IsDel = int.Parse(item["IsDel"].ToString()),
                    ProductCode = item["ProductCode"].ToString().Trim(),
                    AllAmout = decimal.Parse(item["AllAmount"].ToString()),
                    Num = item["Num"].ToString().Trim(),
                    Types = int.Parse(item["types"].ToString()),
                    SceneId = item["SceneId"].ToString().Trim(),
                    FullAmount = decimal.Parse(item["fullAmount"].ToString()),
                    Deduction = decimal.Parse(item["deduction"].ToString()),
                    MaximumAmount = decimal.Parse(item["maximumAmount"].ToString()),
                    Discount = decimal.Parse(item["discount"].ToString()),
                    Number = item["number"].ToString().Trim(),
                    UserId = item["UserID"].ToString(),
                    TypeName = item["Name"].ToString().Trim(),
                    GoodsName = item["goodsname"].ToString(),
                    SceneName=item["SceneName"].ToString(),
                    Area=item["area"].ToString().Trim()
                };
                CouponCentre.yhqzl = CouponCentre.CouponsNumber + CouponCentre.ylsl;
                list.Add(CouponCentre);
            }
            return list;
        }
        /// <summary>
        /// 领取优惠券
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="entId"></param>
        /// <param name="couponCode"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public string GetCoupon(string userId, string entId, int couponCode,out bool flag)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@type","GetCoupon"),
                new SqlParameter("@userId",userId),
                new SqlParameter("@entId",entId),
                new SqlParameter("@couponCode",couponCode)
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataTable table = sql.RunProcedureDR("Proc_OperationCoupon", parameters);
            if (table.Rows.Count > 0)
            {
                flag = 0 == int.Parse(table.Rows[0]["flag"].ToString()) ? true : false;
                return table.Rows[0]["msg"].ToString().Trim();
            }
            else
            { 
                flag = false;
                return "优惠券领取失败！";
            }
        }
        /// <summary>
        /// 结算可用优惠券
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="entId"></param>
        /// <param name="goodsList"></param>
        /// <returns></returns>
        public List<UserCoupon> UsableCoupon(string userId, string entId, string goodsList,int pageIndex,int pageSize,string ywyId, out int pageCount, out int recordCount,string channelName, string Pricelevel,string clientType, decimal OrdersAmount, decimal RealAmount, decimal DiscountAmount,decimal PtAmount)
        {
            pageCount = 0;recordCount = 0;
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@type","UsableCoupon"),
                new SqlParameter("@userId",userId),
                new SqlParameter("@entId",entId),
                new SqlParameter("@ywyId",ywyId),
                new SqlParameter("@goodsList",goodsList),
                new SqlParameter("@pageIndex",pageIndex),
                new SqlParameter("@pageSize",pageSize),
                new SqlParameter("@channelName",channelName),
                new SqlParameter("@jgjb",Pricelevel),
                new SqlParameter("@clientType",clientType),
                new SqlParameter("@ordersAmount",OrdersAmount),
                new SqlParameter("@realAmount",RealAmount),
                new SqlParameter("@discountAmount",DiscountAmount),
                new SqlParameter("@ptAmount",PtAmount)
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            var list = new List<UserCoupon>();
            DataSet set = sql.RunProDataSet("Proc_OperationCoupon", parameters);
            if (set.Tables.Count>=3)
            {
                pageCount = int.Parse(set.Tables[2].Rows[0]["pageCount"].ToString());
                recordCount = int.Parse(set.Tables[2].Rows[0]["recordCount"].ToString());
                if (set.Tables[1].Rows.Count>0)
                {
                    list = FillUableList(set.Tables[1]);
                }
            }
            return list;
        }
        /// <summary>
        /// 结算优惠券填充
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public List<UserCoupon> FillUableList(DataTable table)
        {
            var list = new List<UserCoupon>();
            foreach (DataRow item in table.Rows)
            {
                var coupon = new UserCoupon()
                {
                    CouponName = item["couponName"].ToString().Trim(),
                    CouponCode = int.Parse(item["couponCode"].ToString()),
                    CouponsNumber = int.Parse(item["couponsNumber"].ToString()),
                    StartingTime = item["startingTime"].ToString().Trim(),
                    EndTime = item["endTime"].ToString().Trim(),
                    TypeCoding = int.Parse(item["typeCoding"].ToString()),
                    Status = int.Parse(item["status"].ToString()),
                    ReceivingType = int.Parse(item["receivingType"].ToString()),
                    SceneCoding = int.Parse(item["SceneCoding"].ToString()),
                    IsDel = int.Parse(item["IsDel"].ToString()),
                    ProductCode = item["ProductCode"].ToString().Trim(),
                    AllAmout = decimal.Parse(item["AllAmount"].ToString()),
                    Num = item["Num"].ToString().Trim(),
                    Types = int.Parse(item["types"].ToString()),
                    SceneId = item["SceneId"].ToString().Trim(),
                    FullAmount = decimal.Parse(item["fullAmount"].ToString()),
                    Deduction = decimal.Parse(item["deduction"].ToString()),
                    MaximumAmount = decimal.Parse(item["maximumAmount"].ToString()),
                    Discount = decimal.Parse(item["discount"].ToString()),
                    Number = item["number"].ToString().Trim(),
                    TypeName = item["Name"].ToString().Trim(),
                    GoodsName = item["goodsname"].ToString(),
                    UserCouponId = int.Parse(item["UserCouponId"].ToString()),
                    SceneName = item["SceneName"].ToString().Trim(),
                    Area = item["area"].ToString().Trim()
                };
                list.Add(coupon);
            }
            return list;
        }

        /// <summary>
		/// 单个优惠券信息
		/// </summary>
		/// <param name="entId"></param>
		/// <param name="userId"></param>
		/// <param name="couponId"></param>
		/// <returns></returns>
		public List<Coupon> SingleCoupon(string entId, string userId, string couponId)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@type","SingleCoupon"),
                new SqlParameter("@entId",entId),
                new SqlParameter("@userId",userId),
                new SqlParameter("@couponId",couponId)
            };

            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            var list = new List<Coupon>();
            var set = sql.RunProDataSet("Proc_OperationCoupon", parameters);
            if (set.Tables.Count >0)
            {
                list = FillList(set.Tables[0]);
            }
            return list;
        }
    }
}