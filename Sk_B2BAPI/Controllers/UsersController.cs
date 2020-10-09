using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sk_B2BAPI.App_Code;
using Sk_B2BAPI.DAL;
using Sk_B2BAPI.FileHelper;
using Sk_B2BAPI.Models;
using Sk_B2BAPI.Models.Admin;
using Sk_B2BAPI.Tool;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mail;
using System.Web.Mvc;

namespace Sk_B2BAPI.Controllers
{
    public class UsersController : Controller
    {
        #region 获取用户信息测试
        public ActionResult UserInfoText(string userId, string entId)
        {
            try
            {
                UserInfoDal Udal = new UserInfoDal();
                List<UserInfo> UserList = Udal.GetUserInfo(userId, entId);
                /*证书效期提示*/
                string message = UserList[0].StaleMsg;
                return Json(new { UserList, message });
            }
            catch (Exception ex)
            {
                return Json(new { ex.Message });
            }
        }
        #endregion
       //#region 获取验证码
       // /// <summary>
       // /// 获取验证码
       // /// </summary>
       // /// <param name="Digit">验证码位数</param>
       // /// <returns>返回字符串</returns>
       // public static string GetCheckCode(int Digit)
       // {
       //     StringBuilder sb = new StringBuilder();
       //     Random random = new Random();
       //     int Nums = Digit;
       //     while (Nums > 0)
       //     {
       //         int i = random.Next(1, 10);// 9>=a>=1
       //         if (sb.Length < Digit)
       //         {
       //             sb.Append(i);
       //         }
       //         Nums -= 1;
       //     }
       //     return sb.ToString();
       // }
       // #endregion
        #region 后台登录验证码
        /// <summary>
        /// 后台登录验证码
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AdminImgCode()
        {
            var helper = new VerificationCodeHelper();
            string imgCode = GetCheckCode(4);
            Session.Add("AdminLognCode", imgCode);
            return File(helper.GetVCode(imgCode), "Image/PNG");
        }
        #endregion

        #region 允许多线程访问的安全集合
        private static ConcurrentDictionary<string, string> sessionDic = new ConcurrentDictionary<string, string>();
        private class VerificationCode
        {
            public string SessionID { get; set; }
            public string Img { get; set; }
        }
        #endregion

        #region 前台登录验证码
        /// <summary>
        /// 前台登录验证码
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult LoginImgCode()
        {
            var helper = new VerificationCodeHelper();
            string imgCode = GetCheckCode(4); //GetCheckCode(4);
            var guid = Guid.NewGuid().ToString();
            sessionDic.TryAdd(guid, imgCode);
            VerificationCode verification = new VerificationCode()
            {
                SessionID = guid,
                //Img = Convert.ToBase64String(CreateImage(imgCode).ToArray())
                Img = Convert.ToBase64String(helper.GetVCode(imgCode))
            };
            return Json(verification);
        }
        #endregion

