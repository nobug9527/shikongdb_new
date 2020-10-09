using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sk_B2BAPI.Models.Admin
{
    /// <summary>
    /// 管理角色表
    /// </summary>

    public class Dt_Role
    {
        public int Id { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        [Required]
        [MaxLength(30)]
        public string RoleName { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [MaxLength(100)]
        public string Describe { get; set; }

        /// <summary>
        /// 排序(越大越靠前)
        /// </summary>
        [Required]
        [DefaultValue(0)]
        public int Sort { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        public string Source { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 功能Id
        /// </summary>
        public int MethodId { get; set; }


    }
}
