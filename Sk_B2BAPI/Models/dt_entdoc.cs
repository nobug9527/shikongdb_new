using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SqlSugar;

namespace Sk_B2BAPI.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class dt_entdoc
    {
        [SugarColumn(IsPrimaryKey = true)]
        public int id { get; set; } 
        public string entid { get; set; }
        public string entname { get; set; }
        public string entcode { get; set; }
        public int? status { get; set; }
        public string address { get; set; }
        public string province { get; set; }
        public string city { get; set; }
        public string county { get; set; }
        public string telphone{ get; set; }
    }
}