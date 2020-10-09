using Aop.Api.Domain;
using NPOI.SS.Formula.Functions;
using Sk_B2BAPI.App_Code;
using Sk_B2BAPI.Models;
using Sk_B2BAPI.Tool;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Category = Sk_B2BAPI.Models.Category;

namespace Sk_B2BAPI.DAL
{
    public class ImgInfoDal
    {
        #region 获取图片信息
        /// <summary>
        /// 商城首页图片获取
        /// </summary>
        /// <param name="Num"></param>
        /// <param name="ImgType"></param>
        /// <param name="source">pc/app</param>
        /// <returns></returns>
        public List<ImgInfo> GetImgInfo(int num, string imgType, string entId, string source, string singular)
        {
            DataTable dt = new DataTable();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ");
            if (num != 0)
            {
                strSql.Append("top " + num + "");
            }
            if (singular == "")
            {
                strSql.Append("ID,Sort_id,title,isnull(img_url,'') as img_url,isnull(link_url,'') as link_url,ImgType,article_id,beizhu,entid,isnull(androidlinkurl,'') as androidlinkurl,androidlinktype,TypeName from (select row_number() over(partition by a.ImgType, a.imgTypeS order by add_time desc) as bh, a.ID, a.Sort_id, a.title, a.img_url, a.link_url, a.ImgType, a.imgTypeS, a.ArticleID as article_id, a.status, a.link_name, a.xh, a.beizhu, a.entid, a.source, a.add_time,isnull(a.androidlinkurl,'') as androidlinkurl,isnull(a.androidlinktype,0) as androidlinktype , b.TypeName from dt_mall_attribute a(nolock) join dt_ImgType b(nolock) on a.imgTypeS = b.ImgType where a.ImgType = @ImgType and a.Entid = @entId and a.source = @source and a.status=2 ) t where bh = 1 ");
            }
            else
            {
                strSql.Append(" a.ID, a.Sort_id, a.title, isnull(a.img_url,'') as img_url, isnull(a.link_url,'') as link_url, a.ImgType, a.imgTypeS, a.ArticleID as article_id, a.status, a.link_name, a.xh, a.beizhu, a.entid, a.source, a.add_time,isnull(a.androidlinkurl,'') as androidlinkurl,isnull(a.androidlinktype,0) as androidlinktype , b.TypeName from dt_mall_attribute a(nolock) join dt_ImgType b(nolock) on a.imgTypeS = b.ImgType where a.ImgType = @ImgType and a.Entid = @entId and a.source = @source and a.status=2  order by Sort_id asc ");
            }

            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@ImgType",imgType),
                new SqlParameter("@entId",entId),
                new SqlParameter("@source",source)
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            dt = sql.RunSqlDataTable(strSql.ToString(), param);
            List<ImgInfo> IList = new List<ImgInfo>();
            if (dt.Rows.Count > 0)
            {
                IList = SetImgInfo(dt);
            }
            return IList;
        }
        /// <summary>
        /// 获取商品详情图片
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        public List<ImgInfo> GetGoodsDetailImg(string articleId)
        {
            DataTable dt = new DataTable();
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select entid,id,sort_id,'Goods' as ImgType,article_id,isnull(img_url,'') as img_url,'' as androidlinkurl,0 as androidlinktype,'商品图片' as TypeName");
            strSql.Append(" from dt_article_albums(nolock) where article_id=@articleId");
            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@articleId",articleId)
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            dt = sql.RunSqlDataTable(strSql.ToString(), param);
            List<ImgInfo> IList = new List<ImgInfo>();
            if (dt.Rows.Count > 0)
            {
                IList = SetImgInfo(dt);
            }
            return IList;
        }
        /// <summary>
        /// 填充Models(ImgInfo)
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private List<ImgInfo> SetImgInfo(DataTable dt)
        {
            ///获取网站ip
            string web_url = BaseConfiguration.SercerIp;
            List<ImgInfo> IList = new List<ImgInfo>();
            foreach (DataRow dr in dt.Rows)
            {
                ImgInfo img = new ImgInfo();
                img.Entid = dr["Entid"].ToString().Trim();
                img.Id = int.Parse(dr["ID"].ToString().Trim());
                img.SortId = int.Parse(dr["Sort_id"].ToString().Trim());
                img.ImgType = dr["ImgType"].ToString().Trim();
                img.ArticleId = dr["article_id"].ToString().Trim();
                img.TypeName = dr["TypeName"].ToString().Trim();
                img.AndroidLinkUrl = dr["androidlinkurl"].ToString().Trim();
                img.AndroidLinkType = int.Parse(dr["androidlinktype"].ToString());
                if (dr["img_url"].ToString().Trim() != "")
                {
                    img.ImgUrl = web_url + dr["img_url"].ToString().Trim();
                }
                else
                {
                    img.ImgUrl = "";
                }
                if (dr.Table.Columns.Contains("link_url"))
                {
                    img.LinkUrl = dr["link_url"].ToString().Trim();
                }
                if (dr.Table.Columns.Contains("link_url"))
                {
                    img.Title = dr["title"].ToString().Trim();
                }
                if (dr.Table.Columns.Contains("beizhu"))
                {
                    img.BeiZhu = dr["beizhu"].ToString().Trim();
                }
                IList.Add(img);
            }
            return IList;
        }
        
