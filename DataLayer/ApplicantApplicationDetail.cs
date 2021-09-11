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
    
    public partial class ApplicantApplicationDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ApplicantApplicationDetail()
        {
            this.AllotementNotesheetDetails = new HashSet<AllotementNotesheetDetail>();
            this.ApplicantInvitationLetters = new HashSet<ApplicantInvitationLetter>();
        }
    
        public int ApplicationId { get; set; }
        public string ApplicationNumber { get; set; }
        public string AllotmentNumber { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<bool> ApprovalStatus { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AllotementNotesheetDetail> AllotementNotesheetDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ApplicantInvitationLetter> ApplicantInvitationLetters { get; set; }
    }
}
