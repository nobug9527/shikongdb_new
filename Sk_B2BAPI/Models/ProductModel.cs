using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.Models
{
    public class ProductModel
    {
        public string EntId { get; set; } = "";
        public string Article_Id { get; set; } = "";
        public string sort_Id { get; set; } = "";
        public string FloorId { get; set; } = "";
        public string Is_Brand { get; set; } = "";
        public string BrandCode { get; set; } = "";
        public string FloorTitle { get; set; } = "";
        public string Floor_Link { get; set; } = "";
        public string link_url { get; set; } = "";
        public string left_pic { get; set; } = "";
        public string brand_img_url { get; set; } = "";
        public string sub_title { get; set; } = "";
        public string drug_spec { get; set; } = "";
        public string drug_factory { get; set; } = "";

        public string price { get; set; } = "";
        public string Floor_Img { get; set; } = "";
        public string category_id { get; set; } = "";
        public string billno { get; set; } = "";

        public string Fabh { get; set; } = "";
        public string Cxbs { get; set; } = "";
    }
}