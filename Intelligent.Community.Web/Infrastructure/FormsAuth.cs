using Intelligent.Community.DataObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Intelligent.Community.Web
{
    public class FormsAuth
    {
        public static void SignIn(string loginName, object userData, int expiredMin)
        {
            var data = JsonConvert.SerializeObject(userData);
            //创建一个FormsAuthenticationTicket，它包含登录名以及额外的用户数据。
            var ticket = new FormsAuthenticationTicket(2,
                loginName, DateTime.Now, DateTime.Now.AddDays(1), true, data);
            //加密Ticket，变成一个加密的字符串。
            var cookieValue = FormsAuthentication.Encrypt(ticket);
            //根据加密结果创建登录Cookie
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, cookieValue)
            {
                HttpOnly = true,
                Secure = FormsAuthentication.RequireSSL,
                Domain = FormsAuthentication.CookieDomain,
                Path = FormsAuthentication.FormsCookiePath
            };
            if (expiredMin > 0)
                cookie.Expires = DateTime.Now.AddMinutes(expiredMin);

            var context = HttpContext.Current;
            if (context == null)
                throw new InvalidOperationException();

            //写登录Cookie
            context.Response.Cookies.Remove(cookie.Name);
            context.Response.Cookies.Add(cookie);
        }
        /// <summary>
        ///     登陆
        /// </summary>
        public static void SignOut()
        {
            FormsAuthentication.SignOut();
        }
        /// <summary>
        ///     获取登陆用户信息
        /// </summary>
        /// <returns></returns>
        public static EmployeeDataObject GetLoginModel()
        {
            return GetCookies<EmployeeDataObject>();
        }
        /// <summary>
        ///     获取Cookie信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetCookies<T>() where T : class, new()
        {
            var data = new T();
            try
            {
                var context = HttpContext.Current;
                var cookie = context.Request.Cookies[FormsAuthentication.FormsCookieName];
                var ticket = FormsAuthentication.Decrypt(cookie.Value);
                data = JsonConvert.DeserializeObject<T>(ticket.UserData);
            }
            catch
            { }

            return data;
        }
    }
}