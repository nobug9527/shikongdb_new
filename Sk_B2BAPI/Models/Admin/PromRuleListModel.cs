using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.Models.Admin
{
    public class PromRuleListModel
    {
        public string type { set; get; }
        public string status { set; get; }
        public string faType { set; get; }
        public string strWhere { set; get; }
        public string ruleCode { set; get; }
        public string entid { set; get; }
        public int PageIndex { set; get; }
        public int PageSize { set; get; }
    }
}