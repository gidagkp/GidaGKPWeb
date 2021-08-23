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

namespace GidaGkpWeb.Controllers
{
    [SessionTimeout]
    public class ApplicantController : CommonController
    {
        CCACrypto ccaCrypto = new CCACrypto();
        string workingKey = "AB986C5175D944E600B2567AEDC18A04";//put in the 32bit alpha numeric key in the quotes provided here 	
        string ccaRequest = "";
        public string strMerchantId = "310096";
        public string strEncRequest = "";
        public string strAccessCode = "AVHE00IA10BD01EHDB";// put the access key in the quotes provided here.
        //string apiUrl = "https://apitest.ccavenue.com/apis/servlet/DoWebTrans";
        public ActionResult Dashboard()
        {
            //show gida logo and info
            return View();
        }

        public ActionResult ApplyForPlot()
        {
            //Squire box colofull
            return View();
        }

        public ActionResult ViewAdvertisement()
        {
            //Table with scheme Advertisement
            return View();
        }

        public ActionResult ApplicantDashboard(int? applicationId = null)
        {
            if (applicationId != null && applicationId > 0)
            {
                UserData.ApplicationId = applicationId.Value;
                return View();
            }
            else
            {
                ApplicantDetails _details = new ApplicantDetails();
                var dataCount = _details.GetUserApplicationCount(((CustomPrincipal)User).Id);
                if (dataCount > 0)
                {
                    return RedirectToAction("ApplicantViewApplication");
                }
                else
                {
                    return View();
                }
            }

        }

        public ActionResult ApplicantViewApplication()
        {
            ApplicantDetails _details = new ApplicantDetails();
            ViewData["ApplicationData"] = _details.GetUserApplicationDetail(((CustomPrincipal)User).Id);
            return View();
        }

