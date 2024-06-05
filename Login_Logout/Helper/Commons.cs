using Login_Logout.Models;
using Newtonsoft.Json;
using System;
using System.Web;
using System.Web.Mvc;

namespace Login_Logout.Helper
{
    public class Commons
    {
        public static HttpCookie GenCookie(string name, string value, DateTime expire)
        {
            return new HttpCookie(name)
            {
                Expires = expire,
                Value = value,
                HttpOnly = true
            };
        }
    }

    public static class ExtensionJson
    {
        public static string toJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
    

    public class CustomAuthenticationFilter : ActionFilterAttribute, IAuthorizationFilter
    {
        private int DEPT_ID;
        public CustomAuthenticationFilter(int deptID = 0)
        {
            DEPT_ID = deptID;            
        }
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            var httpContext = filterContext.HttpContext;

            var user = httpContext.Request.Cookies["uif"];

            var d = filterContext.RequestContext.HttpContext.Session["DEPT_ID"];

            if (user == null || d == null)
            {
                filterContext.Result = new RedirectResult("~/UserLogin/Index");
                return;
            }
            //UserLogInfo u = JsonConvert.DeserializeObject<UserLogInfo>(user.Value);
            //if (DEPT_ID != 0)
            //{

            //}
        }
    }

    public class CustomExcepFilter : ActionFilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            
        }
    }

    public class DEPT_ID_E
    {
        public const int IT = 791;
        public const int Security = 793;
    }


}