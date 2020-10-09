using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SqlSugar;

namespace Sk_B2BAPI.Models
{
    ///<summary>
    ///会员主表
    ///</summary>
    [SugarTable("dt_users")]
    public class dt_users
    {
        [SugarColumn(IsPrimaryKey =true)]
        public string userid { get; set; } = "";
        [SugarColumn(IsPrimaryKey = true)]
        public string entid { get; set; } = "";
        public string username { get; set; } = "";
        public string salt { get; set; } = "";
        public string password { get; set; } = "";
        public string sex { get; set; } = "";
        public string birthday { get; set; } = "";
        public string telphone { get; set; } = "";
        public string email { get; set; } = "";
        public string province { get; set; } = "";
        public string city { get; set; } = "";
        public string prefecture { get; set; } = "";
        public string address { get; set; } = "";
        public int? point { get; set; }
        public decimal? balance { get; set; }
        public int? status { get; set; }
        public string logo_img { get; set; } = "";
        public string add_time { get; set; } = "";
        public string businessid { get; set; } = "";
        public string Longitude { get; set; } = "";
        public string Latitude { get; set; } = "";
        public string businessname { get; set; } = "";
        public string img_url { get; set; } = "";
        public string name { get; set; } = "";
        public int? role_id { get; set; }
        public int? role_type { get; set; }
        public int? GroupId { get; set; }
    }
}