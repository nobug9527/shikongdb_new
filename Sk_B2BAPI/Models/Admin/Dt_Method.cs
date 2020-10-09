using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sk_B2BAPI.Models.Admin
{
    /// <summary>
    /// 管理员权限功能
    /// </summary>
    public class Dt_Method
    {
        public int Id { get; set; }
        /// <summary>
        /// 权限名称
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 导航名称
        /// </summary>
        [MaxLength(50)]
        public string NavTitle { get; set; }

        /// <summary>
        /// 控制器
        /// </summary>
        [MaxLength(200)]
        public string Power { get; set; }

        /// <summary>
        /// 父级Id
        /// </summary>
        public int? FatherId { get; set; }

        /// <summary>
        /// 排序(越大越靠前)
        /// </summary>
        [Required]
        [DefaultValue(0)]
        public int Sort { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        public string IcoAddress { get; set; }
        public DateTime AddTime { get; set; }
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public  string Source { get; set; }
        /// <summary>
        /// 转义状态
        /// 'YWT' 对应 'APP'
        /// 'PC' 对应 'PC'
        /// </summary>
        public string RoleTyle { get; set; }
        ///状态
        public int Status { get; set; }

    }
}