        #region PC用户登陆
        /// <summary>
        /// PC用户登陆
        /// </summary>
        /// <param name="UserName">用户名</param>
        /// <param name="PassWord"></param>
        /// <param name="loginCode"></param>
        /// <param name="cookid"></param>
        /// <param name="loginType">登录类型</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UserLogin(string UserName, string PassWord, string loginCode = "", string cookid = "", string loginType = "account")
        {
            if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(PassWord))
            {
                return Json(new { success = false, message = "用户名或密码不能为空！" });
            }
            else
            {
                try
                {
                    //Stopwatch stopwatch = new Stopwatch();
                    //stopwatch.Start();
                    if (!sessionDic.ContainsKey(cookid))
                    {
                        return Json(new { success = false, message = "验证码获取失败！" });
                    }
                    else
                    {
                        var code = sessionDic[cookid].ToString();
                        sessionDic.TryRemove(cookid, out string result);
                        if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(loginCode) || loginCode.ToLower() != code.ToLower())
                        {
                            return Json(new { success = false, message = "验证码输入错误！" });
                        }
                    }
                    //stopwatch.Stop();
                    //LogQueue.Write(LogType.Debug, "登录验证码判断", $"消耗时间:{stopwatch.ElapsedMilliseconds}");


                    UserInfoDal Udal = new UserInfoDal();
                    //获取登陆信息
                    DataTable dt = Udal.UserLoginDal(UserName, loginType, Encryption.GetMD5_16(PassWord));
                    if (dt.Rows.Count != 1 || dt == null)
                    {
                        return Json(new { success = false, message = "该账号不存在或用户名密码错误！" });
                    }
                    else if (dt.Rows[0]["password"].ToString().Trim() != Encryption.GetMD5_16(PassWord))
                    {
                        string b = Encryption.GetMD5_16(PassWord);
                        return Json(new { success = false, message = "密码错误！" });
                    }
                    else if (dt.Rows[0]["status"].ToString().Trim() != "2")
                    {
                        return Json(new { success = false, message = "该账号未通过审核，暂无法使用！" });
                    }
                    else
                    {
                        //stopwatch.Restart();
                        /////获取登陆信息
                        List<UserInfo> UserList = Udal.GetUserInfo(dt.Rows[0]["userid"].ToString(), dt.Rows[0]["entid"].ToString());
                        string role_type = dt.Rows[0]["role_type"].ToString().Trim();
                        string message = "";
                        if (role_type == "1")
                        {
                            /*证书效期提示*/
                            message = UserList[0].StaleMsg;
                        }
                        //stopwatch.Stop();
                        //LogQueue.Write(LogType.Debug, "获取用户登录信息", $"消耗时间:{stopwatch.ElapsedMilliseconds}");
                        //stopwatch.Restart();
                        //记录登录者Host、Ip、Port
                        ServerVariables(dt.Rows[0]["userid"].ToString(), dt.Rows[0]["entid"].ToString());
                        //stopwatch.Stop();
                        //LogQueue.Write(LogType.Debug, "记录登录用户ip", $"消耗时间:{stopwatch.ElapsedMilliseconds}");
                        return Json(new { success = true, list = UserList, message, role_type });
                    }
                }
                catch (Exception ex)
                {
                    LogQueue.Write(LogType.Error, "Users/UserLogin", ex.Message.ToString());
                    return Json(new { success = false, message = "登陆失败！" });
                }
            }
        }
        #endregion

        #region APP用户登录
        /// <summary>
        /// APP用户登录
        /// </summary>
        /// <param name="UserName">用户名</param>
        /// <param name="PassWord">密码</param>
        /// <param name="loginType">登录类型</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AppLogin(string UserName, string PassWord, string loginType = "account")
        {
            if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(PassWord))
            {
                return Json(new { success = false, message = "用户名或密码不能为空！" });
            }
            else
            {
                try
                {
                    UserInfoDal Udal = new UserInfoDal();
                    //获取登陆信息
                    DataTable dt = Udal.UserLoginDal(UserName, loginType, Encryption.GetMD5_16(PassWord));
                    if (dt.Rows.Count != 1 || dt == null)
                    {
                        return Json(new { success = false, message = "该账号不存在！" });
                    }
                    else if (dt.Rows[0]["password"].ToString().Trim() != Encryption.GetMD5_16(PassWord))
                    {
                        string b = Encryption.GetMD5_16(PassWord);
                        return Json(new { success = false, message = "密码错误！" });
                    }
                    else if (dt.Rows[0]["status"].ToString().Trim() != "2")
                    {
                        return Json(new { success = false, message = "该账号未通过审核，暂无法使用！" });
                    }
                    else
                    {
                        /////获取登陆信息
                        List<UserInfo> UserList = Udal.GetUserInfo(dt.Rows[0]["userid"].ToString(), dt.Rows[0]["entid"].ToString());

                        string role_type = dt.Rows[0]["role_type"].ToString().Trim();
                        string message = "";
                        if (role_type == "1")
                        {
                            /*证书效期提示*/
                            message = UserList[0].StaleMsg;
                        }

                        //记录登录者Host、Ip、Port
                        ServerVariables(dt.Rows[0]["userid"].ToString(), dt.Rows[0]["entid"].ToString());

                        return Json(new { success = true, list = UserList, message = message, role_type = role_type });
                    }
                }
                catch (Exception ex)
                {
                    LogQueue.Write(LogType.Error, "Users/AppLogin", ex.Message.ToString());
                    return Json(new { success = false, message = "登陆失败！" });
                }
            }
        }
        #endregion

        #region 修改用户信息
        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdateUserInfo(string entId, string userId, string telPhone, string email = "", string qq = "", string sex = "", string birthday = "", string imgPath = "")
        {
            try
            {
                string dataFile = imgPath;
                if (string.IsNullOrEmpty(dataFile))
                {
                    string filePath = string.Empty;
                    string fileNewName = string.Empty;
                    var file = Request.Files;
                    if (file.Count <= 0)
                    {
                        return Json(new { success = false, message = "请选择要上传的文件！" });//请选择要上传的文件  
                    }
                    var ufile = file[0];
                    string fileName = ufile.FileName;/*获取文件名： C:\Documents and Settings\Administrator\桌面\123.jpg*/
                    //Log.Error("错误",$"上传文件名：{fileName}");//上传文件名：201912261054561.jpg
                    string suffix = fileName.Substring(fileName.LastIndexOf(".") + 1).ToLower();/*获取后缀名并转为小写： jpg*/
                    int bytes = ufile.ContentLength;//获取文件的字节大小  
                    if (suffix != "jpg" && suffix != "png")
                    {
                        return Json(new { success = false, message = "上传文件格式错误！" });//只能上传JPG格式图片  
                    }
                    else if (bytes > 1024 * 1024 * 1)
                    {
                        return Json(new { success = false, message = "图片不能大于1M！" }); //图片不能大于1M 
                    }
                    string path = Server.MapPath("/UploadFile/Users/");
                    //Log.Error("打印", $"物理文件路径：{path}");
                    string year = DateTime.Now.Year.ToString();
                    string month = DateTime.Now.Month.ToString();
                    string date = DateTime.Now.ToFileTimeUtc().ToString();
                    string newfileName = date + "." + suffix;
                    string newfile = year + "_" + month + "/" + newfileName;
                    string newPath = path + year + "_" + month;
                    //Log.Error("打印", $"上传文件路径：{newPath}");//  /UploadFile/Users/System.Web.HttpPostedFileWrapper
                    //Log.Error("打印", $"文件是否存在：{System.IO.Directory.Exists(newPath)}");
                    if (false == System.IO.Directory.Exists(newPath))
                    {
                        //Log.Error("打印", $"创建文件");
                        //创建文件夹
                        System.IO.Directory.CreateDirectory(newPath);
                    }
                    ufile.SaveAs(newPath + "/" + newfileName);//保存图片  
                    //保存图片路径
                    dataFile = "/UploadFile/Users/" + newfile;
                    //Log.Error("打印", $"上传文件保存路径：{dataFile}");
                }
                else
                {
                    var index = dataFile.LastIndexOf('/');
                    dataFile = "/UploadFile/Users/" + dataFile.Substring(index + 1, dataFile.Length - index - 1);
                    //Log.Error("打印", $"头像图片路径：{dataFile}");
                }
                bool flag = UserInfoDal.UpdateUserInfo(entId, userId, telPhone, dataFile, email, qq, sex, birthday);
                if (flag)
                {
                    string webUrl = BaseConfiguration.SercerIp.ToString();
                    //if (webUrl.Contains("/b2b_api"))
                    //{
                    //    var index = webUrl.LastIndexOf('/');
                    //    webUrl = webUrl.Substring(0, index);
                    //}
                    return Json(new { success = true, message = "修改成功！", imgUrl = webUrl + dataFile });
                }
                else
                {
                    return Json(new { success = false, message = "修改失败！", imgUrl = "" });
                }
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Users/UpdateUserInfo", ex.Message.ToString());
                return Json(new { success = false, message = "修改失败！" });
            }
        }

        #endregion

        #region 修改密码
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="entId"></param>
        /// <param name="userId"></param>
        /// <param name="oldPwd">原密码</param>
        /// <param name="newPwd">新密码</param>
        /// <returns></returns>
        public JsonResult UpdateUserPwd(string entId, string userId, string oldPwd, string newPwd)
        {
            try
            {
                if (oldPwd == "" || newPwd == "")
                {
                    return Json(new { success = false, message = "必传参数不能为空" });
                }
                //获取用户信息
                UserInfoDal dal = new UserInfoDal();
                List<UserInfo> user = dal.GetUserInfo(userId, entId);
                if (user.Count <= 0)
                {
                    return Json(new { success = false, message = "E002" });
                }
                if (Encryption.GetMD5_16(oldPwd) != user[0].PassWord)
                {
                    return Json(new { success = false, message = "原密码错误" });
                }
                bool flag = UserInfoDal.UpdateUserPwd(entId, userId, Encryption.GetMD5_16(newPwd));
                if (flag)
                {
                    return Json(new { success = true, message = "修改成功" });
                }
                else
                {
                    return Json(new { success = true, message = "修改失败！" });
                }
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Users/UpdateUserPwd", ex.Message.ToString());
                return Json(new { success = false, message = "修改密码失败！" });
            }
        }
        #endregion

        #region 个人中心首页数据
        /// <summary>
        /// 获取个人中心首页数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetUserCenter(string entId, string userId, int num)
        {
            try
            {
                ///获取用户信息
                UserInfoDal uDal = new UserInfoDal();
                List<UserInfo> user = uDal.GetUserInfo(userId, entId);
                if (user.Count <= 0)
                {
                    return Json(new { success = false, message = "用户未登录，请先登录" });
                }
                //获取用户订单信息
                OrderInfoDal odal = new OrderInfoDal();
                List<OrdersMt> Olist = odal.OrderQueryMt(entId, userId, num);
                ///获取用户收藏列表
                StatisticalDal gdal = new StatisticalDal();
                List<GoodsStatistical> Glist = gdal.GoodsCollectionQuery(entId, userId, 1, num, out int recordCount, out int pageCount);
                SqlRun sql = new SqlRun(SqlRun.sqlstr);
                int couponnum=sql.RunSqlDataTable($"select a.UserCouponId from Zzsk_UserCoupon a join Zzsk_Coupons b on a.CouponId=b.couponCode where a.UserId='{userId}' and a.entid='{entId}' and a.Status=0 and '{DateTime.Now.ToString("yyyy-MM-dd")}'<a.EndTIme  ").Rows.Count;
                return Json(new { success = true, Ulist = user, Olist, Glist, serverTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),couponnum=couponnum });
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Users/GetUserCenter", ex.Message.ToString());
                return Json(new { success = false, message = "数据加载失败！" });
            }
        }
        #endregion

        #region 用户注册
        /// <summary>
        /// 用户注册,资质上传
        /// </summary>
        /// <param name="entId">机构</param>
        /// <param name="businessName">单位名称</param>
        /// <param name="clinettype">客户类型</param>
        /// <param name="telphone">手机</param>
        /// <param name="sex">性别</param>
        /// <param name="email">邮箱</param>
        /// <param name="password">密码</param>
        /// <param name="name">姓名</param>
        /// <param name="province">省份</param>
        /// <param name="city">城市</param>
        /// <param name="prefecture">县/辖区</param>
        /// <param name="address">地址</param>
        /// <param name="birthday">出生日期</param>
        /// <param name="material">资料信息</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Register(string entId, string businessName, string clinettype, string telphone, string sex, string email, string password, string name, string province, string city, string prefecture, string address, string birthday = "", string material = "")
        {
            try
            {
                if (string.IsNullOrEmpty(entId) || string.IsNullOrEmpty(businessName) || string.IsNullOrEmpty(province) || string.IsNullOrEmpty(city) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(name))
                {
                    return Json(new { success = false, message = "参数异常" });
                }

                UserInfoDal dal = new UserInfoDal();
                string result = dal.SaveUserInfo(entId, businessName, clinettype, telphone, sex, email, Encryption.GetMD5_16(password), name, province, city, prefecture, address, birthday, material, out bool flag);

                return Json(new { success = flag, message = result });

            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Users/Register", ex.Message.ToString());
                return Json(new { success = false, message = "用户注册失败！" });
            }
        }

        #endregion

        #region 业务员开放注册
        /// <summary>
        /// 业务员开放注册（ajax）author:魏飞
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="name"></param>
        /// <param name="telphone"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RegisterSalesman(string username, string password, string name, string telphone, string code)
        {
            UserInfoDal userInfoDal = new UserInfoDal();
            int flag = userInfoDal.AddSalesman(username, password, name, telphone);
            string message;
            bool success;
            if (Session["AdminLognCode"].ToString() != code)
            {
                success = false;
                message = "验证码错误!";
            }
            else if (flag == -1)
            {
                success = false;
                message = "用户名已存在!";
            }
            else if (flag > 0)
            {
                success = true;
                message = "注册成功!";
            }
            else
            {
                success = false;
                message = "系统原因，注册失败";
            }
            var result = new { success = success, message = message };
            return Json(result);
        }
        #endregion

        #region 获取业务员的分页列表
        /// <summary>
        /// 获取业务员的分页列表
        /// </summary>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="soustr"></param>
        /// <returns></returns>
        public JsonResult GetSalesmanList(int PageIndex, int PageSize, string SouStr)
        {
            UserInfoDal userInfoDal = new UserInfoDal();
            int total = 0;
            var data = userInfoDal.GetSalesmanList(PageIndex, PageSize, SouStr, ref total);
            var datalist = new { data = data, total = total };
            return Json(datalist);
        }
        #endregion

        #region 批量删除未审批业务员
        /// <summary>
        /// 批量删除未审批业务员
        /// </summary>
        /// <param name="userids"></param>
        /// <returns>返回-1重新登陆</returns>
        [HttpPost]
        public ActionResult BatchDeleteSalesman(string[] userids)
        {
            //判断登陆
            Dt_User user = RoleFuns.IsLoginAdmin(Session["user"]);
            if (user == null)
                return Content("-1");
            UserInfoDal userInfoDal = new UserInfoDal();
            int flag = userInfoDal.BatchDeleteSalesman(userids);

            //记录日志
            List<string> rolestr = Session["role"] != null ? (List<string>)Session["role"] : null;
            RoleFuns.SetAdminLog(user.username, "Pc_DetAuditSalesman", rolestr);

            return Content(flag.ToString());
        }
        #endregion

        #region 分配审核注册业务员
        /// <summary>
        /// 分配审核注册业务员
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="entid"></param>
        /// <returns>返回-1重新登陆，0:此业务员已审核或userid不存在</returns>
        public ActionResult AuditSalesman(string userid, string entid)
        {
            //判断登陆
            Dt_User user = RoleFuns.IsLoginAdmin(Session["user"]);
            if (user == null)
                return Content("-1");
            UserInfoDal userInfoDal = new UserInfoDal();
            int flag = userInfoDal.AuditSalesman(userid, entid);
            //记录日志
            List<string> rolestr = Session["role"] != null ? (List<string>)Session["role"] : null;
            RoleFuns.SetAdminLog(user.username, "Pc_AuditSalesman", rolestr);

            return Content(flag.ToString());
        }
        #endregion

        #region 短信验证码校验
        /// <summary>
        /// 短信验证码校验
        /// </summary>
        /// <param name="loginCode">短信验证码</param>
        /// <param name="cookid"></param>
        /// <returns></returns>
        public JsonResult SecurityCodeVerify(string loginCode = "", string cookid = "")
        {
            try
            {
                string code = "";
                if (cookid != "")
                {
                    code = sessionDic[cookid].ToString();
                    sessionDic.TryRemove(cookid, out string noteCode);
                }
                if (code == loginCode)
                {
                    return Json(new { success = true, message = "短信验证码正确！" });
                }
                else
                {
                    return Json(new { success = false, message = "短信验证码错误！" });
                }
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Users/SecurityCodeVerify", ex.Message.ToString());
                return Json(new { success = false, message = "短信验证码校验失败！" });
            }
        }
        #endregion

        #region 资质图片上传
        /// <summary>
        /// 资质图片上传
        /// </summary>
        /// <returns></returns>
        public JsonResult UploadMaterial()
        {
            try
            {
                string dataFile;
                string filePath = string.Empty;
                string fileNewName = string.Empty;
                var file = Request.Files;
                if (file.Count <= 0)
                {
                    return Json(new { success = false, message = "请选择要上传的文件！" });//请选择要上传的文件  
                }
                var ufile = file[0];
                string fileName = ufile.FileName;/*获取文件名： C:\Documents and Settings\Administrator\桌面\123.jpg*/
                //Log.Error("错误",$"上传文件名：{fileName}");//上传文件名：201912261054561.jpg
                string suffix = fileName.Substring(fileName.LastIndexOf(".") + 1).ToLower();/*获取后缀名并转为小写： jpg*/
                int bytes = ufile.ContentLength;//获取文件的字节大小  
                if (suffix != "jpg" && suffix != "png")
                {
                    return Json(new { success = false, message = "上传文件格式错误！" });//只能上传JPG格式图片  
                }
                else if (bytes > 1024 * 1024 * 3)
                {
                    return Json(new { success = false, message = "图片不能大于3M！" }); //图片不能大于1M 
                }
                /*判断文件夹是否存在*/
                if (Directory.Exists(Server.MapPath("/UploadFile/Material/")) == false)
                {
                    //Log.Error("打印", $"文件夹Material不存在，重新创建");
                    Directory.CreateDirectory(Server.MapPath("/UploadFile/Material/"));
                }
                string path = Server.MapPath("/UploadFile/Material/");
                //Log.Error("打印", $"物理文件路径path：{path}");
                string year = DateTime.Now.Year.ToString();
                string month = DateTime.Now.Month.ToString();
                string date = DateTime.Now.ToFileTimeUtc().ToString();
                string newfileName = date + "." + suffix;
                string newfile = year + "_" + month + "/" + newfileName;
                string newPath = path + year + "_" + month;
                //Log.Error("打印", $"上传文件路径newPath：{newPath}");//  /UploadFile/Users/System.Web.HttpPostedFileWrapper
                //Log.Error("打印", $"newPath文件是否存在：{System.IO.Directory.Exists(newPath)}");
                if (false == System.IO.Directory.Exists(newPath))
                {
                    //Log.Error("打印", $"文件夹newPath不存在，重新创建");
                    //创建文件夹
                    System.IO.Directory.CreateDirectory(newPath);
                }
                ufile.SaveAs(newPath + "/" + newfileName);//保存图片  
                                                          //保存图片路径
                dataFile = "/UploadFile/Material/" + newfile;
                string webUrl = BaseConfiguration.SercerIp.ToString();
                //if (webUrl.Contains("/b2b_api"))
                //{
                //    var index = webUrl.LastIndexOf('/');
                //    webUrl = webUrl.Substring(0, index);
                //}
                return Json(new { success = true, message = "资质图片上传成功！", imgUrl = webUrl + dataFile });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message.ToString(), imgUrl = "" });
            }
        }
        #endregion

        #region 删除资质图片
        /// <summary>
        /// 删除资质图片
        /// </summary>
        /// <param name="imgUrl">图片路径</param>
        /// <returns></returns>
        public JsonResult DeleteMaterial(string imgUrl)
        {
            if (string.IsNullOrEmpty(imgUrl))
            {
                return Json(new { success = false, message = "图片路径不能为空" });
            }
            try
            {
                string webUrl = BaseConfiguration.SercerIp.ToString();
                //if (webUrl.Contains("/b2b_api"))
                //{
                //    var index = webUrl.LastIndexOf('/');
                //    webUrl = webUrl.Substring(0, index);
                //}
                string newUrl;
                if (imgUrl.Contains(webUrl))
                {
                    newUrl = imgUrl.Replace(webUrl, "");
                }
                else
                {
                    newUrl = imgUrl;
                }
                string url = Server.MapPath(newUrl);
                FileInfo file = new FileInfo(url);
                if (file.Exists)
                {
                    file.Delete();
                    return Json(new { success = true, message = "图片删除成功" });
                }
                else
                {
                    return Json(new { success = true, message = "图片不存在" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message.ToString(), imgUrl = "" });
            }
        }
        #endregion

        #region 手机号校验
        /// <summary>
        /// 手机号校验
        /// </summary>
        /// <param name="telphone">电话号码</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PhoneVerify(string telphone)
        {
            try
            {
                if (string.IsNullOrEmpty(telphone))
                {
                    return Json(new { success = false, message = "参数异常！" });
                }
                UserInfoDal dal = new UserInfoDal();
                int status = dal.PhoneVerify(telphone, out string entId);
                string message;
                switch (status)
                {
                    case 0: message = "该手机号已注册,但未通过审核"; break;
                    case 1: message = "该手机号已注册,正在等待审核"; break;
                    case 2: message = "该手机号已注册,已通过审核"; break;
                    default: message = "该手机号未注册"; break;
                }
                return Json(new { success = true, status, message, entId });
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Users/PhoneVerify", ex.Message.ToString());
                return Json(new { success = false, message = "用户手机号校验失败！" });
            }
        }
        #endregion

        #region 用户资料
        /// <summary>
        /// 用户资料
        /// </summary>
        /// <param name="telphone">电话号码</param>
        /// <param name="entId">机构Id</param>
        /// <returns></returns>
        public JsonResult AcquireMaterial(string telphone, string entId)
        {
            try
            {
                if (string.IsNullOrEmpty(telphone))
                {
                    return Json(new { success = false, message = "参数异常！" });
                }
                UserInfoDal dal = new UserInfoDal();
                List<UserRegister> list = dal.AcquireMaterial(telphone, entId);
                return Json(new { success = true, list });
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Users/AcquireMaterial", ex.Message.ToString());
                return Json(new { success = false, message = "用户资料获取失败失败！" });
            }
        }
        #endregion

        #region 用户信贷
        /// <summary>
        /// 用户信贷
        /// </summary>
        /// <param name="userId">用户</param>
        /// <param name="entId">机构</param>
        /// <returns></returns>
        public ActionResult DebtInfo(string userId, string entId)
        {

            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { success = false, message = "用户未登录，请登录" });
                }
                if (string.IsNullOrEmpty(entId))
                {
                    entId = BaseConfiguration.EntId;
                }
                UserInfoDal dal = new UserInfoDal();
                UserDebt debt = dal.DebtInfo(userId, entId);
                return Json(new { success = true, message = "信贷信息获取成功", debt });
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Users/DebtInfo", ex.Message.ToString());
                return Json(new { success = false, message = ex.Message.ToString() });
            }
        }
        #endregion

        #region 发送手机验证码
        /// <summary>
        /// 发送手机验证码
        /// </summary>
        /// <param name="phoneNumber">接受短信的手机号码</param>
        /// <returns></returns>
        public JsonResult SendNote(string phoneNumber)
        {
            try
            {
                if (string.IsNullOrEmpty(phoneNumber))
                {
                    return Json(new { success = false, message = "参数异常！" });
                }
                //访问密钥，标识用户
                string accessKeyId = BaseConfiguration.AccessKeyId;
                //访问密钥，验证用户
                string accessSecret = BaseConfiguration.AccessSecret;
                //短信签名名称 
                string signName = BaseConfiguration.SignName;
                //短信模板ID
                string TemplateCode = BaseConfiguration.TemplateCode;

                string code = GetCheckCode(6); ;//短信模板参数
                string msg = SendCode.SendSms(accessKeyId, accessSecret, phoneNumber, signName, TemplateCode, code);
                JObject jObject = (JObject)JsonConvert.DeserializeObject(msg);
                if (jObject["Code"].ToString() == "OK")
                {
                    var guid = Guid.NewGuid().ToString();
                    sessionDic.TryAdd(guid, code);
                    return Json(new { success = true, message = guid });
                }
                else
                {
                    var message = jObject["Message"].ToString();
                    LogQueue.Write(LogType.Error, "Users/SendNote", message);
                    return Json(new { success = false, message });
                }
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Users/SendNote", ex.Message.ToString());
                return Json(new { success = false, message = "短信验证码发送失败！" });
            }
        }
        #endregion

        #region 用户充值红包
        /// <summary>
        /// 用户充值红包
        /// </summary>
        /// <param name="userId">用户</param>
        /// <param name="entId">机构</param>
        /// <param name="target">目标 unused[未使用]/used[已使用]</param>
        /// <param name="money">金额</param>
        /// <param name="normal">true[正常]/false[超期异常]</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="pageSize">单页数目</param>
        /// <returns></returns>
        public JsonResult RedEnvelopes(string userId, string entId, string target = "unused", decimal money = 0, string normal = "true", int pageIndex = 1, int pageSize = 30)
        {
            try
            {
                if (string.IsNullOrEmpty(entId))
                {
                    entId = BaseConfiguration.EntId.ToString();
                }
                else if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { success = false, message = "参数异常！" });
                }
                UserInfoDal userInfoDal = new UserInfoDal();
                List<Bonus> bonus = userInfoDal.RedEnvelopes(userId, entId, target, money, normal, pageIndex, pageSize, out int pageCount, out int recordCount);
                return Json(new { success = true, pageCount, recordCount, list = bonus, message = "用户充值红包获取成功" });
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Users/RedEnvelopes", ex.Message.ToString());
                return Json(new { success = false, message = ex.Message.ToString() });
            }
        }
        #endregion

        #region 获取资质证书
        public JsonResult GetUserQualifications(string userId, string entId)
        {
            try
            {
                if (string.IsNullOrEmpty(entId))
                {
                    entId = BaseConfiguration.EntId.ToString();
                }
                else if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { success = false, message = "参数异常！" });
                }
                SqlRun sql = new SqlRun(SqlRun.sqlstr);
                DataSet data = sql.RunProDataSet("Proc_UserInfo", new SqlParameter[] {
                    new SqlParameter("type","GetUserQualifications"),
                    new SqlParameter("userid",userId),
                    new SqlParameter("entid",entId)
                });
                List<object> list = new List<object>();
                if (data.Tables.Count == 2)
                {
                    foreach (DataRow obj in data.Tables[0].Rows)
                    {
                        if (obj != null)
                        {
                            int flag = 0;
                            foreach (DataRow entity in data.Tables[1].Rows)
                            {
                                if (entity != null)
                                {
                                    if (entity["materialName"].ToString() == obj["materialName"].ToString())
                                    {
                                        flag++;
                                        list.Add(new[] { obj["materialName"], obj["CustomerTypeId"], obj["userid"], entity["Id"], entity["materialUrl"], entity["TypeID"], entity["clienttype"], entity["name"] });
                                    }
                                }
                            }
                            if (flag == 0)
                            {
                                list.Add(new[] { obj["materialName"], obj["CustomerTypeId"], obj["userid"] });
                            }
                            flag = 0;
                            //list.Add(new[] { obj["materialName"], obj["CustomerTypeId"], obj["userid"] });
                        }
                    }

                }

                //List<Bonus> bonus = userInfoDal.RedEnvelopes();
                return Json(new { success = true, data = list, message = "用户资质证书获取成功" });
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Users/GetUserQualifications", ex.Message.ToString());
                return Json(new { success = false, message = ex.Message.ToString() });
            }
        }

        #endregion

        #region 根据上传图片编辑用户资质
        /// <summary>
        /// 根据上传图片编辑用户资质
        /// </summary>
        /// <param name="materialId">用户资质的信息的主要编号</param>
        /// <param name="imgurl"></param>
        /// <param name="entId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public JsonResult UpdateMaterialImgUrl(string materialId, string imgurl, string entId, string userId, string materialname, string customertypeid)
        {
            try
            {
                if (!StringHelper.IsNull(entId, userId, imgurl, materialname))
                {
                    Log.Info("信息：", entId + userId + imgurl + materialname + materialId.ToString() + customertypeid.ToString());
                    return Json(new { success = false, message = "参数异常" });
                }
                return Json(new { success = UserInfoDal.UpdateUsersMaterial(entId, userId, imgurl, materialId, materialname, customertypeid) });
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Users/Register", ex.Message.ToString());
                return Json(new { success = false, message = "用户资质修改失败！" });
            }
        }
        #endregion

        #region 获取验证码
        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="Digit">验证码位数</param>
        /// <returns>返回字符串</returns>
        public static string GetCheckCode(int Digit)
        {
            StringBuilder sb = new StringBuilder();
            Random random = new Random();
            int Nums = Digit;
            while (Nums > 0)
            {
                int i = random.Next(1, 10);// 9>=a>=1
                if (sb.Length < Digit)
                {
                    sb.Append(i);
                }
                Nums -= 1;
            }
            return sb.ToString();
        }
        #endregion
        
        #region 服务器信息
        /// <summary>
        /// 服务器信息
        /// </summary>
        /// <param name="entId">机构</param>
        /// <param name="userId">用户</param>
        public static void ServerVariables(string userId, string entId)
        {
            HttpRequest request = System.Web.HttpContext.Current.Request;
            UserInfoDal userInfoDal = new UserInfoDal();
            userInfoDal.ServerVariables(entId, userId, request);
        }
        #endregion

        #region 获取openid
        public JsonResult SetCoded(string WxCode)
        {
            try
            {
                if (!StringHelper.IsNull(WxCode))
                {
                    return Json(new AppMsg<string> { Code = false, State = false, Message = "参数为空" });
                }
                //var accesstoken = WeChatEntity.VerificationWXToken();

                var value = URLEncapsulation("", $"https://api.weixin.qq.com/sns/oauth2/access_token?appid=wxb8900ccb20574b49&secret=8bb90306664f578241537508ddf4adc6&code={WxCode}&grant_type=authorization_code", "get");

                var wxOpen = JsonConvert.DeserializeObject<WxOpenid>(value);
                if (wxOpen != null)
                {
                    return Json(new AppMsg<Object> { Code = true, State = true, Message = "获取成功", Source = wxOpen.openid });
                }
                else
                {
                    return Json(new AppMsg<Object> { Code = false, State = true, Message = "网络开小差了，请稍后", });
                }
            }
            catch (Exception ex)
            {
                Log.Error("错误", "系统异常:" + ex.Message);
                return Json(new AppMsg<Object> { Code = false, State = false, Message = "服务已停止" });
            }
        }
        #region App信息返回
        public class AppMsg<T>
        {
            /// <summary>
            /// 状态
            /// </summary>
            public bool State { get; set; }

            /// <summary>
            /// 返回值编码
            /// </summary>
            public bool Code { set; get; }
            /// <summary>
            /// 返回值说明
            /// </summary>
            public string Message { set; get; }
            /// <summary>
            /// 分页用，每页个数
            /// </summary>
            public int PageSize { set; get; }
            /// <summary>
            /// 分页用，当前页码
            /// </summary>
            public int PageIndex { set; get; }
            /// <summary>
            /// 记录总数
            /// </summary>
            public int TotalCount { set; get; }
            /// <summary>
            /// 返回的数据
            /// </summary>
            public T Source { set; get; }

        }
        #endregion

        #region 接受数据
        public class WxOpenid
        {
            public string access_token { get; set; }
            public long expires_in { get; set; }
            public string refresh_token { get; set; }
            public string openid { get; set; }
            public string scope { get; set; }

        }
        #endregion

        #region 发送数据
        /// <summary>
        /// 封装好的Url
        /// </summary>
        /// <param name="strjson">需要传的数据</param>
        /// <param name="url">地址</param>
        /// <param name="Method">get post 类型</param>
        /// /// <param name="type">默认"application/json" 类型</param>
        /// <returns></returns>
        public static string URLEncapsulation(string strjson, string url, string method, string type = "application/json")
        {

            try
            {
                byte[] bt1 = Encoding.UTF8.GetBytes(strjson);
                HttpWebRequest hwr = (HttpWebRequest)WebRequest.Create(url);
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                hwr.Method = method;
                hwr.ContentType = type;//; charset=utf-8
                hwr.ContentLength = bt1.Length;
                if (method == "post")
                {
                    using (Stream reqStream = hwr.GetRequestStream())
                    {
                        reqStream.Write(bt1, 0, bt1.Length);
                    }
                }
                using (WebResponse wr = hwr.GetResponse())
                {
                    if (wr.ToString() == "" || wr == null)
                    {
                        return "";
                    }
                    else
                    {
                        StreamReader reader = new StreamReader(wr.GetResponseStream(), true);
                        return reader.ReadToEnd();
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受     
        }

        /// <summary>
        ///   替换部分字符串
        /// </summary>
        /// <param name="sPassed">需要替换的字符串</param>
        /// <returns></returns>
        public static string ReplaceString(string JsonString)
        {
            if (JsonString == null) { return JsonString; }
            if (JsonString.Contains("\\"))
            {
                JsonString = JsonString.Replace("\\", "\\\\");
            }
            if (JsonString.Contains("\'"))
            {
                JsonString = JsonString.Replace("\'", "\\\'");
            }
            if (JsonString.Contains("\""))
            {
                JsonString = JsonString.Replace(@"\", "");
            }
            //去掉字符串的回车换行符
            JsonString = Regex.Replace(JsonString, @"[\n\r]", "");
            JsonString = JsonString.Trim();
            return JsonString;
        }
        #endregion
        #endregion

        #region 找回密码
        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <returns></returns>
        public JsonResult GetUserPassWord(string qqemail)
        {
            try
            {
                SqlRun sql = new SqlRun(SqlRun.sqlstr);
                StringBuilder str = new StringBuilder();
                str.Append($"select email from dt_users where email='{qqemail}'");
                int num = sql.RunSqlDataTable(str.ToString()).Rows.Count;
                if (num <= 0)
                {
                    return Json(new AppMsg<Object> { Code = false, State = false, Message = "该邮箱不存在！请重新输入绑定的邮箱！" });
                }
                //Log.Error("错误", "系统异常1");
                //正则表达式
                string pattern = @"[\w!#$%&'*+/=?^_`{|}~-]+(?:\.[\w!#$%&'*+/=?^_`{|}~-]+)*@(?:[\w](?:[\w-]*[\w])?\.)+[\w](?:[\w-]*[\w])?";
                //Log.Error("错误", "系统异常2");
                //使用RegexOptions.IgnoreCase枚举值表示不区分大小写
                //Log.Error("错误", "系统异常3");
                Regex r = new Regex(pattern, RegexOptions.IgnoreCase);
                //使用正则表达式匹配字符串，仅返回一次匹配结果
                //Log.Error("错误", "系统异常4");
                Match m = r.Match(qqemail);
                if (!m.Success || !Small(qqemail))
                {
                    return Json(new AppMsg<Object> { Code = false, State = false, Message = "服务暂时停止服务，请联系客服解决！" });
                }
                //if () {
                //    return Json(new AppMsg<Object> { Code = false, State = false, Message = "服务暂时停止服务，请联系客服解决！" });
                //};
                //var code = HttpContext.Session["QDZHMM"];+ code
                return Json(new AppMsg<Object> { Code = true, State = true, Message = "邮件已发送至绑定的邮箱" });
            }
            catch (Exception ex)
            {
                Log.Error("错误", "系统异常:" + ex.Message);
                return Json(new AppMsg<Object> { Code = false, State = false, Message = "服务已停止" });
            }
        }
        #endregion

        #region 修改密码
        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <returns></returns>
        public JsonResult SetUserPassWord(string password, string pwd, string codeyzm, string qqemail)
        {
            try
            {
                string code = CacheHelper.GetCache("QDZHMM") == null ? "" : CacheHelper.GetCache("QDZHMM").ToString();

                if (code == null)
                {
                    return Json(new AppMsg<Object> { Code = false, State = false, Message = "验证码已过期！验证码尽快使用！" });
                }
                else if (password != pwd)
                {
                    return Json(new AppMsg<Object> { Code = false, State = false, Message = "俩次密码不一致，请核对后重置密码！" });
                }
                else if (code != codeyzm)
                {
                    return Json(new AppMsg<Object> { Code = false, State = false, Message = "输入的验证码不正确，请重新输入！" });
                }
                else if (code == codeyzm)
                {
                    SqlRun sql = new SqlRun(SqlRun.sqlstr);
                    StringBuilder str = new StringBuilder();
                    string email = CacheHelper.GetCache(code) == null ? "" : CacheHelper.GetCache(code).ToString();
                    if (email != qqemail)
                    {
                        return Json(new AppMsg<Object> { Code = false, State = false, Message = "发送的验证码的邮箱与当前的要修改邮箱不匹配！" });
                    }
                    if (email == "")
                    {
                        return Json(new AppMsg<Object> { Code = false, State = false, Message = "操作已超时，请重新再试！" });
                    }
                    str.Append($"Update dt_users Set password='{Encryption.GetMD5_16(password)}' where email='{email}'");
                    int num = sql.RunSqlNumber(str.ToString());
                }
                return Json(new AppMsg<Object> { Code = true, State = true, Message = "密码重置成功！" });
            }
            catch (Exception ex)
            {
                Log.Error("错误", "系统异常:" + ex.Message);
                return Json(new AppMsg<Object> { Code = false, State = false, Message = "服务已停止" });
            }
        }
        #endregion

        #region 发送邮件
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="title">邮箱内容</param>
        /// <param name="Email">邮箱地址</param>
        /// <param name="value">收件人姓名</param>
        /// <returns></returns>
        public bool Small(string value)
        {
            try
            {
                var numstr = GetCheckCode(6);
                CacheHelper.SetCache("QDZHMM", numstr, DateTime.Now.AddMinutes(15), TimeSpan.Zero);
                CacheHelper.SetCache(numstr, value, DateTime.Now.AddMinutes(15), TimeSpan.Zero);
                MailMessage mail = new MailMessage();
                string emailtitle = "请注意！您正在找回密码！请及时处理！验证码：" + numstr;
                string emailname = BasisConfig.GetConfigString("EmailName");
                string tongxing = BasisConfig.GetConfigString("EmailPass");
                string emailservice = BasisConfig.GetConfigString("EmailService");
                mail.To = value;
                mail.From = emailname;
                mail.Subject = "请注意！您正在找回密码！";
                mail.BodyFormat = MailFormat.Html;
                mail.Body = $"<font color='red'>{emailtitle}</font>";
                mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", "1"); //basic authentication
                mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusername", emailname); //set your username here
                mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendpassword", tongxing); //set your password here
                mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserverport", 465);//set port
                mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpusessl", "true");//set is ssl
                SmtpMail.SmtpServer = emailservice;// "smtp.163.com";
                SmtpMail.Send(mail);
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion

        #region 文件上传
        [HttpPost]
        public JsonResult UpdateApp(string local)
        {
            try
            {
                //FileLib.UpLoadFile(local                                                ,);
                MailService.ResponseWindowsShared("/App/anzhuangbao.apk", local, "admin", "123456");
                return Json(new { success = true, message = "App上传成功！" });
            }
            catch (Exception ex)
            {
                Log.Error("错误:App上传", ex.Message);
                LogQueue.Write(LogType.Error, "Users/UpdateApp", ex.Message);
                return Json(new { success = false, message = "App上传失败！" });
            }
        }
        #endregion

        #region 角标
        /// <summary>
        /// 个人中心角标
        /// </summary>
        /// <param name="entId">企业</param>
        /// <param name="userId">用户</param>
        /// <returns></returns>
        public JsonResult CornerMark(string entId,string userId)
        {
            try
            {
                UserInfoDal infoDal = new UserInfoDal();
                CornerMark cornerMark = infoDal.CornerMark(entId, userId);
                return Json(new { cornerMark });
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "", $"角标获取失败,原因：{ex.Message}");
                return Json(new { success = false, message = ex.Message.ToString() });
            }
        }
        #endregion
    }
}
