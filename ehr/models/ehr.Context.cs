﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EMR.WebAPI.ehr.models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class EHRDB : DbContext
    {
        public EHRDB()
            : base("name=EHRDB")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<ClaimLine> ClaimLines { get; set; }
        public virtual DbSet<Claim> Claims { get; set; }
        public virtual DbSet<CPTCode> CPTCodes { get; set; }
        public virtual DbSet<Facility> Facilities { get; set; }
        public virtual DbSet<ICDCode> ICDCodes { get; set; }
        public virtual DbSet<Payer> Payers { get; set; }
        public virtual DbSet<Modifier> Modifiers { get; set; }
        public virtual DbSet<Patient> Patients { get; set; }
        public virtual DbSet<Provider> Providers { get; set; }
        public virtual DbSet<PlaceOfService> PlaceOfServices { get; set; }
        public virtual DbSet<TaxonomyCode> TaxonomyCodes { get; set; }
        public virtual DbSet<Subscriber> Subscribers { get; set; }
        public virtual DbSet<SystemNote> SystemNotes { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<ClaimDate> ClaimDates { get; set; }
        public virtual DbSet<ClaimLineDMERC> ClaimLineDMERCs { get; set; }
        public virtual DbSet<ClaimLineDate> ClaimLineDates { get; set; }
        public virtual DbSet<ClaimLineReference> ClaimLineReferences { get; set; }
        public virtual DbSet<ClaimLineSupplemental> ClaimLineSupplementals { get; set; }
        public virtual DbSet<ClaimLineDrug> ClaimLineDrugs { get; set; }
        public virtual DbSet<ClaimLineAttachment> ClaimLineAttachments { get; set; }
        public virtual DbSet<ClaimLineDurable> ClaimLineDurables { get; set; }
        public virtual DbSet<ClaimLineAmbulance> ClaimLineAmbulances { get; set; }
        public virtual DbSet<ClaimLineContract> ClaimLineContracts { get; set; }
        public virtual DbSet<ClaimLineDialysis> ClaimLineDialysis { get; set; }
        public virtual DbSet<ClaimLinePricing> ClaimLinePricings { get; set; }
        public virtual DbSet<ClaimLineProduct> ClaimLineProducts { get; set; }
        public virtual DbSet<Submitter> Submitters { get; set; }
        public virtual DbSet<Receiver> Receivers { get; set; }
        public virtual DbSet<Batch> Batches { get; set; }
        public virtual DbSet<PayTo> PayToes { get; set; }
        public virtual DbSet<ClaimAccident> ClaimAccidents { get; set; }
        public virtual DbSet<ClaimSupplemental> ClaimSupplementals { get; set; }
        public virtual DbSet<ClaimContract> ClaimContracts { get; set; }
        public virtual DbSet<ClaimReference> ClaimReferences { get; set; }
        public virtual DbSet<ClaimAmbulance> ClaimAmbulances { get; set; }
        public virtual DbSet<ClaimSpinal> ClaimSpinals { get; set; }
        public virtual DbSet<ClaimCondition> ClaimConditions { get; set; }
        public virtual DbSet<ClaimRepricing> ClaimRepricings { get; set; }
        public virtual DbSet<ClaimAmount> ClaimAmounts { get; set; }
        public virtual DbSet<Insurer> Insurers { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<PaymentLine> PaymentLines { get; set; }
        public virtual DbSet<ErrorCode> ErrorCodes { get; set; }
        public virtual DbSet<ErrorMessage> ErrorMessages { get; set; }
        public virtual DbSet<AccountPreference> AccountPreferences { get; set; }
    }
}