using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Sk_B2BAPI.Tool
{
    public class CacheHelper
    {
        #region 获取当前应用程序指定CacheKey的Cache值
        /// <summary>
        /// 获取当前应用程序指定CacheKey的Cache值
        /// </summary>
        /// <param name="cacheKey">缓存key</param>
        /// <returns></returns>
        public static object GetCache(string cacheKey)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            return objCache[cacheKey];
        }
        #endregion

        #region 设置当前应用程序指定CacheKey的Cache值
        /// <summary>
        /// 设置当前应用程序指定CacheKey的Cache值
        /// </summary>
        /// <param name="cacheKey">缓存key</param>
        /// <param name="objObject">缓存value</param>
        public static void SetCache(string cacheKey, object objObject)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            objCache.Insert(cacheKey, objObject);
        }
        #endregion

        #region 设置当前应用程序指定CacheKey的Cache值 时间限制

        /// <summary>
        /// 设置当前应用程序指定CacheKey的Cache值
        /// </summary>
        /// <param name="cacheKey">缓存key</param>
        /// <param name="objObject">缓存value</param>
        /// <param name="absoluteExpiration">绝对过期时间（当超过设定时间，立即移除。）</param>
        /// <param name="slidingExpiration">滑动过期时间（当超过设定时间没再使用时，才移除缓存）</param>
        public static void SetCache(string cacheKey, object objObject, DateTime absoluteExpiration, TimeSpan slidingExpiration)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            objCache.Insert(cacheKey, objObject, null, absoluteExpiration, slidingExpiration);
        }
        #endregion
    }
}
