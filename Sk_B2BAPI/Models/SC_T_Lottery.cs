using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SqlSugar;

namespace Sk_B2BAPI.Models
{
    ///<summary>
    ///
    ///</summary>
    public partial class SC_T_Lottery
    {
        public SC_T_Lottery()
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
        public string PrizeBH { get; set; } = "";

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
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// Desc:
        /// Default:1
        /// Nullable:True
        /// </summary>           
        public string isActive { get; set; } = "";

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string LotteryBh { get; set; } = "";

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public DateTime? ExpirationTime { get; set; }

    }
}