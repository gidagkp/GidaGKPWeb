using System;
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
    public class ApplicantDetails
    {
        GidaGKPEntities _db = null;

        public int SaveApplicantTransactionDeatil(ApplicantTransactionDetail detail)
        {
            try
            {
                _db = new GidaGKPEntities();
                _db.Entry(detail).State = EntityState.Added;
                return _db.SaveChanges();
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
                return 0;
            }
        }
        public string SavePlotDetail(int userId, string AppliedFor, string SchemeType, string PlotRange, string SchemeName, string plotArea, string SectorName, string SectorDescription, string EstimatedRate, string PaymemtSchedule, string TotalInvestment, string ApplicationFee, string EarnestMoneyDeposite, string GST, string NetAmount, string TotalAmount, string IndustryOwnershipType, string UnitName, string Name, string dob, string PresentAddress, string PermanentAddress, string RelationshipStatus)
        {
            try
            {
                _db = new GidaGKPEntities();
                int _effectRow = 0;
                ApplicantApplicationDetail app = new ApplicantApplicationDetail();

                var existingPlotDetail = _db.ApplicantPlotDetails.Where(x => x.ApplicationId == UserData.ApplicationId).FirstOrDefault();
                if (existingPlotDetail != null)  // update plot detail
                {
                    app.ApplicationNumber = _db.ApplicantApplicationDetails.Where(x => x.ApplicationId == UserData.ApplicationId).FirstOrDefault().ApplicationNumber;
                    existingPlotDetail.ApplicationFee = Convert.ToDecimal(ApplicationFee);
                    existingPlotDetail.UserId = userId;
                    existingPlotDetail.AppliedFor = Convert.ToInt32(AppliedFor);
                    existingPlotDetail.EarnestMoney = Convert.ToDecimal(EarnestMoneyDeposite);
                    existingPlotDetail.EstimatedRate = EstimatedRate;
                    existingPlotDetail.GST = Convert.ToDecimal(GST);
                    existingPlotDetail.IndustryOwnership = Convert.ToInt32(IndustryOwnershipType);
                    existingPlotDetail.NetAmount = Convert.ToDecimal(NetAmount);
                    existingPlotDetail.PaymentSchedule = Convert.ToInt32(PaymemtSchedule);
                    existingPlotDetail.PlotArea = plotArea;
                    existingPlotDetail.PlotRange = Convert.ToInt32(PlotRange);
                    existingPlotDetail.RelationshipStatus = Convert.ToInt32(RelationshipStatus);
                    existingPlotDetail.SchemeName = Convert.ToInt32(SchemeName);
                    existingPlotDetail.SchemeType = Convert.ToInt32(SchemeType);
                    existingPlotDetail.SectorName = Convert.ToInt32(SectorName);
                    existingPlotDetail.SignatryDateOfBirth = Convert.ToDateTime(dob);
                    existingPlotDetail.SignatryName = Name;
                    existingPlotDetail.SignatryPermanentAddress = PermanentAddress;
                    existingPlotDetail.SignatryPresentAddress = PresentAddress;
                    existingPlotDetail.TotalAmount = Convert.ToDecimal(TotalAmount);
                    existingPlotDetail.TotalInvestment = Convert.ToDecimal(TotalInvestment);
                    existingPlotDetail.UnitName = UnitName;
                    existingPlotDetail.ApplicationId = UserData.ApplicationId;
                    _db.Entry(existingPlotDetail).State = EntityState.Modified;
                    _effectRow = _db.SaveChanges();
                }
                else //save plot detail
                {
                    var applciationDeatil = _db.ApplicantApplicationDetails.OrderByDescending(x => x.ApplicationId).Take(1).FirstOrDefault();
                    int maxId = 0;
                    if (applciationDeatil != null)
                    {
                        maxId = applciationDeatil.ApplicationId + 1;
                    }
                    else
                    {
                        maxId = 1;
                    }
                    app.UserId = userId;
                    app.ApplicationNumber = DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + SectorDescription.Replace("Sector ", "") + maxId.ToString().PadLeft(4, '0');
                    _db.Entry(app).State = EntityState.Added;
                    _db.SaveChanges();
                    UserData.ApplicationId = app.ApplicationId;

                    ApplicantFormStep step = new ApplicantFormStep()
                    {
                        UserId = userId,
                        ApplicantStepCompleted = 1,
                        ApplicationId = UserData.ApplicationId
                    };
                    _db.Entry(step).State = EntityState.Added;

                    ApplicantPlotDetail _newRecord = new ApplicantPlotDetail()
                    {
                        ApplicationFee = Convert.ToDecimal(ApplicationFee),
                        UserId = userId,
                        AppliedFor = Convert.ToInt32(AppliedFor),
                        CreationDate = DateTime.Now,
                        EarnestMoney = Convert.ToDecimal(EarnestMoneyDeposite),
                        EstimatedRate = EstimatedRate,
                        GST = Convert.ToDecimal(GST),
                        IndustryOwnership = Convert.ToInt32(IndustryOwnershipType),
                        NetAmount = Convert.ToDecimal(NetAmount),
                        PaymentSchedule = Convert.ToInt32(PaymemtSchedule),
                        PlotArea = plotArea,
                        PlotRange = Convert.ToInt32(PlotRange),
                        RelationshipStatus = Convert.ToInt32(RelationshipStatus),
                        SchemeName = Convert.ToInt32(SchemeName),
                        SchemeType = Convert.ToInt32(SchemeType),
                        SectorName = Convert.ToInt32(SectorName),
                        SignatryDateOfBirth = Convert.ToDateTime(dob),
                        SignatryName = Name,
                        SignatryPermanentAddress = PermanentAddress,
                        SignatryPresentAddress = PresentAddress,
                        TotalAmount = Convert.ToDecimal(TotalAmount),
                        TotalInvestment = Convert.ToDecimal(TotalInvestment),
                        UnitName = UnitName,
                        ApplicationId = UserData.ApplicationId
                    };

                    _db.Entry(_newRecord).State = EntityState.Added;
                    _effectRow = _db.SaveChanges();
                }


                return _effectRow > 0 ? app.ApplicationNumber : "Error";
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
                return "Error";
            }


        }

        public Enums.CrudStatus SaveApplicantDetail(int userId, string FullName, string FName, string MName, string SName, string DOB, string Gender, string Category, string Nationality, string AdhaarNo, string PAN, string MobileNo, string Phone, string Email, string Religion, string SubCategory, string CAddress, string PAddress, string IdentityProof, string ResidentialProof)
        {
            try
            {
                _db = new GidaGKPEntities();

                ApplicantFormStep step = new ApplicantFormStep()
                {
                    UserId = userId,
                    ApplicantStepCompleted = 2,
                    ApplicationId = UserData.ApplicationId
                };
                _db.Entry(step).State = EntityState.Added;

                int _effectRow = 0;

                var extingApplicantDetail = _db.ApplicantDetails.Where(x => x.ApplicationId == UserData.ApplicationId).FirstOrDefault();
                if (extingApplicantDetail != null)  // update Applicant detail
                {
                    extingApplicantDetail.AdhaarNumber = AdhaarNo;
                    extingApplicantDetail.UserId = userId;
                    extingApplicantDetail.SubCategory = SubCategory;
                    extingApplicantDetail.ApplicantDOB = Convert.ToDateTime(DOB);
                    extingApplicantDetail.CAddress = CAddress;
                    extingApplicantDetail.Category = Category;
                    extingApplicantDetail.CreationDate = DateTime.Now;
                    extingApplicantDetail.EmailId = Email;
                    extingApplicantDetail.FName = FName;
                    extingApplicantDetail.FullApplicantName = FullName;
                    extingApplicantDetail.Gender = Gender;
                    extingApplicantDetail.IdentiyProof = IdentityProof;
                    extingApplicantDetail.MName = MName;
                    extingApplicantDetail.Mobile = MobileNo;
                    extingApplicantDetail.Nationality = Nationality;
                    extingApplicantDetail.PAddress = PAddress;
                    extingApplicantDetail.PAN = PAN;
                    extingApplicantDetail.Phone = Phone;
                    extingApplicantDetail.Religion = Religion;
                    extingApplicantDetail.ResidentialProof = ResidentialProof;
                    extingApplicantDetail.SName = SName;
                    extingApplicantDetail.ApplicationId = UserData.ApplicationId;
                    _db.Entry(extingApplicantDetail).State = EntityState.Modified;
                    _effectRow = _db.SaveChanges();
                }
                else
                {
                    ApplicantDetail _newRecord = new ApplicantDetail()
                    {
                        AdhaarNumber = AdhaarNo,
                        UserId = userId,
                        SubCategory = SubCategory,
                        ApplicantDOB = Convert.ToDateTime(DOB),
                        CAddress = CAddress,
                        Category = Category,
                        CreationDate = DateTime.Now,
                        EmailId = Email,
                        FName = FName,
                        FullApplicantName = FullName,
                        Gender = Gender,
                        IdentiyProof = IdentityProof,
                        MName = MName,
                        Mobile = MobileNo,
                        Nationality = Nationality,
                        PAddress = PAddress,
                        PAN = PAN,
                        Phone = Phone,
                        Religion = Religion,
                        ResidentialProof = ResidentialProof,
                        SName = SName,
                        ApplicationId = UserData.ApplicationId
                    };
                    _db.Entry(_newRecord).State = EntityState.Added;
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

        public Enums.CrudStatus SaveProjectDetail(int userId, string ProposedIndustryType, string ProjectEstimatedCost, string ProposedCoveredArea, string ProposedOpenArea, string PurpuseOpenArea, string ProposedInvestmentLand, string ProposedInvestmentBuilding, string ProposedInvestmentPlant, string FumesNatureQuantity, string LiquidQuantity, string LiquidChemicalComposition, string SolidQuantity, string SolidChemicalComposition, string GasQuantity, string GasChemicalComposition, string PowerRequirement, string FirstYearNoOfTelephone, string FirstYearNoOfFax, string UltimateNoOfTelephone, string UltimateNoOfFax, string Skilled, string UnSkilled)
        {
            try
            {
                _db = new GidaGKPEntities();

                ApplicantFormStep step = new ApplicantFormStep()
                {
                    UserId = userId,
                    ApplicantStepCompleted = 3,
                    ApplicationId = UserData.ApplicationId
                };
                _db.Entry(step).State = EntityState.Added;

                int _effectRow = 0;

                var extingProjectDetail = _db.ApplicantProjectDetails.Where(x => x.ApplicationId == UserData.ApplicationId).FirstOrDefault();
                if (extingProjectDetail != null)  // update project detail
                {
                    extingProjectDetail.UserId = userId;
                    extingProjectDetail.FirstYearNoOfFax = FirstYearNoOfFax;
                    extingProjectDetail.FirstYearNoOfTelephone = FirstYearNoOfTelephone;
                    extingProjectDetail.FumesNatureQuantity = FumesNatureQuantity;
                    extingProjectDetail.GasChemicalComposition = GasChemicalComposition;
                    extingProjectDetail.GasQuantity = GasQuantity;
                    extingProjectDetail.EffluentTreatmentMeasures = "";
                    extingProjectDetail.LiquidChemicalComposition = LiquidChemicalComposition;
                    extingProjectDetail.LiquidQuantity = LiquidQuantity;
                    extingProjectDetail.PowerRequirement = PowerRequirement;
                    extingProjectDetail.ProjectEstimatedCost = ProjectEstimatedCost;
                    extingProjectDetail.ProposedCoveredArea = ProposedCoveredArea;
                    extingProjectDetail.ProposedIndustryType = ProposedIndustryType;
                    extingProjectDetail.ProposedInvestmentBuilding = ProposedInvestmentBuilding;
                    extingProjectDetail.ProposedInvestmentLand = ProposedInvestmentLand;
                    extingProjectDetail.ProposedInvestmentPlant = ProposedInvestmentPlant;
                    extingProjectDetail.ProposedOpenArea = ProposedOpenArea;
                    extingProjectDetail.PurpuseOpenArea = PurpuseOpenArea;
                    extingProjectDetail.Skilled = Skilled;
                    extingProjectDetail.SolidChemicalComposition = SolidChemicalComposition;
                    extingProjectDetail.SolidQuantity = SolidQuantity;
                    extingProjectDetail.UltimateNoOfFax = UltimateNoOfFax;
                    extingProjectDetail.UltimateNoOfTelephone = UltimateNoOfTelephone;
                    extingProjectDetail.UnSkilled = UnSkilled;
                    extingProjectDetail.ApplicationId = UserData.ApplicationId;
                    _db.Entry(extingProjectDetail).State = EntityState.Modified;
                    _effectRow = _db.SaveChanges();
                }
                else
                {
                    ApplicantProjectDetail _newRecord = new ApplicantProjectDetail()
                    {
                        UserId = userId,
                        CreationDate = DateTime.Now,
                        FirstYearNoOfFax = FirstYearNoOfFax,
                        FirstYearNoOfTelephone = FirstYearNoOfTelephone,
                        FumesNatureQuantity = FumesNatureQuantity,
                        GasChemicalComposition = GasChemicalComposition,
                        GasQuantity = GasQuantity,
                        EffluentTreatmentMeasures = "",
                        LiquidChemicalComposition = LiquidChemicalComposition,
                        LiquidQuantity = LiquidQuantity,
                        PowerRequirement = PowerRequirement,
                        ProjectEstimatedCost = ProjectEstimatedCost,
                        ProposedCoveredArea = ProposedCoveredArea,
                        ProposedIndustryType = ProposedIndustryType,
                        ProposedInvestmentBuilding = ProposedInvestmentBuilding,
                        ProposedInvestmentLand = ProposedInvestmentLand,
                        ProposedInvestmentPlant = ProposedInvestmentPlant,
                        ProposedOpenArea = ProposedOpenArea,
                        PurpuseOpenArea = PurpuseOpenArea,
                        Skilled = Skilled,
                        SolidChemicalComposition = SolidChemicalComposition,
                        SolidQuantity = SolidQuantity,
                        UltimateNoOfFax = UltimateNoOfFax,
                        UltimateNoOfTelephone = UltimateNoOfTelephone,
                        UnSkilled = UnSkilled,
                        ApplicationId = UserData.ApplicationId
                    };
                    _db.Entry(_newRecord).State = EntityState.Added;
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

        public Enums.CrudStatus SaveBankDetail(int userId, string BankAccountName, string BankAccountNo, string BankName, string BranchName, string BranchAddress, string IFSCCode)
        {
            try
            {
                _db = new GidaGKPEntities();

                ApplicantFormStep step = new ApplicantFormStep()
                {
                    UserId = userId,
                    ApplicantStepCompleted = 4,
                    ApplicationId = UserData.ApplicationId
                };
                _db.Entry(step).State = EntityState.Added;

                int _effectRow = 0;

                var extingBankDetail = _db.ApplicantBankDetails.Where(x => x.ApplicationId == UserData.ApplicationId).FirstOrDefault();
                if (extingBankDetail != null)  // update bank detail
                {
                    extingBankDetail.AccountHolderName = BankAccountName;
                    extingBankDetail.BankAccountNo = BankAccountNo;
                    extingBankDetail.BankName = BankName;
                    extingBankDetail.BBAddress = BranchAddress;
                    extingBankDetail.BBName = BranchName;
                    extingBankDetail.IFSC_Code = IFSCCode;
                    _db.Entry(extingBankDetail).State = EntityState.Modified;
                    _effectRow = _db.SaveChanges();
                }

                else
                {
                    ApplicantBankDetail _newRecord = new ApplicantBankDetail()
                    {
                        UserId = userId,
                        CreationDate = DateTime.Now,
                        AccountHolderName = BankAccountName,
                        BankAccountNo = BankAccountNo,
                        BankName = BankName,
                        BBAddress = BranchAddress,
                        BBName = BranchName,
                        IFSC_Code = IFSCCode,
                        ApplicationId = UserData.ApplicationId
                    };
                    _db.Entry(_newRecord).State = EntityState.Added;
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

        public Enums.CrudStatus SaveApplicantDocument(int userId, ApplicantUploadDoc docDetail)
        {
            try
            {
                _db = new GidaGKPEntities();

                ApplicantFormStep step = new ApplicantFormStep()
                {
                    UserId = userId,
                    ApplicantStepCompleted = 5,
                    ApplicationId = UserData.ApplicationId
                };
                _db.Entry(step).State = EntityState.Added;

                int _effectRow = 0;

                var extingDocumentDetail = _db.ApplicantUploadDocs.Where(x => x.ApplicationId == UserData.ApplicationId).FirstOrDefault();
                if (extingDocumentDetail != null)  // update doc detail
                {
                    if (docDetail.ApplicantEduTechQualification != null)
                    {
                        extingDocumentDetail.ApplicantEduTechQualification = docDetail.ApplicantEduTechQualification;
                        extingDocumentDetail.ApplicantEduTechQualificationFileName = docDetail.ApplicantEduTechQualificationFileName;
                        extingDocumentDetail.ApplicantEduTechQualificationFileType = docDetail.ApplicantEduTechQualificationFileType;
                    }

                    if (docDetail.ApplicantPhoto != null)
                    {
                        extingDocumentDetail.ApplicantPhoto = docDetail.ApplicantPhoto;
                        extingDocumentDetail.ApplicantPhotoFileName = docDetail.ApplicantPhotoFileName;
                        extingDocumentDetail.ApplicantPhotoFileType = docDetail.ApplicantPhotoFileType;
                    }
                    if (docDetail.ApplicantSignature != null)
                    {
                        extingDocumentDetail.ApplicantSignature = docDetail.ApplicantSignature;
                        extingDocumentDetail.ApplicantSignatureFileName = docDetail.ApplicantSignatureFileName;
                        extingDocumentDetail.ApplicantSignatureFileType = docDetail.ApplicantSignatureFileType;
                    }
                    if (docDetail.BalanceSheet != null)
                    {
                        extingDocumentDetail.BalanceSheet = docDetail.BalanceSheet;
                        extingDocumentDetail.BalanceSheetFileName = docDetail.BalanceSheetFileName;
                        extingDocumentDetail.BalanceSheetFileType = docDetail.BalanceSheetFileType;
                    }
                    if (docDetail.DocProofForIndustrialEstablishedOutsideGida != null)
                    {
                        extingDocumentDetail.DocProofForIndustrialEstablishedOutsideGida = docDetail.DocProofForIndustrialEstablishedOutsideGida;
                        extingDocumentDetail.DocProofForIndustrialEstablishedOutsideGidaFileName = docDetail.DocProofForIndustrialEstablishedOutsideGidaFileName;
                        extingDocumentDetail.DocProofForIndustrialEstablishedOutsideGidaFileType = docDetail.DocProofForIndustrialEstablishedOutsideGidaFileType;
                    }
                    if (docDetail.ExperienceProof != null)
                    {
                        extingDocumentDetail.ExperienceProof = docDetail.ExperienceProof;
                        extingDocumentDetail.ExperienceProofFileName = docDetail.ExperienceProofFileName;
                        extingDocumentDetail.ExperienceProofFileType = docDetail.ExperienceProofFileType;
                    }
                    if (docDetail.FinDetailsEstablishedIndustries != null)
                    {
                        extingDocumentDetail.FinDetailsEstablishedIndustries = docDetail.FinDetailsEstablishedIndustries;
                        extingDocumentDetail.FinDetailsEstablishedIndustriesFileName = docDetail.FinDetailsEstablishedIndustriesFileName;
                        extingDocumentDetail.FinDetailsEstablishedIndustriesFileType = docDetail.FinDetailsEstablishedIndustriesFileType;
                    }
                    if (docDetail.ITReturn != null)
                    {
                        extingDocumentDetail.ITReturn = docDetail.ITReturn;
                        extingDocumentDetail.ITReturnFileName = docDetail.ITReturnFileName;
                        extingDocumentDetail.ITReturnFileType = docDetail.ITReturnFileType;
                    }
                    if (docDetail.BankVerifiedSignature != null)
                    {
                        extingDocumentDetail.BankVerifiedSignature = docDetail.BankVerifiedSignature;
                        extingDocumentDetail.BankVerifiedSignatureFileName = docDetail.BankVerifiedSignatureFileName;
                        extingDocumentDetail.BankVerifiedSignatureFileType = docDetail.BankVerifiedSignatureFileType;
                    }
                    if (docDetail.Memorendum != null)
                    {
                        extingDocumentDetail.Memorendum = docDetail.Memorendum;
                        extingDocumentDetail.MemorendumFileName = docDetail.MemorendumFileName;
                        extingDocumentDetail.MemorendumFileType = docDetail.MemorendumFileType;
                    }
                    if (docDetail.OtherDocForProposedIndustry != null)
                    {
                        extingDocumentDetail.OtherDocForProposedIndustry = docDetail.OtherDocForProposedIndustry;
                        extingDocumentDetail.OtherDocForProposedIndustryFileName = docDetail.OtherDocForProposedIndustryFileName;
                        extingDocumentDetail.OtherDocForProposedIndustryFileType = docDetail.OtherDocForProposedIndustryFileType;
                    }
                    if (docDetail.PreEstablishedIndustriesDoc != null)
                    {
                        extingDocumentDetail.PreEstablishedIndustriesDoc = docDetail.PreEstablishedIndustriesDoc;
                        extingDocumentDetail.PreEstablishedIndustriesDocFileName = docDetail.PreEstablishedIndustriesDocFileName;
                        extingDocumentDetail.PreEstablishedIndustriesDocFileType = docDetail.PreEstablishedIndustriesDocFileType;
                    }
                    if (docDetail.ProjectReport != null)
                    {
                        extingDocumentDetail.ProjectReport = docDetail.ProjectReport;
                        extingDocumentDetail.ProjectReportFileName = docDetail.ProjectReportFileName;
                        extingDocumentDetail.ProjectReportFileType = docDetail.ProjectReportFileType;
                    }
                    if (docDetail.ProposedPlanLandUses != null)
                    {
                        extingDocumentDetail.ProposedPlanLandUses = docDetail.ProposedPlanLandUses;
                        extingDocumentDetail.ProposedPlanLandUsesFileName = docDetail.ProposedPlanLandUsesFileName;
                        extingDocumentDetail.ProposedPlanLandUsesFileType = docDetail.ProposedPlanLandUsesFileType;
                    }
                    if (docDetail.ScanAddressProof != null)
                    {
                        extingDocumentDetail.ScanAddressProof = docDetail.ScanAddressProof;
                        extingDocumentDetail.ScanAddressProofFileName = docDetail.ScanAddressProofFileName;
                        extingDocumentDetail.ScanAddressProofFileType = docDetail.ScanAddressProofFileType;
                    }
                    if (docDetail.ScanCastCert != null)
                    {
                        extingDocumentDetail.ScanCastCert = docDetail.ScanCastCert;
                        extingDocumentDetail.ScanCastCertFileName = docDetail.ScanCastCertFileName;
                        extingDocumentDetail.ScanCastCertFileType = docDetail.ScanCastCertFileType;
                    }
                    if (docDetail.ScanID != null)
                    {
                        extingDocumentDetail.ScanID = docDetail.ScanID;
                        extingDocumentDetail.ScanIDFileName = docDetail.ScanIDFileName;
                        extingDocumentDetail.ScanIDFileType = docDetail.ScanIDFileType;
                    }
                    if (docDetail.ScanPAN != null)
                    {
                        extingDocumentDetail.ScanPAN = docDetail.ScanPAN;
                        extingDocumentDetail.ScanPANFileName = docDetail.ScanPANFileName;
                        extingDocumentDetail.ScanPANFileType = docDetail.ScanPANFileType;
                    }

                    _db.Entry(extingDocumentDetail).State = EntityState.Modified;
                    _effectRow = _db.SaveChanges();
                }
                else
                {
                    docDetail.ApplicationId = UserData.ApplicationId;
                    _db.Entry(docDetail).State = EntityState.Added;
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

        public ApplicationDetailModel GetUserPlotDetail(int? appID)
        {
            try
            {
                _db = new GidaGKPEntities();
                return (from application in _db.ApplicantApplicationDetails
                        join applicantDetail in _db.ApplicantDetails on application.ApplicationId equals applicantDetail.ApplicationId
                        join plotDetail in _db.ApplicantPlotDetails on application.ApplicationId equals plotDetail.ApplicationId
                        join transaction1 in _db.ApplicantTransactionDetails on applicantDetail.ApplicationId equals transaction1.ApplicationId into transaction2
                        from transaction in transaction2.DefaultIfEmpty()
                        where application.ApplicationId == appID
                        select new ApplicationDetailModel
                        {
                            ApplicationNumber = application.ApplicationNumber,
                            FullApplicantName = applicantDetail.FullApplicantName,
                            Emaild = applicantDetail.EmailId,
                            CAddress = applicantDetail.CAddress,
                            Mobile = applicantDetail.Mobile,
                            TotalAmount = plotDetail.TotalAmount,
                            NetAmount = plotDetail.NetAmount,
                            ApplicationFee = plotDetail.ApplicationFee,
                            EarnestMoneyDeposit = plotDetail.EarnestMoney,
                            GST = plotDetail.GST,
                            PaymentDate = transaction != null ? transaction.trans_date : DateTime.Now,
                            PaymentReferenceNumber = transaction != null ? transaction.bank_ref_no : string.Empty
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
                return null;
            }

        }

        public AcknowledgementDetailModel GetAcknowledgementDetail(int? appID)
        {
            try
            {
                _db = new GidaGKPEntities();
                return (from applicationDetail in _db.ApplicantApplicationDetails
                        join applicantDetail in _db.ApplicantDetails on applicationDetail.ApplicationId equals applicantDetail.ApplicationId
                        join plotDetail in _db.ApplicantPlotDetails on applicationDetail.ApplicationId equals plotDetail.ApplicationId
                        join transactionDetail in _db.ApplicantTransactionDetails on applicantDetail.ApplicationId equals transactionDetail.ApplicationId
                        join projectDetail in _db.ApplicantProjectDetails on applicantDetail.ApplicationId equals projectDetail.ApplicationId
                        join documentDetail in _db.ApplicantUploadDocs on applicantDetail.ApplicationId equals documentDetail.ApplicationId
                        join IndustryOwnership in _db.Lookups on plotDetail.IndustryOwnership equals IndustryOwnership.LookupId
                        join lookupSectorName in _db.Lookups on plotDetail.SectorName equals lookupSectorName.LookupId
                        where applicationDetail.ApplicationId == appID
                        select new AcknowledgementDetailModel
                        {
                            ApplicationNumber = applicationDetail.ApplicationNumber,
                            FullApplicantName = applicantDetail.FullApplicantName,
                            CAddress = applicantDetail.CAddress,
                            Mobile = applicantDetail.Mobile,
                            TotalAmount = plotDetail.TotalAmount,
                            NetAmount = plotDetail.NetAmount,
                            ApplicationFee = plotDetail.ApplicationFee,
                            EarnestMoneyDeposit = plotDetail.EarnestMoney,
                            GST = plotDetail.GST,
                            PaymentDate = transactionDetail.trans_date,
                            BankName = transactionDetail.payment_mode,
                            PaymentReferenceNumber = transactionDetail.bank_ref_no,
                            ApplicantPhoto = documentDetail.ApplicantPhoto,
                            ApplicantSignature = documentDetail.ApplicantSignature,
                            EffluentTreatmentMeasures = projectDetail.EffluentTreatmentMeasures,
                            FirstYearNoOfFax = projectDetail.FirstYearNoOfFax,
                            FirstYearNoOfTelephone = projectDetail.FirstYearNoOfTelephone,
                            FumesNatureQuantity = projectDetail.FumesNatureQuantity,
                            GasChemicalComposition = projectDetail.GasChemicalComposition,
                            GasQuantity = projectDetail.GasQuantity,
                            IndustryOwnership = IndustryOwnership.LookupName,
                            LiquidChemicalComposition = projectDetail.LiquidChemicalComposition,
                            LiquidQuantity = projectDetail.LiquidQuantity,
                            PlotArea = plotDetail.PlotArea,
                            PowerRequirement = projectDetail.PowerRequirement,
                            ProjectEstimatedCost = projectDetail.ProjectEstimatedCost,
                            ProposedCoveredArea = projectDetail.ProposedCoveredArea,
                            ProposedIndustryType = projectDetail.ProposedIndustryType,
                            ProposedInvestmentBuilding = projectDetail.ProposedInvestmentBuilding,
                            ProposedInvestmentLand = projectDetail.ProposedInvestmentLand,
                            ProposedInvestmentPlant = projectDetail.ProposedInvestmentPlant,
                            ProposedOpenArea = projectDetail.ProposedOpenArea,
                            PurpuseOpenArea = projectDetail.PurpuseOpenArea,
                            SectorName = lookupSectorName.LookupName,
                            SignatryDateOfBirth = plotDetail.SignatryDateOfBirth,
                            SignatryName = plotDetail.SignatryName,
                            SignatryPermanentAddress = plotDetail.SignatryPermanentAddress,
                            SignatryPermanentPhoneNumber = plotDetail.SignatryPermanentPhoneNumber,
                            SignatryPresentAddress = plotDetail.SignatryPresentAddress,
                            SignatryPresentPhoneNumber = plotDetail.SignatryPresentPhoneNumber,
                            SolidChemicalComposition = projectDetail.SolidChemicalComposition,
                            SolidQuantity = projectDetail.SolidQuantity,
                            UltimateNoOfFax = projectDetail.UltimateNoOfFax,
                            UltimateNoOfTelephone = projectDetail.UltimateNoOfTelephone,
                            UnitName = plotDetail.UnitName,
                            Skilled = projectDetail.Skilled,
                            UnSkilled = projectDetail.UnSkilled
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
                return null;
            }

        }

        public List<ApplicationDetailModel> GetUserApplicationDetail(int userId)
        {
            try
            {
                _db = new GidaGKPEntities();
                return (from application in _db.ApplicantApplicationDetails
                        join applicantDetail1 in _db.ApplicantDetails on application.ApplicationId equals applicantDetail1.ApplicationId into applicantDetail2
                        from applicantDetail in applicantDetail2.DefaultIfEmpty()
                        join plotDetail1 in _db.ApplicantPlotDetails on application.ApplicationId equals plotDetail1.ApplicationId into plotDetail2
                        from plotDetail in plotDetail2.DefaultIfEmpty()
                        join transaction1 in _db.ApplicantTransactionDetails on applicantDetail.ApplicationId equals transaction1.ApplicationId into transaction2
                        from transaction in transaction2.DefaultIfEmpty()
                        join lookupSchemeType1 in _db.Lookups on plotDetail.SchemeType equals lookupSchemeType1.LookupId into lookupSchemeType2
                        from lookupSchemeType in lookupSchemeType2.DefaultIfEmpty()

                        join lookupSchemeName1 in _db.Lookups on plotDetail.SchemeName equals lookupSchemeName1.LookupId into lookupSchemeName2
                        from lookupSchemeName in lookupSchemeName2.DefaultIfEmpty()

                        join lookupSectorName1 in _db.Lookups on plotDetail.SchemeName equals lookupSectorName1.LookupId into lookupSectorName2
                        from lookupSectorName in lookupSectorName2.DefaultIfEmpty()

                        where application.UserId == userId && transaction == null
                        select new ApplicationDetailModel
                        {
                            ApplicationId = application.ApplicationId,
                            ApplicationNumber = application.ApplicationNumber,
                            FullApplicantName = applicantDetail.FullApplicantName,
                            CAddress = applicantDetail.CAddress,
                            Mobile = applicantDetail.Mobile,
                            TotalAmount = plotDetail.TotalAmount,
                            NetAmount = plotDetail.NetAmount,
                            ApplicationFee = plotDetail.ApplicationFee,
                            EarnestMoneyDeposit = plotDetail.EarnestMoney,
                            GST = plotDetail.GST,
                            PaymentDate = transaction != null ? transaction.trans_date : DateTime.Now,
                            PlotArea = plotDetail.PlotArea,
                            SchemeType = lookupSchemeType != null ? lookupSchemeType.LookupName : "",
                            SchemeName = lookupSchemeName != null ? lookupSchemeName.LookupName : "",
                            SectorName = lookupSectorName != null ? lookupSectorName.LookupName : ""
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
                return new List<ApplicationDetailModel>();
            }

        }

        public List<ApplicationDetailModel> GetUserAppliedApplicationDetail(int userId)
        {
            try
            {
                _db = new GidaGKPEntities();
                return (from application in _db.ApplicantApplicationDetails
                        join applicantDetail1 in _db.ApplicantDetails on application.ApplicationId equals applicantDetail1.ApplicationId into applicantDetail2
                        from applicantDetail in applicantDetail2.DefaultIfEmpty()
                        join plotDetail1 in _db.ApplicantPlotDetails on application.ApplicationId equals plotDetail1.ApplicationId into plotDetail2
                        from plotDetail in plotDetail2.DefaultIfEmpty()
                        join transaction1 in _db.ApplicantTransactionDetails on applicantDetail.ApplicationId equals transaction1.ApplicationId into transaction2
                        from transaction in transaction2.DefaultIfEmpty()
                        join lookupSchemeType1 in _db.Lookups on plotDetail.SchemeType equals lookupSchemeType1.LookupId into lookupSchemeType2
                        from lookupSchemeType in lookupSchemeType2.DefaultIfEmpty()

                        join lookupSchemeName1 in _db.Lookups on plotDetail.SchemeName equals lookupSchemeName1.LookupId into lookupSchemeName2
                        from lookupSchemeName in lookupSchemeName2.DefaultIfEmpty()

                        join lookupSectorName1 in _db.Lookups on plotDetail.SchemeName equals lookupSectorName1.LookupId into lookupSectorName2
                        from lookupSectorName in lookupSectorName2.DefaultIfEmpty()

                        where application.UserId == userId && transaction != null
                        select new ApplicationDetailModel
                        {
                            ApplicationId = application.ApplicationId,
                            ApplicationNumber = application.ApplicationNumber,
                            FullApplicantName = applicantDetail.FullApplicantName,
                            CAddress = applicantDetail.CAddress,
                            Mobile = applicantDetail.Mobile,
                            TotalAmount = plotDetail.TotalAmount,
                            NetAmount = plotDetail.NetAmount,
                            ApplicationFee = plotDetail.ApplicationFee,
                            EarnestMoneyDeposit = plotDetail.EarnestMoney,
                            GST = plotDetail.GST,
                            PaymentDate = transaction != null ? transaction.trans_date : DateTime.Now,
                            PlotArea = plotDetail.PlotArea,
                            SchemeType = lookupSchemeType != null ? lookupSchemeType.LookupName : "",
                            SchemeName = lookupSchemeName != null ? lookupSchemeName.LookupName : "",
                            SectorName = lookupSectorName != null ? lookupSectorName.LookupName : ""
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
                return new List<ApplicationDetailModel>();
            }

        }

        public int GetUserApplicationCount(int userId)
        {
            try
            {
                _db = new GidaGKPEntities();
                return (from application in _db.ApplicantApplicationDetails
                        join transaction1 in _db.ApplicantTransactionDetails on application.ApplicationId equals transaction1.ApplicationId into transaction2
                        from transaction in transaction2.DefaultIfEmpty()
                        where application.UserId == userId && transaction == null
                        select new ApplicationDetailModel
                        {
                            ApplicationId = application.ApplicationId,
                            ApplicationNumber = application.ApplicationNumber,
                        }).ToList().Count();
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
                return 0;
            }
        }

        public ApplicantPlotDetailModel GetApplciantPlotDetailByApplicationId(int applicationId)
        {
            try
            {
                _db = new GidaGKPEntities();
                return (from plotDetail in _db.ApplicantPlotDetails
                        join app in _db.ApplicantApplicationDetails on plotDetail.ApplicationId equals app.ApplicationId
                        where app.ApplicationId == applicationId
                        select new ApplicantPlotDetailModel
                        {
                            ApplicationFee = plotDetail.ApplicationFee,
                            ApplicationId = plotDetail.ApplicationId,
                            ApplicationNumber = app.ApplicationNumber,
                            AppliedFor = plotDetail.AppliedFor,
                            CreationDate = plotDetail.CreationDate,
                            EarnestMoney = plotDetail.EarnestMoney,
                            EstimatedRate = plotDetail.EstimatedRate,
                            GST = plotDetail.GST,
                            Id = plotDetail.Id,
                            IndustryOwnership = plotDetail.IndustryOwnership,
                            NetAmount = plotDetail.NetAmount,
                            PaymentSchedule = plotDetail.PaymentSchedule,
                            PlotArea = plotDetail.PlotArea,
                            PlotRange = plotDetail.PlotRange,
                            RelationshipStatus = plotDetail.RelationshipStatus,
                            SchemeName = plotDetail.SchemeName,
                            SchemeType = plotDetail.SchemeType,
                            SectorName = plotDetail.SectorName,
                            SignatryDateOfBirth = plotDetail.SignatryDateOfBirth,
                            SignatryName = plotDetail.SignatryName,
                            SignatryPermanentAddress = plotDetail.SignatryPermanentAddress,
                            SignatryPermanentPhoneNumber = plotDetail.SignatryPermanentPhoneNumber,
                            SignatryPresentAddress = plotDetail.SignatryPresentAddress,
                            SignatryPresentPhoneNumber = plotDetail.SignatryPresentPhoneNumber,
                            TotalAmount = plotDetail.TotalAmount,
                            TotalInvestment = plotDetail.TotalInvestment,
                            UnitName = plotDetail.UnitName,
                            UserId = plotDetail.UserId
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
                return null;
            }
        }

        public ApplicantUploadDoc GetApplicantUploadDocDetail(int applicationId)
        {
            try
            {
                _db = new GidaGKPEntities();
                return (from doc in _db.ApplicantUploadDocs
                        where doc.ApplicationId == applicationId
                        select doc
                        ).FirstOrDefault();
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

        public ApplicantDetailModel GetApplicantPersonalDetail(int applicationId)
        {
            try
            {
                _db = new GidaGKPEntities();
                return (from applicantDetail in _db.ApplicantDetails
                        join app in _db.ApplicantApplicationDetails on applicantDetail.ApplicationId equals app.ApplicationId
                        where app.ApplicationId == applicationId
                        select new ApplicantDetailModel
                        {
                            AdhaarNumber = applicantDetail.AdhaarNumber,
                            UserId = applicantDetail.UserId,
                            SubCategory = applicantDetail.SubCategory,
                            ApplicantDOB = applicantDetail.ApplicantDOB,
                            CAddress = applicantDetail.CAddress,
                            Category = applicantDetail.Category,
                            CreationDate = DateTime.Now,
                            EmailId = applicantDetail.EmailId,
                            FName = applicantDetail.FName,
                            FullApplicantName = applicantDetail.FullApplicantName,
                            Gender = applicantDetail.Gender,
                            IdentiyProof = applicantDetail.IdentiyProof,
                            MName = applicantDetail.MName,
                            Mobile = applicantDetail.Mobile,
                            Nationality = applicantDetail.Nationality,
                            PAddress = applicantDetail.PAddress,
                            PAN = applicantDetail.PAN,
                            Phone = applicantDetail.Phone,
                            Religion = applicantDetail.Religion,
                            ResidentialProof = applicantDetail.ResidentialProof,
                            SName = applicantDetail.SName,
                            ApplicationId = UserData.ApplicationId
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
                return null;
            }

        }

        public ApplicantProjectDetailModel GetApplicantProjectDetail(int applicationId)
        {
            try
            {
                _db = new GidaGKPEntities();
                return (from applicantDetail in _db.ApplicantProjectDetails
                        join app in _db.ApplicantApplicationDetails on applicantDetail.ApplicationId equals app.ApplicationId
                        where app.ApplicationId == applicationId
                        select new ApplicantProjectDetailModel
                        {
                            CreationDate = applicantDetail.CreationDate,
                            EffluentTreatmentMeasures = applicantDetail.EffluentTreatmentMeasures,
                            FirstYearNoOfFax = applicantDetail.FirstYearNoOfFax,
                            FirstYearNoOfTelephone = applicantDetail.FirstYearNoOfTelephone,
                            FullApplicantName = applicantDetail.FullApplicantName,
                            FumesNatureQuantity = applicantDetail.FumesNatureQuantity,
                            GasChemicalComposition = applicantDetail.GasChemicalComposition,
                            GasQuantity = applicantDetail.GasQuantity,
                            Id = applicantDetail.Id,
                            LiquidChemicalComposition = applicantDetail.LiquidChemicalComposition,
                            LiquidQuantity = applicantDetail.LiquidQuantity,
                            PowerRequirement = applicantDetail.PowerRequirement,
                            ProjectEstimatedCost = applicantDetail.ProjectEstimatedCost,
                            ProposedCoveredArea = applicantDetail.ProposedCoveredArea,
                            ProposedIndustryType = applicantDetail.ProposedIndustryType,
                            ProposedInvestmentBuilding = applicantDetail.ProposedInvestmentBuilding,
                            ProposedInvestmentPlant = applicantDetail.ProposedInvestmentPlant,
                            ProposedOpenArea = applicantDetail.ProposedOpenArea,
                            PurpuseOpenArea = applicantDetail.PurpuseOpenArea,
                            Skilled = applicantDetail.Skilled,
                            SolidChemicalComposition = applicantDetail.SolidChemicalComposition,
                            SolidQuantity = applicantDetail.SolidQuantity,
                            UltimateNoOfFax = applicantDetail.UltimateNoOfFax,
                            UltimateNoOfTelephone = applicantDetail.UltimateNoOfTelephone,
                            UnSkilled = applicantDetail.UnSkilled,
                            UserId = applicantDetail.UserId,
                            ProposedInvestmentLand = applicantDetail.ProposedInvestmentLand,
                            ApplicationId = UserData.ApplicationId
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
                return null;
            }

        }

        public ApplicantBankDetailModel GetApplicantBankDetail(int applicationId)
        {
            try
            {
                _db = new GidaGKPEntities();
                return (from bankDetail in _db.ApplicantBankDetails
                        join app in _db.ApplicantApplicationDetails on bankDetail.ApplicationId equals app.ApplicationId
                        where app.ApplicationId == applicationId
                        select new ApplicantBankDetailModel
                        {
                            UserId = bankDetail.UserId,
                            AccountHolderName = bankDetail.AccountHolderName,
                            ApplicationId = bankDetail.ApplicationId,
                            BankAccountNo = bankDetail.BankAccountNo,
                            BankName = bankDetail.BankName,
                            BBAddress = bankDetail.BBAddress,
                            BBName = bankDetail.BBName,
                            CreationDate = bankDetail.CreationDate,
                            Id = bankDetail.Id,
                            IFSC_Code = bankDetail.IFSC_Code
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
                return null;
            }

        }

        public ApplicantUploadDocumentModel GetApplicantDocumentDetail(int applicationId)
        {
            try
            {
                _db = new GidaGKPEntities();
                return (from docDetail in _db.ApplicantUploadDocs
                        join app in _db.ApplicantApplicationDetails on docDetail.ApplicationId equals app.ApplicationId
                        where app.ApplicationId == applicationId
                        select new ApplicantUploadDocumentModel
                        {
                            UserId = docDetail.UserId,
                            Id = docDetail.Id,
                            ApplicantEduTechQualification = docDetail.ApplicantEduTechQualification,
                            //ApplicantEduTechQualificationURL = string.Format("data:" + docDetail.ApplicantEduTechQualificationFileType + ";base64,{0}", Convert.ToBase64String(docDetail.ApplicantEduTechQualification)),
                            ScanPANFileType = docDetail.ScanPANFileType,
                            ScanPANFileName = docDetail.ScanPANFileName,
                            ScanIDFileType = docDetail.ScanIDFileType,
                            ScanIDFileName = docDetail.ScanIDFileName,
                            ScanCastCertFileType = docDetail.ScanCastCertFileType,
                            ScanCastCertFileName = docDetail.ScanCastCertFileName,
                            ScanAddressProofFileType = docDetail.ScanAddressProofFileType,
                            ApplicantEduTechQualificationFileName = docDetail.ApplicantEduTechQualificationFileName,
                            ApplicantEduTechQualificationFileType = docDetail.ApplicantEduTechQualificationFileType,
                            ApplicantPhoto = docDetail.ApplicantPhoto,
                            //ApplicantPhotoURL = string.Format("data:" + docDetail.ApplicantPhotoFileType + ";base64,{0}", Convert.ToBase64String(docDetail.ApplicantPhoto)),
                            ApplicantPhotoFileName = docDetail.ApplicantPhotoFileName,
                            ApplicantPhotoFileType = docDetail.ApplicantPhotoFileType,
                            ApplicantSignature = docDetail.ApplicantSignature,
                            //ApplicantSignatureURL = string.Format("data:" + docDetail.ApplicantSignatureFileType + ";base64,{0}", Convert.ToBase64String(docDetail.ApplicantSignature)),
                            ApplicantSignatureFileName = docDetail.ApplicantSignatureFileName,
                            ApplicantSignatureFileType = docDetail.ApplicantSignatureFileType,
                            ApplicationId = docDetail.ApplicationId,
                            BalanceSheet = docDetail.BalanceSheet,
                            //BalanceSheetURL = string.Format("data:" + docDetail.BalanceSheetFileType + ";base64,{0}", Convert.ToBase64String(docDetail.BalanceSheet)),
                            BalanceSheetFileName = docDetail.BalanceSheetFileName,
                            BalanceSheetFileType = docDetail.BalanceSheetFileType,
                            BankVerifiedSignature = docDetail.BankVerifiedSignature,
                            //BankVerifiedSignatureURL = string.Format("data:" + docDetail.BankVerifiedSignatureFileType + ";base64,{0}", Convert.ToBase64String(docDetail.BankVerifiedSignature)),
                            BankVerifiedSignatureFileName = docDetail.BankVerifiedSignatureFileName,
                            BankVerifiedSignatureFileType = docDetail.BankVerifiedSignatureFileType,
                            CreationDate = docDetail.CreationDate,
                            DocProofForIndustrialEstablishedOutsideGida = docDetail.DocProofForIndustrialEstablishedOutsideGida,
                            //DocProofForIndustrialEstablishedOutsideGidaURL = string.Format("data:" + docDetail.DocProofForIndustrialEstablishedOutsideGidaFileType + ";base64,{0}", Convert.ToBase64String(docDetail.DocProofForIndustrialEstablishedOutsideGida)),
                            DocProofForIndustrialEstablishedOutsideGidaFileName = docDetail.DocProofForIndustrialEstablishedOutsideGidaFileName,
                            DocProofForIndustrialEstablishedOutsideGidaFileType = docDetail.DocProofForIndustrialEstablishedOutsideGidaFileType,
                            ExperienceProof = docDetail.ExperienceProof,
                            //ExperienceProofURL = string.Format("data:" + docDetail.ExperienceProofFileType + ";base64,{0}", Convert.ToBase64String(docDetail.ExperienceProof)),
                            ExperienceProofFileName = docDetail.ExperienceProofFileName,
                            ExperienceProofFileType = docDetail.ExperienceProofFileType,
                            FinDetailsEstablishedIndustries = docDetail.FinDetailsEstablishedIndustries,
                            //FinDetailsEstablishedIndustriesURL = string.Format("data:" + docDetail.FinDetailsEstablishedIndustriesFileType + ";base64,{0}", Convert.ToBase64String(docDetail.FinDetailsEstablishedIndustries)),
                            FinDetailsEstablishedIndustriesFileName = docDetail.FinDetailsEstablishedIndustriesFileName,
                            FinDetailsEstablishedIndustriesFileType = docDetail.FinDetailsEstablishedIndustriesFileType,
                            ITReturn = docDetail.ITReturn,
                            //ITReturnURL = string.Format("data:" + docDetail.ITReturnFileType + ";base64,{0}", Convert.ToBase64String(docDetail.ITReturn)),
                            ITReturnFileName = docDetail.ITReturnFileName,
                            ITReturnFileType = docDetail.ITReturnFileType,
                            Memorendum = docDetail.Memorendum,
                            //MemorendumURL = string.Format("data:" + docDetail.MemorendumFileType + ";base64,{0}", Convert.ToBase64String(docDetail.Memorendum)),
                            MemorendumFileName = docDetail.MemorendumFileName,
                            MemorendumFileType = docDetail.MemorendumFileType,
                            OtherDocForProposedIndustry = docDetail.OtherDocForProposedIndustry,
                            //OtherDocForProposedIndustryURL = string.Format("data:" + docDetail.OtherDocForProposedIndustryFileType + ";base64,{0}", Convert.ToBase64String(docDetail.OtherDocForProposedIndustry)),
                            OtherDocForProposedIndustryFileName = docDetail.OtherDocForProposedIndustryFileName,
                            OtherDocForProposedIndustryFileType = docDetail.OtherDocForProposedIndustryFileType,
                            PreEstablishedIndustriesDoc = docDetail.PreEstablishedIndustriesDoc,
                            //PreEstablishedIndustriesDocURL = string.Format("data:" + docDetail.PreEstablishedIndustriesDocFileType + ";base64,{0}", Convert.ToBase64String(docDetail.PreEstablishedIndustriesDoc)),
                            PreEstablishedIndustriesDocFileName = docDetail.PreEstablishedIndustriesDocFileName,
                            PreEstablishedIndustriesDocFileType = docDetail.PreEstablishedIndustriesDocFileType,
                            ProjectReport = docDetail.ProjectReport,
                            //ProjectReportURL = string.Format("data:" + docDetail.ProjectReportFileType + ";base64,{0}", Convert.ToBase64String(docDetail.ProjectReport)),
                            ProjectReportFileName = docDetail.ProjectReportFileName,
                            ProjectReportFileType = docDetail.ProjectReportFileType,
                            ProposedPlanLandUses = docDetail.ProposedPlanLandUses,
                            //ProposedPlanLandUsesURL = string.Format("data:" + docDetail.ProposedPlanLandUsesFileType + ";base64,{0}", Convert.ToBase64String(docDetail.ProposedPlanLandUses)),
                            ProposedPlanLandUsesFileName = docDetail.ProposedPlanLandUsesFileName,
                            ProposedPlanLandUsesFileType = docDetail.ProposedPlanLandUsesFileType,
                            ScanAddressProof = docDetail.ScanAddressProof,
                            //ScanAddressProofURL = string.Format("data:" + docDetail.ScanAddressProofFileType + ";base64,{0}", Convert.ToBase64String(docDetail.ScanAddressProof)),
                            ScanAddressProofFileName = docDetail.ScanAddressProofFileName,
                            ScanCastCert = docDetail.ScanCastCert,
                            //ScanCastCertURL = string.Format("data:" + docDetail.ScanCastCertFileType + ";base64,{0}", Convert.ToBase64String(docDetail.ScanCastCert)),
                            ScanID = docDetail.ScanID,
                            //ScanIDURL = string.Format("data:" + docDetail.ScanIDFileType + ";base64,{0}", Convert.ToBase64String(docDetail.ScanID)),
                            ScanPAN = docDetail.ScanPAN,
                            //ScanPANURL = string.Format("data:" + docDetail.ScanPANFileType + ";base64,{0}", Convert.ToBase64String(docDetail.ScanPAN))
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
                return null;
            }

        }

        public List<ApplicationDetailModel> GetUserApplicationForOfflinePayment(int userId)
        {
            try
            {
                _db = new GidaGKPEntities();
                return (from application in _db.ApplicantApplicationDetails
                        join applicantDetail1 in _db.ApplicantDetails on application.ApplicationId equals applicantDetail1.ApplicationId into applicantDetail2
                        from applicantDetail in applicantDetail2.DefaultIfEmpty()
                        join plotDetail1 in _db.ApplicantPlotDetails on application.ApplicationId equals plotDetail1.ApplicationId into plotDetail2
                        from plotDetail in plotDetail2.DefaultIfEmpty()
                        join projectDetail1 in _db.ApplicantProjectDetails on application.ApplicationId equals projectDetail1.ApplicationId into projectDetail2
                        from projectDetail in projectDetail2.DefaultIfEmpty()
                        join bankDetail1 in _db.ApplicantBankDetails on application.ApplicationId equals bankDetail1.ApplicationId into bankDetail2
                        from bankDetail in bankDetail2.DefaultIfEmpty()
                        join documentDetail1 in _db.ApplicantUploadDocs on application.ApplicationId equals documentDetail1.ApplicationId into documentDetail2
                        from documentDetail in documentDetail2.DefaultIfEmpty()
                        join transaction1 in _db.ApplicantTransactionDetails on applicantDetail.ApplicationId equals transaction1.ApplicationId into transaction2
                        from transaction in transaction2.DefaultIfEmpty()
                        join lookupSchemeType1 in _db.Lookups on plotDetail.SchemeType equals lookupSchemeType1.LookupId into lookupSchemeType2
                        from lookupSchemeType in lookupSchemeType2.DefaultIfEmpty()

                        join lookupSchemeName1 in _db.Lookups on plotDetail.SchemeName equals lookupSchemeName1.LookupId into lookupSchemeName2
                        from lookupSchemeName in lookupSchemeName2.DefaultIfEmpty()

                        join lookupSectorName1 in _db.Lookups on plotDetail.SchemeName equals lookupSectorName1.LookupId into lookupSectorName2
                        from lookupSectorName in lookupSectorName2.DefaultIfEmpty()

                        where application.UserId == userId &&
                        transaction == null &&
                        applicantDetail != null &&
                        plotDetail != null &&
                        projectDetail != null
                        && bankDetail != null &&
                        documentDetail != null
                        select new ApplicationDetailModel
                        {
                            ApplicationId = application.ApplicationId,
                            ApplicationNumber = application.ApplicationNumber,
                            FullApplicantName = applicantDetail.FullApplicantName,
                            CAddress = applicantDetail.CAddress,
                            Mobile = applicantDetail.Mobile,
                            TotalAmount = plotDetail.TotalAmount,
                            NetAmount = plotDetail.NetAmount,
                            ApplicationFee = plotDetail.ApplicationFee,
                            EarnestMoneyDeposit = plotDetail.EarnestMoney,
                            GST = plotDetail.GST,
                            PaymentDate = transaction != null ? transaction.trans_date : DateTime.Now,
                            PlotArea = plotDetail.PlotArea,
                            SchemeType = lookupSchemeType != null ? lookupSchemeType.LookupName : "",
                            SchemeName = lookupSchemeName != null ? lookupSchemeName.LookupName : "",
                            SectorName = lookupSectorName != null ? lookupSectorName.LookupName : ""
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
                return new List<ApplicationDetailModel>();
            }

        }

        public Enums.CrudStatus SaveApplicantChalanDocument(int userId, ApplicantTransactionDetail docDetail)
        {
            try
            {
                _db = new GidaGKPEntities();

                int _effectRow = 0;

                var extingDocumentDetail = _db.ApplicantTransactionDetails.Where(x => x.ApplicationId == UserData.ApplicationId).FirstOrDefault();
                if (extingDocumentDetail == null)
                {
                    docDetail.ApplicationId = UserData.ApplicationId;
                    _db.Entry(docDetail).State = EntityState.Added;
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

        public int GetEnablePaymentLink(int userId)
        {
            try
            {
                _db = new GidaGKPEntities();
                return (from application in _db.ApplicantApplicationDetails
                        join documentDetail1 in _db.ApplicantUploadDocs on application.ApplicationId equals documentDetail1.ApplicationId into documentDetail2
                        from documentDetail in documentDetail2.DefaultIfEmpty()
                        join transaction1 in _db.ApplicantTransactionDetails on application.ApplicationId equals transaction1.ApplicationId into transaction2
                        from transaction in transaction2.DefaultIfEmpty()
                        where application.UserId == userId && transaction == null && documentDetail != null
                        select new ApplicationDetailModel
                        {
                            ApplicationId = application.ApplicationId,
                            ApplicationNumber = application.ApplicationNumber,
                        }).Distinct().ToList().Count;
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
                return 0;
            }
        }

        public int GetEnablePrintReciptLink(int userId)
        {
            try
            {
                _db = new GidaGKPEntities();
                return (from application in _db.ApplicantApplicationDetails
                        join transaction1 in _db.ApplicantTransactionDetails on application.ApplicationId equals transaction1.ApplicationId into transaction2
                        from transaction in transaction2.DefaultIfEmpty()
                        where application.UserId == userId && transaction != null
                        select new ApplicationDetailModel
                        {
                            ApplicationId = application.ApplicationId,
                            ApplicationNumber = application.ApplicationNumber,
                        }).Distinct().ToList().Count;
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
                return 0;
            }
        }

        public ApplicantTransactionDetail ChecktransactionId(long transactionId)
        {
            try
            {
                _db = new GidaGKPEntities();
                return _db.ApplicantTransactionDetails.Where(x => x.tracking_id == transactionId).FirstOrDefault(); ;
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

        public ApplicantTransactionDetail CheckorderId(long orderId)
        {
            try
            {
                _db = new GidaGKPEntities();
                return _db.ApplicantTransactionDetails.Where(x => x.order_id == orderId).FirstOrDefault();
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

        public PGTransactionInformation GetPGTransactionInformation(string OrderId, string TransactionId)
        {
            try
            {
                _db = new GidaGKPEntities();
                return _db.PGTransactionInformations.Where(x => x.OrderId == OrderId).FirstOrDefault(); ;
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

        public ApplicantTransactionDetail GetApplicationTransactionInformation(int userid, int applicationId)
        {
            try
            {
                _db = new GidaGKPEntities();
                return _db.ApplicantTransactionDetails.Where(x => x.UserId == userid && x.ApplicationId == applicationId).FirstOrDefault(); ;
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
        public Enums.CrudStatus SavePGTransactionInformation(int userId, int applicationId, string OrderId, string TransactionId)
        {
            try
            {
                _db = new GidaGKPEntities();
                int _effectRow = 0;

                //var extingPGInfo = _db.PGTransactionInformations.Where(x => x.ApplicationId == applicationId && x.UserId == userId).FirstOrDefault();
                //if (extingPGInfo != null)  // update detail
                //{
                //    extingPGInfo.OrderId = OrderId;
                //    extingPGInfo.TransactionId = TransactionId;
                //    _db.Entry(extingPGInfo).State = EntityState.Modified;
                //    _effectRow = _db.SaveChanges();
                //}

                //else
                //{
                PGTransactionInformation _newRecord = new PGTransactionInformation()
                {
                    UserId = userId,
                    ApplicationId = UserData.ApplicationId,
                    OrderId = OrderId,
                    TransactionId = TransactionId,
                    CreatedDate = DateTime.Now
                };
                _db.Entry(_newRecord).State = EntityState.Added;
                _effectRow = _db.SaveChanges();
                //}

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
        public ApplicantProjectChangeDetail GetApplicantProjectChangeDetail(int userId)
        {
            _db = new GidaGKPEntities();
            var extingApplicant = _db.ApplicantApplicationDetails.Where(x => x.UserId == userId).FirstOrDefault();
            return (extingApplicant != null) ? _db.ApplicantProjectChangeDetails.Where(x => x.ApplicationId == extingApplicant.ApplicationId)?.FirstOrDefault() : null;
        }

        public Enums.CrudStatus SaveApplicantProjectChangeDetail(ApplicantProjectChangeDetail changeProjectDetail)
        {
            try
            {
                _db = new GidaGKPEntities();
                int _effectRow = 0;

                var extingApplicant = _db.ApplicantApplicationDetails.Where(x => x.ApplicationId == changeProjectDetail.ApplicationId).FirstOrDefault();
                if (extingApplicant == null)
                {
                    throw new Exception("Applicant not exists with given applicant id");
                }

                var existingChangeProjectDetail = _db.ApplicantProjectChangeDetails.Where(x => x.ApplicationId == changeProjectDetail.ApplicationId).FirstOrDefault();

                if (existingChangeProjectDetail != null)  // update bank detail
                {
                    existingChangeProjectDetail.ApplicantName = !string.IsNullOrEmpty(changeProjectDetail.ApplicantName) ? changeProjectDetail.ApplicantName : existingChangeProjectDetail.ApplicantName;
                    existingChangeProjectDetail.AllotmentNumber = !string.IsNullOrEmpty(changeProjectDetail.AllotmentNumber) ? changeProjectDetail.AllotmentNumber : existingChangeProjectDetail.AllotmentNumber;
                    existingChangeProjectDetail.AreaToBeUsedForProject = !string.IsNullOrEmpty(changeProjectDetail.AreaToBeUsedForProject) ? changeProjectDetail.AreaToBeUsedForProject : existingChangeProjectDetail.AreaToBeUsedForProject;
                    existingChangeProjectDetail.IsEffluentMore = !string.IsNullOrEmpty(changeProjectDetail.IsEffluentMore) ? changeProjectDetail.IsEffluentMore : existingChangeProjectDetail.IsEffluentMore;
                    existingChangeProjectDetail.IsEmissionToSurrounding = !string.IsNullOrEmpty(changeProjectDetail.IsEmissionToSurrounding) ? changeProjectDetail.IsEmissionToSurrounding : existingChangeProjectDetail.IsEmissionToSurrounding;

                    if (!string.IsNullOrEmpty(changeProjectDetail.DetailOfProjectFileName))
                    {
                        existingChangeProjectDetail.DetailOfProjectFileName = changeProjectDetail.DetailOfProjectFileName;
                        existingChangeProjectDetail.DetailOfProjectFileType = changeProjectDetail.DetailOfProjectFileType;
                        existingChangeProjectDetail.DetailOfProjectFileContent = changeProjectDetail.DetailOfProjectFileContent;
                    }

                    if (!string.IsNullOrEmpty(changeProjectDetail.NOCOfProjectFileName))
                    {
                        existingChangeProjectDetail.NOCOfProjectFileName = changeProjectDetail.NOCOfProjectFileName;
                        existingChangeProjectDetail.NOCOfProjectFileType = changeProjectDetail.NOCOfProjectFileType;
                        existingChangeProjectDetail.NOCOfProjectFileContent = changeProjectDetail.NOCOfProjectFileContent;
                    }

                    if (!string.IsNullOrEmpty(changeProjectDetail.NotarizedFileName))
                    {
                        existingChangeProjectDetail.NotarizedFileName = changeProjectDetail.NotarizedFileName;
                        existingChangeProjectDetail.NotarizedFileType = changeProjectDetail.NotarizedFileType;
                        existingChangeProjectDetail.NotarizedFileContent = changeProjectDetail.NotarizedFileContent;
                    }

                    if (!string.IsNullOrEmpty(changeProjectDetail.SignatureFileName))
                    {
                        existingChangeProjectDetail.SignatureFileName = changeProjectDetail.SignatureFileName;
                        existingChangeProjectDetail.SignatureFileType = changeProjectDetail.SignatureFileType;
                        existingChangeProjectDetail.SignatureFileContent = changeProjectDetail.SignatureFileContent;
                    }

                    if (!string.IsNullOrEmpty(changeProjectDetail.PhotographFileName))
                    {
                        existingChangeProjectDetail.PhotographFileName = changeProjectDetail.PhotographFileName;
                        existingChangeProjectDetail.PhotographFileType = changeProjectDetail.PhotographFileType;
                        existingChangeProjectDetail.PhotographFileContent = changeProjectDetail.PhotographFileContent;
                    }

                    existingChangeProjectDetail.AmountPaid = changeProjectDetail.AmountPaid.HasValue  ? changeProjectDetail.AmountPaid : existingChangeProjectDetail.AmountPaid.HasValue ? existingChangeProjectDetail.AmountPaid:null;

                    existingChangeProjectDetail.TransactionNumber = !string.IsNullOrEmpty(changeProjectDetail.TransactionNumber) ? changeProjectDetail.TransactionNumber : existingChangeProjectDetail.TransactionNumber;

                    _db.Entry(existingChangeProjectDetail).State = EntityState.Modified;
                }
                else
                {
                    _db.Entry(changeProjectDetail).State = EntityState.Added;
                }

                _effectRow = _db.SaveChanges();

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
    }
}