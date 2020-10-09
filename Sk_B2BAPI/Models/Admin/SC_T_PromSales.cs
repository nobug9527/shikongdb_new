using System;
using System.Linq;
using System.Text;

namespace Sk_B2BAPI.Models.Admin
{
    ///<summary>
    ///促销活动设置
    ///</summary>
    public partial class SC_T_PromSales
    {
            public SC_T_PromSales(){


            }
           /// <summary>
           /// Desc:企业标识
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string entid {get;set;}

           /// <summary>
           /// Desc:活动编号
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string salesCode {get;set;}

           /// <summary>
           /// Desc:活动标题
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string salesName {get;set;}

           /// <summary>
           /// Desc:活动开始时间，截止到秒
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string startDate {get;set;}

           /// <summary>
           /// Desc:活动结束时间，截止到秒
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string endDate {get;set;}

           /// <summary>
           /// Desc:引用的促销规则编号
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string ruleCode {get;set;}

           /// <summary>
           /// Desc:促销活动的类型
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string faType {get;set;}

           /// <summary>
           /// Desc:发布后实际运行表中的方案编号
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string fabh {get;set; }
        /// <summary>
        /// Desc:控制规则是否可以被引用，已上架2 未上架1或空 已删除0
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string isShow { get; set; } = "1";

    }
}
