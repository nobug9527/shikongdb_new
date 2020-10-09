using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.Models
{
    //[SugarTable("dt_custom_form")]
    public class CustomForm
    {
        /// <summary>
        /// 主键 自增
        /// </summary>
        //[SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int ID { get; set; }
        /// <summary>
        /// 自定义表单名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 自定义查询字段（Json）,使用Fields类序列化产生
        /// </summary>
        public string Fields { get; set; }
        /// <summary>
        /// 查询视图SQL语句
        /// </summary>
        public string SQL { get; set; }
        /// <summary>
        /// 业务通APP模块ID
        /// </summary>
        public string ModuleID { get; set; }
    }
}