        #endregion==================================================================

        #region 分类/导航栏
        /// <summary>
        /// 获取商品分类/首页导航栏
        /// </summary>
        /// <param name="Channel_Id"></param>
        /// <returns></returns>
        public List<Models.Category> GetCategory(string entid, int channelId)
        {
            DataTable dt = new DataTable();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id,channel_id,title,call_index,parent_id,class_list,class_layer,sort_id,isnull(link_url,'') as link_url,isnull(img_url,'') as img_url,content,entid ");
            strSql.Append(" from dt_article_category where channel_id=@Channel_Id /*and entid=@EntId*/ ");
            strSql.Append(" order by sort_id asc,id desc ");
            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@Channel_Id",channelId),
                new SqlParameter("@EntId",entid)
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            dt = sql.RunSqlDataTable(strSql.ToString(), param);
            List<Models.Category> CList = new List<Models.Category>();
            if (dt.Rows.Count > 0)
            {
                CList = SetCategory(dt);
            }
            return CList;
        }
        /// <summary>
        /// 根据商品分类id获取列表
        /// </summary>
        /// <param name="CategoryId">分类Id</param>
        /// <returns></returns>
        public List<Category> GetCategory(string categoryId, string entId)
        {
            ///
            if (entId == "")
            {
                entId = BaseConfiguration.EntId;//默认企业id
            }
            DataTable dt = new DataTable();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id,channel_id,title,call_index,parent_id,class_list,class_layer,sort_id,isnull(link_url,'') as link_url,isnull(img_url,'') as img_url,content,entid ");
            strSql.Append(" from dt_article_category(nolock) a where channel_id=7 ");
            //strSql.Append(" from dt_article_category(nolock) a where channel_id=7 and entid=@EntId ");
            strSql.Append(" and exists(select 1 from dt_article_category b(nolock) where b.id=@categoryId and a.parent_id=b.parent_id)");
            strSql.Append(" order by sort_id asc,id desc ");
            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@categoryId",categoryId),
                //new SqlParameter("@EntId",entId)
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            dt = sql.RunSqlDataTable(strSql.ToString(), param);
            List<Category> CList = new List<Category>();
            if (dt.Rows.Count > 0)
            {
                CList = SetCategory(dt);
            }
            return CList;
        }
        /// <summary>
        /// 填充Models(Category)
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public List<Models.Category> SetCategory(DataTable dt)
        {
            ///获取网站ip
            string web_url = BaseConfiguration.SercerIp;
            List<Models.Category> CList = new List<Models.Category>();
            ///加载一级分类
            DataRow[] yjdr = dt.Select("class_layer=1", "sort_id asc");
            foreach (DataRow dr in yjdr)
            {
                Models.Category yj = new Category();
                yj.Entid = dr["entid"].ToString().Trim();
                yj.ID = int.Parse(dr["id"].ToString().Trim());
                yj.Channel_Id = int.Parse(dr["channel_id"].ToString().Trim());
                yj.Call_Index = dr["call_index"].ToString().Trim();
                yj.Title = dr["title"].ToString().Trim();
                yj.Parent_Id = int.Parse(dr["parent_id"].ToString().Trim());
                yj.Class_List = dr["class_list"].ToString().Trim();
                yj.Sort_Id = int.Parse(dr["sort_id"].ToString().Trim());
                yj.Link_Url = dr["link_url"].ToString().Trim();
                if (dr["img_url"].ToString().Trim() != "")
                {
                    yj.Img_Url = web_url + dr["img_url"].ToString().Trim();
                }
                else
                {
                    yj.Img_Url = "";
                }
                yj.Content = dr["content"].ToString().Trim();
                DataRow[] ejdr = dt.Select("class_layer=2 and parent_id=" + yj.Parent_Id + "", "sort_id asc");
                if (ejdr.Length > 0)
                {
                    ///加载二级分类
                    List<Category> CList_Sed = new List<Category>();
                    foreach (DataRow Sdr in ejdr)
                    {
                        Category ej = new Category();
                        ej.Entid = Sdr["entid"].ToString().Trim();
                        ej.ID = int.Parse(Sdr["id"].ToString().Trim());
                        ej.Channel_Id = int.Parse(Sdr["channel_id"].ToString().Trim());
                        ej.Call_Index = Sdr["call_index"].ToString().Trim();
                        ej.Title = Sdr["title"].ToString().Trim();
                        ej.Parent_Id = int.Parse(Sdr["parent_id"].ToString().Trim());
                        ej.Class_List = Sdr["class_list"].ToString().Trim();
                        ej.Sort_Id = int.Parse(Sdr["sort_id"].ToString().Trim());
                        ej.Link_Url = Sdr["link_url"].ToString().Trim();
                        if (Sdr["img_url"].ToString().Trim() != "")
                        {
                            ej.Img_Url = web_url + Sdr["img_url"].ToString().Trim();
                        }
                        else
                        {
                            ej.Img_Url = "";
                        }
                        ej.Content = Sdr["content"].ToString().Trim();
                        CList_Sed.Add(ej);
                    }
                    yj.Category_Scd = CList_Sed;
                }
                CList.Add(yj);
            }
            return CList;
        }
        #endregion==============================================

