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
    
    public partial class ClaimLinePricing
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ClaimLinePricing()
        {
            this.AmbGroupAmount = 0m;
        }
    
        public int Id { get; set; }
        public string Method { get; set; }
        public Nullable<decimal> AmountAllowed { get; set; }
        public Nullable<decimal> AmountSavings { get; set; }
        public string OrgID { get; set; }
        public Nullable<decimal> Rate { get; set; }
        public string AmbGroup { get; set; }
        public Nullable<decimal> AmbGroupAmount { get; set; }
        public string ProductQualifier { get; set; }
        public string HCPCS { get; set; }
        public string Unit { get; set; }
        public Nullable<decimal> Quantity { get; set; }
        public string ComplianceCode { get; set; }
        public string ExceptionCode { get; set; }
        public string RejectReason { get; set; }
        public string SystemNoteKey { get; set; }
    
        public virtual ClaimLine ClaimLine { get; set; }
    }
}
