
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sk_B2BAPI.Models.Admin
{
    /// <summary>
    /// 角色权限
    /// </summary>
   
    public class Dt_RoleMethod
    {
        /// <summary>
        /// 角色Id
        /// </summary>
        [Required]
        [ForeignKey("RoleTarget")]
        public int RoleId { get; set; }

        /// <summary>
        /// 角色对象
        /// </summary>
        public virtual Dt_Role RoleTarget { get; set; }

        /// <summary>
        /// 权限Id
        /// </summary>
        [Required]
        [ForeignKey("MethodTarget")]
        public int MethodId { get; set; }

        public DateTime AddTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
