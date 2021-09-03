using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLayer;
using GidaGkpWeb.Global;
using System.Data.Entity;
using GidaGkpWeb.Models;
using System.Data.Entity.Validation;

namespace GidaGkpWeb.BAL.Login
{
    public class LeasedeedNotesheetDetails
    {
        GidaGKPEntities _db = null;
        public LeasedeedNotesheetModel GetLeesdeedApplicantDetails(int ApplicationId)
        {
            try
            {
                _db = new GidaGKPEntities();
                return (from ApplicantPlotDet in _db.ApplicantPlotDetails
                        join SectorNameLookup in _db.Lookups on ApplicantPlotDet.SectorName equals SectorNameLookup.LookupId
                        join ApplicantDet in _db.ApplicantDetails on ApplicantPlotDet.ApplicationId equals ApplicantDet.ApplicationId
                        join LeaseDeedDetailDet in _db.LeaseDeedDetails on ApplicantPlotDet.ApplicationId equals LeaseDeedDetailDet.ApplicationId
                        join ApplicantInvDetailsDet in _db.ApplicantInvitationLetters on ApplicantPlotDet.ApplicationId equals ApplicantInvDetailsDet.ApplicationId
                        where ApplicantPlotDet.ApplicationId == ApplicationId
                        select new
                        {
                            ApplicantName = ApplicantDet.FullApplicantName,
                            PlotNo = ApplicantInvDetailsDet.PlotId,
                            ApplicantAddress = ApplicantDet.PAddress,
                            SectorName = SectorNameLookup.LookupName,
                            PlotArea = ApplicantPlotDet.PlotArea,
                            EarnestMoneyPaid = LeaseDeedDetailDet.EarnestMoneyPaid,
                            EarnestMoneyChallanNumber = LeaseDeedDetailDet.EarnestMoneyChallanNo,
                            AllotmentMoneyPaid = LeaseDeedDetailDet.AllotmentMoneyPaid,
                            VerifyTotalPremium = "3000000",
                            StampValue = LeaseDeedDetailDet.RequiredStampValue,
                            BankgChallanNumber = LeaseDeedDetailDet.BankGauranteeChallanNo,
                            EntityName = LeaseDeedDetailDet.EntityNameBehalfOfApplicant,
                            SvalueBankGchallanNumber = LeaseDeedDetailDet.StampValueForBankGaurantee,
                        }).ToList()
                        .Select(x => new LeasedeedNotesheetModel()
                        {
                            ApplicantName = x.ApplicantName,
                            PlotNo = Convert.ToString(x.PlotNo),
                            ApplicantAddress = x.ApplicantAddress,
                            SectorName = x.SectorName,
                            PlotArea = x.PlotArea,
                            EarnestMoneyPaid = Convert.ToString(x.EarnestMoneyPaid),
                            EarnestMoneyChallanNumber = Convert.ToString(x.EarnestMoneyChallanNumber),
                            AllotmentMoneyPaid = Convert.ToString(x.AllotmentMoneyPaid),
                            VerifyTotalPremium = x.VerifyTotalPremium,
                            StampValue = Convert.ToString(x.StampValue),
                            BankgChallanNumber = x.BankgChallanNumber,
                            EntityName = x.EntityName,
                            SvalueBankGchallanNumber = Convert.ToString(x.SvalueBankGchallanNumber),
                            //Id = ApplicantDet.Id,

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
    }
}