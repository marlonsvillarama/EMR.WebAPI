//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class ClaimLine
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ClaimLine()
        {
            this.Unit = "UN";
            this.Amount = 0m;
            this.IsEmergency = false;
            this.EPSDT = false;
            this.FamilyPlanning = false;
            this.CopayExempt = false;
            this.DocumentType = "";
        }
    
        public int Id { get; set; }
        public string Code { get; set; }
        public string Modifier { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public string Unit { get; set; }
        public int OrderLine { get; set; }
        public string SystemNoteKey { get; set; }
        public string CPT { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public string Pointer { get; set; }
        public Nullable<decimal> Quantity { get; set; }
        public Nullable<bool> IsEmergency { get; set; }
        public Nullable<bool> EPSDT { get; set; }
        public Nullable<bool> FamilyPlanning { get; set; }
        public Nullable<bool> CopayExempt { get; set; }
        public string DocumentType { get; set; }
        public Nullable<int> ClaimId { get; set; }
    
        public virtual ClaimLineDate Dates { get; set; }
        public virtual ClaimLineReference Reference { get; set; }
        public virtual ClaimLineDrug Drug { get; set; }
        public virtual ClaimLineAttachment Attachment { get; set; }
        public virtual ClaimLineDurable Durable { get; set; }
        public virtual ClaimLineContract Contract { get; set; }
        public virtual ClaimLineDialysis Dialysis { get; set; }
        public virtual ClaimLinePricing Pricing { get; set; }
        public virtual ClaimLineProduct Product { get; set; }
        public virtual ClaimLineAmbulance Ambulance { get; set; }
        public virtual ClaimLineSupplemental Supplemental { get; set; }
        public virtual ClaimLineDMERC DMERC { get; set; }
    }
}