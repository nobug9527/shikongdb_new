using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.Models
{
    public class PromModel
    {
        public string Name { get; set; } = "";
        public List<PromList> PromList { get; set; }
    }
}