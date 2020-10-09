using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SqlSugar;

namespace Sk_B2BAPI.Models
{
    public partial class SC_T_LuckyHis
    {
        public SC_T_LuckyHis()
        {

        }
        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>           
        [SugarColumn(IsPrimaryKey =true,IsIdentity =true)]
        public int ID { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string UserID { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public string EntID { get; set; } = "";

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public DateTime? LuckyTime { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string PrizeBH { get; set; } = "";

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string PrizeName { get; set; } = "";

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string ImgUrl { get; set; } = "";

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string PrizeType { get; set; } = "";

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string PrizeCount { get; set; } = "";

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string UserName { get; set; } = "";

    }
}