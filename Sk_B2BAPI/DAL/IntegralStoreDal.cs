using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sk_B2BAPI.Models;
using System.Data;
using System.Data.SqlClient;
using Sk_B2BAPI.App_Code;
using Sk_B2BAPI.Tool;

namespace Sk_B2BAPI.DAL
{
    /// <summary>
    /// 
    /// </summary>
    public class IntegralStoreDal
    {
        /// <summary>
        /// 积分商城首页楼层及楼层所有数据
        /// </summary>
        /// <param name="entId">机构</param>
        /// <param name="flootType">楼层 Office【办公用品】/Electrical【家用电器】/Supplies【生活用品】/HotFor【热门兑换】亦可传空获取所有楼层，但传空时num>0</param>
        /// <param name="num">前n条</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageCount"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public List<IndexFloor> IntegralFloor(string entId, string flootType, int num, int pageIndex, int pageSize, out int pageCount, out int recordCount)
        {
            SqlParameter[] sqls = new SqlParameter[]
            {
                new SqlParameter("@Type","IndexIntegral"),
                new SqlParameter("@FootFLId",flootType),
                new SqlParameter("@EntId",entId),
                new SqlParameter("@Num",num),
                new SqlParameter("@PageIndex",pageIndex),
                new SqlParameter("@PageSize",pageSize)
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataSet set = sql.RunProDataSet("Proc_PremiunAdmin", sqls);
            List<IndexFloor> integralGoods = new List<IndexFloor>();
            pageCount = 1;
            recordCount = 0;
            if (set.Tables.Count>0)
            {
                integralGoods = FillIntegralGoods(set.Tables[1]);
                pageCount = int.Parse(set.Tables[2].Rows[0]["pageCount"].ToString());
                recordCount = int.Parse(set.Tables[2].Rows[0]["recordCount"].ToString());
            }
            else
            {
                integralGoods = null;
            }
            return integralGoods;
        }

        public List<IndexFloor> FillIntegralGoods(DataTable table)
        {
            ///获取网站ip
            string web_url = BaseConfiguration.SercerIp;
            List<IndexFloor> list = new List<IndexFloor>();
            var query = from p in table.AsEnumerable()
                        group p by new { t1 = p.Field<int>("id"), t2 = p.Field<string>("floottype"), t3 = p.Field<string>("TypeName"),t4=p.Field<string>("entId") } into floors
                        select new { Id = floors.Key.t1, FlootType = floors.Key.t2, TypeName = floors.Key.t3,entId=floors.Key.t4 };
            foreach (var floor in query)
            {
                IndexFloor indexFloor = new IndexFloor()
                {
                    Entid=floor.entId.ToString(),
                    FloorId=floor.Id.ToString(),
                    TypeName = floor.TypeName.ToString(),
                    FloorType=floor.FlootType.ToString().Trim()
                };
                DataTable data = table.Clone();
                DataRow[] dataRows = table.Select("floottype='" + floor.FlootType.ToString() + "'");
                List<ImgInfo> imgInfos = new List<ImgInfo>();
                foreach (DataRow item in dataRows)
                {
                    ImgInfo imgInfo = new ImgInfo()
                    {
                        Entid = item["entId"].ToString(),
                        SubTitle = item["goodsname"].ToString(),
                        ArticleId = item["goodsid"].ToString(),
                        Price = item["price"].ToString(),
                        Integral = item["integral"].ToString(),
                        DrugSpec=item["drug_spec"].ToString(),
                        Quantity=item["quantity"].ToString(),
                        PackageUnit=item["package_unit"].ToString()
                    };
                    if (item["img_url"].ToString() != "")
                    {
                        if (item["img_url"].ToString().Contains("http://"))
                        {
                            imgInfo.ImgUrl = item["img_url"].ToString();
                        }
                        else
                        {
                            imgInfo.ImgUrl = web_url + item["img_url"].ToString();
                        }
                    }
                    else
                    {
                        imgInfo.ImgUrl = "";
                    }
                    imgInfos.Add(imgInfo);
                }
                indexFloor.FloorDetail = imgInfos;
                list.Add(indexFloor);
            }
            return list;
        }

        /// <summary>
        /// PC积分商城下一批
        /// </summary>
        /// <param name="entId"></param>
        /// <param name="flootType"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageCount"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public List<ImgInfo> NextGroup(string entId, string flootType, int pageIndex, int pageSize, out int pageCount, out int recordCount)
        {
            SqlParameter[] sqls = new SqlParameter[]
            {
                new SqlParameter("@Type","NextGroup"),
                new SqlParameter("@FootFLId",flootType),
                new SqlParameter("@EntId",entId),
                new SqlParameter("@PageIndex",pageIndex),
                new SqlParameter("@PageSize",pageSize)
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataSet set = sql.RunProDataSet("Proc_PremiunAdmin", sqls);
            pageCount = 1;
            recordCount = 0;
            List<ImgInfo> imgInfos = new List<ImgInfo>();
            if (set.Tables.Count > 0)
            {
                imgInfos = FillNextGroup(set.Tables[1]);
                pageCount = int.Parse(set.Tables[2].Rows[0]["pageCount"].ToString());
                recordCount = int.Parse(set.Tables[2].Rows[0]["recordCount"].ToString());
            }
            else
            {
                imgInfos = null;
            }
            return imgInfos;
        }

        /// <summary>
        /// 填充下一批数据
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public List<ImgInfo> FillNextGroup(DataTable table)
        {
            ///获取网站ip
            string web_url = BaseConfiguration.SercerIp;
            List<ImgInfo> imgInfos = new List<ImgInfo>();
            foreach (DataRow item in table.Rows)
            {
                ImgInfo imgInfo = new ImgInfo()
                {
                    Entid = item["entId"].ToString(),
                    SubTitle = item["goodsname"].ToString(),
                    ArticleId = item["goodsid"].ToString(),
                    Price = item["price"].ToString(),
                    Integral = item["integral"].ToString(),
                    DrugSpec = item["drug_spec"].ToString(),
                    Quantity = item["quantity"].ToString(),
                    PackageUnit = item["package_unit"].ToString()
                };
                if (item["img_url"].ToString() != "")
                {
                    if (item["img_url"].ToString().Contains("http://"))
                    {
                        imgInfo.ImgUrl = item["img_url"].ToString();
                    }
                    else
                    {
                        imgInfo.ImgUrl = web_url + item["img_url"].ToString();
                    }
                }
                else
                {
                    imgInfo.ImgUrl = "";
                }
                imgInfos.Add(imgInfo);
            }
            return imgInfos;
        }

        /// <summary>
        /// 积分商品详情
        /// </summary>
        /// <param name="entId"></param>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        public string IntegralGoods(string entId, string goodsId)
        {
            string json = string.Empty;
            SqlParameter[] param = new SqlParameter[] 
            {
              new SqlParameter("@type","jifenDetail"),
              new SqlParameter("@goodsid",goodsId),
              new SqlParameter("@entId",entId)
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataSet ds = sql.RunProDataSet("Proc_Premiun_details", param);
            if (ds.Tables[0].Rows.Count > 0)
            {
                json = JsonHelper.GetDataJson(0, 0, 0, ds.Tables[0]);
            }
            else
            {
                json = JsonHelper.GetErrorJson(1, 0, "暂无数据");
            }
            return json;
        }

        /// <summary>
		/// 加入购物车&&删除商品&&修改数量
		/// </summary>
        /// <param name="entId">机构</param>
        /// <param name="goodsId">积分商品</param>
        /// <param name="num">商品数量</param>
        /// <param name="type">操作类型</param>
        /// <param name="userid">用户</param>
		public int AddGwc(string type, string userid,string goodsId,string entId,decimal num)
        {
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataSet ds = sql.RunProDataSet("Proc_Premiun_details", new SqlParameter[]{
                new SqlParameter("@type",type),
                new SqlParameter("@shl",num),
                new SqlParameter("@goodsid",goodsId),
                new SqlParameter("@userid",userid),
                new SqlParameter("@entId",entId)
            });
            if (ds.Tables[0].Rows.Count > 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 获取地址
        /// </summary>
        /// <param name="type">操作类型</param>
        /// <param name="userid">用户</param>
        /// <param name="entId">机构</param>
        /// <returns></returns>
        public string GetAddress(string type, string userid,string entId)
        {
            string json = "";
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataSet ds = sql.RunProDataSet("Proc_Premiun_details", new SqlParameter[]{
            new SqlParameter("@type",type),
            new SqlParameter("@userid",userid),
            new SqlParameter("@entId",entId)
            });
            if (ds.Tables[0].Rows.Count > 0)
            {
                json = JsonHelper.GetDataJson(0, 0, 0, ds.Tables[0]);
            }
            else
            {
                json = JsonHelper.GetErrorJson(1, 0, "暂无数据");
            }
            return json;
        }

        /// <summary>
        /// 获取推荐商品
        /// </summary>
        public string GetGoods(string type,string entId,int page)
        {
            var msg = "";
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataSet dt = sql.RunProDataSet("Proc_Premiun_details", new SqlParameter[]{
                new SqlParameter("@type",type),
                new SqlParameter("@page",page),
                new SqlParameter("@entId",entId)
            });
            if (dt.Tables[0].Rows.Count > 0)
            {
                msg = JsonHelper.GetDataJson(0, int.Parse(dt.Tables[1].Rows[0][0].ToString()), 0, dt.Tables[0]);
            }
            else
            {
                msg = JsonHelper.GetErrorJson(1, 0, "暂无数据");
            }
            return msg;
        }

        /// <summary>
        /// 获取积分
        /// </summary>
        public string GetJF(string type, string userid,string entId)
        {
            var msg = "";
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataSet ds = sql.RunProDataSet("Proc_Premiun_user", new SqlParameter[]{
            new SqlParameter("@type",type),new SqlParameter("@userid",userid),new SqlParameter("@entId",entId)
            });
            if (ds.Tables[0].Rows.Count > 0)
            {
                msg = JsonHelper.GetDataJson(0, 0, 0, ds.Tables[0]);
            }
            else
            {
                msg = JsonHelper.GetErrorJson(1, 0, "暂无数据");
            }
            return msg;
        }

        /// <summary>
        /// 获取积分流水&&获取积分订单汇总
        /// </summary>
        public string GetMsg(string type, string userid,string entId,int page,int size)
        {
            var msg = "";
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataSet ds = sql.RunProDataSet("Proc_Premiun_user", 
                new SqlParameter[]{
                    new SqlParameter("@type",type),
                    new SqlParameter("@userid",userid),
                    new SqlParameter("@entId",entId),
                    new SqlParameter("@pageIndex",page),
                    new SqlParameter("@pageSize",size)
                }
            );
            if (ds.Tables[1].Rows.Count > 0)
            {
                msg = JsonHelper.GetDataJson(0, int.Parse(ds.Tables[2].Rows[0]["recordCount"].ToString()), int.Parse(ds.Tables[2].Rows[0]["pageCount"].ToString()), ds.Tables[1]);
            }
            else
            {
                msg = JsonHelper.GetErrorJson(1, 0, "暂无数据");
            }
            return msg;
        }

        /// <summary>
        /// 获取积分订单明细
        /// </summary>
        public string GetOrderDetail(string type, string userid, string entId,string djbh)
        {
            var msg = "";
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataSet ds = sql.RunProDataSet("Proc_Premiun_user", new SqlParameter[]{
                new SqlParameter("@type",type),
                new SqlParameter("@userid",userid),
                new SqlParameter("@entId",entId),
                new SqlParameter("@djbh",djbh)
            });
            if (ds.Tables[0].Rows.Count > 0)
            {
                msg = JsonHelper.GetDataJson(0, int.Parse(ds.Tables[1].Rows[0][0].ToString()), 0, ds.Tables[0]);
            }
            else
            {
                msg = JsonHelper.GetErrorJson(1, 0, "暂无数据");
            }
            return msg;
        }

        public string SettleAccounts(string goodsidStr,int num,string userid,string entid,string zjf,string address,string contact,string phone,string remarks)
        {
            string result;
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataSet ds = sql.RunProDataSet("Proc_Premiun_JieSuan",
                new SqlParameter[]{
                    new SqlParameter("@type","jiesuan"),
                    new SqlParameter("@contact",contact),
                    new SqlParameter("@phone",phone),
                    new SqlParameter("@remarks",remarks),
                    new SqlParameter("@goodsid",goodsidStr),
                    new SqlParameter("@num",num),
                    new SqlParameter("@userid",userid),
                    new SqlParameter("@entid",entid),
                    new SqlParameter("@totaljf",zjf),
                    new SqlParameter("@address",address)
                }
            );
            if (ds.Tables[0].Rows.Count > 0)
            {
                var djbh = ds.Tables[0].Rows[0][0].ToString();
                var msg = "您的单据编号为：" + djbh + ",此礼品将会随同订购药品一同配送，如此次单独兑换礼品则该礼品随同下次订购药品一同配送.。";
                result = "{\"return_code\":\"0\",\"msg\":\"结算成功," + msg + "\"}";
            }
            else
            {
                result = "{\"return_code\":\"1\",\"msg\":\"结算失败\"}";
            }
            return result;
        }
    }
}