        #region 获取首页楼层信息
        public List<IndexFloor> GetIndexFloor(string userId, string entId)
        {
            ///获取用户信息
            UserInfoDal dal = new UserInfoDal();
            List<UserInfo> user = dal.GetUserInfo(userId, entId);

            string jgjb = "", KhType = "";
            bool landing = false;
            bool staleDated = false;
            if (user.Count > 0)
            {
                entId = user[0].EntId;
                jgjb = user[0].Pricelevel;
                KhType = user[0].KhType;
                landing = true;
                staleDated = user[0].StaleDated;
            }

            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@type","floorImg"),
                new SqlParameter("@jgjb",jgjb),
                new SqlParameter("@khlb",KhType),
                new SqlParameter("@EntId",entId),
                new SqlParameter("@userId",userId),
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            var ds = sql.RunProDataSet("Proc_GoodsList", param);
            List<IndexFloor> list = new List<IndexFloor>();
            if (ds.Tables.Count > 0)
            {
                list = FillFloor(ds.Tables[0], ds.Tables[1], entId, landing, staleDated, jgjb, userId);
            }
            return list;
        }

        #region PrefectureFillFloor
        public List<IndexFloor> GetZQIndexFloor(string userId, string entId)
        {
            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@type","PrefectureRecommend"),
                new SqlParameter("@EntId",entId)
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            var ds = sql.RunProDataSet("Proc_GoodsList", param);
            List<IndexFloor> list = new List<IndexFloor>();
            if (ds.Tables.Count > 0)
            {
                list = PrefectureFillFloor(ds.Tables[0], entId);
            }
            return list;
        }
        #endregion

