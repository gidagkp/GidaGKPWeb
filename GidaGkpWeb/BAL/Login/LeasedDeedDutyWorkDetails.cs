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
    public class LeasedDeedDutyWorkDetails
    {
        GidaGKPEntities _db = null;
        public Enums.CrudStatus SaveLeasedDutyWork(LeaseDeedDetail LeasedDutyWork)
        {
            try
            {
                _db = new GidaGKPEntities();
                int _effectRow = 0;
                if (LeasedDutyWork.Id > 0)
                {
                    var LeaseDeedDetails1 = _db.LeaseDeedDetails.Where(x => x.Id == LeasedDutyWork.Id).FirstOrDefault();
                    if (LeaseDeedDetails1 != null)
                    {
                        LeaseDeedDetails1.ApplicationId = LeasedDutyWork.ApplicationId;
                        LeaseDeedDetails1.AllotmentMoneyPaid = LeasedDutyWork.AllotmentMoneyPaid;
                        LeaseDeedDetails1.BankGauranteeChallanNo = LeasedDutyWork.BankGauranteeChallanNo;
                        LeaseDeedDetails1.PurchasedStampValue = LeasedDutyWork.PurchasedStampValue;
                        LeaseDeedDetails1.StampValueForBankGaurantee = LeasedDutyWork.StampValueForBankGaurantee;
                        LeaseDeedDetails1.BankGauranteeDate = LeasedDutyWork.BankGauranteeDate;
                        LeaseDeedDetails1.EntityNameBehalfOfApplicant = LeasedDutyWork.EntityNameBehalfOfApplicant;
                        LeaseDeedDetails1.CreatedDate = LeasedDutyWork.CreatedDate;

                        _db.Entry(LeaseDeedDetails1).State = EntityState.Modified;
                        _effectRow = _db.SaveChanges();
                    }
                }
                else
                {
                    _db.Entry(LeasedDutyWork).State = EntityState.Added;
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
    }
}