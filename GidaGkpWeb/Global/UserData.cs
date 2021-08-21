using GidaGkpWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GidaGkpWeb.Global
{
    public static class UserData
    {
        public static int UserId
        {
            get
            {
                if (System.Web.HttpContext.Current.Session["userid"] != null)
                {
                    return Convert.ToInt32(HttpContext.Current.Session["userid"].ToString());
                }
                else
                    return 0;
            }
            set { HttpContext.Current.Session["userid"] = value; }
        }
        public static string Username
        {
            get
            {
                if (System.Web.HttpContext.Current.Session["username"] != null)
                {
                    return HttpContext.Current.Session["username"].ToString();
                }
                else
                    return string.Empty;
            }
            set { HttpContext.Current.Session["username"] = value; }
        }
        public static string FullName
        {
            get
            {
                if (System.Web.HttpContext.Current.Session["FullName"] != null)
                {
                    return HttpContext.Current.Session["FullName"].ToString();
                }
                else
                    return string.Empty;
            }
            set { HttpContext.Current.Session["FullName"] = value; }
        }
        public static string MiddleName
        {
            get
            {
                if (System.Web.HttpContext.Current.Session["MiddleName"] != null)
                {
                    return HttpContext.Current.Session["MiddleName"].ToString();
                }
                else
                    return string.Empty;
            }
            set { HttpContext.Current.Session["MiddleName"] = value; }
        }
        public static string LastName
        {
            get
            {
                if (System.Web.HttpContext.Current.Session["LastName"] != null)
                {
                    return HttpContext.Current.Session["LastName"].ToString();
                }
                else
                    return string.Empty;
            }
            set { HttpContext.Current.Session["LastName"] = value; }
        }
        public static string Email
        {
            get
            {
                if (System.Web.HttpContext.Current.Session["Email"] != null)
                {
                    return HttpContext.Current.Session["Email"].ToString();
                }
                else
                    return string.Empty;
            }
            set { HttpContext.Current.Session["Email"] = value; }
        }
        public static string UserType
        {
            get
            {
                if (System.Web.HttpContext.Current.Session["UserType"] != null)
                {
                    return HttpContext.Current.Session["UserType"].ToString();
                }
                else
                    return string.Empty;
            }
            set { HttpContext.Current.Session["UserType"] = value; }
        }
        public static string Department
        {
            get
            {
                if (System.Web.HttpContext.Current.Session["Department"] != null)
                {
                    return HttpContext.Current.Session["Department"].ToString();
                }
                else
                    return string.Empty;
            }
            set { HttpContext.Current.Session["Department"] = value; }
        }
        public static string Designation
        {
            get
            {
                if (System.Web.HttpContext.Current.Session["Designation"] != null)
                {
                    return HttpContext.Current.Session["Designation"].ToString();
                }
                else
                    return string.Empty;
            }
            set { HttpContext.Current.Session["Designation"] = value; }
        }
        public static List<UserPermission> UserPermissions
        {
            get
            {
                if (System.Web.HttpContext.Current.Session["UserPermissions"] != null)
                    return HttpContext.Current.Session["UserPermissions"] as List<UserPermission>;
                else
                    return new List<UserPermission>();
            }
            set { HttpContext.Current.Session["UserPermissions"] = value; }
        }
        public static int ApplicationId
        {
            get
            {
                if (HttpContext.Current.Session["ApplicationId"] != null)
                {
                    return Convert.ToInt32(HttpContext.Current.Session["ApplicationId"]);
                }
                else
                    return 0;
            }
            set { HttpContext.Current.Session["ApplicationId"] = value; }
        }

    }
}