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
            var data = _details.GetApplicantUserDetail(schemeName).Where(x => x.PaidAmount != "").ToList();
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
        public ActionResult ApprovePayment(int applicationId, int schemeName, string status)
        {
            AdminDetails _details = new AdminDetails();
            var data = _details.ApproveRejectPayment(applicationId, status);
            SetAlertMessage("Payment is " + status, "Payment detail staus");
            return RedirectToAction("ApplicationStatusPA", new { applicationId = applicationId, schemeName = schemeName });
        }
        [HttpPost]
        public JsonResult RejectPayment(int applicationId, string status, string comment)
        {
            if (!string.IsNullOrEmpty(comment))
            {
                AdminDetails _details = new AdminDetails();
                var data = _details.ApproveRejectPayment(applicationId, status, comment);
                SetAlertMessage("Payment is " + status, "Payment detail staus");
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(-1, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult ApproveDocument(int applicationId, int schemeName, string status)
        {
            AdminDetails _details = new AdminDetails();
            var data = _details.ApproveRejectDocument(applicationId, status);
            SetAlertMessage("Document is " + status, "document detail staus");
            return RedirectToAction("ApplicationStatusPA", new { applicationId = applicationId, schemeName = schemeName });
        }
        [HttpPost]
        public JsonResult RejectDocument(int applicationId, string status, string comment)
        {
            if (!string.IsNullOrEmpty(comment))
            {
                AdminDetails _details = new AdminDetails();
                var data = _details.ApproveRejectDocument(applicationId, status, comment);
                SetAlertMessage("Document is " + status, "document detail staus");
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(-1, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult CandidateListForInterview()
        {
            return View();
        }
        public ActionResult CandidateListForAllotment()
        {
            return View();
        }
        public ActionResult SchemeWiseAllotmentList()
        {
            return View();
        }
        public ActionResult AllotmentStatus()
        {
            return View();
        }
        public ActionResult PrintAllotmentLetter()
        {
            return View();
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            Session.Clear();
            return RedirectToAction("AdminLogin", "Login");
        }
        public ActionResult Invitation()
        {
            return View();
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
        public ActionResult SaveNewInvitation(string Id, string Applicant, string Address, string SectorName, string ApplicationNo, string PlotRange, string TotalNoOfPlots, string InterviewDateTime, string InterviewMode)
        {
            ApplicantInvitationLetter invt = new ApplicantInvitationLetter();
            invt.Id = !string.IsNullOrEmpty(Id) ? Convert.ToInt32(Id) : 0;
            invt.UserId = Convert.ToInt32(Applicant);
            invt.ApplicationNo = ApplicationNo;
            invt.ApplicantAddress = Address;
            invt.SectorName = Convert.ToInt32(SectorName);
            invt.PlotRange = PlotRange;
            invt.TotalNoOfPlots = TotalNoOfPlots;
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
            return RedirectToAction("Invitation");


        }

    }
}