        #region 获取网站配置
        public List<IndexWebConfig>  GetIndexConfig(string entId, string imgType)
        {
            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@type","PC_GetWebConfig"),
                new SqlParameter("@entId",entId),
                new SqlParameter("@imgType",imgType)
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            var ds = sql.RunProcedureDR("Pc_Log", param);
            List<IndexWebConfig> list = new List<IndexWebConfig>();
            string web_url = BaseConfiguration.SercerIp;
            if (ds.Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Rows)
                {
                    IndexWebConfig lc = new IndexWebConfig
                    {
                        ImgType = dr["ImgType"].ToString().Trim(),
                        TypeName = dr["TypeName"].ToString().Trim(),
                        WebValue = dr["WebValue"].ToString().Trim(),
                        ImgOrText = int.Parse(dr["ImgOrText"].ToString())
                    };
                    if (dr["imgurl"].ToString().Trim() != "")
                    {
                        lc.imgurl = web_url + dr["imgurl"].ToString().Trim();
                    }
                    else
                    {
                        lc.imgurl = "";
                    }
                    list.Add(lc);
                }
            }
            return list;
        }
        public class IndexWebConfig
        {
            /// <summary>
            /// 唯一标识
            /// </summary>
            public string ImgType { get; set; }

            /// <summary>
            /// 配置名称
            /// </summary>
            public string TypeName { get; set; }

            /// <summary>
            /// 图片
            /// </summary>
            public string imgurl { get; set; }

            /// <summary>
            /// 值
            /// </summary>
            public string WebValue { get; set; }

            /// <summary>
            /// 值还是图
            /// </summary>
            public int ImgOrText { get; set; }
        }
        #endregion

