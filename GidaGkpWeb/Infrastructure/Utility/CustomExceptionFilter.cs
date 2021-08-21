using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GidaGkpWeb.Infrastructure.Utility
{
    public class SessionTimeoutAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext ctx = HttpContext.Current;
            if (HttpContext.Current.Session["userid"] == null && filterContext.ActionDescriptor.ActionName != "PaymentResponse")
            {
                filterContext.Result = new RedirectResult("~/Login/ApplicantLogin");
                return;
            }
            base.OnActionExecuting(filterContext);
        }
    }

    public class AdminSessionTimeoutAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext ctx = HttpContext.Current;
            if (HttpContext.Current.Session["userid"] == null)
            {
                filterContext.Result = new RedirectResult("~/Login/AdminLogin");
                return;
            }
            base.OnActionExecuting(filterContext);
        }
    }
}