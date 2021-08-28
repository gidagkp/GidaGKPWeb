﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLayer;
using GidaGkpWeb.Global;
using System.Data.Entity;
using GidaGkpWeb.Models;
using System.Data.Entity.Validation;

namespace GidaGkpWeb.BAL
{
    public class AdminDetails
    {
        GidaGKPEntities _db = null;
        public List<ApplicationUserModel> GetApplicantUser()
        {
            try
            {
                _db = new GidaGKPEntities();
                return (from user in _db.ApplicantUsers
                        join applicant1 in _db.ApplicantDetails on user.Id equals applicant1.UserId into applicant2
                        from applicant in applicant2.DefaultIfEmpty()
                        where user.UserType != "Test"
                        select new
                        {
                            AadharNumber = user.AadharNumber,
                            ContactNo = user.ContactNo,
                            CreationDate = user.CreationDate,
                            Email = user.Email,
                            FatherName = applicant != null ? applicant.FName : "",
                            CAddress = applicant != null ? applicant.CAddress : "",
                            PAddress = applicant != null ? applicant.PAddress : "",
                            FullName = applicant != null ? applicant.FullApplicantName : "",
                            Id = user.Id,
                            SchemeName = user.SchemeName,
                            SchemeType = user.SchemeType,
                            SectorName = user.SectorName,
                            UserType = user.UserType,
                            DOB = user.DOB,
                            UserName = user.UserName,
                            IsActive = user.IsActive
                        }).Distinct().ToList()
                        .Select(x => new ApplicationUserModel()
                        {
                            AadharNumber = x.AadharNumber,
                            ContactNo = x.ContactNo,
                            CreationDate = x.CreationDate,
                            Email = x.Email,
                            FatherName = x.FatherName,
                            CAddress = x.CAddress,
                            PAddress = x.PAddress,
                            FullName = x.FullName,
                            Id = x.Id,
                            SchemeName = x.SchemeName,
                            SchemeType = x.SchemeType,
                            SectorName = x.SectorName,
                            UserType = x.UserType,
                            DOB = x.DOB != null ? x.DOB.Value.ToString("dd/MM/yyyy") : string.Empty,
                            UserName = x.UserName,
                            IsActive = x.IsActive
                        }).ToList();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Elmah.ErrorLog.GetDefault(HttpContext.Current).Log(new Elmah.Error(e));
                    }
                }
                return new List<ApplicationUserModel>();
            }
        }
        public List<ApplicationUserModel> GetApplicantUserDetail(int? schemeName)
        {
            try
            {
                _db = new GidaGKPEntities();
                return (from user in _db.ApplicantUsers
                        join applicant1 in _db.ApplicantDetails on user.Id equals applicant1.UserId into applicant2
                        from applicant in applicant2.DefaultIfEmpty()
                        join application1 in _db.ApplicantApplicationDetails on user.Id equals application1.UserId into application2
                        from application in application2.DefaultIfEmpty()
                        join bankDetail1 in _db.ApplicantBankDetails on user.Id equals bankDetail1.UserId into bankDetail2
                        from bankDetail in bankDetail2.DefaultIfEmpty()
                        join plotDetail1 in _db.ApplicantPlotDetails on user.Id equals plotDetail1.UserId into plotDetail2
                        from plotDetail in plotDetail2.DefaultIfEmpty()
                        join doc1 in _db.ApplicantUploadDocs on user.Id equals doc1.UserId into doc2
                        from doc in doc2.DefaultIfEmpty()
                        join transaction1 in _db.ApplicantTransactionDetails on application.ApplicationId equals transaction1.ApplicationId into transaction2
                        from transaction in transaction2.DefaultIfEmpty()
                        join ProjectDetail1 in _db.ApplicantProjectDetails on application.ApplicationId equals ProjectDetail1.ApplicationId into projectDetail2
                        from ProjectDetail in projectDetail2.DefaultIfEmpty()
                        where user.UserType != "Test" && plotDetail.SchemeName == schemeName
                        select new
                        {
                            ApplicationNumber = doc != null ? application.ApplicationNumber : "",
                            PaidAmount = transaction != null ? transaction.amount : "",
                            ApplicationId = transaction != null ? application.ApplicationId : 0,
                            PlotArea = plotDetail != null ? plotDetail.PlotArea : "",
                            UnitName = plotDetail != null ? plotDetail.UnitName : "",
                            TotalInvestment = plotDetail != null ? plotDetail.TotalInvestment : 0,
                            Skilled = ProjectDetail != null ? ProjectDetail.Skilled : "",
                            AadharNumber = user.AadharNumber,
                            ContactNo = user.ContactNo,
                            CreationDate = user.CreationDate,
                            Email = user.Email,
                            FatherName = applicant != null ? applicant.FName : "",
                            CAddress = applicant != null ? applicant.CAddress : "",
                            PAddress = applicant != null ? applicant.PAddress : "",
                            FullName = applicant != null ? applicant.FullApplicantName : "",
                            Id = user.Id,
                            SchemeName = user.SchemeName,
                            SchemeType = user.SchemeType,
                            SectorName = user.SectorName,
                            UserType = user.UserType,
                            DOB = user.DOB,
                            UserName = user.UserName,
                            IsActive = user.IsActive,
                            ApplicationStatus = application.ApprovalStatus,
                            AccoutnNumber = bankDetail.BankAccountNo,
                            BankName = bankDetail.BankName,
                            BranchName = bankDetail.BBName,
                            IFSCCode = bankDetail.IFSC_Code,
                            TransactionID = transaction != null ? transaction.tracking_id : 0,
                            TransactionDate = transaction != null ? transaction.trans_date : null,
                            PaymentMode = transaction != null ? transaction.payment_mode : null,
                            ApplicationFee = plotDetail != null ? plotDetail.ApplicationFee.ToString() : "",
                            GST = plotDetail != null ? plotDetail.GST.ToString() : "",
                            EarnestMoney = plotDetail != null ? plotDetail.EarnestMoney.ToString() : "",
                            SchemeNameId = plotDetail != null ? plotDetail.SchemeName.ToString() : "",

                            AMPaymentStatus = transaction != null ? transaction.AMApprovalStatus : "",
                            CEOPaymentStatus = transaction != null ? transaction.CEOApprovalStatus : "",
                            GMPaymentStatus = transaction != null ? transaction.GMApprovalStatus : "",

                            AMPaymentComment = transaction != null ? transaction.AMComment : "",
                            CEOPaymentComment = transaction != null ? transaction.CEOComment : "",
                            GMPaymentComment = transaction != null ? transaction.GMComment : "",

                            AMDocumentStatus = doc != null ? doc.AMApprovalStatus : "",
                            ClerkDocumentStatus = doc != null ? doc.ClerkApprovalStatus : "",
                            SIDocumentStatus = doc != null ? doc.SIApprovalStatus : "",
                            GMDocumentStatus = doc != null ? doc.GMApprovalStatus : "",
                            CEODocumentStatus = doc != null ? doc.CEOApprovalStatus : "",

                            AMDocumentComment = doc != null ? doc.AMComment : "",
                            ClerkDocumentComment = doc != null ? doc.ClerkComment : "",
                            SIDocumentComment = doc != null ? doc.SIComment : "",
                            GMDocumentComment = doc != null ? doc.GMComment : "",
                            CEODocumentComment = doc != null ? doc.CEOComment : "",

                            ApplicantDocument = new ApplicantUploadDocumentModel()
                            {
                                ApplicantEduTechQualificationFileName = doc.ApplicantEduTechQualificationFileName,
                                ApplicantEduTechQualificationFileType = doc.ApplicantEduTechQualificationFileType,
                                ApplicantPhotoFileName = doc.ApplicantPhotoFileName,
                                ApplicantPhotoFileType = doc.ApplicantPhotoFileType,
                                ApplicantSignatureFileName = doc.ApplicantSignatureFileName,
                                ApplicantSignatureFileType = doc.ApplicantSignatureFileType,
                                ApplicationId = doc.ApplicationId,
                                BalanceSheetFileName = doc.BalanceSheetFileName,
                                BalanceSheetFileType = doc.BalanceSheetFileType,
                                BankVerifiedSignatureFileName = doc.BankVerifiedSignatureFileName,
                                BankVerifiedSignatureFileType = doc.BankVerifiedSignatureFileType,
                                DocProofForIndustrialEstablishedOutsideGidaFileName = doc.DocProofForIndustrialEstablishedOutsideGidaFileName,
                                DocProofForIndustrialEstablishedOutsideGidaFileType = doc.DocProofForIndustrialEstablishedOutsideGidaFileType,
                                ExperienceProofFileName = doc.ExperienceProofFileName,
                                ExperienceProofFileType = doc.ExperienceProofFileType,
                                FinDetailsEstablishedIndustriesFileName = doc.FinDetailsEstablishedIndustriesFileName,
                                FinDetailsEstablishedIndustriesFileType = doc.FinDetailsEstablishedIndustriesFileType,
                                ITReturnFileName = doc.ITReturnFileName,
                                ITReturnFileType = doc.ITReturnFileType,
                                MemorendumFileName = doc.MemorendumFileName,
                                MemorendumFileType = doc.MemorendumFileType,
                                OtherDocForProposedIndustryFileName = doc.OtherDocForProposedIndustryFileName,
                                OtherDocForProposedIndustryFileType = doc.OtherDocForProposedIndustryFileType,
                                PreEstablishedIndustriesDocFileName = doc.PreEstablishedIndustriesDocFileName,
                                PreEstablishedIndustriesDocFileType = doc.PreEstablishedIndustriesDocFileType,
                                ProposedPlanLandUsesFileName = doc.ProposedPlanLandUsesFileName,
                                ProjectReportFileName = doc.ProjectReportFileName,
                                ProjectReportFileType = doc.ProjectReportFileType,
                                ProposedPlanLandUsesFileType = doc.ProposedPlanLandUsesFileType,
                                ScanAddressProofFileName = doc.ScanAddressProofFileName,
                                ScanAddressProofFileType = doc.ScanAddressProofFileType,
                                ScanCastCertFileName = doc.ScanCastCertFileName,
                                ScanCastCertFileType = doc.ScanCastCertFileType,
                                ScanIDFileName = doc.ScanIDFileName,
                                ScanIDFileType = doc.ScanIDFileType,
                                ScanPANFileName = doc.ScanPANFileName,
                                ScanPANFileType = doc.ScanPANFileType,
                            }
                        }).Distinct().ToList()
                        .Select(x => new ApplicationUserModel()
                        {
                            ApplicationNumber = x.ApplicationNumber,
                            PaidAmount = x.PaidAmount,
                            ApplicationId = x.ApplicationId,
                            PlotArea = x.PlotArea,
                            UnitName = x.UnitName,
                            TotalInvestment = x.TotalInvestment,
                            Skilled = x.Skilled,
                            AadharNumber = x.AadharNumber,
                            ContactNo = x.ContactNo,
                            CreationDate = x.CreationDate,
                            Email = x.Email,
                            FatherName = x.FatherName,
                            CAddress = x.CAddress,
                            PAddress = x.PAddress,
                            FullName = x.FullName,
                            Id = x.Id,
                            SchemeName = x.SchemeName,
                            SchemeType = x.SchemeType,
                            SectorName = x.SectorName,
                            UserType = x.UserType,
                            DOB = x.DOB != null ? x.DOB.Value.ToString("dd/MM/yyyy") : string.Empty,
                            UserName = x.UserName,
                            IsActive = x.IsActive,
                            ApplicationStatus = x.ApplicationStatus,
                            AccoutnNumber = x.AccoutnNumber,
                            BankName = x.BankName,
                            BranchName = x.BranchName,
                            IFSCCode = x.IFSCCode,
                            TransactionDate = x.TransactionDate != null ? x.TransactionDate.Value.ToString("dd/MM/yyyy") : string.Empty,
                            TransactionID = x.TransactionID,
                            PaymentMode = x.PaymentMode,
                            EarnestMoney = x.EarnestMoney,
                            FormFee = x.ApplicationFee,
                            GSTAmount = x.GST,
                            SchemeNameId = x.SchemeNameId,

                            AMPaymentStatus = !string.IsNullOrEmpty(x.AMPaymentComment) ? x.AMPaymentStatus + "(Comment : " + x.AMPaymentComment + ")" : x.AMPaymentStatus,
                            CEOPaymentStatus = !string.IsNullOrEmpty(x.CEOPaymentComment) ? x.CEOPaymentStatus + "(Comment : " + x.CEOPaymentComment + ")" : x.CEOPaymentStatus,
                            GMPaymentStatus = !string.IsNullOrEmpty(x.GMPaymentComment) ? x.GMPaymentStatus + "(Comment : " + x.GMPaymentComment + ")" : x.GMPaymentStatus,

                            AMDocumentStatus = !string.IsNullOrEmpty(x.AMDocumentComment) ? x.AMDocumentStatus + "(Comment : " + x.AMDocumentComment + ")" : x.AMDocumentStatus,
                            ClerkDocumentStatus = !string.IsNullOrEmpty(x.ClerkDocumentComment) ? x.ClerkDocumentStatus + "(Comment : " + x.ClerkDocumentComment + ")" : x.ClerkDocumentStatus,
                            SIDocumentStatus = !string.IsNullOrEmpty(x.SIDocumentComment) ? x.SIDocumentStatus + "(Comment : " + x.SIDocumentComment + ")" : x.SIDocumentStatus,
                            CEODocumentStatus = !string.IsNullOrEmpty(x.CEODocumentComment) ? x.CEODocumentStatus + "(Comment : " + x.CEODocumentComment + ")" : x.CEODocumentStatus,
                            GMDocumentStatus = !string.IsNullOrEmpty(x.GMDocumentComment) ? x.GMDocumentStatus + "(Comment : " + x.GMDocumentComment + ")" : x.GMDocumentStatus,

                            ApplicantDocument = new ApplicantUploadDocumentModel()
                            {
                                ApplicantEduTechQualificationFileName = x.ApplicantDocument.ApplicantEduTechQualificationFileName,
                                ScanPANFileType = x.ApplicantDocument.ScanPANFileType,
                                ScanPANFileName = x.ApplicantDocument.ScanPANFileName,
                                ScanIDFileType = x.ApplicantDocument.ScanIDFileType,
                                ScanIDFileName = x.ApplicantDocument.ScanIDFileName,
                                ScanCastCertFileType = x.ApplicantDocument.ScanCastCertFileType,
                                ApplicantEduTechQualificationFileType = x.ApplicantDocument.ApplicantEduTechQualificationFileType,
                                ApplicantPhotoFileName = x.ApplicantDocument.ApplicantPhotoFileName,
                                ApplicantPhotoFileType = x.ApplicantDocument.ApplicantPhotoFileType,
                                ApplicantSignatureFileName = x.ApplicantDocument.ApplicantSignatureFileName,
                                ApplicantSignatureFileType = x.ApplicantDocument.ApplicantSignatureFileType,
                                BalanceSheetFileName = x.ApplicantDocument.BalanceSheetFileName,
                                BalanceSheetFileType = x.ApplicantDocument.BalanceSheetFileType,
                                BankVerifiedSignatureFileName = x.ApplicantDocument.BankVerifiedSignatureFileName,
                                BankVerifiedSignatureFileType = x.ApplicantDocument.BankVerifiedSignatureFileType,
                                DocProofForIndustrialEstablishedOutsideGidaFileName = x.ApplicantDocument.DocProofForIndustrialEstablishedOutsideGidaFileName,
                                DocProofForIndustrialEstablishedOutsideGidaFileType = x.ApplicantDocument.DocProofForIndustrialEstablishedOutsideGidaFileType,
                                ExperienceProofFileName = x.ApplicantDocument.ExperienceProofFileName,
                                ExperienceProofFileType = x.ApplicantDocument.ExperienceProofFileType,
                                FinDetailsEstablishedIndustriesFileName = x.ApplicantDocument.FinDetailsEstablishedIndustriesFileName,
                                FinDetailsEstablishedIndustriesFileType = x.ApplicantDocument.FinDetailsEstablishedIndustriesFileType,
                                ITReturnFileName = x.ApplicantDocument.ITReturnFileName,
                                ITReturnFileType = x.ApplicantDocument.ITReturnFileType,
                                MemorendumFileName = x.ApplicantDocument.MemorendumFileName,
                                MemorendumFileType = x.ApplicantDocument.MemorendumFileType,
                                OtherDocForProposedIndustryFileName = x.ApplicantDocument.OtherDocForProposedIndustryFileName,
                                OtherDocForProposedIndustryFileType = x.ApplicantDocument.OtherDocForProposedIndustryFileType,
                                PreEstablishedIndustriesDocFileName = x.ApplicantDocument.PreEstablishedIndustriesDocFileName,
                                PreEstablishedIndustriesDocFileType = x.ApplicantDocument.PreEstablishedIndustriesDocFileType,
                                ProjectReportFileName = x.ApplicantDocument.ProjectReportFileName,
                                ProjectReportFileType = x.ApplicantDocument.ProjectReportFileType,
                                ProposedPlanLandUsesFileName = x.ApplicantDocument.ProposedPlanLandUsesFileName,
                                ProposedPlanLandUsesFileType = x.ApplicantDocument.ProposedPlanLandUsesFileType,
                                ScanAddressProofFileName = x.ApplicantDocument.ScanAddressProofFileName,
                                ScanAddressProofFileType = x.ApplicantDocument.ScanAddressProofFileType,
                                ScanCastCertFileName = x.ApplicantDocument.ScanCastCertFileName
                            }
                        }).ToList();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Elmah.ErrorLog.GetDefault(HttpContext.Current).Log(new Elmah.Error(e));
                    }
                }
                return new List<ApplicationUserModel>();
            }
        }

        public Enums.CrudStatus ActivateDeActivateUser(int userId)
        {
            try
            {
                _db = new GidaGKPEntities();
                int _effectRow = 0;
                var applicantUser = _db.ApplicantUsers.Where(x => x.Id == userId).FirstOrDefault();
                if (applicantUser != null)
                {
                    applicantUser.IsActive = !applicantUser.IsActive;
                    _db.Entry(applicantUser).State = EntityState.Modified;
                    _effectRow = _db.SaveChanges();
                }
                return _effectRow > 0 ? Enums.CrudStatus.Saved : Enums.CrudStatus.NotSaved;
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Elmah.ErrorLog.GetDefault(HttpContext.Current).Log(new Elmah.Error(e));
                    }
                }
                return Enums.CrudStatus.InternalError;
            }


        }

        public Enums.CrudStatus SaveNotice(AdminNotice notice)
        {
            try
            {
                _db = new GidaGKPEntities();
                int _effectRow = 0;
                if (notice.Id > 0)
                {
                    var adminNotice = _db.AdminNotices.Where(x => x.Id == notice.Id).FirstOrDefault();
                    if (adminNotice != null)
                    {
                        adminNotice.IsActive = notice.IsActive;
                        adminNotice.Department = notice.Department;
                        if (notice.NoticeDocumentFile != null)
                        {
                            adminNotice.NoticeDocumentFile = notice.NoticeDocumentFile;
                            adminNotice.NoticeDocumentFileType = notice.NoticeDocumentFileType;
                            adminNotice.NoticeDocumentName = notice.NoticeDocumentName;
                        }
                        adminNotice.NoticeNewTag = notice.NoticeNewTag;
                        adminNotice.Notice_Date = notice.Notice_Date;
                        adminNotice.Notice_title = notice.Notice_title;
                        adminNotice.NoticeTypeId = notice.NoticeTypeId;
                        _db.Entry(adminNotice).State = EntityState.Modified;
                        _effectRow = _db.SaveChanges();
                    }
                }
                else
                {
                    _db.Entry(notice).State = EntityState.Added;
                    _effectRow = _db.SaveChanges();
                }

                return _effectRow > 0 ? Enums.CrudStatus.Saved : Enums.CrudStatus.NotSaved;
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Elmah.ErrorLog.GetDefault(HttpContext.Current).Log(new Elmah.Error(e));
                    }
                }
                return Enums.CrudStatus.InternalError;
            }
        }

        public List<AdminNoticeModel> GetNoticeList()
        {
            try
            {
                _db = new GidaGKPEntities();
                return (from notice in _db.AdminNotices
                        join departmentLookup1 in _db.Lookups on notice.Department equals departmentLookup1.LookupId into departmentLookup2
                        from departmentLookup in departmentLookup2.DefaultIfEmpty()
                        join noticeTypeLookup1 in _db.Lookups on notice.NoticeTypeId equals noticeTypeLookup1.LookupId into noticeTypeLookup2
                        from noticeTypeLookup in noticeTypeLookup2.DefaultIfEmpty()
                        select new AdminNoticeModel
                        {
                            NoticeTypeId = notice.NoticeTypeId,
                            DepartmentName = departmentLookup.LookupName,
                            Id = notice.Id,
                            IsActive = notice.IsActive,
                            NoticeDocumentFile = notice.NoticeDocumentFile,
                            NoticeDocumentFileType = notice.NoticeDocumentFileType,
                            NoticeDocumentName = notice.NoticeDocumentName,
                            NoticeNewTag = notice.NoticeNewTag,
                            Notice_Date = notice.Notice_Date,
                            Notice_title = notice.Notice_title,
                            Notice_Type = noticeTypeLookup.LookupName,
                            CreationDate = notice.CreationDate
                        }).Distinct().ToList();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Elmah.ErrorLog.GetDefault(HttpContext.Current).Log(new Elmah.Error(e));
                    }
                }
                return new List<AdminNoticeModel>();
            }
        }

        public AdminNoticeModel GetNoticeById(int noticeId)
        {
            try
            {
                _db = new GidaGKPEntities();
                return (from notice in _db.AdminNotices
                        join departmentLookup1 in _db.Lookups on notice.Department equals departmentLookup1.LookupId into departmentLookup2
                        from departmentLookup in departmentLookup2.DefaultIfEmpty()
                        join noticeTypeLookup1 in _db.Lookups on notice.NoticeTypeId equals noticeTypeLookup1.LookupId into noticeTypeLookup2
                        from noticeTypeLookup in noticeTypeLookup2.DefaultIfEmpty()
                        where notice.Id == noticeId
                        select new AdminNoticeModel
                        {
                            NoticeTypeId = notice.NoticeTypeId,
                            DepartmentName = departmentLookup.LookupName,
                            Department = notice.Department,
                            Id = notice.Id,
                            IsActive = notice.IsActive,
                            NoticeDocumentFile = notice.NoticeDocumentFile,
                            NoticeDocumentFileType = notice.NoticeDocumentFileType,
                            NoticeDocumentName = notice.NoticeDocumentName,
                            NoticeNewTag = notice.NoticeNewTag,
                            Notice_Date = notice.Notice_Date,
                            Notice_title = notice.Notice_title,
                            Notice_Type = noticeTypeLookup.LookupName
                        }).FirstOrDefault();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Elmah.ErrorLog.GetDefault(HttpContext.Current).Log(new Elmah.Error(e));
                    }
                }
                return new AdminNoticeModel();
            }
        }

        public List<AdminNoticeModel> GetNoticeByType(string NoticeType)
        {
            try
            {
                _db = new GidaGKPEntities();
                return (from notice in _db.AdminNotices
                        join noticeTypeLookup in _db.Lookups on notice.NoticeTypeId equals noticeTypeLookup.LookupId
                        join departmentLookup1 in _db.Lookups on notice.Department equals departmentLookup1.LookupId into departmentLookup2
                        from departmentLookup in departmentLookup2.DefaultIfEmpty()
                        where noticeTypeLookup.LookupName == NoticeType && notice.IsActive == true
                        select new AdminNoticeModel
                        {
                            NoticeTypeId = notice.NoticeTypeId,
                            DepartmentName = departmentLookup.LookupName,
                            Department = notice.Department,
                            Id = notice.Id,
                            IsActive = notice.IsActive,
                            NoticeDocumentFile = notice.NoticeDocumentFile,
                            NoticeDocumentFileType = notice.NoticeDocumentFileType,
                            NoticeDocumentName = notice.NoticeDocumentName,
                            NoticeNewTag = notice.NoticeNewTag,
                            Notice_Date = notice.Notice_Date,
                            Notice_title = NoticeType == "Latest Notice" && notice.Notice_title.Length > 95 ? notice.Notice_title.Substring(0, 95) : notice.Notice_title,
                            Notice_Type = noticeTypeLookup.LookupName,
                            CreationDate = notice.CreationDate
                        }).OrderByDescending(x => x.CreationDate).ToList();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Elmah.ErrorLog.GetDefault(HttpContext.Current).Log(new Elmah.Error(e));
                    }
                }
                return new List<AdminNoticeModel>();
            }
        }
        public List<GidaUserModel> GetGidaUsers()
        {
            try
            {
                _db = new GidaGKPEntities();
                return (from user in _db.GidaUsers
                        join departmentLookup in _db.Lookups on user.Department equals departmentLookup.LookupId
                        join designationLookup in _db.Lookups on user.Designation equals designationLookup.LookupId
                        join roleLookup in _db.Lookups on user.UserRoleId equals roleLookup.LookupId
                        where roleLookup.LookupName != "Super Admin"
                        select new GidaUserModel
                        {
                            Department = departmentLookup.LookupName,
                            Designation = designationLookup.LookupName,
                            Email = user.Email,
                            Id = user.Id,
                            IsActive = user.IsActive,
                            MobileNo = user.MobileNo,
                            Name = user.Name,
                            Password = user.Password,
                            UserName = user.UserName,
                            UserRole = roleLookup.LookupName
                        }).OrderByDescending(x => x.Name).ToList();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Elmah.ErrorLog.GetDefault(HttpContext.Current).Log(new Elmah.Error(e));
                    }
                }
                return new List<GidaUserModel>();
            }
        }

        public Enums.CrudStatus SaveGidaUser(GidaUser user)
        {
            try
            {
                _db = new GidaGKPEntities();
                int _effectRow = 0;
                if (user.Id > 0)
                {
                    var gidaUser = _db.GidaUsers.Where(x => x.Id == user.Id).FirstOrDefault();
                    if (gidaUser != null)
                    {
                        gidaUser.IsActive = user.IsActive;
                        gidaUser.Department = user.Department;
                        gidaUser.Designation = user.Designation;
                        gidaUser.Email = user.Email;
                        gidaUser.MobileNo = user.MobileNo;
                        gidaUser.Name = user.Name;
                        gidaUser.UserName = user.UserName;
                        gidaUser.UserRoleId = user.UserRoleId;
                        _db.Entry(gidaUser).State = EntityState.Modified;
                        _effectRow = _db.SaveChanges();
                    }
                }
                else
                {
                    _db.Entry(user).State = EntityState.Added;
                    _effectRow = _db.SaveChanges();
                }

                return _effectRow > 0 ? Enums.CrudStatus.Saved : Enums.CrudStatus.NotSaved;
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Elmah.ErrorLog.GetDefault(HttpContext.Current).Log(new Elmah.Error(e));
                    }
                }
                return Enums.CrudStatus.InternalError;
            }
        }
        public GidaUserPermissionModel GetUserDetail(int userId)
        {
            try
            {
                _db = new GidaGKPEntities();
                var model = new GidaUserPermissionModel();
                model.UserModel = (from user in _db.GidaUsers
                                   join departmentLookup in _db.Lookups on user.Department equals departmentLookup.LookupId
                                   join designationLookup in _db.Lookups on user.Designation equals designationLookup.LookupId
                                   join roleLookup in _db.Lookups on user.UserRoleId equals roleLookup.LookupId
                                   where user.Id == userId
                                   select new GidaUserModel
                                   {
                                       Department = departmentLookup.LookupName,
                                       Designation = designationLookup.LookupName,
                                       Email = user.Email,
                                       Id = user.Id,
                                       IsActive = user.IsActive,
                                       MobileNo = user.MobileNo,
                                       Name = user.Name,
                                       Password = user.Password,
                                       UserName = user.UserName,
                                       UserRole = roleLookup.LookupName,
                                       DepartmentId = departmentLookup.LookupId,
                                       DesignationId = designationLookup.LookupId,
                                       UserRoleId = roleLookup.LookupId,
                                   }).FirstOrDefault();
                return model;

            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Elmah.ErrorLog.GetDefault(HttpContext.Current).Log(new Elmah.Error(e));
                    }
                }
                return null;
            }
        }
        public GidaUserPermissionModel GetRoleWisePermission(int departmentId, int designationId, int roleId)
        {
            try
            {
                _db = new GidaGKPEntities();
                var model = new GidaUserPermissionModel();
                var PageList = (from page in _db.PageMasters
                                where page.IsActive == true
                                select new UserPermissionModel
                                {
                                    PageId = page.Id,
                                    PageName = page.PageName
                                }).ToList();
                PageList.ForEach(x =>
                {
                    List<RoleWisePermission> roles = _db.RoleWisePermissions.Where(y => y.DepartmentId == departmentId && y.DesignationId == designationId && y.RoleId == roleId && y.PageId == x.PageId).ToList();
                    if (roles.Any())
                    {
                        roles.ForEach(y =>
                        {
                            x.PermissionTypeIdList.Add(y.PermissionId.Value);
                        });
                    }
                });

                model.PermissionModel = PageList;
                return model;

            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Elmah.ErrorLog.GetDefault(HttpContext.Current).Log(new Elmah.Error(e));
                    }
                }
                return null;
            }
        }
        public List<PageMasterModel> GePageMaster()
        {
            try
            {
                _db = new GidaGKPEntities();
                return (from page in _db.PageMasters
                        select new PageMasterModel
                        {
                            Id = page.Id,
                            PageName = page.PageName,
                            IsActive = page.IsActive
                        }).OrderByDescending(x => x.PageName).ToList();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Elmah.ErrorLog.GetDefault(HttpContext.Current).Log(new Elmah.Error(e));
                    }
                }
                return new List<PageMasterModel>();
            }
        }

        public Enums.CrudStatus SavePageMaster(PageMaster page)
        {
            try
            {
                _db = new GidaGKPEntities();
                int _effectRow = 0;
                if (page.Id > 0)
                {
                    var pages = _db.PageMasters.Where(x => x.Id == page.Id).FirstOrDefault();
                    if (pages != null)
                    {
                        pages.IsActive = page.IsActive;
                        pages.PageName = page.PageName;
                        _db.Entry(pages).State = EntityState.Modified;
                        _effectRow = _db.SaveChanges();
                    }
                }
                else
                {
                    _db.Entry(page).State = EntityState.Added;
                    _effectRow = _db.SaveChanges();
                }

                return _effectRow > 0 ? Enums.CrudStatus.Saved : Enums.CrudStatus.NotSaved;
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Elmah.ErrorLog.GetDefault(HttpContext.Current).Log(new Elmah.Error(e));
                    }
                }
                return Enums.CrudStatus.InternalError;
            }
        }
        public PageMaster GetPageDetail(int pageId)
        {
            try
            {
                _db = new GidaGKPEntities();
                return _db.PageMasters.Where(x => x.Id == pageId).FirstOrDefault();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Elmah.ErrorLog.GetDefault(HttpContext.Current).Log(new Elmah.Error(e));
                    }
                }
                return null;
            }
        }

        public Enums.CrudStatus SaveUserPermission(List<RoleWisePermission> permissionList)
        {
            try
            {
                _db = new GidaGKPEntities();
                int _effectRow = 0;
                if (permissionList.Any())
                {
                    var roleId = permissionList[0].RoleId;
                    var DepartmentId = permissionList[0].DepartmentId;
                    var DesignationId = permissionList[0].DesignationId;
                    var dataOfRole = _db.RoleWisePermissions.Where(x => x.RoleId == roleId && x.DepartmentId == DepartmentId && x.DesignationId == DesignationId).ToList();
                    dataOfRole.ForEach(x =>
                    {
                        _db.Entry(x).State = EntityState.Deleted;
                    });
                    _db.SaveChanges();
                }

                permissionList.ForEach(permissionData =>
                {
                    permissionData.CreatedBy = UserData.UserId;
                    permissionData.CreatedDate = DateTime.Now;
                    _db.Entry(permissionData).State = EntityState.Added;
                    _effectRow = _db.SaveChanges();
                });

                return _effectRow > 0 ? Enums.CrudStatus.Saved : Enums.CrudStatus.NotSaved;
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Elmah.ErrorLog.GetDefault(HttpContext.Current).Log(new Elmah.Error(e));
                    }
                }
                return Enums.CrudStatus.InternalError;
            }
        }

        public Enums.CrudStatus SavePlotMaster(List<PlotMaster> plotMaster)
        {
            try
            {
                _db = new GidaGKPEntities();
                int _effectRow = 0;
                if (plotMaster.Any())
                {
                    var schemeType = plotMaster[0].SchemeType;
                    var schemeName = plotMaster[0].SchemeName;
                    var sectorName = plotMaster[0].SectorName;
                    var dataOfPlots = _db.PlotMasters.Where(x => x.SchemeType == schemeType && x.SchemeName == schemeName && x.SectorName == sectorName).ToList();
                    dataOfPlots.ForEach(x =>
                    {
                        _db.Entry(x).State = EntityState.Deleted;
                    });
                    _db.SaveChanges();
                }

                plotMaster.ForEach(plotData =>
                {
                    plotData.CreatedBy = UserData.UserId;
                    plotData.CreatedDate = DateTime.Now;
                    _db.Entry(plotData).State = EntityState.Added;
                    _effectRow = _db.SaveChanges();
                });

                return _effectRow > 0 ? Enums.CrudStatus.Saved : Enums.CrudStatus.NotSaved;
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Elmah.ErrorLog.GetDefault(HttpContext.Current).Log(new Elmah.Error(e));
                    }
                }
                return Enums.CrudStatus.InternalError;
            }
        }

        public Enums.CrudStatus ApproveRejectPayment(int applicationId, string status, string comment = "")
        {
            try
            {
                _db = new GidaGKPEntities();
                int _effectRow = 0;
                if (applicationId > 0)
                {
                    var transactionDetail = _db.ApplicantTransactionDetails.Where(x => x.ApplicationId == applicationId).FirstOrDefault();
                    if (transactionDetail != null)
                    {
                        if (UserData.Designation == "Assistant Manager")
                        {
                            transactionDetail.AMApprovalStatus = status;
                            transactionDetail.AMComment = comment;
                        }
                        else if (UserData.Designation == "Manager")
                        {
                            transactionDetail.CEOApprovalStatus = status;
                            transactionDetail.CEOComment = comment;
                        }
                        else if (UserData.Designation == "General Manager")
                        {
                            transactionDetail.GMApprovalStatus = status;
                            transactionDetail.GMComment = comment;
                        }

                        _db.Entry(transactionDetail).State = EntityState.Modified;
                        _effectRow = _db.SaveChanges();
                        return Enums.CrudStatus.Updated;
                    }
                }
                return Enums.CrudStatus.InternalError;
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Elmah.ErrorLog.GetDefault(HttpContext.Current).Log(new Elmah.Error(e));
                    }
                }
                return Enums.CrudStatus.InternalError;
            }
        }
        public Enums.CrudStatus ApproveRejectDocument(int applicationId, string status, string comment = "")
        {
            try
            {
                _db = new GidaGKPEntities();
                int _effectRow = 0;
                if (applicationId > 0)
                {
                    var documentDetail = _db.ApplicantUploadDocs.Where(x => x.ApplicationId == applicationId).FirstOrDefault();
                    if (documentDetail != null)
                    {
                        if (UserData.Designation == "Assistant Manager")
                        {
                            documentDetail.AMApprovalStatus = status;
                            documentDetail.AMComment = comment;
                        }
                        else if (UserData.Designation == "Clerk")
                        {
                            documentDetail.ClerkApprovalStatus = status;
                            documentDetail.ClerkComment = comment;
                        }
                        else if (UserData.Designation == "Section Incharge")
                        {
                            documentDetail.SIApprovalStatus = status;
                            documentDetail.SIComment = comment;
                        }

                        _db.Entry(documentDetail).State = EntityState.Modified;
                        _effectRow = _db.SaveChanges();
                        return Enums.CrudStatus.Updated;
                    }
                }
                return Enums.CrudStatus.InternalError;
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Elmah.ErrorLog.GetDefault(HttpContext.Current).Log(new Elmah.Error(e));
                    }
                }
                return Enums.CrudStatus.InternalError;
            }
        }

        public Enums.CrudStatus ApproveRejectApplication(int applicationId, string status, string comment = "")
        {
            try
            {
                _db = new GidaGKPEntities();
                int _effectRow = 0;
                if (applicationId > 0)
                {
                    var documentDetail = _db.ApplicantUploadDocs.Where(x => x.ApplicationId == applicationId).FirstOrDefault();
                    if (documentDetail != null)
                    {
                        if (UserData.Designation == "General Manager")
                        {
                            documentDetail.GMApprovalStatus = status;
                            documentDetail.GMComment = comment;
                        }
                        else if (UserData.Designation == "CEO")
                        {
                            documentDetail.CEOApprovalStatus = status;
                            documentDetail.CEOComment = comment;
                        }
                        _db.Entry(documentDetail).State = EntityState.Modified;
                        _effectRow = _db.SaveChanges();

                        var paymentDetail = _db.ApplicantTransactionDetails.Where(x => x.ApplicationId == applicationId).FirstOrDefault();
                        if (paymentDetail != null)
                        {
                            if (UserData.Designation == "General Manager")
                            {
                                paymentDetail.GMApprovalStatus = status;
                                paymentDetail.GMComment = comment;
                            }
                            else if (UserData.Designation == "CEO")
                            {
                                paymentDetail.CEOApprovalStatus = status;
                                paymentDetail.CEOComment = comment;
                            }
                            _db.Entry(documentDetail).State = EntityState.Modified;
                            _effectRow = _db.SaveChanges();

                        }

                        return Enums.CrudStatus.Updated;
                    }
                }
                return Enums.CrudStatus.InternalError;
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Elmah.ErrorLog.GetDefault(HttpContext.Current).Log(new Elmah.Error(e));
                    }
                }
                return Enums.CrudStatus.InternalError;
            }
        }

        public List<PlotMasterModel> GetPlotMasterDetail(int SchemeType, int SchemeName, int SectorId)
        {
            try
            {
                _db = new GidaGKPEntities();
                return (from plot in _db.PlotMasters
                        where plot.SchemeName == SchemeName && plot.SchemeType == SchemeType && plot.SectorName == SectorId
                        select new PlotMasterModel
                        {
                            CreatedBy = plot.CreatedBy,
                            CreatedDate = plot.CreatedDate,
                            ExtraCharge = plot.ExtraCharge,
                            GrandTotalCost = plot.GrandTotalCost,
                            NoOfPlots = plot.NoOfPlots,
                            PercentageRate = plot.PercentageRate,
                            PlotArea = plot.PlotArea,
                            PlotCategory = plot.PlotCategory,
                            PlotCost = plot.PlotCost,
                            PlotId = plot.PlotId,
                            PlotRange = plot.PlotRange,
                            PlotRate = plot.PlotRate,
                            PlotSideCorner = plot.PlotSideCorner,
                            PlotSideParkFacing = plot.PlotSideParkFacing,
                            PlotSideWideRoad = plot.PlotSideWideRoad,
                            RateForPlotSide = plot.RateForPlotSide,
                            SchemeName = plot.SchemeName,
                            SchemeType = plot.SchemeType,
                            SectorName = plot.SectorName,
                            UserId = plot.UserId
                        }).ToList();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Elmah.ErrorLog.GetDefault(HttpContext.Current).Log(new Elmah.Error(e));
                    }
                }
                return new List<PlotMasterModel>();
            }
        }

        public ApplicantUploadDoc GetDocumentByApplciationId(int applicantId)
        {
            try
            {
                _db = new GidaGKPEntities();
                return _db.ApplicantUploadDocs.Where(x => x.ApplicationId == applicantId).FirstOrDefault();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Elmah.ErrorLog.GetDefault(HttpContext.Current).Log(new Elmah.Error(e));
                    }
                }
                return new ApplicantUploadDoc();
            }
        }

        public List<ApplicationUserModel> GetApplicantSubmittedForInterview()
        {
            try
            {
                _db = new GidaGKPEntities();
                return (from user in _db.ApplicantUsers
                        join appLetter in _db.ApplicantInvitationLetters on user.Id equals appLetter.UserId
                        join applicant1 in _db.ApplicantDetails on user.Id equals applicant1.UserId into applicant2
                        from applicant in applicant2.DefaultIfEmpty()
                        join application1 in _db.ApplicantApplicationDetails on user.Id equals application1.UserId into application2
                        from application in application2.DefaultIfEmpty()
                        where user.UserType != "Test"
                        select new
                        {
                            PlotNumber = appLetter.TotalNoOfPlots,
                            ApplicationNumber = application != null ? application.ApplicationNumber : "",
                            ApplicationId = application != null ? application.ApplicationId : 0,
                            AadharNumber = user.AadharNumber,
                            ContactNo = user.ContactNo,
                            CreationDate = user.CreationDate,
                            Email = user.Email,
                            FatherName = applicant != null ? applicant.FName : "",
                            CAddress = applicant != null ? applicant.CAddress : "",
                            PAddress = applicant != null ? applicant.PAddress : "",
                            FullName = applicant != null ? applicant.FullApplicantName : "",
                            Id = user.Id,
                            SchemeName = user.SchemeName,
                            SchemeType = user.SchemeType,
                            SectorName = user.SectorName,
                            UserType = user.UserType,
                            DOB = user.DOB,
                            UserName = user.UserName,
                            IsActive = user.IsActive,
                            ApplicationStatus = application.ApprovalStatus,
                            InterviewDateTime = appLetter.InterviewDateTime,
                            UserId = user.Id,
                            InterviewLetterStatus = appLetter.InterviewLetterStatus
                        }).Distinct().ToList()
                        .Select(x => new ApplicationUserModel()
                        {
                            PlotNumber = x.PlotNumber,
                            ApplicationNumber = x.ApplicationNumber,
                            ApplicationId = x.ApplicationId,
                            AadharNumber = x.AadharNumber,
                            ContactNo = x.ContactNo,
                            CreationDate = x.CreationDate,
                            Email = x.Email,
                            FatherName = x.FatherName,
                            CAddress = x.CAddress,
                            PAddress = x.PAddress,
                            FullName = x.FullName,
                            Id = x.Id,
                            SchemeName = x.SchemeName,
                            SchemeType = x.SchemeType,
                            SectorName = x.SectorName,
                            UserType = x.UserType,
                            DOB = x.DOB != null ? x.DOB.Value.ToString("dd/MM/yyyy") : string.Empty,
                            InterviewDateTime = x.InterviewDateTime != null ? x.InterviewDateTime.Value.ToString("dd/MM/yyyy") : string.Empty,
                            UserName = x.UserName,
                            IsActive = x.IsActive,
                            ApplicationStatus = x.ApplicationStatus,
                            UserId = x.UserId,
                            InterviewLetterStatus = x.InterviewLetterStatus
                        }).ToList();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Elmah.ErrorLog.GetDefault(HttpContext.Current).Log(new Elmah.Error(e));
                    }
                }
                return new List<ApplicationUserModel>();
            }
        }

        public Enums.CrudStatus ApplicationInvitationLetterStatusChnage(int userid, string status, string comment = "")
        {
            try
            {
                _db = new GidaGKPEntities();
                int _effectRow = 0;
                if (userid > 0)
                {
                    var inviationDetail = _db.ApplicantInvitationLetters.Where(x => x.UserId == userid).FirstOrDefault();
                    if (inviationDetail != null)
                    {
                        inviationDetail.InterviewLetterStatus = status;
                        //if (UserData.Designation == "Assistant Manager")
                        //{
                        //    transactionDetail.AMApprovalStatus = status;
                        //    transactionDetail.AMComment = comment;
                        //}
                        //else if (UserData.Designation == "Manager")
                        //{
                        //    transactionDetail.MApprovalStatus = status;
                        //    transactionDetail.MComment = comment;
                        //}
                        //else if (UserData.Designation == "General Manager")
                        //{
                        //    transactionDetail.GMApprovalStatus = status;
                        //    transactionDetail.GMComment = comment;
                        //}

                        _db.Entry(inviationDetail).State = EntityState.Modified;
                        _effectRow = _db.SaveChanges();
                        return Enums.CrudStatus.Updated;
                    }
                }
                return Enums.CrudStatus.InternalError;
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Elmah.ErrorLog.GetDefault(HttpContext.Current).Log(new Elmah.Error(e));
                    }
                }
                return Enums.CrudStatus.InternalError;
            }
        }
    }
}