        #region 获取首页广告图
        public IndexAdvert GetIndexAdvert(string entId)
        {
            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@type","GetIndexAdvert"),
                new SqlParameter("@entId",entId)
            };
            IndexAdvert lc=new IndexAdvert();
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            var ds = sql.RunProcedureDR("Pc_Log", param);
            string web_url = BaseConfiguration.SercerIp;
            if (ds.Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Rows)
                {
                    lc = new IndexAdvert
                    {
                        linkurl = dr["linkurl"].ToString().Trim(),
                        height = dr["height"].ToString().Trim(),
                        width = dr["width"].ToString().Trim()

                    };
                    if (dr["imgurl"].ToString().Trim() != "")
                    {
                        lc.imgurl = web_url + dr["imgurl"].ToString().Trim();
                    }
                    else
                    {
                        lc.imgurl = "";
                    }

                }
                return lc;
            }
            else {
                return null;
            }
        }
        public class IndexAdvert
        {
            /// <summary>
            /// 图片
            /// </summary>
            public string imgurl { get; set; }

            /// <summary>
            /// 链接地址
            /// </summary>
            public string linkurl { get; set; }
            /// <summary>
            /// 长
            /// </summary>
            public string height { get; set; }
            /// <summary>
            /// 宽
            /// </summary>
            public string width { get; set; }
        }
        #endregion

        /// <summary>
        /// 楼层数据整理
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <param name="entId"></param>
        /// <param name="landing"></param>
        /// <param name="staleDated"></param>
        /// <param name="jgjb"></param>
        /// <returns></returns>
        private List<IndexFloor> FillFloor(DataTable t1,DataTable t2,string entId,bool landing, bool staleDated,string jgjb,string userId)
        {
            ///获取网站ip
            string web_url = BaseConfiguration.SercerIp;
            List<IndexFloor> list = new List<IndexFloor>();
            foreach (DataRow dr in t1.Rows)
            {
                IndexFloor lc = new IndexFloor
                {
                    Entid = dr["entid"].ToString().Trim(),
                    FloorId = dr["FloorId"].ToString().Trim(),
                    FloorTitle = dr["FloorTitle"].ToString().Trim(),
                    Link_Url = dr["Floor_Link"].ToString().Trim(),
                    TypeName = dr["TypeName"].ToString().Trim(),
                    Type=dr["imgTypeS"].ToString().Trim(),
                    AndroidLinkType = int.Parse(dr["androidlinktype"].ToString()),
                    AndroidLinkUrl = dr["androidlinkurl"].ToString().Trim()
                    
                };
                if (dr["Floor_Img"].ToString().Trim() != "")
                {
                    lc.Floor_Img = web_url + dr["Floor_Img"].ToString().Trim();
                }
                else
                {
                    lc.Floor_Img = "";
                }
                List<ImgInfo> imgList = new List<ImgInfo>();
                if (t2.Rows.Count>0) {
                    DataRow[] imgDr = t2.Select("EntId='" + entId + "' and FloorId='" + lc.FloorId + "'", "sort_Id asc");
                    //楼层详细数据
                    foreach (DataRow drI in imgDr)
                    {
                        ImgInfo img = new ImgInfo
                        {
                            Entid = drI["entid"].ToString().Trim(),
                            Title = drI["FloorTitle"].ToString().Trim(),
                            ArticleId = drI["Article_Id"].ToString().Trim(),
                            SortId = int.Parse(drI["sort_Id"].ToString().Trim()),
                            IsBrand = drI["Is_Brand"].ToString().Trim(),
                            BrandCode = drI["BrandCode"].ToString().Trim(),
                            LinkUrl = drI["link_url"].ToString().Trim(),
                            AndroidLinkType = int.Parse(dr["androidlinktype"].ToString()),
                            AndroidLinkUrl = drI["androidlinkurl"].ToString().Trim()
                        };
                        if (drI["img_url"].ToString().Trim() != "")
                        {
                            img.ImgUrl = web_url + drI["img_url"].ToString().Trim();
                        }
                        else
                        {
                            img.ImgUrl = "";
                        }
                        if (drI["left_pic"].ToString().Trim() != "")
                        {
                            img.LeftPic = web_url + drI["left_pic"].ToString().Trim();
                        }
                        if (drI["brand_img_url"].ToString().Trim() != "")
                        {
                            img.BrandImgUrl = web_url + drI["brand_img_url"].ToString().Trim();
                        }
                        else
                        {
                            img.BrandImgUrl = "";
                        }
                        img.SubTitle = drI["sub_title"].ToString().Trim();
                        img.DrugSpec = drI["drug_spec"].ToString().Trim();
                        img.DrugFactory = drI["drug_factory"].ToString().Trim();
                        if (landing && !staleDated)
                        {
                            img.Price = BasisConfig.ObjToDecimal(drI["price"].ToString(), BaseConfiguration.PricePlace, 0.00M).ToString();
                        }
                        else
                        {
                            img.Price = "会员可见";
                        }
                        img.Cxbs = drI["cxbs"].ToString().Trim();
                        img.Fabh = drI["fabh"].ToString().Trim();
                        imgList.Add(img);
                    }
                    //LogQueue.Write(LogType.Debug, "FillFloor", $"Type:{lc.Type},len:{imgDr.Length}");
                    //品牌推荐  最少12 不足补位
                    if (lc.Type== "Brand" && imgDr.Length<12)
                    {
                        imgList = BrandParatope(imgList,entId,web_url,landing,staleDated,12-imgDr.Length, userId);
                    }
                    else if (lc.Type== "Weekly" && imgDr.Length < 4)
                    {
                        imgList = GoodsParatope(imgList, entId, web_url, landing, staleDated, 4 - imgDr.Length,jgjb,lc.FloorTitle, userId);
                    }
                    else if ((lc.Type== "Family" || lc.Type== "Product" ||lc.Type== "InSeason" || lc.Type== "Health") && imgDr.Length < 7)
                    {
                        imgList = GoodsParatope(imgList, entId, web_url, landing, staleDated, 7 - imgDr.Length, jgjb, lc.FloorTitle, userId);
                    }
                }
                lc.FloorDetail = imgList;
                list.Add(lc);
            }

            return list;
        }

        /// <summary>
        /// 专区数据整理
        /// </summary>
        /// <param name="table">专区数据</param>
        /// <returns></returns>
        private List<IndexFloor> PrefectureFillFloor(DataTable table,string entId)
        {
            ///获取网站ip
            string web_url = BaseConfiguration.SercerIp;
            List<IndexFloor> list = new List<IndexFloor>();
            foreach (DataRow dr in table.Rows)
            {
                IndexFloor lc = new IndexFloor
                {
                    Entid = dr["entid"].ToString().Trim(),
                    FloorId = dr["FloorId"].ToString().Trim(),
                    FloorTitle = dr["FloorTitle"].ToString().Trim(),
                    Link_Url = dr["Floor_Link"].ToString().Trim(),
                    TypeName = dr["TypeName"].ToString().Trim(),
                    Sort = dr["sort"].ToString().Trim(),
                    Type = dr["imgTypeS"].ToString().Trim(),
                    AndroidLinkType = int.Parse(dr["androidlinktype"].ToString()),
                    AndroidLinkUrl = dr["androidlinkurl"].ToString().Trim()
                };
                if (dr["Floor_Img"].ToString().Trim() != "")
                {
                    lc.Floor_Img = web_url + dr["Floor_Img"].ToString().Trim();
                }
                else
                {
                    lc.Floor_Img = "";
                }
                lc.FloorDetail = new List<ImgInfo>();
                list.Add(lc);
            }
            if (table.Rows.Count<4)
            {
                var placehold= XmlOperation.ReadXml("Base", "PrefecturePicture");
                for (int i = 0; i < 4- table.Rows.Count; i++)
                {
                    IndexFloor lc = new IndexFloor
                    {
                        Entid = entId,
                        FloorId = "",
                        FloorTitle = "专区推荐",
                        Link_Url = "",
                        TypeName = "",
                        Type = "",
                        AndroidLinkType = 0,
                        AndroidLinkUrl = "",
                        Floor_Img= placehold
                    };
                    lc.FloorDetail = new List<ImgInfo>();
                    list.Add(lc);
                }
            }
            return list;
        }
        #endregion

        #region 品牌补位
        public List<ImgInfo> BrandParatope(List<ImgInfo> imgList,string entId,string webUrl,bool landing,bool staleDated,int len,string userId)
        {
            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@type","BrandParatope"),
                new SqlParameter("@EntId",entId),
                new SqlParameter("@userId",userId)
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            var dt = sql.RunProcedureDR("Proc_GoodsList",param);
            List<ImgInfo> source = new List<ImgInfo>();
            List<ImgInfo> back = new List<ImgInfo>();
            if (dt.Rows.Count>0)
            {
                //补位数据
                foreach (DataRow item in dt.Rows)
                {
                    ImgInfo img = new ImgInfo
                    {
                        Entid = item["entid"].ToString().Trim(),
                        Title = item["FloorTitle"].ToString().Trim(),
                        ArticleId = item["Article_Id"].ToString().Trim(),
                        SortId = int.Parse(item["sort_Id"].ToString().Trim()),
                        IsBrand = item["Is_Brand"].ToString().Trim(),
                        BrandCode = item["BrandCode"].ToString().Trim(),
                        LinkUrl = item["link_url"].ToString().Trim()
                    };
                    if (item["img_url"].ToString().Trim() != "")
                    {
                        img.ImgUrl = webUrl + item["img_url"].ToString().Trim();
                    }
                    else
                    {
                        img.ImgUrl = "";
                    }
                    if (item["left_pic"].ToString().Trim() != "")
                    {
                        img.LeftPic = webUrl + item["left_pic"].ToString().Trim();
                    }
                    if (item["brand_img_url"].ToString().Trim() != "")
                    {
                        img.BrandImgUrl = webUrl + item["brand_img_url"].ToString().Trim();
                    }
                    else
                    {
                        img.BrandImgUrl = "";
                    }
                    img.SubTitle = item["sub_title"].ToString().Trim();
                    img.DrugSpec = item["drug_spec"].ToString().Trim();
                    img.DrugFactory = item["drug_factory"].ToString().Trim();
                    if (landing && !staleDated)
                    {
                        img.Price = BasisConfig.ObjToDecimal(item["price"].ToString(), BaseConfiguration.PricePlace, 0.00M).ToString();
                    }
                    else
                    {
                        img.Price = "会员可见";
                    }
                    img.Cxbs = item["cxbs"].ToString().Trim();
                    img.Fabh = item["fabh"].ToString().Trim();
                    source.Add(img);
                }
                imgList.ForEach(t => back.Add(t));
                //已存在的品牌
                var exist = imgList.Select(t => t.ArticleId).Distinct().ToList();
                //移除已存在的品牌
                source.RemoveAll(t => exist.Contains(t.ArticleId));
                if (source.Count > 0)
                {
                    int length = source.Count > len ? len : source.Count;
                    Random random = new Random();
                    int index = source.Count > len ? random.Next(source.Count - len + 1) : 0;
                    source = source.GetRange(index, length);
                    source.ForEach(p => { back.Add(p); });
                }
                if (source.Count < len)
                {
                    var placehold = XmlOperation.ReadXml("Base", "RecommendPicture");
                    for (int i = 0; i < len- source.Count; i++)
                    {
                        ImgInfo imgInfo = new ImgInfo
                        {
                            Entid = entId,
                            Title = "品牌推荐",
                            ArticleId = "",
                            SortId = 99,
                            IsBrand = "Y",
                            BrandCode = "",
                            LinkUrl = "",
                            ImgUrl = placehold,
                            LeftPic="",
                            BrandImgUrl= placehold,
                            DrugFactory="",
                            DrugSpec="",
                            SubTitle="",
                            Price="",
                            Cxbs="",
                            Fabh=""
                        };
                        back.Add(imgInfo);
                    }
                }
            }
            return back;
        }
        #endregion

        #region 商品补位
        public List<ImgInfo> GoodsParatope(List<ImgInfo> imgList, string entId, string webUrl, bool landing, bool staleDated, int len,string jgjb,string title,string userId)
        {
            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@type","GoodsParatope"),
                new SqlParameter("@Jgjb",jgjb),
                new SqlParameter("@EntId",entId),
                new SqlParameter("@userId",userId)
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            var dt = sql.RunProcedureDR("Proc_GoodsList", param);
            List<ImgInfo> source = new List<ImgInfo>();
            List<ImgInfo> back = new List<ImgInfo>();
          
            //LogQueue.Write(LogType.Debug, "商品count", $"{dt.Rows.Count}");
            if (dt.Rows.Count > 0)
            {
                //补位数据
                foreach (DataRow item in dt.Rows)
                {
                    ImgInfo img = new ImgInfo
                    {
                        Entid = item["entid"].ToString().Trim(),
                        Title = title,
                        ArticleId = item["Article_Id"].ToString().Trim(),
                        SortId = int.Parse(item["sort_Id"].ToString().Trim()),
                        IsBrand = "N",
                        BrandCode = "",
                        LinkUrl = item["link_url"].ToString().Trim()
                    };
                    if (item["img_url"].ToString().Trim() != "")
                    {
                        img.ImgUrl = webUrl + item["img_url"].ToString().Trim();
                    }
                    else
                    {
                        img.ImgUrl = "";
                    }
                    if (item["left_pic"].ToString().Trim() != "")
                    {
                        img.LeftPic = webUrl + item["left_pic"].ToString().Trim();
                    }
                    if (item["brand_img_url"].ToString().Trim() != "")
                    {
                        img.BrandImgUrl = webUrl + item["brand_img_url"].ToString().Trim();
                    }
                    else
                    {
                        img.BrandImgUrl = "";
                    }
                    img.SubTitle = item["sub_title"].ToString().Trim();
                    img.DrugSpec = item["drug_spec"].ToString().Trim();
                    img.DrugFactory = item["drug_factory"].ToString().Trim();
                    if (landing && !staleDated)
                    {
                        img.Price = BasisConfig.ObjToDecimal(item["price"].ToString(), BaseConfiguration.PricePlace, 0.00M).ToString();
                    }
                    else
                    {
                        img.Price = "会员可见";
                    }
                    img.Cxbs = item["cxbs"].ToString().Trim();
                    img.Fabh = item["fabh"].ToString().Trim();
                    source.Add(img);
                }
                //加入现有商品
                imgList.ForEach(t => back.Add(t));
                //已存在的品牌
                var exist = imgList.Select(t => t.ArticleId).Distinct().ToList();
                //移除已存在的品牌
                source.RemoveAll(t => exist.Contains(t.ArticleId));

                if (source.Count > 0)
                {
                    int length = source.Count > len ? len : source.Count;
                    Random random = new Random();
                    int index = source.Count > len ? random.Next(source.Count - len + 1) : 0;
                    source = source.GetRange(index, length);
                    source.ForEach(p => { back.Add(p); });
                }
            }

            
            return back;
        }
        #endregion

        #region 专区补位 暂时弃用
        //public List<IndexFloor> PrefectureParatope(List<IndexFloor> list,string entId)
        //{
        //    SqlParameter[] param = new SqlParameter[]{
        //        new SqlParameter("@type","PrefectureParatope"),
        //        new SqlParameter("@EntId",entId)
        //    };
        //    SqlRun sql = new SqlRun(SqlRun.sqlstr);
        //    var dt = sql.RunProcedureDR("Proc_GoodsList", param);


        //}
        #endregion

        #region App模块
        /// <summary>
        /// App模块
        /// </summary>
        /// <param name="entid"></param>
        /// <returns></returns>
        public Information AppModule(string entid)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@type","CarouselOrModule"),
                new SqlParameter("@entid",entid)
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            var ds = sql.RunProDataSet("Proc_GoodsList", parameters);
            Information list = new Information();
            if (ds.Tables.Count>0)
            {
                list = FillList(ds);
            }
            return list;
        }

        /// <summary>
        /// App模块数据填充
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public Information FillList(DataSet set)
        {
            Information information = new Information();
            string web_url = BaseConfiguration.SercerIp;
            List<Carousel> carousels = new List<Carousel>();
            foreach (DataRow item in set.Tables[0].Rows)
            {
                Carousel carousel = new Carousel()
                {
                    CarouselName = item["title"].ToString(),
                    SerialNumber = item["xh"].ToString(),
                    ArticleID = item["ArticleID"].ToString(),
                    AndroidLinkType = int.Parse(item["androidlinktype"].ToString())
                };
                if (item["img_url"].ToString() != "")
                {
                    carousel.ImgPath = web_url + item["img_url"].ToString();
                }
                else
                {
                    carousel.ImgPath = "";
                }
                if (item["link_url"].ToString() != "")
                {
                    carousel.SkipLink = item["link_url"].ToString();
                }
                else
                {
                    carousel.SkipLink = "";
                }
                if (item["androidlinkurl"].ToString() != "")
                {
                    carousel.AndroidLinkUrl = item["androidlinkurl"].ToString();
                }
                else
                {
                    carousel.AndroidLinkUrl = "";
                }
                carousels.Add(carousel);
            }
            information.Carousel = carousels;
            List<AppModule> apps = new List<AppModule>();
            foreach (DataRow item in set.Tables[1].Rows)
            {
                AppModule app = new AppModule()
                {
                    ModuleName = item["title"].ToString(),
                    SerialNumber = item["xh"].ToString(),
                    SkipLink = item["link_url"].ToString()
                };
                if (item["img_url"].ToString() != "")
                {
                    app.ImgPath = web_url + item["img_url"].ToString();
                }
                else
                {
                    app.ImgPath = "";
                }
                apps.Add(app);
            }
            information.AppModul = apps;
            return information;
        }
        #endregion

        #region APP个人中心数据
        /// <summary>
        /// APP个人中心数据
        /// </summary>
        /// <param name="entid"></param>
        /// <returns></returns>
        public List<AppModule> PerModule(string entid,string userId, string model)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@type","PersonerModule"),
                new SqlParameter("@entid",entid),
                new SqlParameter("@userId",userId),
                new SqlParameter("@model",model)
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            var dt = sql.RunProcedureDR("Proc_UserInfo", parameters);
            List<AppModule> list = new List<AppModule>();
            if (dt.Rows.Count > 0)
            {
                list = FillPerModule(dt);
            }
            return list;
        }
        
        /// <summary>
        /// APP个人中心填充数据
        /// </summary>
        /// <param name="set"></param>
        /// <returns></returns>
        public List<AppModule> FillPerModule(DataTable table)
        {
            string web_url = BaseConfiguration.SercerIp;
            List<AppModule> apps = new List<AppModule>();
            foreach (DataRow item in table.Rows)
            {
                AppModule app = new AppModule()
                {
                    ModuleName = item["title"].ToString(),
                    SerialNumber = item["xh"].ToString(),
                    Point=int.Parse(item["point"].ToString()),
                    Amount=decimal.Parse(item["amount"].ToString()),
                    Type=item["ImgType"].ToString().Trim()
                };
                if (item["img_url"].ToString()!="")
                {
                    app.ImgPath = web_url + item["img_url"].ToString();
                }
                else
                {
                    app.ImgPath = "";
                }
                if (item["link_url"].ToString()!="")
                {
                    app.SkipLink = item["link_url"].ToString();
                }
                else
                {
                    app.SkipLink = "";
                }
                apps.Add(app);
            }
            
            return apps;
        }
        #endregion
    }
}