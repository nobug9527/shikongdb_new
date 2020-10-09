using Sk_B2BAPI.App_Code;
using Sk_B2BAPI.DAL;
using Sk_B2BAPI.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Sk_B2BAPI.Tool;

namespace Sk_B2BAPI.Controllers
{
    public class BrandController : Controller
    {
        #region 品牌
        /// <summary>
        /// 获取品牌汇总信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="faType">类型MT/DT</param>
        /// <param name="billno">单号</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetBrandList(string userId,string faType,string billno,string entId,int pageIndex=1,int pageSize=3)
        {
            try
            {
                if (string.IsNullOrEmpty(entId))
                {
                    entId = BaseConfiguration.EntId;
                }
                ///获取用户信息
                UserInfoDal dal = new UserInfoDal();
                List<UserInfo> user = dal.GetUserInfo(userId,entId);
                bool landing = false;
                bool staleDated = false;
                string Pricelevel = "";
                string KhType = "";
                if (user.Count > 0)
                {
                    entId = user[0].EntId;
                    Pricelevel = user[0].Pricelevel;
                    KhType = user[0].KhType;
                    landing = true;
                    staleDated = user[0].StaleDated;
                }
                
                BrandDal bdal = new BrandDal();
                List<BrandList> list = bdal.GetBrandList(entId, userId, Pricelevel, KhType, landing, staleDated, faType, billno, pageIndex, pageSize, out int recordCount, out int pageCount);
                return Json(new { success = true, list = list, recordCount= recordCount, pageCount= pageCount });
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Brand/GetBrandList", ex.Message);
                return Json(new { success = false, message = ex.Message });
            }
        }
        #endregion

        #region 药房、门诊
        /// <summary>
        /// 药房、门诊
        /// </summary>
        /// <param name="userId">用户</param>
        /// <param name="ArrondiType">诊所专区0/药店专区1</param>
        /// <param name="num">显示条目数</param>
        /// <param name="entId">机构</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetPharmacy(string userId,string ArrondiType, string imgType, /*int num,*/string entId,int pageIndex=1, int pageSize=8)
        {
            try
            {
                if (string.IsNullOrEmpty(ArrondiType))
                {
                    return Json(new { success = false, message = "必传参数为空" });
                }
                if (string.IsNullOrEmpty(entId))
                {
                    entId = BaseConfiguration.EntId;
                }
                ///获取用户信息
                UserInfoDal dal = new UserInfoDal();
                List<UserInfo> user = dal.GetUserInfo(userId, entId);
                string Pricelevel = "";
                string KhType = "";
                bool landing = false;
                bool staleDated = false;
                if (user.Count > 0)
                {
                    entId = user[0].EntId;
                    Pricelevel = user[0].Pricelevel;
                    KhType = user[0].KhType;
                    staleDated = user[0].StaleDated;
                    landing = true;
                }
                string[] Array = ArrondiType.Split(new char[] { ',' });
                BrandDal brand = new BrandDal();
                List<PromModel> pList = new List<PromModel>();
                int record = pageSize;
                int page = pageIndex;
                for (int i = 0; i < Array.Length; i++)
                {
                    PromModel p = new PromModel();
                    if (Array[i] != "")
                    {
                        p.Name = Array[i];
                        p.PromList = brand.GetPharmacyList(entId, userId, Array[i], Pricelevel, KhType, /*num,*/pageIndex,pageSize, landing, staleDated, out int recordCount, out int pageCount);
                        pList.Add(p);
                        if (Array.Length > 1)
                        {
                            record = pageSize;
                            page = pageIndex;
                        }
                        else
                        {
                            record = recordCount;
                            page = pageCount;
                        }
                    }
                }

                //获取图片信息
                ImgInfoDal idal = new ImgInfoDal();
                List<ImgInfo> ilist = idal.GetImgInfo(2, imgType, entId, "PC","");

                return Json(new { success = true, list = pList, imgList = ilist, recordCount= record, pageCount= page });
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Brand/GetPharmacy", ex.Message);
                return Json(new { success = false, message = "E005" });
            }
        }
        #endregion
    }
}
