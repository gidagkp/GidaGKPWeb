using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLayer;
using GidaGkpWeb.Global;
using System.Data.Entity;
using GidaGkpWeb.Models;
using System.Data.Entity.Validation;

namespace GidaGkpWeb.BAL.Masters
{
    public class InvitationDetails
    {
        GidaGKPEntities _db = null;
        public InvitationModel GetInvApplicantDetails(int UserId)
        {
            try
            {
                _db = new GidaGKPEntities();
                return (from ApplicantPlotDet in _db.ApplicantPlotDetails
                        join SectorNameLookup in _db.Lookups on ApplicantPlotDet.SectorName equals SectorNameLookup.LookupId
                        join PlotNameLookup in _db.Lookups on ApplicantPlotDet.PlotRange equals PlotNameLookup.LookupId
                        join ApplicantDet in _db.ApplicantDetails on ApplicantPlotDet.ApplicationId equals ApplicantDet.ApplicationId
                        join AppApplication in _db.ApplicantApplicationDetails on ApplicantPlotDet.ApplicationId equals AppApplication.ApplicationId
                        where ApplicantPlotDet.UserId == UserId
                        select new InvitationModel
                        {
                            //Id = ApplicantDet.Id,
                            //FullName = ApplicantDet.FullApplicantName,
                            Address = ApplicantDet.PAddress,
                            SectorName = SectorNameLookup.LookupName,
                            ApplicationNumber = AppApplication.ApplicationNumber,
                            PlotRange = PlotNameLookup.LookupName
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

        public List<InvitationModel> GetInvApplicant()
        {
            try
            {
                _db = new GidaGKPEntities();
                return (from ApplicantPlotDet in _db.ApplicantPlotDetails

                        join ApplicantDet in _db.ApplicantDetails on ApplicantPlotDet.ApplicationId equals ApplicantDet.ApplicationId


                        select new InvitationModel
                        {
                            Id = ApplicantDet.UserId,
                            FullName = ApplicantDet.FullApplicantName,

                        }).OrderByDescending(x => x.FullName).ToList();
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
        //public Enums.CrudStatus SaveInvitation(ApplicantInvitationLetter Invitation)
        //{
        //    try
        //    {
        //        _db = new GidaGKPEntities();
        //        int _effectRow = 0;
        //        if (Invitation.Id > 0)
        //        {
        //            var applicantinvitationletter = _db.ApplicantInvitationLetters.Where(x => x.Id == Invitation.Id).FirstOrDefault();
        //            if (applicantinvitationletter != null)
        //            {
        //                applicantinvitationletter.UserId = Invitation.UserId;
        //                applicantinvitationletter.ApplicationNo = Invitation.ApplicationNo;

        //                //applicantinvitationletter.Sector = Invitation.Sector;
        //                applicantinvitationletter.ApplicantAddress = Invitation.ApplicantAddress;
        //                applicantinvitationletter.PlotRange = Invitation.PlotRange;
        //                applicantinvitationletter.TotalNoOfPlots = Invitation.TotalNoOfPlots;
        //                applicantinvitationletter.InterviewMode = Invitation.InterviewMode;
        //                applicantinvitationletter.InterviewDateTime = Invitation.InterviewDateTime;
        //                _db.Entry(applicantinvitationletter).State = EntityState.Modified;
        //                _effectRow = _db.SaveChanges();
        //            }
        //        }
        //        else
        //        {
        //            _db.Entry(Invitation).State = EntityState.Added;
        //            _effectRow = _db.SaveChanges();
        //        }

        //        return _effectRow > 0 ? Enums.CrudStatus.Saved : Enums.CrudStatus.NotSaved;
        //    }
        //    catch (DbEntityValidationException e)
        //    {
        //        foreach (var eve in e.EntityValidationErrors)
        //        {
        //            foreach (var ve in eve.ValidationErrors)
        //            {
        //                Elmah.ErrorLog.GetDefault(HttpContext.Current).Log(new Elmah.Error(e));
        //            }
        //        }
        //        return Enums.CrudStatus.InternalError;
        //    }
        //}
    }
}