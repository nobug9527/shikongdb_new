using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.Models
{
    public class TokenModel
    {
        public string AccessT_token { get; set; } = "";
        public string ExpiresIn { get; set; } = "";
        public string RefreshToken { get; set; } = "";
        public string Openid { get; set; } = "";
        public string Scope { get; set; } = "";
    }
}