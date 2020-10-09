using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Sk_B2BAPI.Tool
{
    public class CookieHelper
    {
        #region 添加一个指定过期时间的双加密Cookie
        /// <summary>
        /// 存储加密后的cookie(双加密)
        /// </summary>
        /// <param name="cookiename">明文cookie名</param>
        /// <param name="cookievalue">明文cookie值</param>
        public static void AddEnCookie(string cookiename, string cookievalue, DateTime expires)
        {
            SetCookie(HttpUtility.UrlEncode(Md5Helper.Encode(cookiename)), HttpUtility.UrlEncode(Md5Helper.Encode(cookievalue)), expires);
        }
        #endregion

        #region 添加一个1天后过期的双加密Cookie
        /// <summary>
        /// 存储加密后的cookie(双加密)
        /// </summary>
        /// <param name="cookiename">明文cookie名</param>
        /// <param name="cookievalue">明文cookie值</param>
        public static void AddEnCookie(string cookiename, string cookievalue)
        {

            SetCookie(HttpUtility.UrlEncode(Md5Helper.Encode(cookiename)), HttpUtility.UrlEncode(Md5Helper.Encode(cookievalue)), DateTime.Now.AddDays(1.0));
        }
        #endregion


        #region 添加一个1天后过期的双加密Cookie，并返回cookie值
        /// <summary>
        /// 存储加密后的cookie(双加密)
        /// </summary>
        /// <param name="cookiename">明文cookie名</param>
        /// <param name="cookievalue">明文cookie值</param>
        public static string AddEnCookieAndReturn(string cookiename, string cookievalue)
        {

            SetCookie(HttpUtility.UrlEncode(Md5Helper.Encode(cookiename)), HttpUtility.UrlEncode(Md5Helper.Encode(cookievalue)), DateTime.Now.AddDays(1.0));
            return HttpUtility.UrlEncode(Md5Helper.Encode(cookievalue));
        }
        #endregion

        #region 获取双加密Cookie的值
        /// <summary>
        /// 获取解密后的cookie的值
        /// </summary>
        /// <param name="cookiename">明文cookie名</param>
        /// <returns>解密后的cookie值</returns>
        public static string GetDeCookie(string cookiename)
        {
            return Md5Helper.Decode(HttpUtility.UrlDecode(GetCookieValue(HttpUtility.UrlEncode(Md5Helper.Encode(cookiename)))));
        }
        #endregion

        #region 删除双加密Cookie
        /// <summary>
        /// 删除加密后的cookie
        /// </summary>
        /// <param name="cookiename">明文cookie名</param>
        public static void DelEdCookie(string cookiename)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[HttpUtility.UrlEncode(Md5Helper.Encode(cookiename))];
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddYears(-3);
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }
        #endregion

        #region 添加一个指定过期时间的Cookie
        /// <summary>
        /// 添加一个Cookie
        /// </summary>
        /// <param name="cookiename">cookie名</param>
        /// <param name="cookievalue">cookie值</param>
        /// <param name="expires">过期时间 DateTime</param>
        public static void SetCookie(string cookiename, string cookievalue, DateTime expires)
        {
            HttpCookie cookie = new HttpCookie(cookiename)
            {
                Value = cookievalue,
                Expires = expires
            };
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
        #endregion

        #region 添加一个1天过期的Cookie（24小时过期）
        /// <summary>
        /// 添加一个Cookie（24小时过期）
        /// </summary>
        /// <param name="cookiename"></param>
        /// <param name="cookievalue"></param>
        public static void SetCookie(string cookiename, string cookievalue)
        {
            SetCookie(cookiename, cookievalue, DateTime.Now.AddDays(1.0));
        }
        #endregion

        #region 获取指定Cookie值
        /// <summary>
        /// 获取指定Cookie值
        /// </summary>
        /// <param name="cookiename">cookiename</param>
        /// <returns></returns>
        public static string GetCookieValue(string cookiename)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookiename];
            string str = string.Empty;
            if (cookie != null)
            {
                str = cookie.Value;
            }
            return str;
        }
        #endregion

        #region 删除Cookie
        /// <summary>
        /// 清除指定Cookie
        /// </summary>
        /// <param name="cookiename">cookiename</param>
        public static void DelCookie(string cookiename)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookiename];
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddYears(-3);
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }
        #endregion
    }
}
