﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class GidaGKPEntities : DbContext
    {
        public GidaGKPEntities()
            : base("name=GidaGKPEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AdminNotice> AdminNotices { get; set; }
        public virtual DbSet<AllocateAllotmentDetail> AllocateAllotmentDetails { get; set; }
        public virtual DbSet<ApplicantApplicationDetail> ApplicantApplicationDetails { get; set; }
        public virtual DbSet<ApplicantBankDetail> ApplicantBankDetails { get; set; }
        public virtual DbSet<ApplicantDetail> ApplicantDetails { get; set; }
        public virtual DbSet<ApplicantFormStep> ApplicantFormSteps { get; set; }
        public virtual DbSet<ApplicantInvitationLetter> ApplicantInvitationLetters { get; set; }
        public virtual DbSet<ApplicantPlotDetail> ApplicantPlotDetails { get; set; }
        public virtual DbSet<ApplicantProjectChangeDetail> ApplicantProjectChangeDetails { get; set; }
        public virtual DbSet<ApplicantProjectDetail> ApplicantProjectDetails { get; set; }
        public virtual DbSet<ApplicantTransactionDetail> ApplicantTransactionDetails { get; set; }
        public virtual DbSet<ApplicantUploadDoc> ApplicantUploadDocs { get; set; }
        public virtual DbSet<ApplicantUser> ApplicantUsers { get; set; }
        public virtual DbSet<GidaUser> GidaUsers { get; set; }
        public virtual DbSet<LeaseDeedDetail> LeaseDeedDetails { get; set; }
        public virtual DbSet<Lookup> Lookups { get; set; }
<<<<<<< HEAD
        public virtual DbSet<MortgageDetail> MortgageDetails { get; set; }
=======
>>>>>>> 201e0431310663a3f60b3136013818da9f17bb30
        public virtual DbSet<PageMaster> PageMasters { get; set; }
        public virtual DbSet<PGTransactionInformation> PGTransactionInformations { get; set; }
        public virtual DbSet<PlotMaster> PlotMasters { get; set; }
        public virtual DbSet<RoleWisePermission> RoleWisePermissions { get; set; }
        public virtual DbSet<SchemewiseTermsCondition> SchemewiseTermsConditions { get; set; }
<<<<<<< HEAD
        public virtual DbSet<LeasdeedNotesheet> LeasdeedNotesheets { get; set; }
=======
>>>>>>> 201e0431310663a3f60b3136013818da9f17bb30
    }
}
