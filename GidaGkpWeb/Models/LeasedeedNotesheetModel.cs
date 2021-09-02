using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GidaGkpWeb.Models
{
    public class LeasedeedNotesheetModel
    {
        public int Id { get; set; }
        public string ApplicationId { get; set; }
        public Nullable<int> ApplicantId { get; set; }
        public Nullable<System.DateTime> Bankguranteedate { get; set; }
        public string Bankaddress { get; set; }
        public string Allotmentnumber { get; set; }
        public string Digsign_Propassist { get; set; }
        public string Comment_Propassist { get; set; }
        public string Digsign_Manager { get; set; }
        public string Comment_Manager { get; set; }
        public string Digsign_FManager { get; set; }
        public string Comment_FManager { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> Createddate { get; set; }
        public string ApplicantName { get; set; }
        public string  PlotNo { get; set; }
        public string SectorName { get; set; }
        public string  PlotArea { get; set; }
        public string ApplicantAddress { get; set; }
        public string EarnestMoneyPaid { get; set; }
        public string EarnestMoneyChallanNumber { get; set; }
        public string AllotmentMoneyPaid { get; set; }
        public string VerifyTotalPremium { get; set; }
        public string StampValue { get; set; }
        public string BankgChallanNumber { get; set; }
        public string EntityName { get; set; }
        public string SvalueBankGchallanNumber { get; set; }

    }
}