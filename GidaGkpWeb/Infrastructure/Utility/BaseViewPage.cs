using DataLayer;
using GidaGkpWeb.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static GidaGkpWeb.Global.Enums;
using GidaGkpWeb.Infrastructure.Authentication;
using GidaGkpWeb.BAL;
using GidaGkpWeb.Models;
using System.ComponentModel.DataAnnotations;

namespace GidaGkpWeb.Infrastructure.Utility
{
    public abstract class BaseViewPage : WebViewPage
    {
    }

    public abstract class BaseViewPage<TModel> : WebViewPage<TModel>
    {
        public virtual new CustomPrincipal User
        {
            get { return base.User as CustomPrincipal; }
        }

        public virtual List<AdminNoticeModel> GetNoticeByType(string NoticeType)
        {
            AdminDetails _details = new AdminDetails();
            return _details.GetNoticeByType(NoticeType);
        }

        public virtual int GetEnablePaymentLink()
        {
            ApplicantDetails _details = new ApplicantDetails();
            return _details.GetEnablePaymentLink(((CustomPrincipal)User).Id);
        }

        public virtual int GetEnablePrintReciptLink()
        {
            ApplicantDetails _details = new ApplicantDetails();
            return _details.GetEnablePrintReciptLink(((CustomPrincipal)User).Id);
        }

        public bool hasPermissionOnPage(string PageName)
        {
            return UserData.UserPermissions.Any(x => x.PageName.Contains(PageName.Trim()));
        }
        public bool hasPermission(string PermissionName)
        {
            return UserData.UserPermissions.Any(x => x.PermissionName == PermissionName);
        }
    }

    public static class PermissionName
    {
        public static string NoAccess = "No Access";
        public static string AllAccess = "All Access";
        public static string Add = "Add";
        public static string AddEdit = "Add Edit";
        public static string View = "View";
        public static string Delete = "Delete";
    }
}