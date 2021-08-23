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
    
    public partial class ApplicantProjectChangeDetail
    {
        public int Id { get; set; }
        public string ApplicantName { get; set; }
        public Nullable<int> ApplicationId { get; set; }
        public string AllotmentNumber { get; set; }
        public string AreaToBeUsedForProject { get; set; }
        public string DetailOfProjectFileName { get; set; }
        public string DetailOfProjectFileType { get; set; }
        public byte[] DetailOfProjectFileContent { get; set; }
        public string IsEffluentMore { get; set; }
        public string IsEmissionToSurrounding { get; set; }
        public string SSIRegistrationNo { get; set; }
        public string NOCOfProjectFileName { get; set; }
        public string NOCOfProjectFileType { get; set; }
        public byte[] NOCOfProjectFileContent { get; set; }
        public string NotarizedFileName { get; set; }
        public string NotarizedFileType { get; set; }
        public byte[] NotarizedFileContent { get; set; }
        public string SignatureFileName { get; set; }
        public string SignatureFileType { get; set; }
        public byte[] SignatureFileContent { get; set; }
        public string PhotographFileName { get; set; }
        public string PhotographFileType { get; set; }
        public byte[] PhotographFileContent { get; set; }
        public string LeaseDeedNumber { get; set; }
        public string PossesionNumber { get; set; }
        public Nullable<decimal> ProcessingFees { get; set; }
        public Nullable<decimal> AmountPaid { get; set; }
        public string TransactionNumber { get; set; }
    }
}