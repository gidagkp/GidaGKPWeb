using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GidaGkpWeb.Models
{
    public class MortgageModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ApplicationId { get; set; }
        public int AllotmentId { get; set; }
        public string ApplicantName { get; set; }
        public string FathersNameOfApplicant { get; set; }
        public string AddressOfApplicant { get; set; }
        public string SectorName { get; set; }
        public int PlotId { get; set; }
        public string PlotNo { get; set; }
        public bool PlotPaymentStatus { get; set; }
        public string NoOfPaidInstallment { get; set; }
        public Decimal TotalPaidAmount { get; set; }
        public string NoOfPaidInterestInstallment { get; set; }
        public Decimal TotalInterestPaidAmount { get; set; }
        public string MortgageBankName { get; set; }
        public string MortgageBankAddress { get; set; }
        public string EmailOfBankOfficer { get; set; }
        public bool BankPayStatusForProvisionalPremium { get; set; }
        public bool BankPayStatusForDueInterests { get; set; }
        public byte[] UploadedMortgageRequestLetter { get; set; }
        public Nullable<System.DateTime> DateOfMortgageRequestLetter { get; set; }
        public byte[] DuplicateCopyRegisteredLeaseDeed { get; set; }
        public string BankChallanNoPaidByBankToGida { get; set; }
        public string PurposeOfMortgage { get; set; }
        public string BankReadyToSactionLoan { get; set; }
        public string QuantumOfTheLoan { get; set; }
        public string LoanSanctionedFor { get; set; }
        public string BankChallanNoForPaidProvisionalPremium { get; set; }
        public Nullable<System.DateTime> DateOfProvisionalPremiumPayToGida { get; set; }
        public string ChallanNoForPayOfInterestDue { get; set; }
        public Nullable<System.DateTime> DateOnWhichInterestDueSendToGida { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    }
}