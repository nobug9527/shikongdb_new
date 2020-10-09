using Sk_B2BAPI.DAL;
using Sk_B2BAPI.Models;
using Sk_B2BAPI.Tool;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using DTcms.Common;

namespace Sk_B2BAPI.Controllers
{
    /// <summary>
    /// 地址管理
    /// </summary>
    public class AddressController : Controller
    {
        #region 获取地址
        /// <summary>
        /// 结算获取地址
        /// </summary>
        /// <param name="entId">企业id</param>
        /// <param name="userId">用户id</param>
        /// <returns></returns>
        public JsonResult GetUserAddress(string entId,string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { success = false, message = "用户未登录，请先登录" });
                }
                AddressDal dal = new AddressDal();
                List<Address> list = new List<Address>();
                list = dal.GetUserAddress(entId, userId);
                return Json(new { success = true, list = list });
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Address/GetUserAddress", ex.Message.ToString());
                return Json(new { success = false, message = ex.Message.ToString() });
            }
        }
        #endregion

        #region 编辑地址
        /// <summary>
        /// 修改或添加用户地址
        /// </summary>
        /// <param name="entId">企业Id</param>
        /// <param name="userId">用户Id</param>
        /// <param name="id">地址Id（修改id/新建0）</param>
        /// <param name="acceptName">收货人</param>
        /// <param name="province">省份</param>
        /// <param name="city">城市</param>
        /// <param name="prefecture">县/辖区</param>
        /// <param name="address">详细地址</param>
        /// <param name="isDefault">默认地址</param>
        /// <param name="telphone">收货人电话</param>
        /// <returns></returns>
        public JsonResult UpdateUserAddress(string entId, string userId, int id, string acceptName,string province, string city,string prefecture, string address, string telphone,int isDefault)
        {
            try 
            {
                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { success = false, message = "用户未登录，请先登录" });
                }
                AddressDal dal=new AddressDal ();
                bool flag = dal.UpdateUserAddress(entId,userId,id,acceptName, province, city, prefecture, address,telphone, isDefault);
                if (flag)
                {
                    return Json(new { success = true, message = "操作成功！" });
                }
                else
                {
                    return Json(new { success = false, message = "操作失败！" });
                }
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Address/UpdateUserAddress", ex.Message.ToString());
                return Json(new { success = false, message = "编辑地址失败" });
            }
        }
        #endregion

        #region 删除地址
        /// <summary>
        /// 删除地址
        /// </summary>
        /// <param name="userId">用户</param>
        /// <param name="entId">机构</param>
        /// <param name="id">地址序号</param>
        /// <returns></returns>
        public JsonResult DeleteAddress(string userId,string entId,int id)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { success = false, message = "用户未登录，请先登录" });
                }
                AddressDal dal = new AddressDal();
                bool flag = dal.DeleteAddress(userId, entId, id, out string msg);
                return Json(new { success=flag,message=msg});
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Address/UpdateUserAddress", ex.Message.ToString());
                return Json(new { success = false, message = "地址删除失败" });
            }
        }
        #endregion
    }
}
