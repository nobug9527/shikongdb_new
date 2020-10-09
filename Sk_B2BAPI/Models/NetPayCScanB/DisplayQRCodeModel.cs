using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.Models.NetPayCScanB
{
    public class DisplayQRCodeModel
    {
        [Display(Name = "平台错误码")]
        public string errCode { get; set; }

        [Display(Name = "平台错误信息")]
        public string errMsg { get; set; }

        [Display(Name = "商户号")]
        public string mid { get; set; }

        [Display(Name = "终端号")]
        public string tid { get; set; }

        [Display(Name = "业务类型")]
        public string instMid { get; set; }
        /// <summary>
        /// APP请求返回内容
        /// </summary>
        public object appPayRequest { get; set; }
    }
}