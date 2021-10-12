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
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System.Data.SqlClient;
using System.Data;

namespace GidaGkpWeb.Controllers
{
    public class Report
    {

        public string UserName { get; set; }
        public string FullName { get; set; }
        public string ApplicationNumber { get; set; }
        public string PlotArea { get; set; }
        public string PaidAmount { get; set; }
        public string FatherName { get; set; }
        public string ContactNo { get; set; }
        public string Email { get; set; }
        public string AadharNumber { get; set; }
        public string UserType { get; set; }
        public string DOB { get; set; }
        public string UnitName { get; set; }
        public string TotalInvestment { get; set; }
        public string Skilled { get; set; }
        public string CAddress { get; set; }
        public string PAddress { get; set; }
        public string ApplicationId { get; set; }
        public string BankName { get; set; }
        public string BBAddress { get; set; }
        public string bank_ref_no { get; set; }
        

    }
    public enum DocumentName
    {
        ApplicantEduTechQualification,
        ScanPAN,
        ScanID,
        ApplicantPhoto,
        ApplicantSignature,
        BalanceSheet,
        BankVerifiedSignature,
        DocProofForIndustrialEstablishedOutsideGida,
        ExperienceProof,
        FinDetailsEstablishedIndustries,
        ITReturn,
        Memorendum,
        OtherDocForProposedIndustry,
        PreEstablishedIndustriesDoc,
        ProjectReport,
        ProposedPlanLandUses,
        ScanAddressProof,
        ScanCastCert
    }

    [AdminSessionTimeout]
    public class AdminController : CommonController
    {
        public ActionResult Dashboard()
        {
            return View();
        }

        public ActionResult ApplicantUser()
        {
            AdminDetails _details = new AdminDetails();
            ViewData["ApplicantData"] = _details.GetApplicantUser();
            return View();
        }
        public ActionResult ActivateDeActivateUser(int userId)
        {
            if (userId > 0)
            {
                AdminDetails _details = new AdminDetails();
                var result = _details.ActivateDeActivateUser(userId);
                if (result == Enums.CrudStatus.Saved)
                    SetAlertMessage("User has been Activated/DeActivated", "User Action");
                else
                    SetAlertMessage("User has not been Activated/DeActivated", "User Action");
                return RedirectToAction("ApplicantUser");
            }
            return RedirectToAction("ApplicantUser");
        }

