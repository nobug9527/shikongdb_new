using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.Admin.customform.entity
{
    [Serializable]
    public class Fields
    {
        /// <summary>
        /// 查询名
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 参数名
        /// </summary>
        public string field { get; set; }
        /// <summary>
        /// 参数类型(int、string、date),int是下拉
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 默认值
        /// </summary>
        public string defaultValue { get; set; }
        /// <summary>
        /// type为int时设置下拉选项
        /// </summary>
        public List<SelectFields> selectItem{ get; set; }
        /// <summary>
        /// 是否展示在搜索栏中，为true时，读取默认值
        /// </summary>
        public bool isShow { get; set; }
    }
    [Serializable]
    public class SelectFields
    {
        public string text { get; set; }
        public string value { get; set; }
    }
}