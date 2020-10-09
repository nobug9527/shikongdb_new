using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.Models
{
    /// <summary>
    /// 用户注册上传资质
    /// </summary>
    public class UserRegister
    {
        /// <summary>
        /// 注册Id
        /// </summary>
        public string UserId { get; set; } = "";
        /// <summary>
        /// 机构Id
        /// </summary>
        public string EntId { get; set; } = "";
        /// <summary>
        /// 密码
        /// </summary>
        public string PassWord { get; set; } = "";
        /// <summary>
        /// 性别
        /// </summary>
        public string Sex { get; set; } = "";
        /// <summary>
        /// 出生日期
        /// </summary>
        public string Birthday { get; set; } = "";
        /// <summary>
        /// 电话号码
        /// </summary>
        public string Telphone { get; set; } = "";
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; } = "";
        /// <summary>
        /// 省
        /// </summary>
        public string Province { get; set; } = "";
        /// <summary>
        /// 市
        /// </summary>
        public string City { get; set; } = "";
        /// <summary>
        /// 县/辖区
        /// </summary>
        public string Prefecture { get; set; } = "";
        /// <summary>
        /// 详细地址
        /// </summary>
        public string Address { get; set; } = "";
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime AddTime { get; set; }
        /// <summary>
        /// 注册公司名称
        /// </summary>
        public string BusinessName { get; set; } = "";
        /// <summary>
        /// 客户类型
        /// </summary>
        public string ClinetType { get; set; } = "";
        /// <summary>
        /// 注册人姓名
        /// </summary>
        public string Name { get; set; } = "";
        /// <summary>
        /// 注册资料
        /// </summary>
        public List<UserMaterial> Materials { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; } = "";
    }
}