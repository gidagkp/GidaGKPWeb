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
    
    public partial class ApplicantInvitationLetter
    {
        public int Id { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> ApplicationId { get; set; }
        public string SectorName { get; set; }
        public Nullable<int> AllotmentId { get; set; }
        public string ApplicantAddress { get; set; }
        public string PlotDetails { get; set; }
        public string PlotRange { get; set; }
        public Nullable<int> PlotId { get; set; }
        public string InterviewMode { get; set; }
        public Nullable<System.DateTime> InterviewDateTime { get; set; }
        public string InterviewLetterStatus { get; set; }
        public Nullable<decimal> AmountToBePaid { get; set; }
        public Nullable<decimal> AmountPaid { get; set; }
        public string CommentByFinance { get; set; }
        public string CommentByAssistant { get; set; }
        public string PaymentStatusByFinance { get; set; }
        public string PaymentStatusByAssistant { get; set; }
        public string Action { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ApplicationNo { get; set; }
    
        public virtual ApplicantApplicationDetail ApplicantApplicationDetail { get; set; }
    }
}
