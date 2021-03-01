using System;
using System.Runtime.Caching;

namespace CoStudy.API.Application.Features
{
    /// <summary>
    /// Cachehelper
    /// </summary>
    public class CacheHelper
    {
        /// <summary>
        /// Get cache value by key
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static object GetValue(string key)
        {
            return MemoryCache.Default.Get(key);
        }

        /// <summary>
        /// Add a cache object with date expiration
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="absExpiration">The abs expiration.</param>
        /// <returns></returns>
        public static bool Add(string key, object value, DateTimeOffset absExpiration)
        {
            return MemoryCache.Default.Add(key, value, absExpiration);
        }

        /// <summary>
        /// Delete cache value from key
        /// </summary>
        /// <param name="key">The key.</param>
        public static void Delete(string key)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            if (memoryCache.Contains(key))
            {
                memoryCache.Remove(key);
            }
        }
    }
}
