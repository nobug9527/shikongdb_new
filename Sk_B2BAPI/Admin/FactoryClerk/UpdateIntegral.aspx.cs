using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DTcms.Web.admin.FactoryClerk
{
    public partial class UpdateIntegral : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string dwmch = Request.QueryString["dwMch"];
            string point = Request.QueryString["point"];
            txtKhmch.Value = dwmch;
            txtXyjf.Value = point;
        }
    }
}