using System;

namespace Intelligent.Community.Infrastructure
{
    /// <summary>
    ///     表示由此特性所描述的方法，能够获得来自Saylor.Application所提供的缓存功能。
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class CacheAttribute : Attribute
    {
        public CacheAttribute()
        {
            this.ExpireTime = 20 * 60;
        }
        /// <summary>
        ///     缓存对象的KEY
        /// </summary>
        public string CacheKey { get; set; }
        /// <summary>
        ///     缓存有效时间【单位：秒】
        /// </summary>
        public int ExpireTime { get; set; }
    }
}