        [HttpPost]
        public JsonResult SavePlotDetail(string AppliedFor, string SchemeType, string PlotRange, string SchemeName, string plotArea, string SectorName, string SectorDescription, string EstimatedRate, string PaymemtSchedule, string TotalInvestment, string ApplicationFee, string EarnestMoneyDeposite, string GST, string NetAmount, string TotalAmount, string IndustryOwnershipType, string UnitName, string Name, string dob, string PresentAddress, string PermanentAddress, string RelationshipStatus)
        {
            ApplicantDetails _details = new ApplicantDetails();
            if (string.IsNullOrEmpty(AppliedFor) || string.IsNullOrEmpty(SchemeType))
            {
                //SetAlertMessage("Incomplete Detail", "Error");
                return null;
            }
            var AppNumber = _details.SavePlotDetail(((CustomPrincipal)User).Id, AppliedFor, SchemeType, PlotRange, SchemeName, plotArea, SectorName, SectorDescription, EstimatedRate, PaymemtSchedule, TotalInvestment, ApplicationFee, EarnestMoneyDeposite, GST, NetAmount, TotalAmount, IndustryOwnershipType, UnitName, Name, dob, PresentAddress, PermanentAddress, RelationshipStatus);
            if (AppNumber != "Error")
                Session["ApplicationNumber"] = AppNumber;
            return Json(AppNumber, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult SaveApplicantDetails(string FullName, string FName, string MName, string SName, string DOB, string Gender, string Category, string Nationality, string AdhaarNo, string PAN, string MobileNo, string Phone, string Email, string Religion, string SubCategory, string CAddress, string PAddress, string IdentityProof, string ResidentialProof)
        {
            ApplicantDetails _details = new ApplicantDetails();
            if (string.IsNullOrEmpty(FullName) || string.IsNullOrEmpty(FName))
            {
                //SetAlertMessage("Incomplete Detail", "Error");
                return null;
            }
            return Json(CrudResponse(_details.SaveApplicantDetail(((CustomPrincipal)User).Id, FullName, FName, MName, SName, DOB, Gender, Category, Nationality, AdhaarNo, PAN, MobileNo, Phone, Email, Religion, SubCategory, CAddress, PAddress, IdentityProof, ResidentialProof)), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveProjectDetails(string ProposedIndustryType, string ProjectEstimatedCost, string ProposedCoveredArea, string ProposedOpenArea, string PurpuseOpenArea, string ProposedInvestmentLand, string ProposedInvestmentBuilding, string ProposedInvestmentPlant, string FumesNatureQuantity, string LiquidQuantity, string LiquidChemicalComposition, string SolidQuantity, string SolidChemicalComposition, string GasQuantity, string GasChemicalComposition, string PowerRequirement, string FirstYearNoOfTelephone, string FirstYearNoOfFax, string UltimateNoOfTelephone, string UltimateNoOfFax, string Skilled, string UnSkilled)
        {
            ApplicantDetails _details = new ApplicantDetails();
            if (string.IsNullOrEmpty(ProposedIndustryType) || string.IsNullOrEmpty(ProjectEstimatedCost))
            {
                //SetAlertMessage("Incomplete Detail", "Error");
                return null;
            }
            return Json(CrudResponse(_details.SaveProjectDetail(((CustomPrincipal)User).Id, ProposedIndustryType, ProjectEstimatedCost, ProposedCoveredArea, ProposedOpenArea, PurpuseOpenArea, ProposedInvestmentLand, ProposedInvestmentBuilding, ProposedInvestmentPlant, FumesNatureQuantity, LiquidQuantity, LiquidChemicalComposition, SolidQuantity, SolidChemicalComposition, GasQuantity, GasChemicalComposition, PowerRequirement, FirstYearNoOfTelephone, FirstYearNoOfFax, UltimateNoOfTelephone, UltimateNoOfFax, Skilled, UnSkilled)), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveBankDetail(string BankAccountName, string BankAccountNo, string BankName, string BranchName, string BranchAddress, string IFSCCode)
        {
            ApplicantDetails _details = new ApplicantDetails();
            if (string.IsNullOrEmpty(BankAccountName) || string.IsNullOrEmpty(BankAccountNo))
            {
                //SetAlertMessage("Incomplete Detail", "Error");
                return null;
            }
            return Json(CrudResponse(_details.SaveBankDetail(((CustomPrincipal)User).Id, BankAccountName, BankAccountNo, BankName, BranchName, BranchAddress, IFSCCode)), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveApplicantDocument(HttpPostedFileBase ProjectReports, HttpPostedFileBase Proposedplan, HttpPostedFileBase PartnershipDeed,
            HttpPostedFileBase PanCard, HttpPostedFileBase AddressProof, HttpPostedFileBase BalanceSheet, HttpPostedFileBase IncomeTaxreturn,
            HttpPostedFileBase Experienceproof, HttpPostedFileBase educationalqualification, HttpPostedFileBase electricitybill,
            HttpPostedFileBase financialdetails, HttpPostedFileBase Otherproposedindustry, HttpPostedFileBase CasteCertificate,
            HttpPostedFileBase IdentityProof, HttpPostedFileBase AllotmentLetter, HttpPostedFileBase BankVerifiedSignature,
            HttpPostedFileBase outsideGIDAElectricitybill, HttpPostedFileBase ApplicantPhoto, HttpPostedFileBase ApplicantSignature)
        {
            ApplicantUploadDoc documentDetail = new ApplicantUploadDoc();
            if (ProjectReports != null && ProjectReports.ContentLength > 0)
            {
                documentDetail.ProjectReport = new byte[ProjectReports.ContentLength];
                ProjectReports.InputStream.Read(documentDetail.ProjectReport, 0, ProjectReports.ContentLength);
                documentDetail.ProjectReportFileName = ProjectReports.FileName;
                documentDetail.ProjectReportFileType = ProjectReports.ContentType;
            }
            if (Proposedplan != null && Proposedplan.ContentLength > 0)
            {
                documentDetail.ProposedPlanLandUses = new byte[Proposedplan.ContentLength];
                Proposedplan.InputStream.Read(documentDetail.ProposedPlanLandUses, 0, Proposedplan.ContentLength);
                documentDetail.ProposedPlanLandUsesFileName = Proposedplan.FileName;
                documentDetail.ProposedPlanLandUsesFileType = Proposedplan.ContentType;
            }
            if (PartnershipDeed != null && PartnershipDeed.ContentLength > 0)
            {
                documentDetail.Memorendum = new byte[PartnershipDeed.ContentLength];
                PartnershipDeed.InputStream.Read(documentDetail.Memorendum, 0, PartnershipDeed.ContentLength);
                documentDetail.MemorendumFileName = PartnershipDeed.FileName;
                documentDetail.MemorendumFileType = PartnershipDeed.ContentType;
            }

            if (PanCard != null && PanCard.ContentLength > 0)
            {
                documentDetail.ScanPAN = new byte[PanCard.ContentLength];
                PanCard.InputStream.Read(documentDetail.ScanPAN, 0, PanCard.ContentLength);
                documentDetail.ScanPANFileName = PanCard.FileName;
                documentDetail.ScanPANFileType = PanCard.ContentType;
            }
            if (AddressProof != null && AddressProof.ContentLength > 0)
            {
                documentDetail.ScanAddressProof = new byte[AddressProof.ContentLength];
                AddressProof.InputStream.Read(documentDetail.ScanAddressProof, 0, AddressProof.ContentLength);
                documentDetail.ScanAddressProofFileName = AddressProof.FileName;
                documentDetail.ScanAddressProofFileType = AddressProof.ContentType;
            }

            if (BalanceSheet != null && BalanceSheet.ContentLength > 0)
            {
                documentDetail.BalanceSheet = new byte[BalanceSheet.ContentLength];
                BalanceSheet.InputStream.Read(documentDetail.BalanceSheet, 0, BalanceSheet.ContentLength);
                documentDetail.BalanceSheetFileName = BalanceSheet.FileName;
                documentDetail.BalanceSheetFileType = BalanceSheet.ContentType;
            }
            if (IncomeTaxreturn != null && IncomeTaxreturn.ContentLength > 0)
            {
                documentDetail.ITReturn = new byte[IncomeTaxreturn.ContentLength];
                IncomeTaxreturn.InputStream.Read(documentDetail.ITReturn, 0, IncomeTaxreturn.ContentLength);
                documentDetail.ITReturnFileName = IncomeTaxreturn.FileName;
                documentDetail.ITReturnFileType = IncomeTaxreturn.ContentType;
            }
            if (Experienceproof != null && Experienceproof.ContentLength > 0)
            {
                documentDetail.ExperienceProof = new byte[Experienceproof.ContentLength];
                Experienceproof.InputStream.Read(documentDetail.ExperienceProof, 0, Experienceproof.ContentLength);
                documentDetail.ExperienceProofFileName = Experienceproof.FileName;
                documentDetail.ExperienceProofFileType = Experienceproof.ContentType;
            }

            if (educationalqualification != null && educationalqualification.ContentLength > 0)
            {
                documentDetail.ApplicantEduTechQualification = new byte[educationalqualification.ContentLength];
                educationalqualification.InputStream.Read(documentDetail.ApplicantEduTechQualification, 0, educationalqualification.ContentLength);
                documentDetail.ApplicantEduTechQualificationFileName = educationalqualification.FileName;
                documentDetail.ApplicantEduTechQualificationFileType = educationalqualification.ContentType;
            }

            if (electricitybill != null && electricitybill.ContentLength > 0)
            {
                documentDetail.PreEstablishedIndustriesDoc = new byte[electricitybill.ContentLength];
                electricitybill.InputStream.Read(documentDetail.PreEstablishedIndustriesDoc, 0, electricitybill.ContentLength);
                documentDetail.PreEstablishedIndustriesDocFileName = electricitybill.FileName;
                documentDetail.PreEstablishedIndustriesDocFileType = electricitybill.ContentType;
            }

            if (financialdetails != null && financialdetails.ContentLength > 0)
            {
                documentDetail.FinDetailsEstablishedIndustries = new byte[financialdetails.ContentLength];
                financialdetails.InputStream.Read(documentDetail.FinDetailsEstablishedIndustries, 0, financialdetails.ContentLength);
                documentDetail.FinDetailsEstablishedIndustriesFileName = financialdetails.FileName;
                documentDetail.FinDetailsEstablishedIndustriesFileType = financialdetails.ContentType;
            }

            if (Otherproposedindustry != null && Otherproposedindustry.ContentLength > 0)
            {
                documentDetail.OtherDocForProposedIndustry = new byte[Otherproposedindustry.ContentLength];
                Otherproposedindustry.InputStream.Read(documentDetail.OtherDocForProposedIndustry, 0, Otherproposedindustry.ContentLength);
                documentDetail.OtherDocForProposedIndustryFileName = Otherproposedindustry.FileName;
                documentDetail.OtherDocForProposedIndustryFileType = Otherproposedindustry.ContentType;
            }

            if (CasteCertificate != null && CasteCertificate.ContentLength > 0)
            {
                documentDetail.ScanCastCert = new byte[CasteCertificate.ContentLength];
                CasteCertificate.InputStream.Read(documentDetail.ScanCastCert, 0, CasteCertificate.ContentLength);
                documentDetail.ScanCastCertFileName = CasteCertificate.FileName;
                documentDetail.ScanCastCertFileType = CasteCertificate.ContentType;
            }

            if (IdentityProof != null && IdentityProof.ContentLength > 0)
            {
                documentDetail.ScanID = new byte[IdentityProof.ContentLength];
                IdentityProof.InputStream.Read(documentDetail.ScanID, 0, IdentityProof.ContentLength);
                documentDetail.ScanIDFileName = IdentityProof.FileName;
                documentDetail.ScanIDFileType = IdentityProof.ContentType;
            }


            if (BankVerifiedSignature != null && BankVerifiedSignature.ContentLength > 0)
            {
                documentDetail.BankVerifiedSignature = new byte[BankVerifiedSignature.ContentLength];
                BankVerifiedSignature.InputStream.Read(documentDetail.BankVerifiedSignature, 0, BankVerifiedSignature.ContentLength);
                documentDetail.BankVerifiedSignatureFileName = BankVerifiedSignature.FileName;
                documentDetail.BankVerifiedSignatureFileType = BankVerifiedSignature.ContentType;
            }

            if (ApplicantPhoto != null && ApplicantPhoto.ContentLength > 0)
            {
                documentDetail.ApplicantPhoto = new byte[ApplicantPhoto.ContentLength];
                ApplicantPhoto.InputStream.Read(documentDetail.ApplicantPhoto, 0, ApplicantPhoto.ContentLength);
                documentDetail.ApplicantPhotoFileName = ApplicantPhoto.FileName;
                documentDetail.ApplicantPhotoFileType = ApplicantPhoto.ContentType;
            }

            if (ApplicantSignature != null && ApplicantSignature.ContentLength > 0)
            {
                documentDetail.ApplicantSignature = new byte[ApplicantSignature.ContentLength];
                ApplicantSignature.InputStream.Read(documentDetail.ApplicantSignature, 0, ApplicantSignature.ContentLength);
                documentDetail.ApplicantSignatureFileName = ApplicantSignature.FileName;
                documentDetail.ApplicantSignatureFileType = ApplicantSignature.ContentType;
            }

            if (outsideGIDAElectricitybill != null && outsideGIDAElectricitybill.ContentLength > 0)
            {
                documentDetail.DocProofForIndustrialEstablishedOutsideGida = new byte[outsideGIDAElectricitybill.ContentLength];
                outsideGIDAElectricitybill.InputStream.Read(documentDetail.DocProofForIndustrialEstablishedOutsideGida, 0, outsideGIDAElectricitybill.ContentLength);
                documentDetail.DocProofForIndustrialEstablishedOutsideGidaFileName = outsideGIDAElectricitybill.FileName;
                documentDetail.DocProofForIndustrialEstablishedOutsideGidaFileType = outsideGIDAElectricitybill.ContentType;
            }

            documentDetail.UserId = ((CustomPrincipal)User).Id;
            ApplicantDetails _details = new ApplicantDetails();
            var result = _details.SaveApplicantDocument(((CustomPrincipal)User).Id, documentDetail);
            if (result == Enums.CrudStatus.Saved)
            {
                SetAlertMessage("document detail saved", "Document Entry");
                return RedirectToAction("PaymentRequest");
            }
            else
            {
                SetAlertMessage("Document detail not saved, Please Attach all reqired document", "Document Entry");
                return RedirectToAction("ApplicantDashboard");
            }
        }

        public ActionResult PaymentRequest()
        {
            ApplicantDetails _details = new ApplicantDetails();
            var data = _details.GetUserPlotDetail(UserData.ApplicationId);
            string transactionId = VerificationCodeGeneration.GetUniqueNumber(), orderId = VerificationCodeGeneration.GetUniqueNumber();
            while (_details.ChecktransactionId(Convert.ToInt64(transactionId)) != null)
            {
                transactionId = VerificationCodeGeneration.GetUniqueNumber();
            }
            while (_details.CheckorderId(Convert.ToInt64(orderId)) != null)
            {
                orderId = VerificationCodeGeneration.GetUniqueNumber();
            }
            if (UserData.UserId > 0 && UserData.ApplicationId > 0)
            {
                var _effectRow = _details.SavePGTransactionInformation(UserData.UserId, UserData.ApplicationId, orderId, transactionId);
                if (_effectRow <= 0)
                {
                    SetAlertMessage("Session Timeout", "Error");
                    return View();
                }
            }
            else
            {
                SetAlertMessage("Session Timeout", "Error");
                return RedirectToAction("ApplicantLogin", "Login");
            }

            string baseURL = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            string amountToBePaid = data.NetAmount.ToString();/*"1";*/
            string ccaRequest = "tid=" + transactionId + "&merchant_id=" + strMerchantId + "&order_id=" + orderId + "&amount=" + amountToBePaid + "&currency=INR&redirect_url=" + baseURL + "Applicant/PaymentResponse&cancel_url=" + baseURL + "Applicant/PaymentResponse&language=EN&billing_name=" + data.FullApplicantName + "&billing_address=" + data.CAddress + "&billing_city=&billing_state=&billing_zip=&billing_country=&billing_tel=&billing_email=&delivery_name=" + data.FullApplicantName + "&delivery_address=" + data.CAddress + "&delivery_city=&delivery_state=&delivery_zip=&delivery_country=&delivery_tel=&merchant_param1=" + UserData.UserId + "&merchant_param2=" + UserData.ApplicationId + "&merchant_param3=" + UserData.Username + "&merchant_param4=" + UserData.UserType + "&merchant_param5=" + orderId + "&";
            //string ccaRequest = "tid=" + VerificationCodeGeneration.GetUniqueNumber() + "&merchant_id=" + strMerchantId + "&order_id=" + VerificationCodeGeneration.GetUniqueNumber() + "&amount=" + amountToBePaid + "&currency=INR&redirect_url=" + baseURL + "Applicant/PaymentResponse&cancel_url=" + baseURL + "Applicant/PaymentResponse&language=EN&billing_name=" + data.FullApplicantName + "&billing_address=" + data.CAddress + "&billing_city=&billing_state=&billing_zip=&billing_country=&billing_tel=&billing_email=&delivery_name=" + data.FullApplicantName + "&delivery_address=" + data.CAddress + "&delivery_city=&delivery_state=&delivery_zip=&delivery_country=&delivery_tel=&merchant_param1=" + UserData.UserId + "&merchant_param2=" + UserData.ApplicationId + "&merchant_param3=" + UserData.Username + "&merchant_param4=" + UserData.Email + "&merchant_param5=" + UserData.FullName + "&";
            Session["strEncRequest"] = ccaCrypto.Encrypt(ccaRequest, workingKey);
            Session["strAccessCode"] = strAccessCode;
            ViewData["UserData"] = data;
            return View();
        }
        public ActionResult PaymentResponse()
        {
            ApplicantDetails _details = new ApplicantDetails();
            CCACrypto ccaCrypto = new CCACrypto();
            string encResponse = ccaCrypto.Decrypt(Request.Form["encResp"], workingKey);
            NameValueCollection Params = new NameValueCollection();
            string[] segments = encResponse.Split('&');
            foreach (string seg in segments)
            {
                string[] parts = seg.Split('=');
                if (parts.Length > 0)
                {
                    string Key = parts[0].Trim();
                    string Value = parts[1].Trim();
                    Params.Add(Key, Value);
                }
            }

            UserData.UserId = Convert.ToInt32(Params["merchant_param1"]);
            UserData.ApplicationId = Convert.ToInt32(Params["merchant_param2"]);
            UserData.Username = Convert.ToString(Params["merchant_param3"]);
            LoginDetails _logindetails = new LoginDetails();
            string _response = string.Empty;
            Enums.LoginMessage message = _logindetails.GetLoginByUsrrname(UserData.Username);
            _response = LoginResponse(message);
            if (message != Enums.LoginMessage.Authenticated)
            {
                SetAlertMessage("Session Timeout", "Error");
                return RedirectToAction("ApplicantLogin", "Login");
            }

            setUserClaim();

            var pdinfo = _details.GetPGTransactionInformation(Convert.ToString(Params["merchant_param5"]), Convert.ToString(Params["tracking_id"]));

            if (pdinfo != null)
            {
                if (Convert.ToInt64(pdinfo.OrderId) != Convert.ToInt64(Params["order_id"]))
                {
                    SetAlertMessage("Payment Not Done", "Error");
                    return RedirectToAction("PaymentRequest");
                }

            }
            else
            {
                SetAlertMessage("Payment Not Done", "Error");
                return RedirectToAction("PaymentRequest");
            }

            if (Params["order_status"] == "Success")
            {
                #region API to check order status
                //var input = new
                //{
                //    access_code = strAccessCode,
                //    command = "orderStatusTracker",
                //    request_type = "STRING",
                //    response_type = "STRING",
                //    order_no = Convert.ToString(Params["order_id"]),
                //    enc_request = ccaCrypto.Encrypt("|" + Convert.ToString(Params["order_id"]) + "|", workingKey)
                //};
                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                //string inputJson = (new JavaScriptSerializer()).Serialize(input);
                //HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(new Uri(apiUrl + "?access_code=" + input.access_code + "&command=orderStatusTracker&request_type=STRING&response_type=STRING&order_no=" + input.order_no + "&enc_request=" + input.enc_request + ""));
                //httpRequest.ContentType = "application/json";
                //httpRequest.Method = "POST";

                //byte[] bytes = Encoding.UTF8.GetBytes(inputJson);
                //using (Stream stream = httpRequest.GetRequestStream())
                //{
                //    stream.Write(bytes, 0, bytes.Length);
                //    stream.Close();
                //}

                //using (HttpWebResponse httpResponse = (HttpWebResponse)httpRequest.GetResponse())
                //{
                //    using (Stream stream = httpResponse.GetResponseStream())
                //    {
                //        string json = (new StreamReader(stream)).ReadToEnd();

                //        var responseData = json.Split('&');
                //        if (responseData.Length > 0)
                //        {
                //            var statusData = responseData[1].Split('=');
                //            if (statusData.Length > 1)
                //            {
                //                var responsePayload = ccaCrypto.Decrypt(statusData[1].Replace("\r", "").Replace("\n", "").Replace("\t", ""), workingKey);
                //                if (Convert.ToInt32(responsePayload.Split('|')[0]) == 0) //0 means successfull 
                //                {
                //                    ApplicantDetails _details = new ApplicantDetails();
                //                    ApplicantTransactionDetail detail = new ApplicantTransactionDetail()
                //                    {
                //                        UserId = Convert.ToInt32(Params["merchant_param1"]),
                //                        amount = Params["amount"],
                //                        bank_ref_no = Params["bank_ref_no"],
                //                        billing_address = Params["billing_address"],
                //                        billing_name = Params["billing_name"],
                //                        card_name = Params["card_name"],
                //                        created_date = DateTime.Now,
                //                        order_id = Convert.ToInt64(Params["order_id"]),
                //                        order_status = Params["order_status"],
                //                        payment_mode = Params["payment_mode"],
                //                        status_message = Params["status_message"],
                //                        tracking_id = Convert.ToInt64(Params["tracking_id"]),
                //                        ApplicationId = Convert.ToInt32(Params["merchant_param2"]),
                //                        trans_date = DateTime.Now,
                //                    };
                //                    _details.SaveApplicantTransactionDeatil(detail);
                //                    SetAlertMessage("Payment done successfully", "Payment Status");
                //                    return RedirectToAction("PaymentResponseSuccess");
                //                }
                //                else
                //                {
                //                    SetAlertMessage("Payment Failed", "Error");
                //                    return RedirectToAction("PaymentRequest");
                //                }
                //            }
                //            else
                //            {
                //                SetAlertMessage("Payment Failed", "Error");
                //                return RedirectToAction("PaymentRequest");
                //            }
                //        }
                //        else
                //        {
                //            SetAlertMessage("Payment Failed", "Error");
                //            return RedirectToAction("PaymentRequest");
                //        }
                //    }
                //}
                #endregion
                var payData = _details.GetApplicationTransactionInformation(UserData.UserId, UserData.ApplicationId);
                if (payData == null)
                {
                    ApplicantTransactionDetail detail = new ApplicantTransactionDetail()
                    {
                        UserId = Convert.ToInt32(Params["merchant_param1"]),
                        amount = Params["amount"],
                        bank_ref_no = Params["bank_ref_no"],
                        billing_address = Params["billing_address"],
                        billing_name = Params["billing_name"],
                        card_name = Params["card_name"],
                        created_date = DateTime.Now,
                        order_id = Convert.ToInt64(Params["order_id"]),
                        order_status = Params["order_status"],
                        payment_mode = Params["payment_mode"],
                        status_message = Params["status_message"],
                        tracking_id = Convert.ToInt64(Params["tracking_id"]),
                        ApplicationId = Convert.ToInt32(Params["merchant_param2"]),
                        trans_date = DateTime.Now,
                        TransactionType = "Online"
                    };
                    var rowEffected = _details.SaveApplicantTransactionDeatil(detail);
                    if (rowEffected > 0)
                    {
                        SetAlertMessage("Payment done successfully", "Payment Status");
                    }
                    else
                    {
                        SetAlertMessage("Some Error occured in the System but payment done successfully, Please contact system administrator", "Payment Status");
                    }

                    return RedirectToAction("PaymentResponseSuccess");
                }
                else
                {
                    SetAlertMessage("Payment already made for current Application", "Payment Error");
                    return RedirectToAction("PaymentRequest");
                }
            }
            else if (Params["order_status"] == "Aborted")
            {
                SetAlertMessage("Payment Aborted", "Error");
                return RedirectToAction("PaymentRequest");
            }
            else
            {
                SetAlertMessage("Payment Failed", "Error");
                return RedirectToAction("PaymentRequest");
            }
        }

        public ActionResult PaymentResponseSuccess()
        {
            ApplicantDetails _details = new ApplicantDetails();
            var data = _details.GetUserAppliedApplicationDetail(((CustomPrincipal)User).Id);
            if (data.Count > 1)
            {
                ViewData["ApplicationData"] = data;
            }
            else if (data.Count == 1)
            {
                ViewData["PrintReciept"] = true;
                UserData.ApplicationId = data[0].ApplicationId;
            }
            return View();
        }

        public ActionResult PaymentReciept(int? applicationId = null)
        {
            ApplicantDetails _details = new ApplicantDetails();
            var data = _details.GetUserPlotDetail(applicationId != null ? applicationId : UserData.ApplicationId);
            ViewData["UserData"] = data;
            return View();
        }

        public ActionResult PaymentAcknowledgement(int? applicationId = null)
        {
            ApplicantDetails _details = new ApplicantDetails();
            var data = _details.GetAcknowledgementDetail(applicationId != null ? applicationId : UserData.ApplicationId);
            ViewData["UserData"] = data;
            return View();
        }

        [HttpPost]
        public JsonResult GetPlotRegistrationDetail(int applicationId)
        {
            ApplicantDetails _details = new ApplicantDetails();
            if (applicationId > 0)
            {
                var data = _details.GetApplciantPlotDetailByApplicationId(applicationId);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        [HttpPost]
        public JsonResult GetApplicantPersonalDetail()
        {
            ApplicantDetails _details = new ApplicantDetails();
            if (UserData.ApplicationId > 0)
            {
                var data = _details.GetApplicantPersonalDetail(UserData.ApplicationId);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        [HttpPost]
        public JsonResult GetApplicantProjectDetail()
        {
            ApplicantDetails _details = new ApplicantDetails();
            if (UserData.ApplicationId > 0)
            {
                var data = _details.GetApplicantProjectDetail(UserData.ApplicationId);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        [HttpPost]
        public JsonResult GetApplicantBankDetail()
        {
            ApplicantDetails _details = new ApplicantDetails();
            if (UserData.ApplicationId > 0)
            {
                var data = _details.GetApplicantBankDetail(UserData.ApplicationId);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        [HttpPost]
        public JsonResult GetApplicantDocumentDetail()
        {
            ApplicantDetails _details = new ApplicantDetails();
            if (UserData.ApplicationId > 0)
            {

                ApplicantUploadDocumentModel model = new ApplicantUploadDocumentModel();
                var docDetail = _details.GetApplicantDocumentDetail(UserData.ApplicationId);
                if (docDetail != null)
                {
                    model = new ApplicantUploadDocumentModel()
                    {
                        //ApplicantEduTechQualificationURL = string.Format("data:" + docDetail.ApplicantEduTechQualificationFileType + ";base64,{0}", Convert.ToBase64String(docDetail.ApplicantEduTechQualification)),
                        //ApplicantPhotoURL = string.Format("data:" + docDetail.ApplicantPhotoFileType + ";base64,{0}", Convert.ToBase64String(docDetail.ApplicantPhoto)),
                        //ApplicantSignatureURL = string.Format("data:" + docDetail.ApplicantSignatureFileType + ";base64,{0}", Convert.ToBase64String(docDetail.ApplicantSignature)),
                        //BalanceSheetURL = string.Format("data:" + docDetail.BalanceSheetFileType + ";base64,{0}", Convert.ToBase64String(docDetail.BalanceSheet)),
                        //BankVerifiedSignatureURL = string.Format("data:" + docDetail.BankVerifiedSignatureFileType + ";base64,{0}", Convert.ToBase64String(docDetail.BankVerifiedSignature)),
                        //DocProofForIndustrialEstablishedOutsideGidaURL = string.Format("data:" + docDetail.DocProofForIndustrialEstablishedOutsideGidaFileType + ";base64,{0}", Convert.ToBase64String(docDetail.DocProofForIndustrialEstablishedOutsideGida)),
                        //ExperienceProofURL = string.Format("data:" + docDetail.ExperienceProofFileType + ";base64,{0}", Convert.ToBase64String(docDetail.ExperienceProof)),
                        //FinDetailsEstablishedIndustriesURL = string.Format("data:" + docDetail.FinDetailsEstablishedIndustriesFileType + ";base64,{0}", Convert.ToBase64String(docDetail.FinDetailsEstablishedIndustries)),
                        //ITReturnURL = string.Format("data:" + docDetail.ITReturnFileType + ";base64,{0}", Convert.ToBase64String(docDetail.ITReturn)),
                        //MemorendumURL = string.Format("data:" + docDetail.MemorendumFileType + ";base64,{0}", Convert.ToBase64String(docDetail.Memorendum)),
                        //OtherDocForProposedIndustryURL = string.Format("data:" + docDetail.OtherDocForProposedIndustryFileType + ";base64,{0}", Convert.ToBase64String(docDetail.OtherDocForProposedIndustry)),
                        //PreEstablishedIndustriesDocURL = string.Format("data:" + docDetail.PreEstablishedIndustriesDocFileType + ";base64,{0}", Convert.ToBase64String(docDetail.PreEstablishedIndustriesDoc)),
                        //ProjectReportURL = string.Format("data:" + docDetail.ProjectReportFileType + ";base64,{0}", Convert.ToBase64String(docDetail.ProjectReport)),
                        //ProposedPlanLandUsesURL = string.Format("data:" + docDetail.ProposedPlanLandUsesFileType + ";base64,{0}", Convert.ToBase64String(docDetail.ProposedPlanLandUses)),
                        //ScanAddressProofURL = string.Format("data:" + docDetail.ScanAddressProofFileType + ";base64,{0}", Convert.ToBase64String(docDetail.ScanAddressProof)),
                        //ScanCastCertURL = string.Format("data:" + docDetail.ScanCastCertFileType + ";base64,{0}", Convert.ToBase64String(docDetail.ScanCastCert)),
                        //ScanIDURL = string.Format("data:" + docDetail.ScanIDFileType + ";base64,{0}", Convert.ToBase64String(docDetail.ScanID)),
                        //ScanPANURL = string.Format("data:" + docDetail.ScanPANFileType + ";base64,{0}", Convert.ToBase64String(docDetail.ScanPAN)),
                        ScanPANFileName = docDetail.ScanPANFileName,
                        ScanIDFileName = docDetail.ScanIDFileName,
                        ScanCastCertFileName = docDetail.ScanCastCertFileName,
                        ApplicantEduTechQualificationFileName = docDetail.ApplicantEduTechQualificationFileName,
                        ApplicantPhotoFileName = docDetail.ApplicantPhotoFileName,
                        ApplicantSignatureFileName = docDetail.ApplicantSignatureFileName,
                        ApplicationId = docDetail.ApplicationId,
                        BalanceSheetFileName = docDetail.BalanceSheetFileName,
                        BankVerifiedSignatureFileName = docDetail.BankVerifiedSignatureFileName,
                        DocProofForIndustrialEstablishedOutsideGidaFileName = docDetail.DocProofForIndustrialEstablishedOutsideGidaFileName,
                        ExperienceProofFileName = docDetail.ExperienceProofFileName,
                        FinDetailsEstablishedIndustriesFileName = docDetail.FinDetailsEstablishedIndustriesFileName,
                        ITReturnFileName = docDetail.ITReturnFileName,
                        MemorendumFileName = docDetail.MemorendumFileName,
                        OtherDocForProposedIndustryFileName = docDetail.OtherDocForProposedIndustryFileName,
                        PreEstablishedIndustriesDocFileName = docDetail.PreEstablishedIndustriesDocFileName,
                        ProjectReportFileName = docDetail.ProjectReportFileName,
                        ProposedPlanLandUsesFileName = docDetail.ProposedPlanLandUsesFileName,
                        ScanAddressProofFileName = docDetail.ScanAddressProofFileName,
                    };
                }


                return Json(model, JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        [HttpPost]
        public JsonResult SetApplicantId(int applicationId)
        {
            UserData.ApplicationId = applicationId;
            return Json("application id set", JsonRequestBehavior.AllowGet);
        }

        //public FileContentResult GetFile(int id)
        //{
        //    byte[] fileContent = null;
        //    string mimeType = ""; string fileName = "";
        //    ApplicantDetails _details = new ApplicantDetails();
        //    var data = _details.GetApplicantPersonalDetail(UserData.ApplicationId);

        //    fileContent = (byte[])rdr["FileContent"];
        //    mimeType = rdr["MimeType"].ToString();
        //    fileName = rdr["FileName"].ToString();

        //    return File(fileContent, mimeType, fileName);
        //}

        public ActionResult ApplicantUploadTransactionDetail()
        {
            ApplicantDetails _details = new ApplicantDetails();
            ViewData["ApplicationData"] = _details.GetUserApplicationForOfflinePayment(((CustomPrincipal)User).Id);
            return View();
        }

        public ActionResult ApplicantUploadTransactionInDetail(int? applicationId = null)
        {
            if (applicationId != null && applicationId > 0)
            {
                UserData.ApplicationId = applicationId.Value;
                return View();
            }
            else
            {
                return RedirectToAction("ApplicantUploadTransactionDetail");
            }

        }

        [HttpPost]
        public ActionResult SaveApplicantChalanDocument(HttpPostedFileBase bankSlip, string BankReferenceNo, string PaymentMode, string AmountPaid, string TransactionDate)
        {
            ApplicantTransactionDetail documentDetail = new ApplicantTransactionDetail();
            if (bankSlip != null && bankSlip.ContentLength > 0)
            {
                documentDetail.ChalanDocument = new byte[bankSlip.ContentLength];
                bankSlip.InputStream.Read(documentDetail.ChalanDocument, 0, bankSlip.ContentLength);
                documentDetail.ChalanDocumentFileName = bankSlip.FileName;
                documentDetail.ChalanDocumentFileType = bankSlip.ContentType;
            }

            documentDetail.UserId = ((CustomPrincipal)User).Id;
            documentDetail.bank_ref_no = BankReferenceNo;
            documentDetail.payment_mode = PaymentMode;
            documentDetail.amount = AmountPaid;
            documentDetail.TransactionType = "Offline";
            documentDetail.trans_date = Convert.ToDateTime(TransactionDate);

            ApplicantDetails _details = new ApplicantDetails();
            var result = _details.SaveApplicantChalanDocument(((CustomPrincipal)User).Id, documentDetail);
            if (result == Enums.CrudStatus.Saved)
            {
                SetAlertMessage("Payment detail saved", "Chalan Entry");
                return RedirectToAction("PaymentResponseSuccess");
            }
            else
            {
                SetAlertMessage("Payment detail not saved, Please enter all reqired feilds", "Chalan Entry");
                return RedirectToAction("ApplicantUploadTransactionDetail");
            }
        }


        public ActionResult Logout()
        {
            Session.Abandon();
            Session.Clear();
            return RedirectToAction("ApplicantLogin", "Login");
        }

        private void setUserClaim()
        {
            CustomPrincipalSerializeModel serializeModel = new CustomPrincipalSerializeModel();
            serializeModel.Id = UserData.UserId;
            serializeModel.FullName = string.IsNullOrEmpty(UserData.FullName) ? string.Empty : UserData.FullName;
            serializeModel.Email = string.IsNullOrEmpty(UserData.Email) ? string.Empty : UserData.Email;

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            string userData = serializer.Serialize(serializeModel);

            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                     1,
                     UserData.Email,
                     DateTime.Now,
                     DateTime.Now.AddMinutes(15),
                     false,
                     userData);

            string encTicket = FormsAuthentication.Encrypt(authTicket);
            HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
            Response.Cookies.Add(faCookie);
        }
    }
}