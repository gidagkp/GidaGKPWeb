using DataLayer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using GidaGkpWeb.Global;
using GidaGkpWeb.Models;
using GidaGkpWeb.BAL.Login;
using GidaGkpWeb.BAL;
using CCA.Util;
using System.Collections.Specialized;
using GidaGkpWeb.Infrastructure.Utility;
using GidaGkpWeb.Infrastructure.Authentication;
using System.Web.Script.Serialization;
using System.Web.Security;
using System.Net.Http;
using System.Net;
using System.Text;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using System.Threading.Tasks;
using GidaGkpWeb.Infrastructure;
using System.Configuration;
using GidaGkpWeb.BAL.Masters;


namespace GidaGkpWeb.Controllers
{
    [AdminSessionTimeout]
    public class ReportController : CommonController
    {
        public ActionResult SelectedInterviewCandidate()
        {
            return View();
        }
        public ActionResult RejectedInterviewCandidate()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetSelectedApplicant(string schemeName)
        {
            AdminDetails _details = new AdminDetails();
            var data = _details.GetApplicantSubmittedForInterview().Where(x => x.InterviewLetterStatus == "Selected" && x.SchemeName == schemeName).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetRejectedApplicant(string schemeName)
        {
            AdminDetails _details = new AdminDetails();
            var data = _details.GetApplicantSubmittedForInterview().Where(x => x.InterviewLetterStatus == "Rejected" && x.SchemeName == schemeName).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}