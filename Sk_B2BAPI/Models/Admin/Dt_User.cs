using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.Models.Admin
{
    public class Dt_User
    {
        /// <summary>
        /// 管理员主键
        /// </summary>
        public string userId { get; set; }

        /// <summary>
        /// 管理员机构标识
        /// </summary>
        public string entId { get; set; }

        /// <summary>
        /// 权限角色
        /// </summary>
        public int role_id { get; set; }

        /// <summary>
        /// 权限类型
        /// </summary>
        public int role_type { get; set; }

        /// <summary>
        /// 管理员账号
        /// </summary>
        public string username { get; set; }

        /// <summary>
        /// 管理员密码
        /// </summary>
        public string password { get; set; }

        /// <summary>
        /// 联系方式
        /// </summary>
        public string telphone { get; set; }

        /// <summary>
        /// 管理员状态 1 冻结 2 正常
        /// </summary>
        public int status { get; set; }
        /// <summary>
        /// 父级
        /// </summary>
        public string fatheruserId { get; set; }

    }
}