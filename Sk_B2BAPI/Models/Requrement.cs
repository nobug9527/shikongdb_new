using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.Models
{
    /// <summary>
    /// 求购
    /// </summary>
    public class Requrement
    {
		/// <summary>
		/// 求购人
		/// </summary>
		public string BuyName { get; set; } = "";
		/// <summary>
		/// 求购人联系方式
		/// </summary>
		public string BuyTel { get; set; } = "";
		/// <summary>
		/// 求购药品名称
		/// </summary>
		public string BuyGoods { get; set; } = "";
		/// <summary>
		/// 求购药品厂家
		/// </summary>
		public string ProductName { get; set; } = "";
		/// <summary>
		/// 求购药品规格
		/// </summary>
		public string BuySpec { get; set; } = "";
		/// <summary>
		/// 求购药品数量
		/// </summary>
		public string BuyNumber  { get; set; } = "";
		/// <summary>
		/// 求购药品价格
		/// </summary>
		public string BuyPrice { get; set; } = "";
		/// <summary>
		/// 求购备注
		/// </summary>
		public string Message { get; set; } = "";
		/// <summary>
		/// 记录日期
		/// </summary>
		public string RecordDate { get; set; } = "";
		/// <summary>
		/// 回复
		/// </summary>
		public string Reply { get; set; } = "";
		/// <summary>
		/// 用户
		/// </summary>
		public string UserId { get; set; } = "";
		/// <summary>
		/// 机构
		/// </summary>
		public string EntId { get; set; } = "";
	}
}