        public ActionResult ApplicantFormSubmitted(string SchemeName)
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetFormSubmittedDetail(int schemeName)
        {
            AdminDetails _details = new AdminDetails();
            var data = _details.GetApplicantUserDetail(schemeName).Where(x => x.ApplicationNumber != "" && x.PaidAmount == "").ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ApplicantTransactionCompleted()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetTransactionCompletedDetail(int schemeName)
        {
            AdminDetails _details = new AdminDetails();
            //var data = _details.GetApplicantUserDetail(schemeName).Where(x => x.PaidAmount != "").ToList();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GidaGKPEntities123"].ConnectionString);
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            DataSet ds = new DataSet();
            SqlDataAdapter adp = new SqlDataAdapter("STP_Select_Report " + schemeName + "", conn);
            adp.Fill(ds);
            List<Report> data = new List<Report>();
            Report aaa = null;
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    aaa = new Report();
                    aaa.ApplicationId = dr["ApplicationId"].ToString();
                    aaa.UserName = dr["UserName"].ToString();
                    aaa.FullName = dr["FullApplicantName"].ToString();
                    aaa.ApplicationNumber = dr["ApplicationNumber"].ToString();
                    aaa.PlotArea = dr["PlotArea"].ToString();
                    aaa.PaidAmount = dr["AmountPaid"].ToString();
                    aaa.FatherName = dr["FatherName"].ToString();
                    aaa.ContactNo = dr["ContactNo"].ToString();
                    aaa.AadharNumber = dr["AadharNumber"].ToString();
                    aaa.Email = dr["Email"].ToString();
                    aaa.DOB = dr["DOB"].ToString();
                    aaa.UserType = dr["UserType"].ToString();
                    aaa.UnitName = dr["CompanyName"].ToString();
                    aaa.CAddress = dr["CAddress"].ToString();
                    aaa.TotalInvestment = dr["TotalInvestment"].ToString();
                    aaa.Skilled = dr["Skilled"].ToString();
                    aaa.BankName = dr["BankName"].ToString();
                    aaa.BBAddress = dr["BBAddress"].ToString();
                    aaa.PAddress = dr["PAddress"].ToString();
                    aaa.bank_ref_no = dr["bank_ref_no"].ToString();

                    data.Add(aaa);
                }
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ApplicantFinancialDetails()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetApplicantFinancialDetails(int schemeName)
        {
            AdminDetails _details = new AdminDetails();
            var data = _details.GetApplicantUserDetail(schemeName).Where(x => x.PaidAmount != "" && x.ApplicationStatus != null && x.ApplicationStatus == false).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PropertyDashboard()
        {
            return View();
        }
        public ActionResult GrievancesDashboard()
        {
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public FileResult ExportApplicantUser(string GridHtml)
        {
            return File(Encoding.ASCII.GetBytes(GridHtml), "application/vnd.ms-excel", "Total Active Applicant User.xls");
        }

        [HttpPost]
        [ValidateInput(false)]
        public FileResult ExportFormSubmitted(string GridHtml)
        {
            return File(Encoding.ASCII.GetBytes(GridHtml), "application/vnd.ms-excel", "Plot Registered Only.xls");
        }

        [HttpPost]
        [ValidateInput(false)]
        public FileResult ExportFormCompleted(string GridHtml)
        {
            return File(Encoding.ASCII.GetBytes(GridHtml), "application/vnd.ms-excel", "Payment Completed.xls");
        }
        [HttpPost]
        [ValidateInput(false)]
        public FileResult ExportFormRejetced(string GridHtml)
        {
            return File(Encoding.ASCII.GetBytes(GridHtml), "application/vnd.ms-excel", "Applicant Financial Details.xls");
        }
        [HttpPost]
        [ValidateInput(false)]
        public FileResult ExportFormSelected(string GridHtml)
        {
            return File(Encoding.ASCII.GetBytes(GridHtml), "application/vnd.ms-excel", "Application Selected.xls");
        }
        public ActionResult Notice(int? NoticeId = null)
        {
            if (NoticeId != null)
            {
                AdminDetails _details = new AdminDetails();
                var result = _details.GetNoticeById(NoticeId.Value);
                result.NoticeDocumentFile = null;
                ViewData["ApplicantData"] = result;
            }
            return View();
        }
        public ActionResult NoticeList()
        {
            AdminDetails _details = new AdminDetails();
            ViewData["ApplicantData"] = _details.GetNoticeList();
            return View();
        }
        [HttpPost]
        public JsonResult GetNoticeDetail(int NoticeId)
        {
            AdminDetails _details = new AdminDetails();
            if (NoticeId > 0)
            {
                var data = _details.GetNoticeById(NoticeId);
                data.NoticeDocumentFile = null;
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return null;
        }
        [HttpPost]
        public ActionResult SaveNotice(HttpPostedFileBase Document, string NoticeType, string Title, string DepartmentNotice, string NoticeDate, string NewTag, string Publish, int? NoticeId)
        {
            AdminNotice notice = new AdminNotice();
            if (Document != null && Document.ContentLength > 0)
            {
                notice.NoticeDocumentFile = new byte[Document.ContentLength];
                Document.InputStream.Read(notice.NoticeDocumentFile, 0, Document.ContentLength);
                notice.NoticeDocumentName = Document.FileName;
                notice.NoticeDocumentFileType = Document.ContentType;
            }
            notice.NoticeTypeId = Convert.ToInt32(NoticeType);
            notice.Notice_title = Title;
            notice.Department = !string.IsNullOrEmpty(DepartmentNotice) ? Convert.ToInt32(DepartmentNotice) : 0;
            if (NoticeDate != "")
                notice.Notice_Date = Convert.ToDateTime(NoticeDate);
            if (NewTag == "on")
                notice.NoticeNewTag = true;
            else
                notice.NoticeNewTag = false;
            if (Publish == "on")
                notice.IsActive = true;
            else
                notice.IsActive = false;

            if (NoticeId <= 0 || NoticeId == null)
            {
                notice.CreatedBy = UserData.UserId;
                notice.CreationDate = DateTime.Now;
            }
            else
            {
                notice.Id = NoticeId.Value;
            }

            AdminDetails _details = new AdminDetails();
            var result = _details.SaveNotice(notice);
            if (result == Enums.CrudStatus.Saved)
                SetAlertMessage("Notice has been Saved", "Notice Save");
            else
                SetAlertMessage("Notice Saving failed", "Notice Save");
            if (NoticeId != null && NoticeId > 0)
            {
                return RedirectToAction("NoticeList");
            }
            else
            {
                return RedirectToAction("Notice");
            }

        }

        [HttpGet]
        public FileResult DownloadAttachment(string applicationId)
        {
            ApplicantDetails detail = new ApplicantDetails();
            var documentData = detail.GetApplicantUploadDocDetail(Convert.ToInt32(applicationId));
            if (documentData == null)
            {
                SetAlertMessage("Something went wrong in downloading the attchment, try again later", "Downlaod Attachment");
                return null;
            }
            //byte[] bytes = noticeData.NoticeDocumentFile;
            //return File(bytes, System.Net.Mime.MediaTypeNames.Application.Octet, noticeData.NoticeDocumentName);
            var fileName = string.Format("{0}_files.zip", DateTime.Today.Date.ToString("dd-MM-yyyy") + "_UserId_" + documentData.UserId);
            var temppath = Server.MapPath("~/TempFiles/");
            if (!Directory.Exists(temppath))
            {
                Directory.CreateDirectory(temppath);
            }
            var tempOutPutPath = Path.Combine(temppath, fileName);
            using (ZipOutputStream s = new ZipOutputStream(System.IO.File.Create(tempOutPutPath)))
            {
                s.SetLevel(9); // 0-9, 9 being the highest compression  

                if (documentData.ProjectReport != null)
                {
                    ZipEntry entry = new ZipEntry(documentData.ProjectReportFileName);
                    entry.DateTime = DateTime.Now;
                    entry.IsUnicodeText = true;
                    s.PutNextEntry(entry);
                    s.Write(documentData.ProjectReport, 0, documentData.ProjectReport.Length);
                }

                if (documentData.ProposedPlanLandUses != null)
                {
                    ZipEntry entry = new ZipEntry(documentData.ProposedPlanLandUsesFileName);
                    entry.DateTime = DateTime.Now;
                    entry.IsUnicodeText = true;
                    s.PutNextEntry(entry);
                    s.Write(documentData.ProposedPlanLandUses, 0, documentData.ProposedPlanLandUses.Length);
                }

                if (documentData.Memorendum != null)
                {
                    ZipEntry entry = new ZipEntry(documentData.MemorendumFileName);
                    entry.DateTime = DateTime.Now;
                    entry.IsUnicodeText = true;
                    s.PutNextEntry(entry);
                    s.Write(documentData.Memorendum, 0, documentData.Memorendum.Length);
                }

                if (documentData.ScanPAN != null)
                {
                    ZipEntry entry = new ZipEntry(documentData.ScanPANFileName);
                    entry.DateTime = DateTime.Now;
                    entry.IsUnicodeText = true;
                    s.PutNextEntry(entry);
                    s.Write(documentData.ScanPAN, 0, documentData.ScanPAN.Length);
                }

                if (documentData.ScanAddressProof != null)
                {
                    ZipEntry entry = new ZipEntry(documentData.ScanAddressProofFileName);
                    entry.DateTime = DateTime.Now;
                    entry.IsUnicodeText = true;
                    s.PutNextEntry(entry);
                    s.Write(documentData.ScanAddressProof, 0, documentData.ScanAddressProof.Length);
                }

                if (documentData.BalanceSheet != null)
                {
                    ZipEntry entry = new ZipEntry(documentData.BalanceSheetFileName);
                    entry.DateTime = DateTime.Now;
                    entry.IsUnicodeText = true;
                    s.PutNextEntry(entry);
                    s.Write(documentData.BalanceSheet, 0, documentData.BalanceSheet.Length);
                }

                if (documentData.ITReturn != null)
                {
                    ZipEntry entry = new ZipEntry(documentData.ITReturnFileName);
                    entry.DateTime = DateTime.Now;
                    entry.IsUnicodeText = true;
                    s.PutNextEntry(entry);
                    s.Write(documentData.ITReturn, 0, documentData.ITReturn.Length);
                }

                if (documentData.ExperienceProof != null)
                {
                    ZipEntry entry = new ZipEntry(documentData.ExperienceProofFileName);
                    entry.DateTime = DateTime.Now;
                    entry.IsUnicodeText = true;
                    s.PutNextEntry(entry);
                    s.Write(documentData.ExperienceProof, 0, documentData.ExperienceProof.Length);
                }

                if (documentData.ApplicantEduTechQualification != null)
                {
                    ZipEntry entry = new ZipEntry(documentData.ApplicantEduTechQualificationFileName);
                    entry.DateTime = DateTime.Now;
                    entry.IsUnicodeText = true;
                    s.PutNextEntry(entry);
                    s.Write(documentData.ApplicantEduTechQualification, 0, documentData.ApplicantEduTechQualification.Length);
                }

                if (documentData.PreEstablishedIndustriesDoc != null)
                {
                    ZipEntry entry = new ZipEntry(documentData.PreEstablishedIndustriesDocFileName);
                    entry.DateTime = DateTime.Now;
                    entry.IsUnicodeText = true;
                    s.PutNextEntry(entry);
                    s.Write(documentData.PreEstablishedIndustriesDoc, 0, documentData.PreEstablishedIndustriesDoc.Length);
                }

                if (documentData.FinDetailsEstablishedIndustries != null)
                {
                    ZipEntry entry = new ZipEntry(documentData.FinDetailsEstablishedIndustriesFileName);
                    entry.DateTime = DateTime.Now;
                    entry.IsUnicodeText = true;
                    s.PutNextEntry(entry);
                    s.Write(documentData.FinDetailsEstablishedIndustries, 0, documentData.FinDetailsEstablishedIndustries.Length);
                }

                if (documentData.OtherDocForProposedIndustry != null)
                {
                    ZipEntry entry = new ZipEntry(documentData.OtherDocForProposedIndustryFileName);
                    entry.DateTime = DateTime.Now;
                    entry.IsUnicodeText = true;
                    s.PutNextEntry(entry);
                    s.Write(documentData.OtherDocForProposedIndustry, 0, documentData.OtherDocForProposedIndustry.Length);
                }

                if (documentData.ScanCastCert != null)
                {
                    ZipEntry entry = new ZipEntry(documentData.ScanCastCertFileName);
                    entry.DateTime = DateTime.Now;
                    entry.IsUnicodeText = true;
                    s.PutNextEntry(entry);
                    s.Write(documentData.ScanCastCert, 0, documentData.ScanCastCert.Length);
                }

                if (documentData.ScanID != null)
                {
                    ZipEntry entry = new ZipEntry(documentData.ScanIDFileName);
                    entry.DateTime = DateTime.Now;
                    entry.IsUnicodeText = true;
                    s.PutNextEntry(entry);
                    s.Write(documentData.ScanID, 0, documentData.ScanID.Length);
                }

                if (documentData.BankVerifiedSignature != null)
                {
                    ZipEntry entry = new ZipEntry(documentData.BankVerifiedSignatureFileName);
                    entry.DateTime = DateTime.Now;
                    entry.IsUnicodeText = true;
                    s.PutNextEntry(entry);
                    s.Write(documentData.BankVerifiedSignature, 0, documentData.BankVerifiedSignature.Length);
                }

                if (documentData.DocProofForIndustrialEstablishedOutsideGida != null)
                {
                    ZipEntry entry = new ZipEntry(documentData.DocProofForIndustrialEstablishedOutsideGidaFileName);
                    entry.DateTime = DateTime.Now;
                    entry.IsUnicodeText = true;
                    s.PutNextEntry(entry);
                    s.Write(documentData.DocProofForIndustrialEstablishedOutsideGida, 0, documentData.DocProofForIndustrialEstablishedOutsideGida.Length);
                }

                if (documentData.ApplicantPhoto != null)
                {
                    ZipEntry entry = new ZipEntry(documentData.ApplicantPhotoFileName);
                    entry.DateTime = DateTime.Now;
                    entry.IsUnicodeText = true;
                    s.PutNextEntry(entry);
                    s.Write(documentData.ApplicantPhoto, 0, documentData.ApplicantPhoto.Length);
                }

                if (documentData.ApplicantSignature != null)
                {
                    ZipEntry entry = new ZipEntry(documentData.ApplicantSignatureFileName);
                    entry.DateTime = DateTime.Now;
                    entry.IsUnicodeText = true;
                    s.PutNextEntry(entry);
                    s.Write(documentData.ApplicantSignature, 0, documentData.ApplicantSignature.Length);
                }

                s.Finish();
                s.Flush();
                s.Close();
            }
            byte[] finalResult = System.IO.File.ReadAllBytes(tempOutPutPath);
            if (System.IO.File.Exists(tempOutPutPath))
                System.IO.File.Delete(tempOutPutPath);

            if (finalResult == null || !finalResult.Any())
                throw new Exception(String.Format("No Files found with Image"));
            return File(finalResult, "application/zip", fileName);
        }

        public ActionResult AddUser()
        {
            AdminDetails _details = new AdminDetails();
            ViewData["UserData"] = _details.GetGidaUsers();
            return View();
        }

        [HttpPost]
        public JsonResult GetGidaUserList()
        {
            AdminDetails _details = new AdminDetails();
            var usrs = _details.GetGidaUsers().Where(x => x.IsActive == true).ToList();
            return Json(usrs, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveNewUser(string userId, string username, string name, string Role, string email, string mobileNumber, string Designation, string Department, string active)
        {
            GidaUser user = new GidaUser();
            user.Id = !string.IsNullOrEmpty(userId) ? Convert.ToInt32(userId) : 0;
            user.UserName = username;
            user.Name = name;
            user.Email = email;
            user.MobileNo = mobileNumber;
            user.Designation = Convert.ToInt32(Designation);
            user.Department = Convert.ToInt32(Department);
            user.CreatedBy = UserData.UserId;
            user.CreatedDate = DateTime.Now;
            user.IsActive = active == "on" ? true : false;
            user.Password = ConfigurationManager.AppSettings["GidaUserPassword"].ToString();
            user.UserRoleId = Convert.ToInt32(Role);

            AdminDetails _details = new AdminDetails();
            var result = _details.SaveGidaUser(user);
            if (result == Enums.CrudStatus.Saved)
            {
                SetAlertMessage("User created", "Save User");
                SendMailForGidaUserCreation(name, email, username, user.Password);
            }
            else
                SetAlertMessage("User creation failed", "Save User");
            return RedirectToAction("AddUser");

        }

        private async Task SendMailForGidaUserCreation(string fullName, string email, string userName, string password)
        {
            await Task.Run(() =>
            {
                //Send Email
                Message msg = new Message()
                {
                    MessageTo = email,
                    MessageNameTo = fullName,
                    Subject = "Gida User Created",
                    Body = EmailHelper.GetGidaUserCreationSuccessEmail(fullName, userName, password)
                };
                ISendMessageStrategy sendMessageStrategy = new SendMessageStrategyForEmail(msg);
                sendMessageStrategy.SendMessages();
            });
        }

        [HttpPost]
        public JsonResult GetUserDetail(int userId)
        {
            AdminDetails _details = new AdminDetails();
            var result = _details.GetUserDetail(userId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddPageMaster()
        {
            AdminDetails _details = new AdminDetails();
            ViewData["PageData"] = _details.GePageMaster();
            return View();
        }
        [HttpPost]
        public ActionResult SavePageMaster(string PageId, string pagename, string active)
        {
            PageMaster pages = new PageMaster();
            pages.Id = !string.IsNullOrEmpty(PageId) ? Convert.ToInt32(PageId) : 0;
            pages.PageName = pagename;
            pages.CreatedBy = UserData.UserId;
            pages.CreatedDate = DateTime.Now;
            pages.IsActive = active == "on" ? true : false;
            AdminDetails _details = new AdminDetails();
            var result = _details.SavePageMaster(pages);
            if (result == Enums.CrudStatus.Saved)
            {
                SetAlertMessage("Page created", "Save Page master");
            }
            else
                SetAlertMessage("Page creation failed", "Save Page master");
            return RedirectToAction("AddPageMaster");

        }

        [HttpPost]
        public JsonResult GetPageDetail(int PageId)
        {
            AdminDetails _details = new AdminDetails();
            var result = _details.GetPageDetail(PageId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult RoleUserMaster()
        {
            MasterDetails _details = new MasterDetails();
            ViewData["PermissionType"] = _details.GetLookupDetail(null, "PermissionType").OrderBy(x => x.LookupId).ToList();
            return View();
        }

        [HttpPost]
        public JsonResult GetLookupDetail(int? lookupTypeId, string lookupType, bool active = true)
        {
            MasterDetails _details = new MasterDetails();
            if (lookupTypeId == 0)
            {
                lookupTypeId = null;
            }
            return Json(_details.GetLookupDetail(lookupTypeId, lookupType, active).OrderBy(x => x.LookupId).ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetRoleWisePermission(int departmentId, int designationId, int roleId)
        {
            AdminDetails _details = new AdminDetails();
            var usrs = _details.GetRoleWisePermission(departmentId, designationId, roleId);
            return Json(usrs, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveUserPermission(List<RoleWisePermission> permissionList)
        {
            AdminDetails _details = new AdminDetails();
            var result = _details.SaveUserPermission(permissionList);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CreatePlotMaster()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SavePlotMaster(List<PlotMaster> plotMaster)
        {
            AdminDetails _details = new AdminDetails();
            var result = _details.SavePlotMaster(plotMaster);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetPlotMasterDetail(int SchemeType, int SchemeName, int SectorId)
        {
            AdminDetails _details = new AdminDetails();
            return Json(_details.GetPlotMasterDetail(SchemeType, SchemeName, SectorId), JsonRequestBehavior.AllowGet);
        }

        public ActionResult SchemeWiseApplicantList()
        {
            return View();
        }

        public ActionResult ApplicationStatusPA(int applicationId, int schemeName)
        {
            AdminDetails _details = new AdminDetails();
            var data = _details.GetApplicantUserDetail(schemeName).Where(x => x.ApplicationId == applicationId).FirstOrDefault();
            ViewData["UserDetail"] = data;
            return View();
        }
        public JsonResult ApprovePayment(int applicationId, int schemeName, string status, string comment)
        {
            AdminDetails _details = new AdminDetails();
            return Json(_details.ApproveRejectPayment(applicationId, status, comment), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult RejectPayment(int applicationId, int schemeName, string status, string comment)
        {
            AdminDetails _details = new AdminDetails();
            return Json(_details.ApproveRejectPayment(applicationId, status, comment), JsonRequestBehavior.AllowGet);

        }
        public JsonResult ApproveDocument(int applicationId, int schemeName, string status, string comment)
        {
            AdminDetails _details = new AdminDetails();
            return Json(_details.ApproveRejectDocument(applicationId, status, comment), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult RejectDocument(int applicationId, int schemeName, string status, string comment)
        {
            AdminDetails _details = new AdminDetails();
            return Json(_details.ApproveRejectDocument(applicationId, status, comment), JsonRequestBehavior.AllowGet);
        }
        public JsonResult AdminApproveRejectApplication(int applicationId, int schemeName, string status, string comment)
        {
            AdminDetails _details = new AdminDetails();
            return Json(_details.ApproveRejectApplication(applicationId, status, comment), JsonRequestBehavior.AllowGet);
        }
        public ActionResult CandidateListForInterview()
        {
            AdminDetails _details = new AdminDetails();
            var data = _details.GetApplicantSubmittedForInterview(null).Where(x => x.InterviewLetterStatus == null || x.InterviewLetterStatus == "").ToList();
            ViewData["UserDetail"] = data;
            return View();
        }

        public ActionResult SendMailtoApplicantForInterview(int userid)
        {
            AdminDetails _details = new AdminDetails();
            var data = _details.GetApplicantSubmittedForInterview(null).Where(x => x.UserId == userid).FirstOrDefault();
            _details.ApplicationInvitationLetterStatusChnage(userid, "InvitationSentForInterview", null);
            this.SendMailToApplicantInterview(data.FullName, data.Email, data.InterviewDateTime);
            SetAlertMessage("Email Send", "Email Send for Interview Invitation send.");
            return RedirectToAction("CandidateListForInterview");
        }

        private async Task SendMailToApplicantInterview(string fullName, string email, string InterviewDateTime)
        {
            await Task.Run(() =>
            {
                //Send Email
                Message msg = new Message()
                {
                    MessageTo = email,
                    MessageNameTo = fullName,
                    Subject = "Interview Schedule",
                    Body = EmailHelper.GetINterviewScheduleMailEmail(fullName, InterviewDateTime)
                };
                ISendMessageStrategy sendMessageStrategy = new SendMessageStrategyForEmail(msg);
                sendMessageStrategy.SendMessages();
            });
        }
        public ActionResult CandidateListForAllotment()
        {
            AdminDetails _details = new AdminDetails();
            var data = _details.GetApplicantSubmittedForInterview(null).Where(x => x.InterviewLetterStatus == "InvitationSentForInterview").ToList();
            ViewData["UserDetail"] = data;
            return View();
        }

        [HttpPost]
        public JsonResult SendMailtoApplicantForInterviewResult(int userId, string status, int plotId)
        {
            AdminDetails _details = new AdminDetails();
            var data = _details.GetApplicantSubmittedForInterview(null).Where(x => x.UserId == userId).FirstOrDefault();
            _details.ApplicationInvitationLetterStatusChnage(userId, status, plotId);
            this.SendMailToApplicantInterviewResult(data.FullName, data.Email, status);
            SetAlertMessage("Email Send", "Email Send for Interview Invitation send.");
            //return RedirectToAction("CandidateListForAllotment");
            return Json(-1, JsonRequestBehavior.AllowGet);
        }

        private async Task SendMailToApplicantInterviewResult(string fullName, string email, string result)
        {
            await Task.Run(() =>
            {
                //Send Email
                Message msg = new Message()
                {
                    MessageTo = email,
                    MessageNameTo = fullName,
                    Subject = "Interview Schedule",
                    Body = EmailHelper.GetINterviewScheduleResultMailEmail(fullName, result)
                };
                ISendMessageStrategy sendMessageStrategy = new SendMessageStrategyForEmail(msg);
                sendMessageStrategy.SendMessages();
            });
        }
        public ActionResult SchemeWiseAllotmentList()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetSchemeWiseAllotmentList(int schemeName)
        {
            AdminDetails _details = new AdminDetails();
            var data = _details.GetApplicantSubmittedForInterview(schemeName).Where(x => x.InterviewLetterStatus == "Selected").ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AllotmentStatus(int applicationId)
        {
            AdminDetails _details = new AdminDetails();
            var data = _details.GetApplicantUserDetailByApplicationId(applicationId);
            ViewData["ApplicantData"] = data;
            return View();
        }
        public ActionResult PrintAllotmentLetter(int applicationId)
        {
            AdminDetails _details = new AdminDetails();
            var data = _details.GetApplicantUserDetailByApplicationId(applicationId);
            ViewData["ApplicantData"] = data;
            return View();
        }
        public ActionResult IndustrialDashboard()
        {
            return View();
        }
        public ActionResult Invitation(int applicationId, int schemeName)
        {
            ViewData["applicationId"] = applicationId;
            ViewData["schemeName"] = schemeName;
            return View();
        }

        [HttpPost]
        public JsonResult GetApplicantInvitation(int applicationId, int schemeName)
        {
            AdminDetails _details = new AdminDetails();
            var data = _details.GetApplicantUserDetail(schemeName).Where(x => x.ApplicationId == applicationId).FirstOrDefault();
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetInvitationList()
        {
            InvitationDetails _details = new InvitationDetails();
            var usrs = _details.GetInvApplicant();
            return Json(usrs, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetSelectedInvitationDetail(int UserId)
        {
            InvitationDetails _details = new InvitationDetails();
            var usrs = _details.GetInvApplicantDetails(UserId);
            return Json(usrs, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult SaveNewInvitation(string Id, string applicationId, string Applicant, string Address, string SectorName, string ApplicationNo, string PlotRange, string InterviewDateTime, string InterviewMode)
        {
            ApplicantInvitationLetter invt = new ApplicantInvitationLetter();
            invt.Id = !string.IsNullOrEmpty(Id) ? Convert.ToInt32(Id) : 0;
            invt.UserId = Convert.ToInt32(Applicant);
            invt.ApplicationId = Convert.ToInt32(applicationId);
            invt.ApplicationNo = ApplicationNo;
            invt.ApplicantAddress = Address;
            invt.SectorName = SectorName;
            invt.PlotRange = PlotRange;
            //invt.PlotId = Convert.ToInt32(PlotId);
            invt.InterviewDateTime = Convert.ToDateTime(InterviewDateTime);
            invt.InterviewMode = InterviewMode;

            //user.CreatedBy = UserData.UserId;
            //user.CreatedDate = DateTime.Now;
            //user.IsActive = active == "on" ? true : false;
            //user.Password = ConfigurationManager.AppSettings["GidaUserPassword"].ToString();
            //user.UserType = "user";

            InvitationDetails _details = new InvitationDetails();
            var result = _details.SaveInvitation(invt);
            if (result == Enums.CrudStatus.Saved)
            {
                SetAlertMessage("Invitation created", "Save Invitation");
                //SendMailForGidaUserCreation(name, email, username, user.Password);
            }
            else
                SetAlertMessage("User creation failed", "Save User");
            return RedirectToAction("CandidateListForInterview");


        }
        [HttpGet]
        public FileResult DownloadApplicantDocument(string applicationId, string documentName)
        {
            AdminDetails detail = new AdminDetails();
            var documentData = detail.GetDocumentByApplciationId(Convert.ToInt32(applicationId));
            if (documentName == DocumentName.ApplicantEduTechQualification.ToString())
            {
                byte[] bytes = documentData.ApplicantEduTechQualification;
                return File(bytes, System.Net.Mime.MediaTypeNames.Application.Octet, documentData.ApplicantEduTechQualificationFileName);
            }
            else if (documentName == DocumentName.ApplicantPhoto.ToString())
            {
                byte[] bytes = documentData.ApplicantPhoto;
                return File(bytes, System.Net.Mime.MediaTypeNames.Application.Octet, documentData.ApplicantPhotoFileName);
            }
            else if (documentName == DocumentName.ApplicantSignature.ToString())
            {
                byte[] bytes = documentData.ApplicantSignature;
                return File(bytes, System.Net.Mime.MediaTypeNames.Application.Octet, documentData.ApplicantSignatureFileName);
            }
            else if (documentName == DocumentName.BalanceSheet.ToString())
            {
                byte[] bytes = documentData.BalanceSheet;
                return File(bytes, System.Net.Mime.MediaTypeNames.Application.Octet, documentData.BalanceSheetFileName);
            }
            else if (documentName == DocumentName.BankVerifiedSignature.ToString())
            {
                byte[] bytes = documentData.BankVerifiedSignature;
                return File(bytes, System.Net.Mime.MediaTypeNames.Application.Octet, documentData.BankVerifiedSignatureFileName);
            }
            else if (documentName == DocumentName.DocProofForIndustrialEstablishedOutsideGida.ToString())
            {
                byte[] bytes = documentData.DocProofForIndustrialEstablishedOutsideGida;
                return File(bytes, System.Net.Mime.MediaTypeNames.Application.Octet, documentData.DocProofForIndustrialEstablishedOutsideGidaFileName);
            }
            else if (documentName == DocumentName.ExperienceProof.ToString())
            {
                byte[] bytes = documentData.ExperienceProof;
                return File(bytes, System.Net.Mime.MediaTypeNames.Application.Octet, documentData.ExperienceProofFileName);
            }
            else if (documentName == DocumentName.FinDetailsEstablishedIndustries.ToString())
            {
                byte[] bytes = documentData.FinDetailsEstablishedIndustries;
                return File(bytes, System.Net.Mime.MediaTypeNames.Application.Octet, documentData.FinDetailsEstablishedIndustriesFileName);
            }
            else if (documentName == DocumentName.ITReturn.ToString())
            {
                byte[] bytes = documentData.ITReturn;
                return File(bytes, System.Net.Mime.MediaTypeNames.Application.Octet, documentData.ITReturnFileName);
            }
            else if (documentName == DocumentName.Memorendum.ToString())
            {
                byte[] bytes = documentData.Memorendum;
                return File(bytes, System.Net.Mime.MediaTypeNames.Application.Octet, documentData.MemorendumFileName);
            }
            else if (documentName == DocumentName.OtherDocForProposedIndustry.ToString())
            {
                byte[] bytes = documentData.OtherDocForProposedIndustry;
                return File(bytes, System.Net.Mime.MediaTypeNames.Application.Octet, documentData.OtherDocForProposedIndustryFileName);
            }
            else if (documentName == DocumentName.PreEstablishedIndustriesDoc.ToString())
            {
                byte[] bytes = documentData.PreEstablishedIndustriesDoc;
                return File(bytes, System.Net.Mime.MediaTypeNames.Application.Octet, documentData.PreEstablishedIndustriesDocFileName);
            }
            else if (documentName == DocumentName.ProjectReport.ToString())
            {
                byte[] bytes = documentData.ProjectReport;
                return File(bytes, System.Net.Mime.MediaTypeNames.Application.Octet, documentData.ProjectReportFileName);
            }
            else if (documentName == DocumentName.ProposedPlanLandUses.ToString())
            {
                byte[] bytes = documentData.ProposedPlanLandUses;
                return File(bytes, System.Net.Mime.MediaTypeNames.Application.Octet, documentData.ProposedPlanLandUsesFileName);
            }
            else if (documentName == DocumentName.ScanAddressProof.ToString())
            {
                byte[] bytes = documentData.ScanAddressProof;
                return File(bytes, System.Net.Mime.MediaTypeNames.Application.Octet, documentData.ScanAddressProofFileName);
            }
            else if (documentName == DocumentName.ScanCastCert.ToString())
            {
                byte[] bytes = documentData.ScanCastCert;
                return File(bytes, System.Net.Mime.MediaTypeNames.Application.Octet, documentData.ScanCastCertFileName);
            }
            else if (documentName == DocumentName.ScanID.ToString())
            {
                byte[] bytes = documentData.ScanID;
                return File(bytes, System.Net.Mime.MediaTypeNames.Application.Octet, documentData.ScanIDFileName);
            }
            else if (documentName == DocumentName.ScanPAN.ToString())
            {
                byte[] bytes = documentData.ScanPAN;
                return File(bytes, System.Net.Mime.MediaTypeNames.Application.Octet, documentData.ScanPANFileName);
            }
            return null;
        }
        public ActionResult LeasedDutyWork()
        {
            return View();
        }
        public ActionResult SaveNewLeasedDutyWork(string Id, string ApplicantionId, string AllotmentMoneyPaid, string BankGauranteeChallanNo, string PurchasedStampValue, string StampValueForBankGaurantee, string BankGauranteeDate, string EntityNameBehalfOfApplicant)
        {
            LeaseDeedDetail LeasedDutyWork1 = new LeaseDeedDetail();
            LeasedDutyWork1.Id = !string.IsNullOrEmpty(Id) ? Convert.ToInt32(Id) : 0;
            LeasedDutyWork1.ApplicationId = Convert.ToInt32(ApplicantionId);
            LeasedDutyWork1.AllotmentMoneyPaid = Convert.ToDecimal(AllotmentMoneyPaid);
            LeasedDutyWork1.BankGauranteeChallanNo = BankGauranteeChallanNo;
            LeasedDutyWork1.PurchasedStampValue = Convert.ToDecimal(PurchasedStampValue);
            LeasedDutyWork1.StampValueForBankGaurantee = Convert.ToDecimal(StampValueForBankGaurantee);
            LeasedDutyWork1.BankGauranteeDate = Convert.ToDateTime(BankGauranteeDate);
            LeasedDutyWork1.EntityNameBehalfOfApplicant = EntityNameBehalfOfApplicant;
            LeasedDutyWork1.CreatedDate = DateTime.Now;

            //user.CreatedBy = UserData.UserId;
            //user.CreatedDate = DateTime.Now;
            //user.IsActive = active == "on" ? true : false;
            //user.Password = ConfigurationManager.AppSettings["GidaUserPassword"].ToString();
            //user.UserType = "user";

            //LeasedDutyDetails _details = new LeasedDutyDetails();
            LeasedDeedDutyWorkDetails _details = new LeasedDeedDutyWorkDetails();
            var result = _details.SaveLeasedDutyWork(LeasedDutyWork1);
            if (result == Enums.CrudStatus.Saved)
            {
                SetAlertMessage("Leased Duty Work created", "Save Leased Duty Work");
                //SendMailForGidaUserCreation(name, email, username, user.Password);
            }
            else
                SetAlertMessage("Leased Duty Work creation failed", "Save Leased Duty Work");
            return RedirectToAction("LeasedDutyWork");


        }
        public ActionResult SchemeWiseInvitationList()
        {
            return View();
        }


        public ActionResult AllocateAllotmentLetter(int applicationId)
        {
            ViewData["ApplicationId"] = applicationId;
            return View();
        }
        [HttpPost]
        public JsonResult GetApplicantUserDetailByApplicationId(int applicationId)
        {
            AdminDetails _details = new AdminDetails();
            var data = _details.GetApplicantUserDetailByApplicationId(applicationId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult SaveAllocateAllotmentLetter(HttpPostedFileBase Document, string applicationId, string AllotmentNumber,
            string Allotmentdate, string InterviewStartDate, string InterviewEndDate, string AllotmentLetterDate)
        {
            AllocateAllotmentDetail allotementDetail = new AllocateAllotmentDetail();
            if (Document != null && Document.ContentLength > 0)
            {
                allotementDetail.CEO_Sign = new byte[Document.ContentLength];
                Document.InputStream.Read(allotementDetail.CEO_Sign, 0, Document.ContentLength);
                allotementDetail.CEO_SignFileName = Document.FileName;
                allotementDetail.CEO_SignFileType = Document.ContentType;
            }
            allotementDetail.ApplicationId = Convert.ToInt32(applicationId);
            allotementDetail.AllotmentNumber = AllotmentNumber;
            if (Allotmentdate != "")
                allotementDetail.AllotmentDate = Convert.ToDateTime(Allotmentdate);
            if (InterviewStartDate != "")
                allotementDetail.StartingDateofInterview_L = Convert.ToDateTime(InterviewStartDate);
            if (InterviewEndDate != "")
                allotementDetail.EndDateofInterview_L = Convert.ToDateTime(InterviewEndDate);
            if (AllotmentLetterDate != "")
                allotementDetail.DateofAllotmentLetter = Convert.ToDateTime(AllotmentLetterDate);

            AdminDetails _details = new AdminDetails();
            var result = _details.SaveAllocateAllotmentLetter(allotementDetail);
            if (result == Enums.CrudStatus.Saved)
                SetAlertMessage("Allotment Letter has been Saved", "Allotment Letter Save");
            else
                SetAlertMessage("Allotment Letter Saving failed", "Allotment Letter Save");
            return RedirectToAction("AllotmentStatus", new { applicationId = applicationId });

        }
        public ActionResult SchemeTermsAndCondition()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SaveSchemeWiseTermAndCondition(string Id, string SchemeName, string InterestDueDate, string AllotementDueDate, string FirstInstallmentDueDate, string SecondInstallmentDueDate, string ThirdInstallmentDueDate)
        {
            SchemewiseTermsCondition termnCondition = new SchemewiseTermsCondition();
            termnCondition.SchemeName = Convert.ToInt32(SchemeName);
            if (InterestDueDate != "")
                termnCondition.Firstduedateofpaymentofinterest = Convert.ToDateTime(InterestDueDate);
            if (AllotementDueDate != "")
                termnCondition.AllotmentMoneyDueDate = Convert.ToDateTime(AllotementDueDate);
            if (FirstInstallmentDueDate != "")
                termnCondition.DateofgivingfirstInstallment = Convert.ToDateTime(FirstInstallmentDueDate);
            if (SecondInstallmentDueDate != "")
                termnCondition.DateofgivingsecondInstallment = Convert.ToDateTime(SecondInstallmentDueDate);
            if (ThirdInstallmentDueDate != "")
                termnCondition.DateofgivingthirdInstallent = Convert.ToDateTime(ThirdInstallmentDueDate);
            if (!string.IsNullOrEmpty(Id))
                termnCondition.Id = Convert.ToInt32(Id);

            AdminDetails _details = new AdminDetails();
            var result = _details.SaveTermAndCondition(termnCondition);
            if (result == Enums.CrudStatus.Saved)
                SetAlertMessage("Term and Condition has been Saved", "Term and Condition Save");
            else
                SetAlertMessage("Term and Condition Saving failed", "Term and Condition Save");
            return RedirectToAction("SchemeTermsAndCondition");

        }

        [HttpPost]
        public JsonResult GetPlotNumber()
        {
            AdminDetails _details = new AdminDetails();
            return Json(_details.GetPlotNumber(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult AllotmentMoneyWithInstallment(int applicationId)
        {
            ViewData["ApplicationId"] = applicationId;
            AdminDetails _details = new AdminDetails();
            var data = _details.GetApplicantUserDetailByApplicationId(applicationId);
            ViewData["UserDetail"] = data;
            return View();
        }
        [HttpPost]
        public JsonResult GetApprovedApplicantDetail(int schemeName)
        {
            AdminDetails _details = new AdminDetails();
            var data = _details.GetApplicantUserDetail(schemeName).Where(x => !string.IsNullOrEmpty(x.CEOPaymentStatus)
                                            && !string.IsNullOrEmpty(x.CEODocumentStatus)
                                            && !x.CEOPaymentStatus.Contains("Not Approved")
                                            && !x.CEODocumentStatus.Contains("Not Approved")).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public FileResult ExportSchemeWiseInvitationList(string GridHtml)
        {
            using (MemoryStream stream = new System.IO.MemoryStream())
            {
                StringReader sr = new StringReader(GridHtml);
                Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 100f, 0f);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                pdfDoc.Open();
                XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                pdfDoc.Close();
                return File(stream.ToArray(), "application/pdf", "SchemeWiseInvitationList.pdf");
            }
        }
        public ActionResult LeaseddeedNotesheet()
        {

            return View();
        }
        [HttpPost]
        public JsonResult GetSelectedLeaseddeedAppDetail(int ApplicationId)
        {
            LeasedeedNotesheetDetails _details = new LeasedeedNotesheetDetails();
            var usrs = _details.GetLeesdeedApplicantDetails(ApplicationId);
            return Json(usrs, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LeasedeedStatus(int applicationId)
        {
            AdminDetails _details = new AdminDetails();
            var data = _details.GetApplicantUserDetailByApplicationId(applicationId);
            ViewData["UserDetail"] = data;
            return View();
        }
        public ActionResult AllotteeListForLeasedeed()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetAllotteeListForLeasedeed(int schemeName)
        {
            AdminDetails _details = new AdminDetails();
            var data = _details.GetApplicantUserDetail(schemeName).Where(x => !string.IsNullOrEmpty(x.AllotmentNumber) && string.IsNullOrEmpty(x.AllotmentTransactionAmount)).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SendAllotmentEmailToApplicant(int applicationId)
        {
            AdminDetails _details = new AdminDetails();
            var data = _details.GetApplicantUserDetailByApplicationId(applicationId);
            string baseUrl = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            var encryptedApplicationId = CryptoEngine.Encrypt(data.ApplicationId.ToString());
            this.SendMailToApplicantAllotmentMoney(data.FullName, data.Email, data.AllotementMoneyTobePaid, data.AllotmentDate, encryptedApplicationId, baseUrl);
            SetAlertMessage("Email Send", "Pay Allotement Money Reminder send.");
            return RedirectToAction("AllotmentMoneyWithInstallment", new { applicationId = applicationId });
        }
        private async Task SendMailToApplicantAllotmentMoney(string fullName, string email, string allotmentMoney, DateTime? AllotmentDate, string ApplicationId, string baseUrl)
        {
            await Task.Run(() =>
            {
                //Send Email
                string payAllotementURL = "Applicant/PayAllotementMoney?Id=" + ApplicationId;
                Message msg = new Message()
                {
                    MessageTo = email,
                    MessageNameTo = fullName,
                    Subject = "Pay Allotement Money Reminder",
                    Body = EmailHelper.GetAllotmentMoneySendEmail(fullName, allotmentMoney, AllotmentDate, baseUrl + payAllotementURL)
                };
                ISendMessageStrategy sendMessageStrategy = new SendMessageStrategyForEmail(msg);
                sendMessageStrategy.SendMessages();
            });
        }
        [HttpPost]
        public ActionResult SaveLeaseddeedNotesheet(string Id, HttpPostedFileBase Sfile, HttpPostedFileBase Mfile, HttpPostedFileBase AMfile, string BankGDate, string ApplicationId, string BankAddress, string Comments)
        {
            LeasdeedNotesheet Lnotesheet = new LeasdeedNotesheet();
            Lnotesheet.Id = !string.IsNullOrEmpty(Id) ? Convert.ToInt32(Id) : 0;
            if (UserData.Designation == "Assistant Manager")
            {
                if (Sfile != null && Sfile.ContentLength > 0)
                {
                    Lnotesheet.Digsign_Propassist = new byte[Sfile.ContentLength];
                    Sfile.InputStream.Read(Lnotesheet.Digsign_Propassist, 0, Sfile.ContentLength);
                    Lnotesheet.Docname_Propassist = Sfile.FileName;
                    Lnotesheet.Doctype_Propassist = Sfile.ContentType;
                }
                Lnotesheet.Comment_Propassist = Comments;
            }
            if (UserData.Designation == "General Manager")
            {
                if (Mfile != null && Mfile.ContentLength > 0)
                {
                    Lnotesheet.Digsign_Manager = new byte[Sfile.ContentLength];
                    Sfile.InputStream.Read(Lnotesheet.Digsign_Manager, 0, Sfile.ContentLength);
                    Lnotesheet.Docname_Digsignmanager = Mfile.FileName;
                    Lnotesheet.Doctype_Digsignmanager = Mfile.ContentType;
                }
                Lnotesheet.Comment_Manager = Comments;
            }
            if (UserData.Designation == "Manager")
            {
                if (AMfile != null && AMfile.ContentLength > 0)
                {
                    Lnotesheet.Digsign_FManager = new byte[Sfile.ContentLength];
                    Sfile.InputStream.Read(Lnotesheet.Digsign_FManager, 0, Sfile.ContentLength);
                    Lnotesheet.Docname_FManager = AMfile.FileName;
                    Lnotesheet.Doctype_FManager = AMfile.ContentType;
                }
                Lnotesheet.Doctype_FManager = Comments;
            }


            Lnotesheet.ApplicationId = ApplicationId;
            Lnotesheet.Bankaddress = BankAddress;
            Lnotesheet.ApplicantId = 55;
            Lnotesheet.Allotmentnumber = "10";



            if (BankGDate != "")
            {
                Lnotesheet.Bankguranteedate = Convert.ToDateTime(BankGDate);

            }

            Lnotesheet.CreatedBy = UserData.UserId;
            Lnotesheet.Createddate = DateTime.Now;




            LeasedeedNotesheetDetails _details = new LeasedeedNotesheetDetails();
            var result = _details.SaveLeasedeedNotesheet(Lnotesheet);
            if (result == Enums.CrudStatus.Saved)
                SetAlertMessage("Leaseddeed Notesheet has been Saved", "Leaseddeed Notesheet Save");
            else
                SetAlertMessage("Leaseddeed Notesheet Saving failed", "Leaseddeed Notesheet Save");

            return RedirectToAction("LeaseddeedNotesheet");

        }

        public ActionResult DataForAllotmentNotesheet(int applicationId)
        {
            AdminDetails _details = new AdminDetails();
            var data = _details.GetApplicantUserDetailByApplicationId(applicationId);
            ViewData["ApplicantData"] = data;
            return View();
        }

        public ActionResult PrintAllotmentNotesheet(int applicationId)
        {
            AdminDetails _details = new AdminDetails();
            var data = _details.GetApplicantUserDetailByApplicationId(applicationId);
            ViewData["ApplicantData"] = data;
            return View();
        }

        [HttpPost]
        public ActionResult SaveAllotmentNotesheet(int ApplicationId, string UserComment, string CEOSigningDate,
            HttpPostedFileBase CEOSignInFile,
            HttpPostedFileBase AssistantFile,
            HttpPostedFileBase ManagepropertyFile,
            HttpPostedFileBase SIFile,
            HttpPostedFileBase GMFinanceFile,
            HttpPostedFileBase ACEOFile
            )
        {
            AllotementNotesheetDetail notesheet = new AllotementNotesheetDetail();
            notesheet.ApplicationId = ApplicationId;
            if (UserData.Department == "Property" && UserData.Designation == "Clerk")
            {
                notesheet.AssistantComment = UserComment;
            }
            else if (UserData.Department == "Property" && UserData.Designation == "Manager")
            {
                notesheet.ManagerPropertyComment = UserComment;
            }
            else if (UserData.Department == "Property" && UserData.Designation == "Section Incharge")
            {
                notesheet.SectionInchargeComment = UserComment;
            }
            else if (UserData.Department == "Finance & Accounts" && UserData.Designation == "General Manager")
            {
                notesheet.GMFinanceComment = UserComment;
            }
            else if (UserData.Department == "Administration" && UserData.Designation == "ACEO")
            {
                notesheet.ACEOComment = UserComment;
            }
            else if (UserData.Department == "Administration" && UserData.Designation == "CEO")
            {
                notesheet.CEOComment = UserComment;
                if (CEOSigningDate != "")
                    notesheet.DateoOfSigningByCEO = Convert.ToDateTime(CEOSigningDate);
            }

            if (CEOSignInFile != null && CEOSignInFile.ContentLength > 0)
            {
                notesheet.DigiSignByCEO = new byte[CEOSignInFile.ContentLength];
                CEOSignInFile.InputStream.Read(notesheet.DigiSignByCEO, 0, CEOSignInFile.ContentLength);
                notesheet.DigiSignByCEOFileName = CEOSignInFile.FileName;
                notesheet.DigiSignByCEOFileType = CEOSignInFile.ContentType;
            }
            if (AssistantFile != null && AssistantFile.ContentLength > 0)
            {
                notesheet.DigiSignByAssistant = new byte[AssistantFile.ContentLength];
                AssistantFile.InputStream.Read(notesheet.DigiSignByAssistant, 0, AssistantFile.ContentLength);
                notesheet.DigiSignByAssistantFileName = AssistantFile.FileName;
                notesheet.DigiSignByAssistantFileType = AssistantFile.ContentType;
            }
            if (ManagepropertyFile != null && ManagepropertyFile.ContentLength > 0)
            {
                notesheet.DigiSignByManagerProperty = new byte[ManagepropertyFile.ContentLength];
                ManagepropertyFile.InputStream.Read(notesheet.DigiSignByManagerProperty, 0, ManagepropertyFile.ContentLength);
                notesheet.DigiSignByManagerPropertyFileName = ManagepropertyFile.FileName;
                notesheet.DigiSignByManagerPropertyFileType = ManagepropertyFile.ContentType;
            }
            if (SIFile != null && SIFile.ContentLength > 0)
            {
                notesheet.DigiSignBySectionIncharge = new byte[SIFile.ContentLength];
                SIFile.InputStream.Read(notesheet.DigiSignBySectionIncharge, 0, SIFile.ContentLength);
                notesheet.DigiSignBySectionInchargeFileName = SIFile.FileName;
                notesheet.DigiSignBySectionInchargeFileType = SIFile.ContentType;
            }
            if (GMFinanceFile != null && GMFinanceFile.ContentLength > 0)
            {
                notesheet.DigiSignByGMFinance = new byte[GMFinanceFile.ContentLength];
                GMFinanceFile.InputStream.Read(notesheet.DigiSignByGMFinance, 0, GMFinanceFile.ContentLength);
                notesheet.DigiSignByGMFinanceFileName = GMFinanceFile.FileName;
                notesheet.DigiSignByGMFinanceFileType = GMFinanceFile.ContentType;
            }
            if (ACEOFile != null && ACEOFile.ContentLength > 0)
            {
                notesheet.DigiSignByACEO = new byte[ACEOFile.ContentLength];
                ACEOFile.InputStream.Read(notesheet.DigiSignByACEO, 0, ACEOFile.ContentLength);
                notesheet.DigiSignByACEOFileName = ACEOFile.FileName;
                notesheet.DigiSignByACEOFileType = ACEOFile.ContentType;
            }

            AdminDetails _details = new AdminDetails();
            var result = _details.SaveAllotementNotesheet(notesheet);
            if (result == Enums.CrudStatus.Saved)
                SetAlertMessage("Allotement Notesheet has been Saved", "Allotement Notesheet");
            else
                SetAlertMessage("Allotement Notesheet save failed", "Allotement Notesheet");
            return RedirectToAction("DataForAllotmentNotesheet", new { applicationId = ApplicationId });
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            Session.Clear();
            return RedirectToAction("AdminLogin", "Login");
        }

    }

}