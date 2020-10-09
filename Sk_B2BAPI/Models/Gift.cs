using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.Models
{
    public class Gift
    {
        public string GoodsId { get; set; } = "";
        public string GoodsCode { get; set; } = "";
        public string GoodsName { get; set; } = "";
        public string DrugSpec{ get; set; } = "";
        public string PackageUnit{ get; set; } = "";
        public int Quantity{ get; set; }
        public int Status{ get; set; }
        public string FlootType{ get; set; } = "";
        public string GoodsType{ get; set; } = "";
        public decimal Price{ get; set; }
        public int Integral{ get; set; }
        public string DrugFactory{ get; set; } = "";
        public string ImgUrl{ get; set; } = "";
        public int Inventory{ get; set; }
        public string Entid { get; set; } = "";
    }
}