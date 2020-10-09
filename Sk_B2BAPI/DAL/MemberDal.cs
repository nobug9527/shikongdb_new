using Sk_B2BAPI.Models.Admin;
using Sk_B2BAPI.Tool;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.DAL
{
    public class MemberDal
    {
        public bool SaveMerchantsDal(string userId, string entIdList, int status)
        {
            try
            {

                SqlSugarHelper helper = new SqlSugarHelper();


                var entIdDoc = entIdList.Split(',');
                string entId = "";
                ///解除绑定关系
                if (status ==0)
                {

                    int count = helper.Db.Ado.ExecuteCommand("update Dt_UserEntDoc set status=0 where entId=@entId  and userId=@userId ",
                        new SugarParameter("@userId", userId),
                        new SugarParameter("@entId", entId));
                    if (count > 0)
                        return true;
                    else
                        return false;
                }
                else
                {
                    ///验证数据是否存在
                    var list = helper.Db.Queryable<Dt_UserEntDoc>().Where(u => u.userId == userId && u.entId == entId).ToList();
                    if (list != null && list.Count > 0)
                    {
                        return true;
                    }
                    ///插入数据
                    Dt_UserEntDoc model = new Dt_UserEntDoc();
                    model.userId = userId;
                    model.entId = entId;
                    model.status = status;
                    model.addTime = System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                    int count = helper.Db.Insertable(model)
                        .IgnoreColumns(u =>new {u.id,u.PriceLevel })
                        .ExecuteReturnIdentity();

                    if (count > 0)
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "MemberDal/SaveMerchants", ex.Message.ToString());
                return false;
            }
        }
    }
}