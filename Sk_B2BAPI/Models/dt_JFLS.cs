using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.Models
{
    ///<summary>
    ///
    ///</summary>
    public partial class dt_JFLS
    {
        public dt_JFLS()
        {


        }
        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>           
        public int id { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string rq { get; set; } = "";

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string ontime { get; set; } = "";

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string userid { get; set; } = "";

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string lcjf { get; set; } = "";

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string jyjf { get; set; } = "";

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string beizhu { get; set; } = "";

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string djbh { get; set; } = "";

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string erpdjbh { get; set; } = "";

        /// <summary>
        /// Desc:
        /// Default:1
        /// Nullable:True
        /// </summary>           
        public decimal? Multiple { get; set; }

    }
}