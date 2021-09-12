using DataLayer;
using GidaGkpWeb.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace GidaGkpWeb.BAL.Masters
{
    public class MortageDetails
    {
        GidaGKPEntities _db = null;
        public MortgageModel GetmortgageDetailById(int ApplicationId)
        {
            try
            {
                _db = new GidaGKPEntities();
                return (from AppApplication in _db.ApplicantDetails
                        join ApplicantDet in _db.ApplicantPlotDetails on AppApplication.ApplicationId equals ApplicantDet.ApplicationId
                        join SectorNameLookup in _db.Lookups on ApplicantDet.SectorName equals SectorNameLookup.LookupId
                        join plotMaster in _db.PlotMasters on SectorNameLookup.LookupId equals plotMaster.SectorName
                        join allotment in _db.AllocateAllotmentDetails on AppApplication.ApplicationId equals allotment.ApplicationId
                        where AppApplication.ApplicationId == ApplicationId
                        select new MortgageModel
                        {
                            //Id = ApplicantDet.Id,
                            //FullName = ApplicantDet.FullApplicantName,
                            AddressOfApplicant = AppApplication.PAddress,
                            SectorName = ApplicantDet.SectorName.ToString(),
                            PlotNo = plotMaster.PlotNumber,
                            AllotmentId = allotment.Id,
                            DateOfMortgageRequestLetter = allotment.DateofAllotmentLetter



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