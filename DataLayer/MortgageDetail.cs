//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataLayer
{
    using System;
    using System.Collections.Generic;
    
    public partial class MortgageDetail
    {
        public int Id { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> ApplicationId { get; set; }
        public Nullable<int> AllotmentId { get; set; }
        public string ApplicantName { get; set; }
        public string FathersNameOfApplicant { get; set; }
        public string AddressOfApplicant { get; set; }
        public string SectorName { get; set; }
        public string PlotNo { get; set; }
        public Nullable<bool> PlotPaymentStatus { get; set; }
        public string NoOfPaidInstallment { get; set; }
        public Nullable<decimal> TotalPaidAmount { get; set; }
        public string NoOfPaidInterestInstallment { get; set; }
        public Nullable<decimal> TotalInterestPaidAmount { get; set; }
        public string MortgageBankName { get; set; }
        public string MortgageBankAddress { get; set; }
        public string EmailOfBankOfficer { get; set; }
        public Nullable<bool> BankPayStatusForProvisionalPremium { get; set; }
        public Nullable<bool> BankPayStatusForDueInterests { get; set; }
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
